using Pdoxcl2Sharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class ProvinceCollection : IEnumerable<Province>, IParadoxRead, IParadoxWrite
    {
        private IList<Province> provinces;

        private const int ProvincesEstimate = 2048;

        public ProvinceCollection()
        {
            this.provinces = new List<Province>(ProvincesEstimate);
        }

        public void Add(Province prov)
        {
            this.provinces.Add(prov);
        }

        public IEnumerator<Province> GetEnumerator()
        {
            return this.provinces.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.provinces.GetEnumerator();
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            this.provinces.Add(parser.Parse(new Province(-int.Parse(token))));
        }

        public void Write(ParadoxStreamWriter writer)
        {
            foreach (var prov in provinces)
            {
                writer.Write((-prov.Id).ToString(), prov);
            }
        }
    }
}
