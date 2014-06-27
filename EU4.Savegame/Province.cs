using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pdoxcl2Sharp;

namespace EU4.Savegame
{
    public partial class Province : IParadoxRead, IParadoxWrite
    {
        public Province(int id)
            : this()
        {
            this.Id = id;
        }

        public int Id { get; private set; }
    }
}
