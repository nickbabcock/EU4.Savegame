using Mustache;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace EU4.Stats.Web
{
    public class StatsModule : NancyModule
    {
        static int id;
        static readonly string basedir;
        static readonly string gamedir;

        static StatsModule()
        {
            basedir = Environment.GetEnvironmentVariable("EU4_STATS_LOC");
            if (string.IsNullOrWhiteSpace(basedir))
                throw new ApplicationException("Env EU4_STATS_LOC must be set");

            gamedir = Path.Combine(basedir, "games");
            var q = Directory.EnumerateFiles(gamedir)
                .Select(x => Path.GetFileNameWithoutExtension(x))
                .Select(x => int.Parse(x))
                .DefaultIfEmpty();

            id = q == Enumerable.Empty<int>() ? 0 : q.Max();
        }

        public StatsModule()
        {
            Post["/games"] = _ =>
            {
                var file = Request.Headers["X-FILE"].FirstOrDefault();
                if (file == null)
                    throw new ArgumentException("File can't be null");

                var savegame = new EU4.Savegame.Savegame(file);
                FormatCompiler compiler = new FormatCompiler();
                string template = File.ReadAllText("template.hb");
                Generator generator = compiler.Compile(template);
                string contents = generator.Render(savegame);
                string filename = Interlocked.Increment(ref id) + ".html";
                string loc = Path.Combine(gamedir, filename);
                File.WriteAllText(loc, contents);
                return loc;
            };
        }
    }
}
