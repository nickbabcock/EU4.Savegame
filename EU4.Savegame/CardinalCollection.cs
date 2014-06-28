using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class CardinalCollection : IEnumerable<Cardinal>, IParadoxRead, IParadoxWrite
    {
        private IList<Cardinal> cardinals;

        public CardinalCollection()
        {
            cardinals = new List<Cardinal>();
        }

        public void Write(ParadoxStreamWriter writer)
        {
            foreach (var cardinal in cardinals)
            {
                writer.Write("cardinal", cardinal);
            }
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            if (token == "cardinal")
            {
                cardinals.Add(parser.Parse(new Cardinal()));
            }
        }

        public IEnumerator<Cardinal> GetEnumerator()
        {
            return cardinals.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return cardinals.GetEnumerator();
        }
    }
}
