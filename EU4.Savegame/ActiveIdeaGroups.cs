using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class ActiveIdeaGroups : IEnumerable<Tuple<string, int>>, IParadoxRead, IParadoxWrite
    {
        private IList<Tuple<string, int>> ideas;

        public ActiveIdeaGroups()
        {
            ideas = new List<Tuple<string, int>>();
        }

        public void Write(ParadoxStreamWriter writer)
        {
            foreach (var tuple in ideas)
            {
                writer.WriteLine(tuple.Item1, tuple.Item2);
            }
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            ideas.Add(Tuple.Create(token, parser.ReadInt32()));
        }

        public IEnumerator<Tuple<string, int>> GetEnumerator()
        {
            return ideas.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ideas.GetEnumerator();
        }
    }
}
