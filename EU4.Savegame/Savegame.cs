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

        public void Save(string old, string filepath)
        {
            using (var open = new FileStream(old, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var sr = new StreamReader(open, Encoding.GetEncoding(Globals.WindowsCodePage)))
            using (var fs = new FileStream(filepath, FileMode.CreateNew, FileAccess.Write, FileShare.ReadWrite))
            {
                var writer = new ParadoxSaver(fs);
                for (string line; (line = sr.ReadLine()) != "active_war="; writer.Write(line))
                    ;

                foreach (var war in this.ActiveWars)
                {
                    writer.Write("active_war", war);
                }
            }
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
