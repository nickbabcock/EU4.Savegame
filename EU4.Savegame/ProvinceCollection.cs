using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class ProvinceCollection : IEnumerable<SaveProvince>
    {
        // This is a sorted set of provinces based on Ids.
        // It tries to take advantage of provinces that have
        // Ids that are monotomically increasing; however it
        // still works if there gaps in province Ids
        private SortedList<int, SaveProvince> provinces;

        public ProvinceCollection()
        {
            const int ProvincesEstimate = 2048;
            this.provinces = new SortedList<int, SaveProvince>(ProvincesEstimate);
        }

        public SaveProvince Get(int provinceId)
        {
            if (provinceId - 1 < this.provinces.Count && this.provinces.Values[provinceId - 1].Id == provinceId)
            {
                return this.provinces.Values[provinceId - 1];
            }
            else if (this.provinces.ContainsKey(provinceId))
            {
                return this.provinces[provinceId];
            }

            throw new ArgumentException(string.Format("Province with an Id of {0} was not found", provinceId), "id");
        }

        public void Add(SaveProvince prov)
        {
            this.provinces.Add(prov.Id, prov);
        }

        public IEnumerator<SaveProvince> GetEnumerator()
        {
            return this.provinces.Values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.provinces.Values.GetEnumerator();
        }
    }
}
