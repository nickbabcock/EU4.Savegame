using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Pdoxcl2Sharp;

namespace EU4.Savegame
{
    public class Savegame
    {
        public Savegame(Stream stream)
        {
            this.Init(stream);
        }

        public Savegame(string filepath)
        {
            using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                this.Init(fs);
            }
        }

        public string Player { get; set; }
        public ProvinceCollection Provinces { get; private set; }

        public void Save(Stream stream)
        {
            throw new NotImplementedException();
        }

        public void Save(string filepath)
        {
            throw new NotImplementedException();
        }

        private void Init(Stream data)
        {
            this.Provinces = new ProvinceCollection();
            ParadoxParser.Parse(data, new SavegameParser(this));
        }
    }
}
