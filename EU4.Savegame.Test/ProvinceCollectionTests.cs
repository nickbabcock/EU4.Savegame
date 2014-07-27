using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame.Test
{
    [TestFixture]
    public class ProvinceCollectionTests
    {
        [Test]
        public void ProvinceCollectionCanAdd()
        {
            ProvinceCollection col = new ProvinceCollection();
            col.Add(new Province(1));
            col.Add(new Province(2));
            Assert.That(col.Count(), Is.EqualTo(2));
        }

        [Test]
        public void ProvinceCollectionCanGet()
        {
            ProvinceCollection col = new ProvinceCollection();
            col.Add(new Province(1));
            col.Add(new Province(2));
            Assert.That(col.Get(1).Id, Is.EqualTo(1));
        }

        [Test]
        public void ProvinceCollectionEnsureIncrementalAdd()
        {
            ProvinceCollection col = new ProvinceCollection();
            col.Add(new Province(1));
            Assert.Throws<ApplicationException>(() => col.Add(new Province(3)));
        }
    }
}
