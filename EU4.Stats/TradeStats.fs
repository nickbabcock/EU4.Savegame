module EU4.Stats.TradeStats

open EU4.Savegame
open EU4.Stats.Types

type PowerStats = {
    country: string; lightShips: int; shipPower: double;
    power: double; provincePower: double; money: double; total: double;
}

let calc (save:Save) =
    save.Trade
    |> Seq.collect (fun x -> x.Powers)
    |> Seq.groupBy (fun x -> x.Country)
    |> Seq.map (fun (abbr, col) ->
        { country = abbr;
          lightShips = col |> Seq.sumBy (fun x -> x.LightShip);
          shipPower = col |> Seq.sumBy (fun x -> x.ShipPower);
          power = col |> Seq.sumBy (fun x -> x.Current);
          provincePower = col |> Seq.sumBy (fun x -> x.ProvincePower)
          money = col |> Seq.sumBy (fun x -> x.Money)
          total = col |> Seq.sumBy (fun x -> x.Total)
        })
    |> Seq.sortBy (fun x -> (~-) x.power)
