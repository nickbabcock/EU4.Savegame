using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Pdoxcl2Sharp;

namespace EU4.Savegame.Test
{
    [TestFixture]
    public class HistoricCombatantTests
    {
        public static HistoricCombatant GetDefensiveNavalCombatant()
        {
            return new HistoricCombatant()
            {
                Galley = 3,
                LightShip = 15,
                HeavyShip = 12,
                Transport = 15,
                Losses = 0,
                Country = "ENG",
                WarGoal = 0,
                Commander = "Matthew Leicester"
            };
        }

        public static HistoricCombatant GetOffensiveLandCombatant()
        {
            return new HistoricCombatant()
            {
                Cavalry = 2000,
                Infantry = 18000,
                Artillery = 1000,
                Losses = 12000,
                Country = "POL",
                WarGoal = 0,
                Commander = "Wladyslaw III"
            };
        }

        [Test]
        public void ParseHistoricLandCombatantCorrectly()
        {
            var result = ParadoxParser.Parse(File.OpenRead("HistoricCombatantLand.txt"), new HistoricCombatant());
            Assert.AreEqual(2000, result.Cavalry);
            Assert.AreEqual(18000, result.Infantry);
            Assert.AreEqual(1000, result.Artillery);
            Assert.AreEqual(12000, result.Losses);
            Assert.AreEqual("POL", result.Country);
            Assert.AreEqual(0, result.WarGoal);
            Assert.AreEqual("Wladyslaw III", result.Commander);
        }

        [Test]
        public void SaveHistoricLandCombatantCorrectly()
        {
            HistoricCombatant expected = GetOffensiveLandCombatant();

            using (ParadoxSaver saver = new ParadoxSaver(File.Create("HistoricCombatantLand.out")))
            {
                saver.Write("attacker", expected);
            }

            FileAssert.AreEqual("HistoricCombatantLand.txt", "HistoricCombatantLand.out");
        }

        [Test]
        public void ParseHistoricNavalCombatantCorrectly()
        {
            var result = ParadoxParser.Parse(File.OpenRead("HistoricCombatantNaval.txt"), new HistoricCombatant());
            Assert.AreEqual(3, result.Galley);
            Assert.AreEqual(15, result.LightShip);
            Assert.AreEqual(12, result.HeavyShip);
            Assert.AreEqual(15, result.Transport);
            Assert.AreEqual(0, result.Losses);
            Assert.AreEqual("ENG", result.Country);
            Assert.AreEqual(0, result.WarGoal);
            Assert.AreEqual("Matthew Leicester", result.Commander);
        }

        [Test]
        public void SaveHistoricNavalCombatantCorrectly()
        {
            HistoricCombatant expected = GetDefensiveNavalCombatant();

            using (ParadoxSaver saver = new ParadoxSaver(File.Create("HistoricCombatantNaval.out")))
            {
                saver.Write("defender", expected);
            }

            FileAssert.AreEqual("HistoricCombatantNaval.txt", "HistoricCombatantNaval.out");
        }
    }
}
