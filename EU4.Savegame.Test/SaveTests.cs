using NUnit.Framework;
using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EU4.Savegame.Test
{
    [TestFixture]
    public class SaveTests
    {
        [Test]
        public void SaveCanParse()
        {
            string data = "EU4txt\r\ndate=\"1821.1.1\"";
            Save savegame;
            using (var stream = new MemoryStream(Globals.ParadoxEncoding.GetBytes(data)))
            {
                savegame = new Save(stream);
            }

            Assert.That(savegame.Date, Is.EqualTo(new DateTime(1821, 1, 1)));
        }

        [Test]
        public void SaveNeedMagicStringWithRecognized()
        {
            string data = "date=\"1821.1.1\"";
            using (var stream = new MemoryStream(Globals.ParadoxEncoding.GetBytes(data)))
            {
                Assert.That(() => new Save(stream), Throws.InstanceOf<ApplicationException>());
            }
        }

        [Test]
        public void SaveNeedMagicStringWithUnrecognized()
        {
            string data = "pngtxt\r\ndate=\"1821.1.1\"";
            using (var stream = new MemoryStream(Globals.ParadoxEncoding.GetBytes(data)))
            {
                Assert.That(() => new Save(stream), Throws.InstanceOf<ApplicationException>());
            }
        }
    }
}
