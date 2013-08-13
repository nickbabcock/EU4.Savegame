using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using EU4.Savegame;
using NUnit.Framework;

namespace EU4.Savegame.Test
{
    [TestFixture]
    public class LedgerDataTests
    {
        [Test]
        public void LedgerDataParseCorrectly()
        {
            LedgerData ld;
            using (FileStream fs = new FileStream("LedgerDataFile.txt", FileMode.Open))
            using (Pdoxcl2Sharp.ParadoxParser parser = new Pdoxcl2Sharp.ParadoxParser(fs))
            {
                // Advance through the "ledger_data"
                parser.ReadString();
                ld = parser.Parse(new LedgerData());
            }

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

            using (FileStream fs = new FileStream("LedgerDataFile.out", FileMode.OpenOrCreate))
            using (Pdoxcl2Sharp.ParadoxSaver saver = new Pdoxcl2Sharp.ParadoxSaver(fs))
            {
                saver.Write("ledger_data", ld);
            }

            FileAssert.AreEqual("LedgerDataFile.txt", "LedgerDataFile.out");
        }
    }
}
