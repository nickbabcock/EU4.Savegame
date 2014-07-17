module EU4.Stats.WarStats

open EU4.Savegame;
open EU4.Stats.Types

type LeaderReport = {
    leader : Leader; forces: int; losses: int; faced: int; kills: int;
    battles: int; wins: int;
}

type DeploymentsWarReport = {
    name : string; attacker : string; defender : string; attackingForces : int;
    battles: int; attackingLosses : int; defendingForces: int; defendingLosses: int;
    attackingWins: int; defendingWins: int;
}

type BiggestRivalries = {
    description: string; battles: int; forces1: int; losses1: int; wins1: int;
    forces2: int; losses2: int; wins2 : int;
}

type WarStatsCalculation = {
    BiggestRivalries: seq<BiggestRivalries>;
    BiggestLandWars: seq<DeploymentsWarReport>;
    BiggestNavalWars: seq<DeploymentsWarReport>;
    LeaderReport: seq<LeaderReport>
}

type private Precomputations = {
    wars: War array; battles : seq<War * seq<BattleResult>>;
    leaders: seq<string * seq<Leader>>; leaderMap : Map<string, Leader>;
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

/// Summation of all units a side deployed in a battle
let private forces side = navalDeployments side + landDeployments side

let private precompute (save:Save) =
    let wars = (save.ActiveWars, save.PreviousWars) ||> Seq.append |> Seq.toArray
    let battles =
        wars
        |> Seq.map (fun war ->
            let warBattles =
                if isNull war.History then Seq.empty else
                war.History
                |> Seq.choose (fun x -> match x with
                                        | :? BattleResult as b -> Some(b)
                                        | _ -> None)
            (war, warBattles))

    let leaders =
        save.Countries
        |> Seq.map (fun country ->
            let leads = 
                if isNull country.History then Seq.empty else seq {
                    for event in country.History do
                        match event with
                        | :? Leader as l -> yield l
                        | :? Monarch as m when notNull m.Leader -> yield m.Leader
                        | :? Heir as h when notNull h.Leader -> yield h.Leader
                        | _ -> ()
                }
            
            (country.Abbreviation, leads))

    let leaderMap =
        leaders
        |> Seq.collect (fun (_, leads) -> 
            leads |> Seq.map (fun x -> (x.Name, x)))
        |> Map.ofSeq

    { wars = wars; battles = battles; leaders = leaders; leaderMap = leaderMap }

/// Computes the number of battles, forces, and losses in a type of battle (land
/// or sea) in a war.
let private biggestWars (state:Precomputations) fn =
    state.battles
    |> Seq.map (fun (war, allBattles) ->
        let battles = allBattles |> Seq.where (fun x -> (fn x.Attacker) <> 0)
        let attackers = battles |> Seq.map (fun x -> x.Attacker)
        let defenders = battles |> Seq.map (fun x -> x.Defender)
        let attackingWins = battles |> Seq.map (fun x -> x.Result)
                            |> Seq.where id |> Seq.length

        { name = war.Name; 
          attacker = war.OriginalAttacker;
          defender = war.OriginalDefender;
          attackingForces = attackers |> Seq.sumBy fn;
          defendingForces = defenders |> Seq.sumBy fn;
          attackingLosses = attackers |> Seq.sumBy (fun x -> x.Losses);
          defendingLosses = defenders |> Seq.sumBy (fun x -> x.Losses);
          battles = Seq.length battles;
          attackingWins = attackingWins;
          defendingWins = Seq.length battles - attackingWins
        })
    |> Seq.sortBy (fun x -> (~-) x.battles)

/// For all the leaders that fought in a battle compile statistics based on
/// their wins, losses, kills, etc.
let private leaderReport (state:Precomputations) =
    state.battles
    |> Seq.collect snd
    |> Seq.collect (fun x -> seq {
            yield (x.Attacker, x.Defender, x.Result, x.Location)
            yield (x.Defender, x.Attacker, not x.Result, x.Location)
        })
    |> Seq.where (fun (side,_,_,_) -> side.Commander <> "")
    |> Seq.groupBy (fun (side,_,_,_) -> side.Commander)
    |> Seq.choose (fun (name, battles) -> 
        match (state.leaderMap.TryFind name) with
        | Some leader -> Some (leader, battles)
        | None -> None)
    |> Seq.map (fun (leader, battles) ->
        let forces, losses, faces, kills, wins =
            battles
            |> Seq.fold (fun (f,l,f2,k,w) (me,you,win,_) ->
                (f + forces me,
                 l + me.Losses,
                 f2 + forces you,
                 k + you.Losses,
                 w + (if win then 1 else 0))) (0,0,0,0,0)
        { leader = leader; forces = forces; losses = losses; faced = faces;
          kills = kills; battles = Seq.length battles; wins = wins })

/// Rivalries are calculated based on the number of times two commanders faced
/// off on the battlefield. This statistics is a culmination of of all wars that
/// the two commanders fought against each other in.
let private BiggestCommanderRivalry (state:Precomputations) =
    state.battles
    |> Seq.collect snd
    |> Seq.where (fun x -> x.Attacker.Commander <> "")
    |> Seq.where (fun x -> x.Defender.Commander <> "")

    // We actually need to check to make sure that a country has the specified
    // commander as there has been commanders that are listed in battles that
    // do not show up under any country
    |> Seq.where (fun x -> state.leaderMap.TryFind(x.Attacker.Commander) <> None)
    |> Seq.where (fun x -> state.leaderMap.TryFind(x.Defender.Commander) <> None)

    // The group battles by commanders -- they are grouped alphabetically. It's
    // very subtle but the country that is listed in the fighting side may not
    // be the same country as the commander. I'm not sure in what circumstances
    // this occurs, but I know "He Li" of MNG has fought wars that have a country
    // of "DAI"
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
        let com1Wins = findbs com1 |> Seq.map snd |> Seq.where id |> Seq.length

        let stats name =
            let leader = state.leaderMap.[name]
            sprintf "(%d/%d/%d/%d)" leader.Fire leader.Shock
                                    leader.Manuever leader.Siege

        let com1Stats = stats com1
        let com2Stats = stats com2

        { description = sprintf "%s%s and %s%s of %s and %s" com1 com1Stats
                                 com2 com2Stats c1 c2;
          battles = Seq.length battles;
          forces1 = com1bs |> Seq.sumBy forces;
          losses1 = com1bs |> Seq.sumBy (fun x -> x.Losses);
          wins1 = com1Wins; wins2 = Seq.length battles - com1Wins;
          forces2 = com2bs |> Seq.sumBy forces; 
          losses2 = com2bs |> Seq.sumBy (fun x -> x.Losses); })
    |> Seq.sortBy (fun rivalry -> (~-) rivalry.battles)

let Calc (save:Save) =
    let state = precompute save

    { BiggestRivalries = BiggestCommanderRivalry state |> Seq.take 10;
      BiggestLandWars =  biggestWars state landDeployments |> Seq.take 10;
      BiggestNavalWars = biggestWars state navalDeployments |> Seq.take 10;
      LeaderReport = leaderReport state;
     }