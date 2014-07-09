using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public interface IHistory<T> : IParadoxWrite
    {
        void Apply(T obj, bool advance);
        DateTime EventDate { get; }
    }
}
