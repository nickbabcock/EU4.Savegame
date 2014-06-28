using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public partial class Religion
    {
        public string Name { get; private set; }

        public Religion(string name)
            : this()
        {
            Name = name;
        }
    }
}
