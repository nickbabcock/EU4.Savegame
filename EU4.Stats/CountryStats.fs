module CountryStats

open EU4.Savegame;
open Types

/// Calculates the desire for various AI countries to want something
let highestAiPriorities (save:Savegame) (fn:Ai -> seq<AiProvincePriority>) =
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
let techSummary (save:Savegame) (fn:Technology -> int) =
    save.Countries
    |> Seq.where (fun x -> x.NumOfCities > 0)
    |> Seq.groupBy (fun x -> x.TechnologyGroup)
    |> Seq.map (fun (key, grp) ->
        let summary =
            grp
            |> Seq.map (fun x -> float (fn x.Technology))
            |> summarize
        (key, summary))