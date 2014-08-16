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
                case "succession": return new TargetChange(date, parser.ReadString());
                case "name": return new NameChange(date, parser.ReadString());
                case "war_goal": return parser.Parse(new WarGoal(date));
                case "add_attacker": return new AddAttacker(date, parser.ReadString());
                case "add_defender": return new AddDefender(date, parser.ReadString());
                case "rem_defender": return new RemoveDefender(date, parser.ReadString());
                case "rem_attacker": return new RemoveAttacker(date, parser.ReadString());
                case "battle": return parser.Parse(new BattleResult(date));
#if THOROUGH_PARSING
                default: throw new ApplicationException("Unrecognized token " + token);
#endif
            }

            return null;
        }
    }

    public class AddAttacker : IHistory<War>
    {
        private readonly DateTime evt;
        public string Attacker { get; set; }
        public AddAttacker(DateTime evt, string attacker)
        {
            this.evt = evt;
            this.Attacker = attacker;
        }

        public void Apply(War obj, bool advance)
        {
            if (advance)
                obj.Attackers.Add(Attacker);
            else
                obj.Attackers.Remove(Attacker);
        }

        public DateTime EventDate { get { return evt; } }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("add_attacker", Attacker, ValueWrite.Quoted);
        }
    }

    public class RemoveAttacker : IHistory<War>
    {
        private readonly DateTime evt;
        public string Attacker { get; set; }
        public RemoveAttacker(DateTime evt, string attacker)
        {
            this.evt = evt;
            this.Attacker = attacker;
        }

        public void Apply(War obj, bool advance)
        {
            if (advance)
                obj.Attackers.Remove(Attacker);
            else
                obj.Attackers.Add(Attacker);
        }

        public DateTime EventDate { get { return evt; } }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("rem_attacker", Attacker, ValueWrite.Quoted);
        }
    }

    public class AddDefender : IHistory<War>
    {
        private readonly DateTime evt;
        public string Defender { get; set; }
        public AddDefender(DateTime evt, string defender)
        {
            this.evt = evt;
            this.Defender = defender;
        }

        public void Apply(War obj, bool advance)
        {
            if (advance)
                obj.Defenders.Add(Defender);
            else
                obj.Defenders.Remove(Defender);
        }

        public DateTime EventDate { get { return evt; } }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("add_defender", Defender, ValueWrite.Quoted);
        }
    }

    public class RemoveDefender : IHistory<War>
    {
        private readonly DateTime evt;
        public string Defender { get; set; }
        public RemoveDefender(DateTime evt, string defender)
        {
            this.evt = evt;
            this.Defender = defender;
        }

        public void Apply(War obj, bool advance)
        {
            if (advance)
                obj.Defenders.Remove(Defender);
            else
                obj.Defenders.Add(Defender);
        }

        public DateTime EventDate { get { return evt; } }

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

        public NameChange(DateTime evt, string name)
        {
            this.evt = evt;
            this.Name = name;
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

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("name", Name, ValueWrite.Quoted);
        }
    }

    public class TargetChange : IHistory<War>
    {
        private readonly DateTime evt;
        public string Target { get; set; }
        public string OldValue { get; set; }

        public TargetChange(DateTime evt, string succession)
        {
            this.evt = evt;
            this.Target = succession;
        }

        public void Apply(War obj, bool advance)
        {
            if (advance)
            {
                OldValue = obj.Name;
                obj.Target = Target;
            }
            else
            {
                obj.Target = OldValue;
            }
        }

        public DateTime EventDate { get { return evt; } }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("succession", Target, ValueWrite.Quoted);
        }
    }

    public partial class WarGoal : IHistory<War>
    {
        public string Header { get; set; }
        private readonly DateTime evt;
        public WarGoal(DateTime evt)
        {
            this.evt = evt;
        }

        public WarGoal(string name)
        {
            Header = name;
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
