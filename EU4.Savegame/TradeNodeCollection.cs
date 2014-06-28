using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class TradeNodeCollection : IEnumerable<TradeNode>, IParadoxRead, IParadoxWrite
    {
        private IList<TradeNode> nodes;

        public TradeNodeCollection()
        {
            nodes = new List<TradeNode>();
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            if (token == "node")
            {
                nodes.Add(parser.Parse(new TradeNode()));
            }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            foreach (var node in nodes)
            {
                writer.Write("node", node);
            }
        }

        public IEnumerator<TradeNode> GetEnumerator()
        {
            return nodes.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return nodes.GetEnumerator();
        }
    }
}
