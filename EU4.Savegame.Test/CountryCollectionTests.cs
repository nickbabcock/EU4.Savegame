using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace EU4.Savegame.Test
{
    [TestFixture]
    public class CountryCollectionTests
    {
        [Test]
        public void CanAddCountriesToCollection()
        {
            var c = new Country("MEE");
            var collection = new CountryCollection();
            collection.Add(c);
            Assert.That(collection.Count(), Is.EqualTo(1));
        }

        [Test]
        public void CantAddDuplicateCountriesToCollection()
        {
            var c = new Country("MEE");
            var c2 = new Country("MEE");
            var collection = new CountryCollection();
            collection.Add(c);
            Assert.That(() => collection.Add(c2), Throws.ArgumentException);
        }
    }
}
