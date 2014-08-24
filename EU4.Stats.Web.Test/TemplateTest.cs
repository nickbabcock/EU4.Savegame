using EU4.Savegame;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Stats.Web.Test
{
    [TestFixture]
    public class TemplateTest
    {
        [Test]
        public async void TemplateRendersSuccessfully()
        {
            var tmpl = await Templater.CreateTemplater("template.jade");
            var save = new Save();
            save.Player = "MEE";
            string contents = await tmpl.Render(StatsModule.Aggregate(save));
            StringAssert.Contains("MEE", contents);
        }
    }
}
