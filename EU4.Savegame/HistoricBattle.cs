using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pdoxcl2Sharp;

namespace EU4.Savegame
{
    public class HistoricBattle : HistoricEvent, IParadoxRead
    {
        private string name;
        private int location;
        private bool result;
        private HistoricCombatant attacker;
        private HistoricCombatant defender;

        public HistoricBattle(DateTime eventDate)
            : base(eventDate)
        {
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public int Location
        {
            get { return this.location; }
            set { this.location = value; }
        }

        public bool Result
        {
            get { return this.result; }
            set { this.result = value; }
        }

        public HistoricCombatant Attacker
        {
            get { return this.attacker; }
            set { this.attacker = value; }
        }

        public HistoricCombatant Defender
        {
            get { return this.defender; }
            set { this.defender = value; }
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            switch (token)
            {
                case "name": this.name = parser.ReadString(); break;
                case "location": this.location = parser.ReadInt32(); break;
                case "result": this.result = parser.ReadBool(); break;
                case "attacker": this.attacker = parser.Parse(new HistoricCombatant()); break;
                case "defender": this.defender = parser.Parse(new HistoricCombatant());  break;
            }
        }

        public override void Write(ParadoxStreamWriter writer)
        {
            writer.Write("battle", this.WriteInnerInner);
        }

        private void WriteInnerInner(ParadoxStreamWriter writer)
        {
            writer.WriteLine("name", this.name, ValueWrite.Quoted);
            writer.WriteLine("location", this.location);
            writer.WriteLine("result", this.result);
            writer.Write("attacker", this.attacker);
            writer.Write("defender", this.defender);
        }
    }
}
