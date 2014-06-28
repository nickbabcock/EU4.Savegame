using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class LedgerDataCollection : IEnumerable<LedgerData>, IParadoxRead, IParadoxWrite
    {
        private IList<LedgerData> data;

        public LedgerDataCollection()
        {
            data = new List<LedgerData>();
        }

        public void Write(ParadoxStreamWriter writer)
        {
            foreach (var d in data)
            {
                writer.Write("ledger_data", d);
            }
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            if (token == "ledger_data")
            {
                data.Add(parser.Parse(new LedgerData()));
            }
        }

        public IEnumerator<LedgerData> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return data.GetEnumerator();
        }
    }
}
