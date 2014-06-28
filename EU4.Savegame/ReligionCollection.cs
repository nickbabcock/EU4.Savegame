using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class ReligionCollection : IEnumerable<Religion>, IParadoxRead, IParadoxWrite
    {
        private IList<Religion> religions;

        public ReligionCollection()
        {
            religions = new List<Religion>();
        }

        public void Write(ParadoxStreamWriter writer)
        {
            foreach (var religion in religions)
            {
                writer.Write(religion.Name, religion);
            }
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            religions.Add(parser.Parse(new Religion(token)));
        }

        public IEnumerator<Religion> GetEnumerator()
        {
            return religions.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return religions.GetEnumerator();
        }
    }
}
