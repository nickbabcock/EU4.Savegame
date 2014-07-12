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
        private bool readMagic = false;

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

        partial void unrecognizedToken(ParadoxParser parser, string token)
        {
            readMagic = !readMagic && token == "EU4txt";
            if (!readMagic)
                throw new ApplicationException("Unrecognized token: " + token);
        }
    }
}
