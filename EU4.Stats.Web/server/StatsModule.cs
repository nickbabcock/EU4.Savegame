using Nancy;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using EU4.Stats;
using EU4.Savegame;
using Pdoxcl2Sharp;
using ICSharpCode.SharpZipLib.Zip;
using Mustache;

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

            // Pick up with the last id written so we don't overwrite
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
                // Get the temporary location of the file on the server
                var file = Request.Headers["X-FILE"].FirstOrDefault();

                // Get the extension of the file when it was uploaded as the
                // temporary file doesn't have an extension
                var extension = Request.Headers["X-FILE-EXTENSION"].FirstOrDefault();
                if (file == null)
                    throw new ArgumentException("File can't be null");
                if (extension == null)
                    throw new ArgumentException("File extension can't be null");

                Save savegame;
                using (var stream = getStream(file, extension))
                    savegame = new Save(stream);

                // Turn the savegame into html and return the url for it
                FormatCompiler compiler = new FormatCompiler();
                var template = File.ReadAllText("template.html");
                Generator generator = compiler.Compile(template);
                string contents = generator.Render(new
                {
                    Player = savegame.Player,
                    Players = string.Join(", ", savegame.Countries.Where(x => 
                        x.Human.GetValueOrDefault()).Select(x => x.Abbreviation)),
                    Date = savegame.Date,
                    WarStats = WarStats.Calc(savegame),
                    LedgerCorrelations = LedgerStats.correlations(savegame)
                });


                string filename = Interlocked.Increment(ref id) + ".html";
                string loc = Path.Combine(gamedir, filename);
                File.WriteAllText(loc, contents, Pdoxcl2Sharp.Globals.ParadoxEncoding);
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
                case ".zip":
                    var zf = new ZipFile(stream);
                    foreach (ZipEntry e in zf)
                    {
                        if (Path.GetExtension(e.Name) == ".eu4")
                            return zf.GetInputStream(e);
                    }

                    zf.Close();
                    throw new ApplicationException("EU4 file not found in zip file");
                default:
                    stream.Close();
                    throw new ArgumentException("Extension not recognized: " + extension);
            }
        }
    }
}
