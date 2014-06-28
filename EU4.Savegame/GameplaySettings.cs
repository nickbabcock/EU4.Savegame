using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class GameplaySettings : IParadoxRead, IParadoxWrite
    {
        private IList<int> options;

        public void Write(ParadoxStreamWriter writer)
        {
            writer.Write("setgameplayoptions={ ");
            foreach (var option in options)
            {
                writer.Write(option);
                writer.Write(" ");
            }
            writer.WriteLine("}");
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            if (token == "setgameplayoptions")
            {
                options = parser.ReadIntList();
            }
        }
    }
}
