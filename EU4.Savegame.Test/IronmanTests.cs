using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame.Test
{
    [TestFixture]
    public class IronmanTests
    {
        [Test]
        public void ironmanException()
        {
            var ex = Assert.Throws<ApplicationException>(() => new Save("ironman_head.eu4"));
            Assert.That(ex.Message, Is.EqualTo("Ironman saves are not supported at this time"));
        }
    }
}
