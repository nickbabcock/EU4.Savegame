using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;

namespace EU4.Savegame
{
    public class TransferCollection : IParadoxRead, IParadoxWrite, IEnumerable<Tuple<String, double>>
    {
        IList<Tuple<string, double>> transfers = new List<Tuple<string, double>>();

        public void Write(ParadoxStreamWriter writer)
        {
            foreach (var pair in transfers)
            {
                writer.WriteLine(pair.Item1, pair.Item2);
            }
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            transfers.Add(Tuple.Create(token, parser.ReadDouble()));
        }

        public IEnumerator<Tuple<string, double>> GetEnumerator()
        {
            return transfers.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return transfers.GetEnumerator();
        }
    }
}
