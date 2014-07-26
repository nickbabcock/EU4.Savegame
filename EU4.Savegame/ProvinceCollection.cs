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
        private IList<Province> provinces = new List<Province>(ProvincesEstimate);

        // Last checked in 1.7 there were 2025 provinces, we initialize our list
        // of provinces to the next highest power of two so that the list
        // doesn't need to resize as many times.
        private const int ProvincesEstimate = 2048;

        /// <summary>
        /// Adds a given province to the collection and ensures that it
        /// follows suit with an increasing id
        /// </summary>
        /// <param name="prov">Province to add to the collection</param>
        /// <exception cref="ApplicationException">
        /// If the added province doesn't have the expected id
        /// </exception>
        public void Add(Province prov)
        {
            provinces.Add(prov);
            if (prov.Id != provinces.Count)
                throw new ApplicationException("Added province with an id of "
                    + prov.Id + " but expected a province with an id of "
                    + provinces.Count);
        }

        /// <summary>
        /// Returns a province with the given province id in O(1) time
        /// </summary>
        public Province Get(int provinceId)
        {
            return provinces[provinceId - 1];
        }

        public IEnumerator<Province> GetEnumerator()
        {
            return provinces.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return provinces.GetEnumerator();
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            provinces.Add(parser.Parse(new Province(-int.Parse(token))));
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
