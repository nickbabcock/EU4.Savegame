using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pdoxcl2Sharp;

namespace EU4.Savegame
{
    public partial class Save
    {
        public Save(Stream stream)
            : this()
        {
            ParadoxParser.Parse(stream, TokenCallback);
        }

        public Save(string filepath)
            : this()
        {
            using (var fs = File.OpenRead(filepath))
            {
                ParadoxParser.Parse(fs, TokenCallback);
            }
        }
    }
}
