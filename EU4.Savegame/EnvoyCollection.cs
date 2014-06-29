using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class EnvoyCollection : IEnumerable<Envoy>, IParadoxRead, IParadoxWrite
    {
        private IList<Envoy> envoys;

        public EnvoyCollection()
        {
            envoys = new List<Envoy>();
        }

        public void Write(ParadoxStreamWriter writer)
        {
            foreach (var envoy in envoys)
            {
                writer.Write("envoy", envoy);
            }
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            if (token == "envoy")
            {
                envoys.Add(parser.Parse(new Envoy()));
            }
        }

        public IEnumerator<Envoy> GetEnumerator()
        {
            return envoys.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return envoys.GetEnumerator();
        }
    }
}
