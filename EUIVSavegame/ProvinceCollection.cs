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
        private IList<SaveProvince> provinces;

        public ProvinceCollection()
        {
            const int ProvincesEstimate = 2048;
            this.provinces = new List<SaveProvince>(ProvincesEstimate);
        }

        public SaveProvince Get(int provinceId)
        {
            if (provinceId - 1 < this.provinces.Count && this.provinces[provinceId - 1].Id == provinceId)
            {
                return this.provinces[provinceId - 1];
            }

            for (int i = 0; i < this.provinces.Count; i++)
            {
                if (this.provinces[i].Id == provinceId)
                {
                    return this.provinces[i];
                }
            }

            throw new ArgumentException(string.Format("Province with an Id of {0} was not found", provinceId), "id");
        }

        public void Add(SaveProvince prov)
        {
            // Optimize for insertion at the end of the list
            if (this.provinces.Count == 0 || prov.Id > this.provinces[this.provinces.Count - 1].Id)
            {
                this.provinces.Add(prov);
            }
            else
            {
                // Insert the province so that all subsequent provinces
                // have a greater Id.
                for (int i = 0; i < this.provinces.Count; i++)
                {
                    if (this.provinces[i].Id > prov.Id)
                    {
                        this.provinces.Insert(i, prov);
                        break;
                    }
                    else if (this.provinces[i].Id == prov.Id)
                    {
                        throw new ArgumentException("Province already added to collection", "prov");
                    }
                }
            }
        }

        public IEnumerator<SaveProvince> GetEnumerator()
        {
            return this.provinces.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.provinces.GetEnumerator();
        }
    }
}
