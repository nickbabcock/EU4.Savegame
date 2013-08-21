using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pdoxcl2Sharp;

namespace EU4.Savegame
{
    public abstract class War : IParadoxRead, IParadoxWrite
    {
        private readonly Func<History> produceHistory;
        private string name;
        private string originalAttacker;
        private string originalDefender;
        private DateTime action;
        private int warDirectionQuarter;
        private int warDirectionYear;
        private int lastWarscoreQuarter;
        private int lastWarscoreYear;
        private DateTime nextQuarterUpdate;
        private DateTime nextYearUpdate;
        private int stalledYears;
        private History history;
        
        protected War(Func<History> produceHistory)
        {
            this.produceHistory = produceHistory;
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string OriginalAttacker
        {
            get { return this.originalAttacker; }
            set { this.originalAttacker = value; }
        }

        public string OriginalDefender
        {
            get { return this.originalDefender; }
            set { this.originalDefender = value; }
        }

        public DateTime Action
        {
            get { return this.action; }
            set { this.action = value; }
        }

        public int WarDirectionQuarter
        {
            get { return this.warDirectionQuarter; }
            set { this.warDirectionQuarter = value; }
        }

        public int WarDirectionYear
        {
            get { return this.warDirectionYear; }
            set { this.warDirectionYear = value; }
        }

        public int LastWarscoreQuarter
        {
            get { return this.lastWarscoreQuarter; }
            set { this.lastWarscoreQuarter = value; }
        }

        public int LastWarscoreYear
        {
            get { return this.lastWarscoreYear; }
            set { this.lastWarscoreYear = value; }
        }

        public DateTime NextQuarterUpdate
        {
            get { return this.nextQuarterUpdate; }
            set { this.nextQuarterUpdate = value; }
        }

        public DateTime NextYearUpdate
        {
            get { return this.nextYearUpdate; }
            set { this.nextYearUpdate = value; }
        }

        public int StalledYears
        {
            get { return this.stalledYears; }
            set { this.stalledYears = value; }
        }

        public History History
        {
            get { return this.history; }
            set { this.history = value; }
        }

        public virtual void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("name", this.name, ValueWrite.Quoted);
            writer.Write("history", this.history);
            writer.WriteLine("original_attacker", this.originalAttacker, ValueWrite.Quoted);
            writer.WriteLine("original_defender", this.originalDefender, ValueWrite.Quoted);
            writer.WriteLine("action", this.action);
            writer.WriteLine("war_direction_quarter", this.warDirectionQuarter);
            writer.WriteLine("war_direction_year", this.warDirectionYear);
            writer.WriteLine("last_warscore_quarter", this.lastWarscoreQuarter);
            writer.WriteLine("last_warscore_year", this.lastWarscoreYear);
            writer.WriteLine("next_quarter_update", this.nextQuarterUpdate);
            writer.WriteLine("next_year_update", this.nextYearUpdate);
            writer.WriteLine("stalled_years", this.stalledYears);
        }

        public virtual void TokenCallback(ParadoxParser parser, string token)
        {
            switch (token)
            {
                case "name": this.name = parser.ReadString(); break;
                case "history": this.history = parser.Parse(this.produceHistory()); break;
                case "original_attacker": this.originalAttacker = parser.ReadString(); break;
                case "original_defender": this.originalDefender = parser.ReadString(); break;
                case "action": this.action = parser.ReadDateTime(); break;
                case "war_direction_quarter": this.warDirectionQuarter = parser.ReadInt32(); break;
                case "war_direction_year": this.warDirectionYear = parser.ReadInt32(); break;
                case "last_warscore_quarter": this.lastWarscoreQuarter = parser.ReadInt32(); break;
                case "last_warscore_year": this.lastWarscoreYear = parser.ReadInt32(); break;
                case "next_quarter_update": this.nextQuarterUpdate = parser.ReadDateTime(); break;
                case "next_year_update": this.nextYearUpdate = parser.ReadDateTime(); break;
                case "stalled_years": this.stalledYears = parser.ReadInt32(); break;
                default: this.TokenNotFound(parser, token); break;
            }
        }

        public virtual void TokenNotFound(ParadoxParser parser, string token)
        {
        }

        public IEnumerable<HistoricBattle> GetBattles()
        {
            return this.history.OfType<HistoricBattle>();
        }
    }
}
