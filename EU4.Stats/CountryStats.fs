module EU4.Stats.CountryStats

open EU4.Savegame;
open EU4.Stats.Types

type CountryRankings = {
    rank: int; name: string; score: double;
    dip: double; adm: double; mil: double;
}
