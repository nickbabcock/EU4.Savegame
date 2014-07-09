using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class Demands : IEnumerable<string>, IParadoxRead, IParadoxWrite
    {
        private IList<string> demands = new List<string>();

        public void Write(ParadoxStreamWriter writer)
        {
            writer.Write("demand", (w) =>
                {
                    foreach (var val in demands)
                        w.WriteLine("demand", val, ValueWrite.Quoted);
                });
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            if (token != "demand")
                throw new ApplicationException("Unrecognized token " + token);
            demands.Add(parser.ReadString());
        }

        public IEnumerator<string> GetEnumerator()
        {
            return demands.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return demands.GetEnumerator();
        }
    }
}
