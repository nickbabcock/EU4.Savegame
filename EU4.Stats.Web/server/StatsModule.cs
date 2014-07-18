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
        public StatsModule(ITemplate tmpl, IIdGenerator idgen, SavegameStorage storage)
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
                string contents = tmpl.Render(aggregate(savegame));
                string id = idgen.NextId();
                return storage.Store(contents, id);
            };
        }

        private static Stream getStream(string filename, string extension)
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

        private static object aggregate(Save savegame)
        {
            return new
            {
                Player = savegame.Player,
                Players = string.Join(", ", savegame.Countries.Where(x =>
                    x.WasPlayer.GetValueOrDefault()).Select(x => x.Abbreviation)),
                Date = savegame.Date,
                WarStats = WarStats.Calc(savegame),
                LedgerCorrelations = LedgerStats.correlations(savegame),
                ScoreStats = CountryStats.scoreRankings(savegame),
                Debt = CountryStats.inDebt(savegame).Take(10),
                WorldStats = WorldStats.calc(savegame),
                TradePower = TradeStats.calc(savegame).Take(10)
            };
        }
    }
}
