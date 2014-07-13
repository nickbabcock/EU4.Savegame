module EU4.Stats.WarStats

open EU4.Savegame;
open EU4.Stats.Types

type LeaderReport = {
    leader : Leader; country : string; battles : seq<HistoricCombatant * HistoricCombatant * bool * int>; 
}

let private wars (save:Save) = (save.ActiveWars, save.PreviousWars) ||> Seq.append
let private getBattles (war:War) =
    if war = null || war.History = null then Seq.empty else seq {
        for event in war.History do
            match event with
            | :? BattleResult as b -> yield b
            | _ -> ()
    }

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

type DeploymentsWarReport = {
    name : string; attacker : string; defender : string; attackerDeployments : int;
    battles: int; attackerLosses : int; defenderDeployments: int; defenderLosses: int;
    depsAndLosses: int
}

/// Summation of all the naval units a side deployed in a naval battle
let private navalDeployments (combatant:HistoricCombatant) =
    combatant.HeavyShip.GetValueOrDefault() +
    combatant.LightShip.GetValueOrDefault() +
    combatant.Galley.GetValueOrDefault() +
    combatant.Transport.GetValueOrDefault()

/// Summation of all the land units a side deployed in a land battle
let private landDeployments (combatant:HistoricCombatant) =
    combatant.Artillery.GetValueOrDefault() +
    combatant.Cavalry.GetValueOrDefault() +
    combatant.Infantry.GetValueOrDefault();

/// Calculates the "biggest" wars, which is the sum of all the deployments
/// and casualties in a war
let private biggestWars (save:Save) (fn:HistoricCombatant -> int) =
    wars save
    |> Seq.map (fun war ->
        let attackers = getBattles war |> Seq.map (fun x -> x.Attacker) 
                        |> Seq.where (fun x -> fn x <> 0)
        let defenders = getBattles war |> Seq.map (fun x -> x.Defender) 
                        |> Seq.where (fun x -> fn x <> 0)
        let attackerDeps = attackers |> Seq.map fn |> Seq.fold (+) 0
        let defenderDeps = defenders |> Seq.map fn |> Seq.fold (+) 0
        let attackerLosses = attackers |> Seq.map (fun x -> x.Losses) |> Seq.fold (+) 0
        let defenderLosses = defenders |> Seq.map (fun x -> x.Losses) |> Seq.fold (+) 0

        { name = war.Name; attacker = war.OriginalAttacker;
          defender = war.OriginalDefender; battles = Seq.length attackers;
          attackerDeployments = attackerDeps; attackerLosses = attackerLosses;
          defenderDeployments = defenderDeps; defenderLosses = defenderLosses;
          depsAndLosses = attackerDeps + defenderDeps + attackerLosses + defenderLosses })
    |> Seq.sortBy (fun x -> (~-) (x.depsAndLosses))

let BiggestNavalWars (save:Save) = biggestWars save navalDeployments
let BiggestLandWars (save:Save) = biggestWars save landDeployments
