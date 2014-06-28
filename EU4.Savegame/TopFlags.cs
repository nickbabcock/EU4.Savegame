using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class TopFlags : IEnumerable<Tuple<string, DateTime>>, IParadoxRead, IParadoxWrite
    {
        private IList<Tuple<string, DateTime>> flags;

        public TopFlags()
        {
            flags = new List<Tuple<string, DateTime>>();
        }

        public void Write(ParadoxStreamWriter writer)
        {
            foreach (var tuple in flags)
            {
                // The dates are unquoted which are unusual.
                writer.WriteLine(tuple.Item1, tuple.Item2.ToParadoxString());
            }
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            flags.Add(Tuple.Create(token, parser.ReadDateTime()));
        }

        public IEnumerator<Tuple<string, DateTime>> GetEnumerator()
        {
            return flags.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return flags.GetEnumerator();
        }
    }
}
