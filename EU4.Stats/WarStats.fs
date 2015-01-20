module EU4.Stats.WarStats

open EU4.Savegame
open EU4.Stats.Types
open System

type LeaderReport = {
    leader : Leader; forces: int; losses: int; faced: int; kills: int;
    battles: int; wins: int; country: string;
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

type BiggestBattles = {
    warName : string; battleName : string; battleDate : DateTime;
    attacker : string; defender : string;
    forces1 : int; losses1 : int; forces2 : int; losses2 : int; won : bool
}

type BattleReport = {
    commander1 : Leader; country1 : Country;
    commander2 : Leader; country2 : Country;
    side1 : HistoricCombatant; side2 : HistoricCombatant;
    won : bool
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
