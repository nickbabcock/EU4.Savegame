using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class ProvinceHistory : History<Province>
    {
        protected override IHistory<Province> InnerToken(ParadoxParser parser, string token, DateTime date)
        {
            if (token == "controller" || token == "advisor" || token == "revolt")
                parser.Parse((p, s) => { });
            else
                parser.ReadString();
            return null;
        }
    }
}
