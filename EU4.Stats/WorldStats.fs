module EU4.Stats.WorldStats

open EU4.Savegame;
open EU4.Stats.Types

type WorldReport = {
    manpower: double; potentialManpower: double
}

let calc (save:Save) =
    let countries = save.Countries |> Seq.where (fun x -> x.NumOfCities <> 0)

    { manpower = (countries |> Seq.sumBy (fun x -> x.Manpower)) * 1000.0;
      potentialManpower = (countries |> Seq.sumBy (fun x -> x.MaxManpower)) * 1000.0}