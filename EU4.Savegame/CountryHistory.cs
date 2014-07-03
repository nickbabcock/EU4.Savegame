using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class CountryHistory : IParadoxRead, IParadoxWrite
    {
        public void Write(ParadoxStreamWriter writer)
        {
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
        }
    }
}
