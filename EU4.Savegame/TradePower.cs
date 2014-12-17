using Pdoxcl2Sharp;

namespace EU4.Savegame
{
    partial class TradePower : IParadoxRead, IParadoxWrite
    {
        public string Country { get; set; }

        public TradePower(string country)
        {
            Country = country;
        }
    }
}
