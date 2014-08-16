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
            var groups = history.GroupBy(x => x.EventDate).OrderBy(x => x.Key);
            foreach (var pair in groups)
            {
                if (pair.Key.Equals(DateTime.MinValue))
                {
                    foreach (var val in pair)
                        val.Write(writer);
                }
                else
                {
                    string header = pair.Key.ToParadoxString();
                    writer.Write(header, (w) =>
                    {
                        foreach (var val in pair)
                            val.Write(w);
                    });
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
                        addToHistory(InnerToken(p, s, evt));
                    });
            }
            else
            {
                addToHistory(InnerToken(parser, token, evt));
            }
        }

        private void addToHistory(IHistory<T> evt)
        {
            if (evt != null)
                history.Add(evt);
        }

        public void Add(IHistory<T> val)
        {
            history.Add(val);
        }

        protected abstract IHistory<T> InnerToken(
            ParadoxParser parser,
            string token,
            DateTime date);
    }
}
