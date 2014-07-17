using Mustache;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EU4.Stats.Web
{
    /// <summary>
    /// The Templater provides a thread safe mechanism for rendering objects
    /// into html, caching the template, and updating the template whenever the
    /// template changes. The current implementation uses mutual exclusive
    /// locking. The reason for this is two-fold. One, it is easy to get
    /// threading right, and secondly, generating the resulting html isn't
    /// computationally intensive so the lock will only be acquired for a few
    /// moments.
    /// </summary>
    public class Templater : ITemplate
    {
        private readonly string file;
        private Generator gen;
        private readonly FormatCompiler compiler;
        private readonly FileSystemWatcher watcher;

        public Templater(string filepath)
        {
            file = filepath;
            compiler = new FormatCompiler();
            gen = compiler.Compile(File.ReadAllText(filepath));

            string parent = Path.GetDirectoryName(filepath);
            string shortname = Path.GetFileName(file);

            // File watcher normally uses a glob pattern to match a series of
            // files, but since we are just watching for one file we just use
            // the name of the file as the glob.
            watcher = new FileSystemWatcher(parent, shortname);
            watcher.Changed += watcher_Changed;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.EnableRaisingEvents = true;
        }

        private void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            Generator temp = compiler.Compile(File.ReadAllText(file));
            lock (gen)
                gen = temp;
        }

        public string Render(object obj)
        {
            lock (gen)
                return gen.Render(obj);
        }
    }
}
