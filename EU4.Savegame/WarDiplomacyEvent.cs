using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pdoxcl2Sharp;

namespace EU4.Savegame
{
    public enum WarDiplomacyType
    {
        AddAttacker,
        AddDefender,
        RemoveAttacker,
        RemoveDefender
    }

    public class WarDiplomacyEvent : HistoricEvent
    {
        private string country;
        private WarDiplomacyType type;

        public WarDiplomacyEvent(DateTime date, string country, WarDiplomacyType type)
            : base(date)
        {
            this.country = country;
            this.type = type;
        }

        public string Country
        {
            get { return this.country; }
            set { this.country = value; }
        }

        public WarDiplomacyType Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        public override void Write(ParadoxStreamWriter writer)
        {
            switch (this.type)
            {
                case WarDiplomacyType.AddAttacker: writer.WriteLine("add_attacker", this.country, ValueWrite.Quoted); break;
                case WarDiplomacyType.AddDefender: writer.WriteLine("add_defender", this.country, ValueWrite.Quoted); break;
                case WarDiplomacyType.RemoveAttacker: writer.WriteLine("rem_attacker", this.country, ValueWrite.Quoted); break;
                case WarDiplomacyType.RemoveDefender: writer.WriteLine("rem_defender", this.country, ValueWrite.Quoted); break;
            }
        }
    }
}
