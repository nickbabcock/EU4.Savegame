module EU4.Stats.CountryStats

open EU4.Savegame;
open EU4.Stats.Types

type CountryRankings = {
    rank: int; name: string; score: double;
    dip: double; adm: double; mil: double;
}

type PlayerStats = {
    name : string; player : string; treasury : float; prestige : float;
    income: float;
    inflation : double; stability : float;
    cities : int; colonies : int; armyTradition : float;
    navalForces : seq<string>; landForces : seq<string>;
    baseTax : float;
    admTech : byte; dipTech : byte; milTech : byte;
    navyTradition : float; mercantilism : float;
    cultures : seq<string>; religion: string;
    government : string;
    warExhaustion : float;
    manpower : float; maxManpower : float;
    colonists : int; missionaries : int; diplomats : int; merchants : int;
    marriages : seq<string>;
    vassals : seq<string>;
    allies : seq<string>;
    advisors : seq<Advisor>;
    buildings : seq<string * int>
}