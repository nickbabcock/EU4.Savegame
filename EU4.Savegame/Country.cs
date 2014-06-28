using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public partial class Country
    {
        public string Name { get; private set; }

        public Country(string name)
            : this()
        {
            Name = name;
        }
    }
}
