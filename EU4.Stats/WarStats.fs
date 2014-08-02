module EU4.Stats.WarStats

open EU4.Savegame
open EU4.Stats.Types

type LeaderReport = {
    leader : Leader; forces: int; losses: int; faced: int; kills: int;
    battles: int; wins: int;
}

type WarReport = {
    name : string; attacker : string; defender : string; attackingForces : int;
    battles: int; attackingLosses : int; defendingForces: int; defendingLosses: int;
    attackingWins: int; defendingWins: int;
}

type BiggestRivalries = {
    description: string; battles: int; forces1: int; losses1: int; wins1: int;
    forces2: int; losses2: int; wins2 : int;
}

/// Summation of all the naval units a side deployed in a naval battle
let navalDeployments (combatant:HistoricCombatant) =
    combatant.HeavyShip.GetValueOrDefault() +
    combatant.LightShip.GetValueOrDefault() +
    combatant.Galley.GetValueOrDefault() +
    combatant.Transport.GetValueOrDefault()

/// Summation of all the land units a side deployed in a land battle
let landDeployments (combatant:HistoricCombatant) =
    combatant.Artillery.GetValueOrDefault() +
    combatant.Cavalry.GetValueOrDefault() +
    combatant.Infantry.GetValueOrDefault();

/// Summation of all units a side deployed in a battle
let forces side = navalDeployments side + landDeployments side
