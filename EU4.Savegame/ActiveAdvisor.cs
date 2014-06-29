using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public partial class ActiveAdvisor
    {
        public string Country { get; private set; }

        public ActiveAdvisor(string country)
            : this()
        {
            Country = country;
        }
    }
}
