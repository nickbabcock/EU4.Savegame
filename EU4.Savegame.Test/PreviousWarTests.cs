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
    public class PreviousWarTests
    {
        [Test]
        public void BlankPreviousWarParsedCorrectly()
        {
            var previousWar = ParadoxParser.Parse(File.OpenRead("BlankWarFile.txt"), new PreviousWar());
            Assert.AreEqual(string.Empty, previousWar.Name);
            Assert.AreEqual("---", previousWar.OriginalAttacker);
            Assert.AreEqual("---", previousWar.OriginalDefender);
            Assert.AreEqual(new DateTime(1, 1, 1), previousWar.Action);
            Assert.AreEqual(-1, previousWar.WarDirectionQuarter);
            Assert.AreEqual(-5, previousWar.WarDirectionYear);
            Assert.AreEqual(-3, previousWar.LastWarscoreQuarter);
            Assert.AreEqual(15, previousWar.LastWarscoreYear);
            Assert.AreEqual(new DateTime(1, 1, 1), previousWar.NextQuarterUpdate);
            Assert.AreEqual(new DateTime(1, 1, 1), previousWar.NextYearUpdate);
            Assert.AreEqual(-1, previousWar.StalledYears);
            Assert.AreEqual(0, previousWar.History.Count());
        }

        [Test]
        public void SaveBlankPreviousWarCorrectly()
        {
            var previousWar = new PreviousWar()
            {
                Name = string.Empty,
                History = new PreviousWarHistory(),
                OriginalAttacker = "---",
                OriginalDefender = "---",
                Action = new DateTime(1, 1, 1),
                WarDirectionQuarter = -1,
                WarDirectionYear = -5,
                LastWarscoreQuarter = -3,
                LastWarscoreYear = 15,
                NextQuarterUpdate = new DateTime(1, 1, 1),
                NextYearUpdate = new DateTime(1, 1, 1),
                StalledYears = -1
            };
            using (var saver = new ParadoxSaver(File.Create("BlankWarFile.out")))
            {
                saver.Write("previous_war", previousWar);
            }

            FileAssert.AreEqual("BlankWarFile.txt", "BlankWarFile.out");
        }
    }
}
