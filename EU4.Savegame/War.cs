using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public partial class War : IParadoxRead, IParadoxWrite
    {
        public WarGoal WarGoal { get; set; }

        partial void unrecognizedToken(ParadoxParser parser, string token)
        {
            if (!parser.NextIsBracketed())
                throw new ApplicationException("Unrecognized token: " + token);
            WarGoal = parser.Parse(new WarGoal(token));
        }
    }
}
