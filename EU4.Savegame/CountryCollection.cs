using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class CountryCollection : IEnumerable<Country>, IParadoxRead, IParadoxWrite
    {
        private IList<Country> countries;

        private const int CountriesEstimate = 512;


        public CountryCollection()
        {
            countries = new List<Country>(CountriesEstimate);
        }

        public void Write(ParadoxStreamWriter writer)
        {
            foreach (var country in countries)
            {
                writer.Write(country.Name, country);
            }
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            countries.Add(parser.Parse(new Country(token)));
        }

        public IEnumerator<Country> GetEnumerator()
        {
            return countries.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return countries.GetEnumerator();
        }
    }
}
