using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EU4.Savegame;
using NUnit.Framework;
using Pdoxcl2Sharp;

namespace EU4.Savegame.Test
{
    [TestFixture]
    public class LedgerDataTests
    {
        [Test]
        public void LedgerDataParseCorrectly()
        {
            LedgerData ld = ParadoxParser.Parse(File.OpenRead("LedgerDataFile.txt"), new LedgerData());
            Assert.AreEqual("---", ld.Name);
            CollectionAssert.AreEqual(new[] { 0, 1, 0, 2, 0, 0 }, ld.YData);
            CollectionAssert.AreEqual(new[] { 1445, 1446, 1447, 1448, 1449, 1450 }, ld.XData);
        }

        [Test]
        public void SaveLedgerCorrectly()
        {
            LedgerData ld = new LedgerData()
            {
                Name = "---",
                XData = new[] { 1445, 1446, 1447, 1448, 1449, 1450 },
                YData = new[] { 0, 1, 0, 2, 0, 0 }
            };

            using (Pdoxcl2Sharp.ParadoxSaver saver = new Pdoxcl2Sharp.ParadoxSaver(File.OpenWrite("LedgerDataFile.out")))
            {
                saver.Write("ledger_data", ld);
            }

            FileAssert.AreEqual("LedgerDataFile.txt", "LedgerDataFile.out");
        }
    }
}
