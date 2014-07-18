using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EU4.Stats.Web
{
    public class SavegameStorage
    {
        private readonly string gamedir;
        public SavegameStorage(string gamedir)
        {
            this.gamedir = gamedir;
        }

        public string Store(string contents, string id)
        {
            string loc = Path.Combine(gamedir, id + ".html");
            File.WriteAllText(loc, contents, Pdoxcl2Sharp.Globals.ParadoxEncoding);
            return Path.Combine("games", id);
        }
    }
}
