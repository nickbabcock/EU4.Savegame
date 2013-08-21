using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pdoxcl2Sharp;

namespace EU4.Savegame
{
    public class Savegame
    {
        public Savegame(Stream stream)
        {
            this.Init(stream);
        }

        public Savegame(string filepath)
        {
            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                this.Init(fs);
            }
        }

        public string Player { get; set; }
        public ProvinceCollection Provinces { get; private set; }
        public IList<LedgerData> NationSizeStatistics { get; private set; }
        public IList<LedgerData> IncomeStatistics { get; private set; }
        public IList<LedgerData> ScoreStatistics { get; private set; }
        public IList<LedgerData> InflationStatistics { get; private set; }
        public IList<PreviousWar> PreviousWars { get; private set; }
        public IList<ActiveWar> ActiveWars { get; private set; }

        public void Save(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void Save(string filepath)
        {
            throw new NotImplementedException();
        }

        private void Init(Stream data)
        {
            this.Provinces = new ProvinceCollection();
            this.NationSizeStatistics = new List<LedgerData>();
            this.IncomeStatistics = new List<LedgerData>();
            this.ScoreStatistics = new List<LedgerData>();
            this.InflationStatistics = new List<LedgerData>();
            this.PreviousWars = new List<PreviousWar>();
            this.ActiveWars = new List<ActiveWar>();
            ParadoxParser.Parse(data, new SavegameParser(this));
        }
    }
}
