using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pdoxcl2Sharp;

namespace EU4.Savegame
{
    public class ActiveWar : War
    {
        private double? attackerScore;
        private double? defenderScore;
        private IList<string> demands;
        private IList<string> attackers;
        private IList<string> defenders;
        private string warGoalHeader;
        private WarGoal warGoal;
        private bool revolutionaryWar;

        public ActiveWar()
            : base(() => new History())
        {
            this.demands = new List<string>();
            this.attackers = new List<string>();
            this.defenders = new List<string>();
            this.revolutionaryWar = false;
        }

        public double AttackerScore
        {
            get
            {
                if (this.attackerScore.HasValue)
                {
                    return this.attackerScore.Value;
                }

                return -this.defenderScore.GetValueOrDefault();
            }

            set
            {
                if ((this.attackerScore.HasValue || !this.defenderScore.HasValue) && value >= 0)
                {
                    this.attackerScore = value;
                }
                else
                {
                    this.defenderScore = -value;
                    this.attackerScore = null;
                }
            }
        }

        public double DefenderScore
        {
            get
            {
                if (this.defenderScore.HasValue)
                {
                    return this.defenderScore.Value;
                }

                return -this.attackerScore.GetValueOrDefault();
            }

            set
            {
                if ((this.defenderScore.HasValue || !this.attackerScore.HasValue) && value >= 0)
                {
                    this.defenderScore = value;
                }
                else
                {
                    this.attackerScore = -value;
                    this.defenderScore = null;
                }
            }
        }

        public IList<string> Demands
        {
            get { return this.demands; }
            set { this.demands = value; }
        }

        public IList<string> Attackers
        {
            get { return this.attackers; }
            set { this.attackers = value; }
        }

        public IList<string> Defenders
        {
            get { return this.defenders; }
            set { this.defenders = value; }
        }

        public WarGoal WarGoal
        {
            get { return this.warGoal; }
            set { this.warGoal = value; }
        }

        public string WarGoalHeader
        {
            get { return this.warGoalHeader; }
            set { this.warGoalHeader = value; }
        }

        public bool RevolutionaryWar
        {
            get { return this.revolutionaryWar; }
            set { this.revolutionaryWar = value; }
        }

        public override void TokenCallback(ParadoxParser parser, string token)
        {
            switch (token)
            {
                case "attacker_score": this.attackerScore = parser.ReadDouble(); break;
                case "defender_score": this.defenderScore = parser.ReadDouble(); break;
                case "demand": parser.Parse((p, s) => { this.demands.Add(p.ReadString()); }); break;
                case "attacker": this.attackers.Add(parser.ReadString()); break;
                case "defender": this.defenders.Add(parser.ReadString()); break;
                case "revolutionary_war": this.revolutionaryWar = parser.ReadBool(); break;
                default: base.TokenCallback(parser, token); break;
            }
        }

        public override void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("name", this.Name, ValueWrite.Quoted);
            writer.Write("history", this.History);
            foreach (var attacker in this.attackers)
            {
                writer.WriteLine("attacker", attacker, ValueWrite.Quoted);
            }

            foreach (var defender in this.defenders)
            {
                writer.WriteLine("defender", defender, ValueWrite.Quoted);
            }

            writer.WriteLine("original_attacker", this.OriginalAttacker, ValueWrite.Quoted);
            writer.WriteLine("original_defender", this.OriginalDefender, ValueWrite.Quoted);
            if (this.revolutionaryWar == true)
            {
                writer.WriteLine("revolutionary_war", this.revolutionaryWar);
            }

            writer.WriteLine("action", this.Action);
            if (this.attackerScore.HasValue)
            {
                writer.WriteLine("attacker_score", this.attackerScore.Value);
            }
            else if (this.defenderScore.HasValue)
            {
                writer.WriteLine("defender_score", this.defenderScore.Value);
            }

            if (this.warGoal != null)
            {
                writer.Write(this.warGoalHeader, this.warGoal);
            }

            if (this.demands.Count > 0)
            {
                writer.Write(
                    "demand",
                    (w) =>
                    {
                        foreach (var demand in this.demands)
                        {
                            w.WriteLine("demand", demand, ValueWrite.Quoted);
                        }
                    });
            }

            writer.WriteLine("war_direction_quarter", this.WarDirectionQuarter);
            writer.WriteLine("war_direction_year", this.WarDirectionYear);
            writer.WriteLine("last_warscore_quarter", this.LastWarscoreQuarter);
            writer.WriteLine("last_warscore_year", this.LastWarscoreYear);
            writer.WriteLine("next_quarter_update", this.NextQuarterUpdate);
            writer.WriteLine("next_year_update", this.NextYearUpdate);
            writer.WriteLine("stalled_years", this.StalledYears);
        }

        public override void TokenNotFound(ParadoxParser parser, string token)
        {
            this.warGoalHeader = token;
            this.warGoal = parser.Parse(new WarGoal());
        }
    }
}
