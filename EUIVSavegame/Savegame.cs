using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pdoxcl2Sharp;

namespace EU4.Savegame
{
    public class Savegame : IParadoxRead, IParadoxWrite 
    {
        private int currentParsedProvince;
        private string currentParsedProvinceStr;
        public Savegame(Stream stream)
        {
            this.Provinces = new ProvinceCollection();
            this.currentParsedProvince = 1;
            this.currentParsedProvinceStr = this.currentParsedProvince.ToString();
            Pdoxcl2Sharp.ParadoxParser.Parse(stream, this);
        }

        public string Player { get; set; }
        public ProvinceCollection Provinces { get; private set; }

        public void Save(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            if (token == this.currentParsedProvinceStr && parser.CurrentIndent == 0)
            {
                var newProv = new SaveProvince(this.currentParsedProvince++);
                this.Provinces.Add(parser.Parse<SaveProvince>(newProv));
                this.currentParsedProvinceStr = this.currentParsedProvince.ToString();
            }
        }

        public void Write(ParadoxSaver writer)
        {
            throw new NotImplementedException();
        }
    }
}
