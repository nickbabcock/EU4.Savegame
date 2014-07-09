using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class War : IParadoxRead, IParadoxWrite
    {
        public string Name { get; set; }
        public WarHistory History { get; set; }
        public IList<string> Attackers { get; set; }
        public IList<string> Defenders { get; set; }
        public string Target { get; set; }
        public string OriginalAttacker { get; set; }
        public string OriginalDefender { get; set; }
        public int? Independence { get; set; }
        public bool? RevolutionaryWar { get; set; }
        public bool? Succession { get; set; }
        public DateTime Action { get; set; }
        public double? AttackerScore { get; set; }
        public double? DefenderScore { get; set; }
        public Demands Demands { get; set; }
        public int WarDirectionQuarter { get; set; }
        public int WarDirectionYear { get; set; }
        public int LastWarscoreQuarter { get; set; }
        public int LastWarscoreYear { get; set; }
        public DateTime NextQuarterUpdate { get; set; }
        public DateTime NextYearUpdate { get; set; }
        public int StalledYears { get; set; }
        public WarGoal WarGoal { get; set; }

        public War()
        {
            Attackers = new List<string>();
            Defenders = new List<string>();

        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            switch (token)
            {
                case "name": Name = parser.ReadString(); break;
                case "history": History = parser.Parse(new WarHistory()); break;
                case "attacker": Attackers.Add(parser.ReadString()); break;
                case "defender": Defenders.Add(parser.ReadString()); break;
                case "target": Target = parser.ReadString(); break;
                case "original_attacker": OriginalAttacker = parser.ReadString(); break;
                case "original_defender": OriginalDefender = parser.ReadString(); break;
                case "independence": Independence = parser.ReadInt32(); break;
                case "revolutionary_war": RevolutionaryWar = parser.ReadBool(); break;
                case "succession": Succession = parser.ReadBool(); break;
                case "action": Action = parser.ReadDateTime(); break;
                case "attacker_score": AttackerScore = parser.ReadDouble(); break;
                case "defender_score": DefenderScore = parser.ReadDouble(); break;
                case "demand": Demands = parser.Parse(new Demands()); break;
                case "war_direction_quarter": WarDirectionQuarter = parser.ReadInt32(); break;
                case "war_direction_year": WarDirectionYear = parser.ReadInt32(); break;
                case "last_warscore_quarter": LastWarscoreQuarter = parser.ReadInt32(); break;
                case "last_warscore_year": LastWarscoreYear = parser.ReadInt32(); break;
                case "next_quarter_update": NextQuarterUpdate = parser.ReadDateTime(); break;
                case "next_year_update": NextYearUpdate = parser.ReadDateTime(); break;
                case "stalled_years": StalledYears = parser.ReadInt32(); break;
                default:
                    if (!parser.NextIsBracketed())
                        throw new ApplicationException("Unrecognized token: " + token);
                    WarGoal = parser.Parse(new WarGoal(token));
                    break;
            }

        }

        public void Write(ParadoxStreamWriter writer)
        {
            if (Name != null)
            {
                writer.WriteLine("name", Name, ValueWrite.Quoted);
            }
            if (History != null)
            {
                writer.Write("history", History);
            }
            foreach (var val in Attackers)
            {
                writer.WriteLine("attacker", val, ValueWrite.Quoted);
            }
            foreach (var val in Defenders)
            {
                writer.WriteLine("defender", val, ValueWrite.Quoted);
            }
            if (Target != null)
            {
                writer.WriteLine("target", Target, ValueWrite.Quoted);
            }
            if (OriginalAttacker != null)
            {
                writer.WriteLine("original_attacker", OriginalAttacker, ValueWrite.Quoted);
            }
            if (OriginalDefender != null)
            {
                writer.WriteLine("original_defender", OriginalDefender, ValueWrite.Quoted);
            }
            if (Independence.HasValue)
            {
                writer.WriteLine("independence", Independence.Value);
            }
            if (RevolutionaryWar.HasValue)
            {
                writer.WriteLine("revolutionary_war", RevolutionaryWar.Value);
            }
            if (Succession.HasValue)
            {
                writer.WriteLine("succession", Succession.Value);
            }
            writer.WriteLine("action", Action);
            if (AttackerScore.HasValue)
            {
                writer.WriteLine("attacker_score", AttackerScore.Value);
            }
            if (DefenderScore.HasValue)
            {
                writer.WriteLine("defender_score", DefenderScore.Value);
            }
            if (WarGoal != null)
            {
                writer.Write(WarGoal.Header, WarGoal);
            }
            if (Demands != null)
            {
                writer.Write("demand", Demands);
            }
            writer.WriteLine("war_direction_quarter", WarDirectionQuarter);
            writer.WriteLine("war_direction_year", WarDirectionYear);
            writer.WriteLine("last_warscore_quarter", LastWarscoreQuarter);
            writer.WriteLine("last_warscore_year", LastWarscoreYear);
            writer.WriteLine("next_quarter_update", NextQuarterUpdate);
            writer.WriteLine("next_year_update", NextYearUpdate);
            writer.WriteLine("stalled_years", StalledYears);

        }
    }
}
