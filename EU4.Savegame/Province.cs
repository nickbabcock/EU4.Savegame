using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pdoxcl2Sharp;

namespace EU4.Savegame
{
    public partial class Province : IParadoxRead, IParadoxWrite
    {
        public int Id { get; private set; }
        public IList<string> Buildings { get; set; }

        public Province(int id)
            : this()
        {
            Id = id;
            Buildings = new List<string>();
        }

        partial void unrecognizedToken(ParadoxParser parser, string token)
        {
#if THOROUGH_PARSING
            if (parser.ReadBool())
                Buildings.Add(token);
#endif
        }
    }
}
