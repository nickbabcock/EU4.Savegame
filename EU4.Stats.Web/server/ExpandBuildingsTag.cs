using Mustache;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EU4.Stats.Web
{
    public class ExpandBuildingsTag : InlineTagDefinition
    {
        private const string tag = "expandBuildings";
        private static readonly TagParameter tp = 
            new TagParameter(tag) { IsRequired = true };

        public ExpandBuildingsTag()
            : base(tag)
        {
        }

        protected override IEnumerable<TagParameter> GetParameters()
        {
            return new TagParameter[] { tp };
        }

        public override void GetText(TextWriter writer, Dictionary<string, object> arguments, Scope context)
        {
            var stats = (IEnumerable<CountryStats.PlayerStats>)arguments[tag];
            if (stats == null || !stats.Any())
                return;

            var buildings = stats.First().buildings.Select(x => x.Item1).ToList();
            foreach (var building in buildings)
            {
                writer.Write(string.Format("<tr><td>{0}</td></tr>", building));
            }
        }
    }
}
