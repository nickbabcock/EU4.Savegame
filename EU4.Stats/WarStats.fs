module EU4.Stats.WarStats

open EU4.Savegame;
open EU4.Stats.Types

let private wars (save:Save) = (save.ActiveWars, save.PreviousWars) ||> Seq.append
let private getBattles (war:War) =
    war.History
    |> Seq.fold (fun lst elem -> match elem with
                                 | :? BattleResult as b -> b :: lst
                                 | _ -> lst) []

let private battles (wars:seq<War>) = wars |> Seq.collect getBattles

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

let WinsOutnumbers (save:Save) =
    wars save |> battles |> commanders
    |> Seq.map(fun ((name, country), battles) ->
        let ws = battles
                 |> Seq.where (fun (me,you,win,_) ->
                        (forces me) < (forces you) && win)
        (name, country, Seq.length battles, Seq.length ws))