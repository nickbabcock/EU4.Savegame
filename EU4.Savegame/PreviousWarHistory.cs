using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pdoxcl2Sharp;

namespace EU4.Savegame
{
    public class PreviousWarHistory : History
    {
        private string name;
        private WarGoal warGoal;

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public WarGoal WarGoal
        {
            get { return this.warGoal; }
            set { this.warGoal = value; }
        }

        public override void TokenCallback(ParadoxParser parser, string token)
        {
            switch (token)
            {
                case "name": this.name = parser.ReadString(); break;
                case "war_goal": this.warGoal = parser.Parse(new WarGoal()); break;
                default: base.TokenCallback(parser, token); break;
            }
        }

        public override void Write(ParadoxStreamWriter writer)
        {
            if (!string.IsNullOrEmpty(this.name))
            {
                writer.WriteLine("name", this.name, ValueWrite.Quoted);
            }

            if (this.warGoal != null)
            {
                writer.Write("war_goal", this.warGoal);
            }

            base.Write(writer);
        }
    }
}
