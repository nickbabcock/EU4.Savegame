module EU4.Stats.CountryStats

open EU4.Savegame;
open EU4.Stats.Types

type CountryRankings = {
    rank: int; name: string; player: bool; score: double;
    dip: double; adm: double; mil: double;
}

/// Calculates the desire for various AI countries to want something
let highestAiPriorities (save:Save) (fn:Ai -> seq<AiProvincePriority>) =
    save.Countries
    |> Seq.where (fun x -> x.NumOfCities > 0)
    |> Seq.collect (fun x -> fn x.Ai)
    |> Seq.groupBy (fun x -> x.Id)
    |> Seq.map (fun (key, grp) ->
        let summation = 
            grp
            |> Seq.map (fun x -> x.Value)
            |> Seq.fold (+) 0
        (key, summation))

/// Creates a five number summary on countries in a tech group
let techSummary (save:Save) (fn:Technology -> byte) =
    save.Countries
    |> Seq.where (fun x -> x.NumOfCities > 0)
    |> Seq.groupBy (fun x -> x.TechnologyGroup)
    |> Seq.map (fun (key, grp) ->
        let summary =
            grp
            |> toSummary (fun x -> float (fn x.Technology))
        (key, summary))

let scoreRankings (save:Save) =
    save.Countries
    |> Seq.where (fun x -> notNull x.ScoreRating && x.ScoreRating.Count = 3)
    |> Seq.sortBy (fun x -> (~-) x.Score)
    |> Seq.mapi (fun ind x ->
        { rank = ind + 1; name = x.Abbreviation; score = x.Score;
          player = x.WasPlayer.GetValueOrDefault(); adm = x.ScoreRating.[0];
          dip = x.ScoreRating.[1]; mil = x.ScoreRating.[2] })
    |> Seq.where (fun x -> x.rank <= 10 || x.player)

let inDebt (save:Save) =
    save.Countries
    |> Seq.map (fun x ->
        (x.Abbreviation, x.Loans.Count, x.Loans |> Seq.sumBy (fun y -> y.Amount)))
    |> Seq.sortBy (fun (_,_,amount) -> (~-) amount)