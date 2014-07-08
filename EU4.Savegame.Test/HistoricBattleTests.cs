using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Pdoxcl2Sharp;

namespace EU4.Savegame.Test
{
    public class HistoricBattleTests
    {
        [Test]
        public void ParseHistoricBattleCorrectly()
        {
            var result = ParadoxParser.Parse(File.OpenRead("HistoricBattle.txt"), new BattleResult(new DateTime(1, 1, 1)));
            Assert.AreEqual("Temes", result.Name);
            Assert.AreEqual(156, result.Location);
            Assert.AreEqual(true, result.Result);
        }

        [Test]
        public void SaveHistoricBattleCorrectly()
        {
            var expected = new BattleResult(new DateTime(1, 1, 1))
            {
                Attacker = HistoricCombatantTests.GetOffensiveLandCombatant(),
                Defender = HistoricCombatantTests.GetDefensiveNavalCombatant(),
                Location = 156,
                Name = "Temes",
                Result = true
            };

            using (ParadoxSaver saver = new ParadoxSaver(File.Create("HistoricBattle.out")))
            {
                expected.Write(saver);
            }

            FileAssert.AreEqual("HistoricBattle.txt", "HistoricBattle.out");
        }
    }
}
