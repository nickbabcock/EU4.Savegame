using Pdoxcl2Sharp;
using System.Collections.Generic;

namespace EU4.Savegame
{
    partial class TradeNode : IParadoxRead, IParadoxWrite
    {
        private IList<TradePower> _powers = new List<TradePower>();

        public IList<TradePower> Powers
        { 
            get { return _powers; } 
            set { _powers = value; }
        }

        partial void unrecognizedToken(ParadoxParser parser, string token)
        {
            if (token.Length == 3 && token.ToUpper() == token)
            {
                _powers.Add(parser.Parse(new TradePower(token)));
            }
        }
    }
}
