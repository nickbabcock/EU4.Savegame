using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class ActiveRelationCollection : IEnumerable<ActiveRelation>, IParadoxRead, IParadoxWrite
    {
        private IList<ActiveRelation> relations;

        public ActiveRelationCollection()
        {
            relations = new List<ActiveRelation>();
        }

        public void Write(ParadoxStreamWriter writer)
        {
            foreach (var relation in relations)
            {
                writer.Write(relation.Country, relation);
            }
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            relations.Add(parser.Parse(new ActiveRelation(token)));
        }

        public IEnumerator<ActiveRelation> GetEnumerator()
        {
            return relations.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return relations.GetEnumerator();
        }
    }
}
