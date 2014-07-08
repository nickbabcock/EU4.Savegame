using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pdoxcl2Sharp;

namespace EU4.Savegame
{
    public abstract class History<T> :  IEnumerable<IHistory<T>>, IParadoxRead, IParadoxWrite
        where T : IParadoxWrite
    {
        private IList<IHistory<T>> history = new List<IHistory<T>>();

        public IEnumerator<IHistory<T>> GetEnumerator()
        {
            return history.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public virtual void Write(ParadoxStreamWriter writer)
        {
            foreach (var pair in history.GroupBy(x => x.EventDate))
            {
                if (pair.Key.Equals(DateTime.MinValue))
                {
                    foreach (var val in pair)
                        val.Write(writer);
                }
                else
                {
                    string header = pair.Key.ToParadoxString();
                    foreach (var val in pair)
                        writer.Write(header, val);
                }
            }
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            DateTime evt;
            if (ParadoxParser.TryParseDate(token, out evt))
            {
                parser.Parse((p, s) =>
                    {
                        history.Add(InnerToken(p, s, evt));
                    });
            }
            else
            {
                history.Add(InnerToken(parser, token, evt));
            }
        }

        protected abstract IHistory<T> InnerToken(
            ParadoxParser parser,
            string token,
            DateTime date);
    }
}
