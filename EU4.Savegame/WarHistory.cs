using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class WarHistory : History<War>
    {
        protected override IHistory<War> InnerToken(ParadoxParser parser, string token, DateTime date)
        {
            switch (token)
            {
                case "name": return parser.Parse(new NameChange(date));
                case "war_goal": return parser.Parse(new WarGoal(date));
                case "add_attacker": return parser.Parse(new AddAttacker(date));
                case "add_defender": return parser.Parse(new AddDefender(date));
                case "rem_defender": return parser.Parse(new RemoveDefender(date));
                case "rem_attacker": return parser.Parse(new RemoveAttacker(date));
                case "battle": return parser.Parse(new BattleResult(date));
                default: throw new ApplicationException("Unrecognized token " + token);
            }
        }
    }

    public class AddAttacker : IHistory<War>
    {
        private readonly DateTime evt;
        public string Attacker { get; set; }
        public AddAttacker(DateTime evt)
        {
            this.evt = evt;
        }

        public void Apply(War obj, bool advance)
        {
            if (advance)
                obj.Attackers.Add(Attacker);
            else
                obj.Attackers.Remove(Attacker);
        }

        public DateTime EventDate { get { return evt; } }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            Attacker = token;
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("add_attacker", Attacker, ValueWrite.Quoted);
        }
    }

    public class RemoveAttacker : IHistory<War>
    {
        private readonly DateTime evt;
        public string Attacker { get; set; }
        public RemoveAttacker(DateTime evt)
        {
            this.evt = evt;
        }

        public void Apply(War obj, bool advance)
        {
            if (advance)
                obj.Attackers.Remove(Attacker);
            else
                obj.Attackers.Add(Attacker);
        }

        public DateTime EventDate { get { return evt; } }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            Attacker = token;
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("rem_attacker", Attacker, ValueWrite.Quoted);
        }
    }

    public class AddDefender : IHistory<War>
    {
        private readonly DateTime evt;
        public string Defender { get; set; }
        public AddDefender(DateTime evt)
        {
            this.evt = evt;
        }

        public void Apply(War obj, bool advance)
        {
            if (advance)
                obj.Defenders.Add(Defender);
            else
                obj.Defenders.Remove(Defender);
        }

        public DateTime EventDate { get { return evt; } }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            Defender = token;
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("add_defender", Defender, ValueWrite.Quoted);
        }
    }

    public class RemoveDefender : IHistory<War>
    {
        private readonly DateTime evt;
        public string Defender { get; set; }
        public RemoveDefender(DateTime evt)
        {
            this.evt = evt;
        }

        public void Apply(War obj, bool advance)
        {
            if (advance)
                obj.Defenders.Remove(Defender);
            else
                obj.Defenders.Add(Defender);
        }

        public DateTime EventDate { get { return evt; } }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            Defender = token;
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("rem_defender", Defender, ValueWrite.Quoted);
        }
    }

    public partial class BattleResult : IHistory<War>
    {
        private readonly DateTime evt;
        public BattleResult(DateTime evt)
        {
            this.evt = evt;
        }

        public void Apply(War obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }
    }

    public class NameChange : IHistory<War>
    {
        private readonly DateTime evt;
        public string Name { get; set; }
        public string OldValue { get; set; }

        public NameChange(DateTime evt)
        {
            this.evt = evt;
        }

        public void Apply(War obj, bool advance)
        {
            if (advance)
            {
                OldValue = obj.Name;
                obj.Name = Name;
            }
            else
            {
                obj.Name = OldValue;
            }
        }

        public DateTime EventDate { get { return evt; } }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            Name = token;
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("name", Name, ValueWrite.Quoted);
        }
    }
    public partial class WarGoal : IHistory<War>
    {
        private readonly DateTime evt;
        public WarGoal(DateTime evt)
        {
            this.evt = evt;
        }

        public void Apply(War obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }
    }

}
