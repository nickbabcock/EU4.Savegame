using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;
using Pdoxcl2Sharp;

namespace EU4.Savegame.Test
{
    [TestFixture]
    public class BattleHistoryTests
    {
        [Test]
        public void ParseBattleHistoryCorrectly()
        {
            var history = ParadoxParser.Parse(File.OpenRead("BattleHistory.txt"), new PreviousWarHistory());
            var expected = new[]
            {
                new WarDiplomacyEvent(new DateTime(1440, 1, 1), "CAS", WarDiplomacyType.AddAttacker),
                new WarDiplomacyEvent(new DateTime(1440, 1, 1), "ARA", WarDiplomacyType.AddAttacker),
                new WarDiplomacyEvent(new DateTime(1440, 1, 1), "POR", WarDiplomacyType.AddAttacker),
                new WarDiplomacyEvent(new DateTime(1440, 1, 1), "GRA", WarDiplomacyType.AddDefender),
                new WarDiplomacyEvent(new DateTime(1443, 1, 1), "CAS", WarDiplomacyType.RemoveAttacker),
                new WarDiplomacyEvent(new DateTime(1444, 1, 1), "ARA", WarDiplomacyType.RemoveAttacker),
                new WarDiplomacyEvent(new DateTime(1444, 1, 1), "POR", WarDiplomacyType.RemoveAttacker),
                new WarDiplomacyEvent(new DateTime(1444, 1, 1), "GRA", WarDiplomacyType.RemoveDefender)
            };

            Assert.AreEqual(expected.Count(), history.Count());
            CollectionAssert.AreEqual(expected, history, new WarDiplomacyEventComparer());
            Assert.AreEqual("Crusade for Granada", history.Name);
            Assert.AreEqual("superiority_crusade", history.WarGoal.Type);
            Assert.AreEqual("cb_crusade", history.WarGoal.CasusBelli);
        }

        [Test]
        public void SaveBattleHistoryCorrectly()
        {
            PreviousWarHistory history = new PreviousWarHistory()
            {
                Name = "Crusade for Granada",
                WarGoal = new WarGoal()
                {
                    Type = "superiority_crusade",
                    CasusBelli = "cb_crusade"
                },
                Events = new List<Tuple<DateTime, List<HistoricEvent>>>()
                {
                    Tuple.Create(
                        new DateTime(1440, 1, 1),
                        new List<HistoricEvent>()
                        {
                            new WarDiplomacyEvent(new DateTime(1440, 1, 1), "CAS", WarDiplomacyType.AddAttacker),
                            new WarDiplomacyEvent(new DateTime(1440, 1, 1), "ARA", WarDiplomacyType.AddAttacker),
                            new WarDiplomacyEvent(new DateTime(1440, 1, 1), "POR", WarDiplomacyType.AddAttacker),
                            new WarDiplomacyEvent(new DateTime(1440, 1, 1), "GRA", WarDiplomacyType.AddDefender)
                        }),
                    Tuple.Create(
                        new DateTime(1443, 1, 1),
                        new List<HistoricEvent>()
                        {
                            new WarDiplomacyEvent(new DateTime(1443, 1, 1), "CAS", WarDiplomacyType.RemoveAttacker)
                        }),
                    Tuple.Create(
                        new DateTime(1444, 1, 1),
                        new List<HistoricEvent>() 
                        {
                            new WarDiplomacyEvent(new DateTime(1444, 1, 1), "ARA", WarDiplomacyType.RemoveAttacker),
                            new WarDiplomacyEvent(new DateTime(1444, 1, 1), "POR", WarDiplomacyType.RemoveAttacker),
                            new WarDiplomacyEvent(new DateTime(1444, 1, 1), "GRA", WarDiplomacyType.RemoveDefender)
                        })
                }
            };

            using (ParadoxSaver saver = new ParadoxSaver(File.Create("BattleHistory.out")))
            {
                saver.Write("history", history);
            }

            FileAssert.AreEqual("BattleHistory.txt", "BattleHistory.out");
        }

        public class WarDiplomacyEventComparer : IComparer<WarDiplomacyEvent>, IComparer
        {
            public int Compare(WarDiplomacyEvent x, WarDiplomacyEvent y)
            {
                if (x.Country != y.Country)
                {
                    return x.Country.CompareTo(y.Country);
                }
                else if (x.EventDate != y.EventDate)
                {
                    return x.EventDate.CompareTo(y.EventDate);
                }
                else if (x.Type != y.Type)
                {
                    return x.Type.CompareTo(y.Type);
                }

                return 0;
            }

            public int Compare(object x, object y)
            {
                return this.Compare((WarDiplomacyEvent)x, (WarDiplomacyEvent)y);
            }
        }
    }
}
