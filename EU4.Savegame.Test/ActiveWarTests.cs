using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Pdoxcl2Sharp;

namespace EU4.Savegame.Test
{
    [TestFixture]
    public class ActiveWarTests
    {
        [Test]
        public void ParseActiveWarCorrectly()
        {
            ActiveWar war = null;
            ParadoxParser.Parse(
                File.OpenRead("ActiveWar.txt"),
                (p, s) =>
                {
                    if (!string.IsNullOrEmpty(s) && s != "\0")
                    {
                        war = p.Parse(new ActiveWar());
                    }
                });

            Assert.AreEqual("Orissi Conquest of Bengal Delta", war.Name);
            CollectionAssert.AreEqual(new[] { "ORI", "GRJ" }, war.Attackers);
            CollectionAssert.AreEqual(new[] { "BNG", "DLH", "JAN" }, war.Defenders);
            Assert.AreEqual("ORI", war.OriginalAttacker);
            Assert.AreEqual("BNG", war.OriginalDefender);
            Assert.AreEqual(new DateTime(1460, 9, 1), war.Action);
            Assert.AreEqual(5.200, war.AttackerScore);
            Assert.AreEqual("cb_conquest", war.WarGoal.CasusBelli);
            Assert.AreEqual("take_claim", war.WarGoal.Type);
            Assert.AreEqual(561, war.WarGoal.Province);
            CollectionAssert.AreEqual(new[] { "JAN", "DLH" }, war.Demands);
            Assert.AreEqual(2, war.WarDirectionQuarter);
            Assert.AreEqual(7, war.WarDirectionYear);
            Assert.AreEqual(12, war.LastWarscoreQuarter);
            Assert.AreEqual(7, war.LastWarscoreYear);
            Assert.AreEqual(new DateTime(1460, 9, 14), war.NextQuarterUpdate);
            Assert.AreEqual(new DateTime(1460, 12, 9), war.NextYearUpdate);
            Assert.AreEqual(0, war.StalledYears);
        }

        [Test]
        public void SaveActiveWarCorrectly()
        {
            ActiveWar expected = new ActiveWar()
            {
                Name = "Orissi Conquest of Bengal Delta",
                Attackers = new List<string>() { "ORI", "GRJ" },
                Defenders = new List<string>() { "BNG", "DLH", "JAN" },
                OriginalAttacker = "ORI",
                OriginalDefender = "BNG",
                Action = new DateTime(1460, 9, 1),
                AttackerScore = 5.200,
                WarGoalHeader = "take_province",
                WarGoal = new WarGoal()
                {
                    CasusBelli = "cb_conquest",
                    Type = "take_claim",
                    Province = 561
                },
                Demands = new List<string>() { "JAN", "DLH" },
                WarDirectionQuarter = 2,
                WarDirectionYear = 7,
                LastWarscoreQuarter = 12,
                LastWarscoreYear = 7,
                NextQuarterUpdate = new DateTime(1460, 9, 14),
                NextYearUpdate = new DateTime(1460, 12, 9),
                StalledYears = 0,
                History = new History()
                {
                    Events = new List<Tuple<DateTime, List<HistoricEvent>>>()
                    {
                        Tuple.Create(
                            new DateTime(1458, 12, 7),
                            new List<HistoricEvent>()
                            {
                                new WarDiplomacyEvent(new DateTime(1458, 12, 7), "ORI", WarDiplomacyType.AddAttacker)
                            }),
                        Tuple.Create(
                            new DateTime(1458, 12, 7),
                            new List<HistoricEvent>()
                            {
                                new WarDiplomacyEvent(new DateTime(1458, 12, 7), "BNG", WarDiplomacyType.AddDefender)
                            }),
                        Tuple.Create(
                            new DateTime(1458, 12, 7),
                            new List<HistoricEvent>()
                            {
                                new WarDiplomacyEvent(new DateTime(1458, 12, 7), "GRJ", WarDiplomacyType.AddAttacker)
                            }),
                        Tuple.Create(
                            new DateTime(1458, 12, 8),
                            new List<HistoricEvent>()
                            {
                                new WarDiplomacyEvent(new DateTime(1458, 12, 8), "DLH", WarDiplomacyType.AddDefender)
                            }),
                        Tuple.Create(
                            new DateTime(1458, 12, 8),
                            new List<HistoricEvent>()
                            {
                                new WarDiplomacyEvent(new DateTime(1458, 12, 8), "JAN", WarDiplomacyType.AddDefender)
                            }),
                    }
                }
            };

            using (ParadoxSaver saver = new ParadoxSaver(File.Create("ActiveWar.out")))
            {
                saver.Write("active_war", expected);
            }

            FileAssert.AreEqual("ActiveWar.txt", "ActiveWar.out");
        }
    }
}
