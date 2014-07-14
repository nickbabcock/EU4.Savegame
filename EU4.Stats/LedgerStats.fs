module EU4.Stats.LedgerStats

open System;
open System.Collections.Generic;
open EU4.Savegame;
open EU4.Stats.Types
open MathNet.Numerics.Statistics;

type LedgerStat = {
    Country : string;
    NationSize : IList<int>;
    Income : IList<int>;
    Score : IList<int>;
    Inflation : IList<int>;
}

type Correlations = {
    SizeAndIncome : FiveNumberSummary;
    SizeAndScore : FiveNumberSummary;
    SizeAndInflation : FiveNumberSummary;
    IncomeAndScore : FiveNumberSummary;
    IncomeAndInflation : FiveNumberSummary;
    ScoreAndInflation : FiveNumberSummary;
}

/// Returns the five number summaries of the correlations between
/// the ledger information stored in the save file
let correlations (data:Save) =
    let q = query {
        for nationSize in data.NationSizeStatistics do
        join income in data.IncomeStatistics on
            (nationSize.Name = income.Name)
        join score in data.ScoreStatistics on
            (nationSize.Name = score.Name)
        join inflation in data.InflationStatistics on
            (nationSize.Name = inflation.Name)
        where (Seq.last nationSize.YData > 0)

        select { Country = nationSize.Name;
                    NationSize = nationSize.YData;
                    Income = income.YData;
                    Score = score.YData;
                    Inflation = inflation.YData }
    }

    let cast (lis:IList<int>) : seq<float> = lis |> Seq.map(fun x -> float x) 

    let correlate (f1:LedgerStat -> IList<int>) (f2:LedgerStat -> IList<int>) =
        // We must make sure that the difference between the max value and
        // the min value in a list is non-zero else the variance will be
        // zero and there will be dividing by zero error. So filter it to
        // only non-zero difference lists
        q
        |> Seq.where(fun x -> Seq.min (f1(x)) <> Seq.max (f1(x)))
        |> Seq.where(fun x -> Seq.min (f2(x)) <> Seq.max (f2(x)))
        |> toSummary (fun x -> Correlation.Pearson(cast(f1(x)), cast(f2(x))))

    { 
        SizeAndIncome = correlate (fun x -> x.NationSize) (fun x -> x.Income);
        SizeAndScore = correlate (fun x -> x.NationSize) (fun x -> x.Score);
        SizeAndInflation = correlate (fun x -> x.NationSize) (fun x -> x.Inflation);
        IncomeAndScore = correlate (fun x -> x.Income) (fun x -> x.Score);
        IncomeAndInflation = correlate (fun x -> x.Income) (fun x -> x.Inflation);
        ScoreAndInflation = correlate (fun x -> x.Score) (fun x -> x.Inflation);
    }

/// Given LedgerData return a sequence of 3-tuples where it's CountryName
/// * Year * Rank
let rankings (data:seq<LedgerData>) =
    data
    |> Seq.collect(fun d -> seq { for (x, y) in (Seq.zip d.XData d.YData)
                                        do yield (d.Name, x, y) })
    |> Seq.groupBy (fun (_, date, _) -> date)
    |> Seq.collect(fun (_, col) ->
        col
        |> Seq.map(fun (_, _, y) -> float y)
        |> (fun l -> Statistics.Ranks(l, RankDefinition.Sports))
        |> Seq.zip col
        |> Seq.map(fun ((name, x, y), (rank)) -> (name, x, int rank)))

// Return a sequence of tuples of (name, x) where x is the number of
// years that the name was number 1 in the rankings
let yearsFirst(data:seq<LedgerData>) : seq<string * int> =
    data
    |> rankings
    |> Seq.groupBy (fun (name, _, _) -> name)
    |> Seq.map (fun (key, grp) -> 
        grp
        |> Seq.filter (fun (name, x, rank) -> rank = 0)
        |> (fun x -> (key, Seq.length x)))

// Creates a sequence of (name, date, rank difference) where
// the date is the year of occurrence. A negative rank difference
// is a good thing, depicting moving "up" in the ranks.
// Takes only values where a country is either exiting or entering
// the top 30, as, in general, we don't care about the bottom feeders
let GreatestChangeInRelativity (data:seq<LedgerData>) rankLimit =
    data
    |> rankings
    |> Seq.groupBy (fun (name, _, _) -> name)
    |> Seq.collect(fun (name, group) ->
        group
        |> Seq.sortBy (fun (_, date, _) -> date)
        |> Seq.pairwise
        |> Seq.filter (fun ((name, x1, y1), (_, _, y2)) -> y2 < rankLimit || y1 < rankLimit)
        |> Seq.map(fun ((name, x1, y1), (_, _, y2)) -> (name, x1, y2 - y1)))
