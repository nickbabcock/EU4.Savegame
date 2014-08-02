module EU4.Stats.LedgerStats

open EU4.Savegame
open EU4.Stats.Types
open MathNet.Numerics.Statistics;

type LedgerStat = {
    Country : string;
    NationSize : seq<int>;
    Income : seq<int>;
    Score : seq<int>;
    Inflation : seq<int>;
}

type Correlations = {
    SizeAndIncome : FiveNumberSummary;
    SizeAndScore : FiveNumberSummary;
    SizeAndInflation : FiveNumberSummary;
    IncomeAndScore : FiveNumberSummary;
    IncomeAndInflation : FiveNumberSummary;
    ScoreAndInflation : FiveNumberSummary;
}

/// Given LedgerData return a sequence of 3-tuples where it's CountryName
/// * Year * Rank
let LedgerRankings (data:seq<LedgerData>) =
    data
    |> Seq.collect(fun d -> seq { for (x, y) in (Seq.zip d.XData d.YData)
                                        do yield (d.Name, x, y) })
    |> Seq.groupBy (fun (_, date, _) -> date)
    |> Seq.collect(fun (_, col) ->
        col
        |> Seq.map(fun (_, _, y) -> (~-) (float y))
        |> (fun l -> Statistics.Ranks(l, RankDefinition.Sports))
        |> Seq.zip col
        |> Seq.map(fun ((name, x, y), (rank)) -> (name, x, int rank)))

/// Return a sequence of tuples of (name, x) where x is the number of
/// years that the name was number 1 in the rankings
let YearsLeadingLedger(data:seq<LedgerData>) : seq<string * int> =
    data
    |> LedgerRankings
    |> Seq.groupBy (fun (name, _, _) -> name)
    |> Seq.map (fun (key, grp) -> 
        grp
        |> Seq.filter (fun (name, x, rank) -> rank = 1)
        |> (fun x -> (key, Seq.length x)))

/// Creates a sequence of (name, date, rank difference) where the date is the
/// year of occurrence. A negative rank difference is a good thing, depicting
/// moving "up" in the ranks. Takes only values where a country is either
/// exiting or entering the top ranklimit, as, in general, we don't care about
/// the bottom feeders
let RelativeRankChange (data:seq<LedgerData>) rankLimit =
    data
    |> LedgerRankings
    |> Seq.groupBy (fun (name, _, _) -> name)
    |> Seq.collect(fun (name, group) ->
        group
        |> Seq.sortBy (fun (_, date, _) -> date)
        |> Seq.pairwise
        |> Seq.filter (fun ((name, x1, y1), (_, _, y2)) ->
            match rankLimit with | None -> true | Some x -> y2 < x || y1 < x)
        |> Seq.map(fun ((name, x1, y1), (_, _, y2)) -> (name, x1, y2 - y1)))