using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace EU4.Stats.Web
{
    /// <summary>
    /// Thread safe incremental counter that picks up with the last id
    /// from the previous run of the program
    /// </summary>
    public class IncrementIdGenerator : IIdGenerator
    {
        private int id;

        /// <summary>
        /// Initializes the first id to the max numerical filename
        /// in a given directory.
        /// </summary>
        /// <param name="gamedir">
        /// Directory filled with files like "1.html",  "2.html"
        /// </param>
        public IncrementIdGenerator(string gamedir)
        {
            // Pick up with the last id written so we don't overwrite
            var q = Directory.EnumerateFiles(gamedir)
                .Select(x => Path.GetFileNameWithoutExtension(x))
                .Select(x => int.Parse(x))
                .DefaultIfEmpty();

            id = q == Enumerable.Empty<int>() ? 0 : q.Max();
        }

        public string NextId()
        {
            return Interlocked.Increment(ref id).ToString();
        }
    }
}
