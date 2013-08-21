using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace EU4.Savegame.Test
{
    [TestFixture]
    public class ProvinceCollectionTests
    {
        [Test]
        public void ProvinceCollectionGetCorrectInOrder()
        {
            ProvinceCollection pc = new ProvinceCollection();
            pc.Add(new Province(1));
            pc.Add(new Province(2));
            pc.Add(new Province(3));

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, pc.Select(x => x.Id));
            Assert.AreEqual(1, pc.Get(1).Id);
            Assert.AreEqual(2, pc.Get(2).Id);
            Assert.AreEqual(3, pc.Get(3).Id);
        }

        [Test]
        public void ProvinceCollectionGetCorrectOutOfOrder()
        {
            ProvinceCollection pc = new ProvinceCollection();
            pc.Add(new Province(2));
            pc.Add(new Province(3));
            pc.Add(new Province(1));

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, pc.Select(x => x.Id));
            Assert.AreEqual(1, pc.Get(1).Id);
            Assert.AreEqual(2, pc.Get(2).Id);
            Assert.AreEqual(3, pc.Get(3).Id);
        }

        [Test]
        public void ProvinceCollectionGetCorrectGaps()
        {
            ProvinceCollection pc = new ProvinceCollection();
            pc.Add(new Province(3));
            pc.Add(new Province(5));
            pc.Add(new Province(1));

            CollectionAssert.AreEqual(new[] { 1, 3, 5 }, pc.Select(x => x.Id));
            Assert.AreEqual(1, pc.Get(1).Id);
            Assert.AreEqual(3, pc.Get(3).Id);
            Assert.AreEqual(5, pc.Get(5).Id);
        }

        [Test]
        public void ProvinceCollectionUniqueIds()
        {
            ProvinceCollection pc = new ProvinceCollection();
            pc.Add(new Province(1));
            Assert.Throws<ArgumentException>(() => pc.Add(new Province(1)));
        }
    }
}
