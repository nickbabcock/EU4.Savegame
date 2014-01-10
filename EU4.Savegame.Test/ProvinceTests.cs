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
    public class ProvinceTests
    {
        private Province prov;

        [TestFixtureSetUp]
        public void Setup()
        {
            ParadoxParser.Parse(
                File.OpenRead("ProvinceTestData.txt"), 
                (p, id) => this.prov = p.Parse(new Province(int.Parse(id.Substring(4)))));
        }

        [Test]
        public void TestProvinceId()
        {
            Assert.AreEqual(111, this.prov.Id);
        }

        [Test]
        public void TestProvinceName()
        {
            Assert.AreEqual("Friuli", this.prov.Name);
        }

        [Test]
        public void TestProvinceClaims()
        {
            CollectionAssert.AreEquivalent(new[] { "HAB", "HUN" }, this.prov.Claims);
        }
    }
}
