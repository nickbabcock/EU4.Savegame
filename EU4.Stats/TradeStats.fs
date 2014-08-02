module EU4.Stats.TradeStats

open EU4.Stats.Types

type PowerStats = {
    country: string; lightShips: int; shipPower: double;
    power: double; provincePower: double; money: double; total: double;
}
