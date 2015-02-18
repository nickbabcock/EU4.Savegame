using EU4.Savegame;
using ICSharpCode.SharpZipLib.Zip;
using Metrics;
using Nancy;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace EU4.Stats.Web
{
    public class StatsModule : NancyModule
    {
        private readonly Timer parsingTimer = Metric.Timer("Savegame Parsing",
            Metrics.Unit.Requests, SamplingType.FavourRecent);

        private readonly Timer statsTimer = Metric.Timer("Stats Generation",
            Metrics.Unit.Requests, SamplingType.FavourRecent);

        private readonly Timer templateTimer = Metric.Timer("Template Render",
            Metrics.Unit.Requests, SamplingType.FavourRecent);

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
                using (parsingTimer.NewContext())
                    savegame = new Save(stream);

                // Turn the savegame into html and return the url for it
                var stats = statsTimer.Time(() => Aggregate(savegame));
                string contents = templateTimer.Time(() => tmpl.Render(stats));
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

        public static StatsModel Aggregate(Save savegame)
        {
            var stats = new SaveStats(savegame);
            var regiments = stats.WorldRegiments();
            var ships = stats.WorldShips();
            return new StatsModel()
            {
                Player = savegame.Player,
                Players = string.Join(", ", 
                    (savegame.Countries ?? Enumerable.Empty<Country>()).Where(x =>
                    x.WasPlayer.GetValueOrDefault()).Select(x => x.Abbreviation)),
                PlayerCountries = stats.PlayerStats(),
                Date = savegame.Date,
                Manpower = stats.WorldManpower(),
                PotentialManpower = stats.MaxWorldManpower(),
                LeaderReport = stats.LeaderReport(),
                BiggestNavalWars = stats.NavalWarReport().Take(10),
                BiggestLandWars = stats.LandWarReport().Take(10),
                BiggestLandBattles = stats.LandBattleReport().Take(10),
                BiggestNavalBattles = stats.NavalBattleReport().Take(10),
                BiggestRivalries = stats.CommanderRivalries().Take(10),
                CountryMilitaryStats = stats.CountryKillsAndLosses().Take(10),
                LedgerCorrelations = stats.LedgerCorrelations(),
                ScoreStats = stats.ScoreRankings().Where(x => x.rank <= 10 || stats.IsPlayer(x.name)),
                Debt = stats.CountryDebts().Take(10),
                TradePower = stats.CountryTradeReport().Take(10),
                RegimentCount = regiments.Item1,
                RegimentSum = regiments.Item2,
                ShipCount = ships.Item1,
                ShipSum = ships.Item2
            };
        }
    }
}
