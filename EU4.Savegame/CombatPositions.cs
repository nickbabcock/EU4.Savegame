using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class CombatPositions : IEnumerable<Tuple<int, ParadoxId>>, IParadoxRead, IParadoxWrite
    {
        private IList<Tuple<int, ParadoxId>> positions = new List<Tuple<int, ParadoxId>>();

        public void Write(ParadoxStreamWriter writer)
        {
            foreach (var val in positions)
            {
                writer.Write(val.Item1.ToString(), val.Item2);
            }
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            int result;
            if (int.TryParse(token, NumberStyles.None, CultureInfo.InvariantCulture, out result))
                positions.Add(Tuple.Create(result, parser.Parse(new ParadoxId())));
            else
                throw new ApplicationException("Unrecognized token: " + token);
        }

        public IEnumerator<Tuple<int, ParadoxId>> GetEnumerator()
        {
            return positions.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return positions.GetEnumerator();
        }
    }
}
