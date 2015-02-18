using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EU4.Stats.Web
{
    public class StatsModel
    {
        public string Player { get; set; }
        public string Players { get; set; }
        public IEnumerable<CountryStats.PlayerStats> PlayerCountries { get; set; }
        public DateTime Date { get; set; }
        public double Manpower { get; set; }
        public double PotentialManpower { get; set; }
        public int RegimentCount { get; set; }
        public double RegimentSum { get; set; }
        public int ShipCount { get; set; }
        public double ShipSum { get; set; }
        public IEnumerable<WarStats.LeaderReport> LeaderReport { get; set; }
        public IEnumerable<WarStats.WarReport> BiggestNavalWars { get; set; }
        public IEnumerable<WarStats.WarReport> BiggestLandWars { get; set; }
        public IEnumerable<WarStats.BiggestBattles> BiggestLandBattles { get; set; }
        public IEnumerable<WarStats.BiggestBattles> BiggestNavalBattles { get; set; }
        public IEnumerable<WarStats.BiggestRivalries> BiggestRivalries { get; set; }
        public LedgerStats.Correlations LedgerCorrelations { get; set; }
        public IEnumerable<CountryStats.CountryRankings> ScoreStats { get; set; }
        public IEnumerable<Tuple<string, int, int>> Debt { get; set; }
        public IEnumerable<TradeStats.PowerStats> TradePower { get; set; }
        public IEnumerable<WarStats.CountryMilitary> CountryMilitaryStats { get; set; }
    }
}
