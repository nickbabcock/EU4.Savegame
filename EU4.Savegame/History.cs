using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pdoxcl2Sharp;

namespace EU4.Savegame
{
    public class History : IEnumerable<HistoricEvent>, IParadoxRead, IParadoxWrite
    {
        private IList<Tuple<DateTime, List<HistoricEvent>>> events;

        public History()
        {
            this.events = new List<Tuple<DateTime, List<HistoricEvent>>>();
        }

        public IList<Tuple<DateTime, List<HistoricEvent>>> Events
        {
            get { return this.events; }
            set { this.events = value; }
        }

        public virtual void TokenCallback(ParadoxParser parser, string token)
        {
            DateTime d;
            if (ParadoxParser.TryParseDate(token, out d))
            {
                this.events.Add(Tuple.Create(d, History.EventParser(parser, d)));
            }
        }

        public IEnumerator<HistoricEvent> GetEnumerator()
        {
            foreach (var group in this.events)
            {
                foreach (var e in group.Item2)
                {
                    yield return e;
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public virtual void Write(ParadoxStreamWriter writer)
        {
            foreach (var group in this.events)
            {
                writer.Write(
                    group.Item1.ToParadoxString(),
                    (w) =>
                    {
                        foreach (var e in group.Item2)
                        {
                            e.Write(w);
                        }
                    });
            }
        }

        private static List<HistoricEvent> EventParser(ParadoxParser parser, DateTime date)
        {
            var result = new List<HistoricEvent>();

            parser.Parse((p, s) =>
                {
                    switch (s)
                    {
                        case "battle": result.Add(p.Parse(new HistoricBattle(date))); break;
                        case "add_attacker": result.Add(new WarDiplomacyEvent(date, p.ReadString(), WarDiplomacyType.AddAttacker)); break;
                        case "add_defender": result.Add(new WarDiplomacyEvent(date, p.ReadString(), WarDiplomacyType.AddDefender)); break;
                        case "rem_defender": result.Add(new WarDiplomacyEvent(date, p.ReadString(), WarDiplomacyType.RemoveDefender)); break;
                        case "rem_attacker": result.Add(new WarDiplomacyEvent(date, p.ReadString(), WarDiplomacyType.RemoveAttacker)); break;
                    }
                });

            return result;
        }
    }
}
