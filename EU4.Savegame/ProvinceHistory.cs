using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class ProvinceHistory : IParadoxRead, IParadoxWrite
    {
        public void TokenCallback(ParadoxParser parser, string token)
        {
        }

        public void Write(ParadoxStreamWriter writer)
        {
        }
    }
}
