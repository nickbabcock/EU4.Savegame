module ProvinceStats
open EU4.Savegame;
open Types;
open MathNet.Numerics.Statistics;

let ProvinceSummary (save:Savegame) (fn:Province -> double) : FiveNumberSummary = 
    save.Provinces
    |> Seq.map fn
    |> Statistics.FiveNumberSummary
    |> ArrayToSummary

// Sequence of tuples of trade good and the number of provinces with
// that trade good
let TradeGoods (save:Savegame) =
    save.Provinces
    |> Seq.groupBy(fun x -> x.TradeGoods)
    |> Seq.map(fun (key, grp) -> (key, Seq.length grp))

// Sequence of tuples of country's name and number of units being built
let CountriesRecruiting (save:Savegame) =
    save.Provinces
    |> Seq.collect(fun x -> x.MilitaryConstructions)
    |> Seq.groupBy (fun x -> x.Country)
    |> Seq.map(fun (key, grp) -> (key, Seq.length grp))