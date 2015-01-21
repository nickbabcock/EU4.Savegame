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
            ParadoxParser.Parse(stream, TopToken);
        }

        public Save(string filepath)
            : this()
        {
            using (var fs = File.OpenRead(filepath))
            {
                ParadoxParser.Parse(fs, TopToken);
            }
        }

        public void TopToken(ParadoxParser parser, string token)
        {
            if (readMagic)
                TokenCallback(parser, token);
            else if (!(readMagic = token == "EU4txt"))
            {
                if (token.StartsWith("EU4bin"))
                    throw new ApplicationException("Ironman saves are not " +
                        "supported at this time");
                throw new ApplicationException("First token must be EU4txt");
            }
        }
    }
}
