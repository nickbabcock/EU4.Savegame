using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pdoxcl2Sharp;

namespace EU4.Savegame
{
    public class PreviousWar : War
    {
        public PreviousWar()
            : base(() => new PreviousWarHistory())
        {
        }
    }
}
