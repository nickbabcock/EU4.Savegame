module EU4.Stats.WarStats

open EU4.Savegame;
open EU4.Stats.Types

type LeaderReport = {
    leader : Leader; country : string; battles : seq<HistoricCombatant * HistoricCombatant * bool * int>; 
}

let private wars (save:Save) = (save.ActiveWars, save.PreviousWars) ||> Seq.append
let private getBattles (war:War) =
    war.History
    |> Seq.fold (fun lst elem -> match elem with
                                 | :? BattleResult as b -> b :: lst
                                 | _ -> lst) []

let private battles (wars:seq<War>) = wars |> Seq.collect getBattles
let private leaders (country:Country) =
    if country.History = null then Seq.empty else seq {
        for event in country.History do
            match event with
            | :? Leader as l -> yield l
            | :? Monarch as m when m.Leader <> null -> yield m.Leader
            | :? Heir as h when h.Leader <> null -> yield h.Leader
            | _ -> ()
    }

let private commanders (battles:seq<BattleResult>) =
    battles
    |> Seq.collect (fun bat -> 
        seq { 
        yield (bat.Attacker, bat.Defender, bat.Result, bat.Location)
        yield (bat.Defender, bat.Attacker, not bat.Result, bat.Location)
        })
    |> Seq.where (fun (side,_,_,_) -> side.Commander <> "")
    |> Seq.groupBy (fun (side,_,_,_) -> (side.Commander, side.Country))

let forces (side:HistoricCombatant) =
    side.Artillery.GetValueOrDefault() +
    side.Cavalry.GetValueOrDefault() +
    side.Galley.GetValueOrDefault() +
    side.HeavyShip.GetValueOrDefault() +
    side.Infantry.GetValueOrDefault() +
    side.LightShip.GetValueOrDefault() +
    side.Transport.GetValueOrDefault()

let LeaderReport (save:Save) =
    let battleCommanders = wars save |> battles |> commanders
    let countryLeaders =
        save.Countries 
        |> Seq.collect (fun x -> (leaders x)
                                  |> Seq.map (fun l -> (x.Abbreviation, l)))

    let merge = seq {
        for ((name, bcountry), battles) in battleCommanders do
          for (lcountry, leader) in countryLeaders do
            if bcountry = lcountry && name = leader.Name then
              yield { leader = leader; country = bcountry; battles = battles }
    }

    merge
    |> Seq.map (fun x -> 
        let forces, kills, losses =
            x.battles
            |> Seq.fold(fun (f, k, l) (me,you,_,_) ->
                (f + forces me, k + you.Losses, l + me.Losses)) (0, 0, 0)

        (x.leader, x.country, forces, kills, losses))
