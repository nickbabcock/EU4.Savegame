using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pdoxcl2Sharp;

namespace EU4.Savegame
{
    public class HistoricCombatant : IParadoxRead, IParadoxWrite
    {
        private int lightShip;
        private int heavyShip;
        private int transport;
        private int galley;
        private int losses;
        private string country;
        private int warGoal;
        private string commander;
        private int infantry;
        private int cavalry;
        private int artillery;

        public int LightShip
        {
            get { return this.lightShip; }
            set { this.lightShip = value; }
        }

        public int HeavyShip
        {
            get { return this.heavyShip; }
            set { this.heavyShip = value; }
        }

        public int Transport
        {
            get { return this.transport; }
            set { this.transport = value; }
        }

        public int Galley
        {
            get { return this.galley; }
            set { this.galley = value; }
        }

        public int Losses
        {
            get { return this.losses; }
            set { this.losses = value; }
        }

        public string Country
        {
            get { return this.country; }
            set { this.country = value; }
        }

        public int WarGoal
        {
            get { return this.warGoal; }
            set { this.warGoal = value; }
        }

        public string Commander
        {
            get { return this.commander; }
            set { this.commander = value; }
        }

        public int Infantry
        {
            get { return this.infantry; }
            set { this.infantry = value; }
        }

        public int Cavalry
        {
            get { return this.cavalry; }
            set { this.cavalry = value; }
        }

        public int Artillery
        {
            get { return this.artillery; }
            set { this.artillery = value; }
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            switch (token)
            {
                case "light_ship": this.lightShip = parser.ReadInt32(); break;
                case "heavy_ship": this.heavyShip = parser.ReadInt32(); break;
                case "galley": this.galley = parser.ReadInt32(); break;
                case "transport": this.transport = parser.ReadInt32(); break;
                case "infantry": this.infantry = parser.ReadInt32(); break;
                case "cavalry": this.cavalry = parser.ReadInt32(); break;
                case "artillery": this.artillery = parser.ReadInt32(); break;
                case "losses": this.losses = parser.ReadInt32(); break;
                case "country": this.country = parser.ReadString(); break;
                case "war_goal": this.warGoal = parser.ReadInt32(); break;
                case "commander": this.commander = parser.ReadString(); break;
            }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            var forces = new[] 
            { 
                Tuple.Create("cavalry", this.cavalry),
                Tuple.Create("infantry", this.infantry),
                Tuple.Create("artillery", this.artillery),
                Tuple.Create("galley", this.galley),
                Tuple.Create("light_ship", this.lightShip),
                Tuple.Create("heavy_ship", this.heavyShip),
                Tuple.Create("transport", this.transport)
            };

            foreach (var pair in forces.Where(x => x.Item2 != 0))
            {
                writer.WriteLine(pair.Item1, pair.Item2);
            }

            writer.WriteLine("losses", this.losses);
            writer.WriteLine("country", this.country, ValueWrite.Quoted);
            writer.WriteLine("war_goal", this.warGoal);
            writer.WriteLine("commander", this.commander, ValueWrite.Quoted);
        }
    }
}
