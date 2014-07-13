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

type BiggestRivalries = {
    description: string; battles: int; forces1: int; losses1: int; wins1: int;
    forces2: int; losses2: int; wins2 : int;
}

let BiggestCommanderRivalry (save:Save) =
    let countryLeaders =
        save.Countries 
        |> Seq.collect (fun x -> 
            (leaders x)
            |> Seq.map (fun l -> (l.Name, l)))
        |> Map.ofSeq

    wars save 
    |> battles
    |> Seq.where (fun x -> x.Attacker.Commander <> "")
    |> Seq.where (fun x -> x.Defender.Commander <> "")

    // We actually need to check to make sure that a country has the specified
    // commander has there has been commanderes that are listed in battles do
    // not show up under any country
    |> Seq.where (fun x -> countryLeaders.TryFind(x.Attacker.Commander) <> None)
    |> Seq.where (fun x -> countryLeaders.TryFind(x.Defender.Commander) <> None)

    // The group battles by commanders -- they are grouped alphabetically
    |> Seq.groupBy (fun x ->
        if x.Attacker.Commander < x.Defender.Commander then
            (x.Attacker.Commander, x.Attacker.Country, x.Defender.Commander, x.Defender.Country)
        else
            (x.Defender.Commander, x.Defender.Country, x.Attacker.Commander, x.Attacker.Country))
    |> Seq.map (fun ((com1, c1, com2, c2), battles) -> 
        let findbs name = seq { 
            for b in battles -> if name = b.Attacker.Commander
                                then (b.Attacker, b.Result)
                                else (b.Defender, not b.Result) }
        let com1bs = findbs com1 |> Seq.map fst
        let com2bs = findbs com2 |> Seq.map fst
        let com1l = com1bs |> Seq.map (fun x -> x.Losses) |> Seq.fold (+) 0
        let com2l = com2bs |> Seq.map (fun x -> x.Losses) |> Seq.fold (+) 0
        let com1f = com1bs |> Seq.map forces |> Seq.fold (+) 0
        let com2f = com2bs |> Seq.map forces |> Seq.fold (+) 0
        let com1Wins = findbs com1 |> Seq.map snd |> Seq.where id |> Seq.length
        let com2Wins = findbs com2 |> Seq.map snd |> Seq.where id |> Seq.length

        let stats name =
            let leader = countryLeaders.[name]
            sprintf "(%d/%d/%d/%d)" leader.Fire leader.Shock
                                    leader.Manuever leader.Siege

        let com1Stats = stats com1
        let com2Stats = stats com2

        { description = sprintf "%s%s and %s%s of %s and %s" com1 com1Stats
                                 com2 com2Stats c1 c2;
          battles = Seq.length battles; forces1 = com1f; losses1 = com1l;
          wins1 = com1Wins; forces2 = com2f; losses2 = com2l; wins2 = com2Wins })
    |> Seq.sortBy (fun rivalry -> (~-) rivalry.battles)