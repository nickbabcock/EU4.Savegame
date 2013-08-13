using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pdoxcl2Sharp;

namespace EU4.Savegame
{
    public class Province : IParadoxRead, IParadoxWrite
    {
        private string name;
        private string owner;
        private string controller;
        private string culture;
        private string religion;
        private IList<string> cores;
        private bool isInHre;

        public Province(int id)
        {
            this.Id = id;
            this.cores = new List<string>();
        }

        public int Id { get; private set; }
        public string Name 
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public string Owner
        {
            get { return this.owner; }
            set { this.owner = value; }
        }

        public string Controller
        {
            get { return this.controller; }
            set { this.controller = value; }
        }

        public string Culture
        {
            get { return this.culture; }
            set { this.culture = value; }
        }

        public string Religion
        {
            get { return this.religion; }
            set { this.religion = value; }
        }

        public IList<string> Cores
        {
            get { return this.cores; }
            set { this.cores = value; }
        }

        public bool IsInHre
        {
            get { return this.isInHre; }
            set { this.isInHre = value; }
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            switch (token)
            {
                case "name": this.name = parser.ReadString(); break;
                case "owner": this.owner = parser.ReadString(); break;
                case "controller": this.controller = parser.ReadString(); break;
                case "culture": this.culture = parser.ReadString(); break;
                case "religion": this.religion = parser.ReadString(); break;
                case "core": this.cores.Add(parser.ReadString()); break;
            }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            throw new NotImplementedException();
        }
    }
}
