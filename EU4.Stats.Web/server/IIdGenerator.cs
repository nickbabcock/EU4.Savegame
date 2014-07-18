using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Stats.Web
{
    public interface IIdGenerator
    {
        string NextId();
    }
}
