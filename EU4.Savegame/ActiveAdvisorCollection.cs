using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class ActiveAdvisorCollection : IEnumerable<ActiveAdvisor>, IParadoxRead, IParadoxWrite
    {
        private IList<ActiveAdvisor> advisors;

        public ActiveAdvisorCollection()
        {
            advisors = new List<ActiveAdvisor>();
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            advisors.Add(parser.Parse(new ActiveAdvisor(token)));
        }

        public void Write(ParadoxStreamWriter writer)
        {
            foreach (var adv in advisors)
            {
                writer.Write(adv.Country, adv);
            }
        }

        IEnumerator<ActiveAdvisor> IEnumerable<ActiveAdvisor>.GetEnumerator()
        {
            return advisors.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return advisors.GetEnumerator();
        }
    }
}
