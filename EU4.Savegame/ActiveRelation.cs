using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public partial class ActiveRelation
    {
        public string Country { get; private set; }

        public ActiveRelation(string country)
            : this()
        {
            Country = country;
        }
    }
}
