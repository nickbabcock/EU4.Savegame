using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class PendingEvents : IEnumerable<Tuple<string, Scope>>, IParadoxRead, IParadoxWrite
    {
        public IList<Tuple<string, Scope>> events = new List<Tuple<string, Scope>>();

        public void Write(ParadoxStreamWriter writer)
        {
            foreach (var val in events)
                writer.Write(val.Item1, val.Item2);
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            events.Add(Tuple.Create(token, parser.Parse(new Scope())));
        }

        public IEnumerator<Tuple<string, Scope>> GetEnumerator()
        {
            return events.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return events.GetEnumerator();
        }
    }
}
