using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pdoxcl2Sharp;

namespace EU4.Savegame
{
    public class WarGoal : IParadoxRead, IParadoxWrite
    {
        private string type;
        private string casusBelli;
        private int? province;

        public string Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        public string CasusBelli
        {
            get { return this.casusBelli; }
            set { this.casusBelli = value; }
        }

        public int? Province
        {
            get { return this.province; }
            set { this.province = value; }
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            switch (token)
            {
                case "type": this.type = parser.ReadString(); break;
                case "casus_belli": this.casusBelli = parser.ReadString(); break;
                case "province": this.province = parser.ReadInt32(); break;
            }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("type", this.type, ValueWrite.Quoted);
            if (this.province.HasValue)
            {
                writer.WriteLine("province", this.province.Value);
            }

            writer.WriteLine("casus_belli", this.casusBelli, ValueWrite.Quoted);
        }
    }
}
