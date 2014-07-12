using Mustache;
using Nancy;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
            basedir = "/var/www/stats";
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
                var extension = Request.Headers["X-FILE-EXTENSION"].FirstOrDefault();
                if (file == null)
                    throw new ArgumentException("File can't be null");
                if (extension == null)
                    throw new ArgumentException("File extension can't be null");

                EU4.Savegame.Savegame savegame;
                using (var stream = getStream(file, extension))
                    savegame = new EU4.Savegame.Savegame(stream);

                FormatCompiler compiler = new FormatCompiler();
                string template = File.ReadAllText("template.hb");
                Generator generator = compiler.Compile(template);
                string contents = generator.Render(savegame);
                string filename = Interlocked.Increment(ref id) + ".html";
                string loc = Path.Combine(gamedir, filename);
                File.WriteAllText(loc, contents);
                return Path.Combine("games", Path.GetFileNameWithoutExtension(loc));
            };
        }

        private Stream getStream(string filename, string extension)
        {
            var stream = new FileStream(filename, FileMode.Open,
                            FileAccess.Read, FileShare.ReadWrite, 1024 * 64);
            switch (extension)
            {
                case ".eu4": return stream;
                case ".gz": return new GZipStream(stream, CompressionMode.Decompress);
                default:
                    throw new ArgumentException("Extension not recognized: " + filename);
            }
        }
    }
}
