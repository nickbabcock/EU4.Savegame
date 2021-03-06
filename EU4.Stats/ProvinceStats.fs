module EU4.Stats.ProvinceStats
open System;
open EU4.Savegame;
open EU4.Stats.Types;
open MathNet.Numerics.Statistics;

// Sequence of tuples of trade good and the number of provinces with
// that trade good
let TradeGoods (save:Save) =
    save.Provinces
    |> Seq.groupBy(fun x -> x.TradeGoods)
    |> Seq.map(fun (key, grp) -> (key, Seq.length grp))

// Sequence of tuples of country's name and number of units being built
let CountriesRecruiting (save:Save) =
    save.Provinces
    |> Seq.collect(fun x -> x.MilitaryConstructions)
    |> Seq.groupBy (fun x -> x.Country)
    |> Seq.map(fun (key, grp) -> (key, Seq.length grp))