using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class ProvinceHistory : History<Province>
    {
        protected override IHistory<Province> InnerToken(ParadoxParser parser, string token, DateTime date)
        {
            switch (token)
            {
                case "revolt": return parser.Parse(new Revolt(date));
                case "controller": return parser.Parse(new ControllerChange(date));
                case "owner": return new OwnerChange(date, parser.ReadString());
                case "discovered_by": return new DiscoveredBy(date, parser.ReadString());
                case "add_core": return new AddCore(date, parser.ReadString());
                case "add_claim": return new AddClaim(date, parser.ReadString());
                case "religion": return new ReligionChange(date, parser.ReadString());
                case "trade_goods": return new TradeGoodsChange(date, parser.ReadString());
                case "culture": return new CultureChange(date, parser.ReadString());
                case "manpower": return new ManpowerChange(date, parser.ReadDouble());
                case "advisor": return parser.Parse(new Advisor(date));
                default:
                    /*if (parser.ReadBool())
                        return new BuildingChange(date, token);*/
                    parser.ReadString(); break;
            }

            return null;
        }
    }

    public partial class Revolt : IHistory<Province>
    {
        private readonly DateTime evt;
        public Revolt(DateTime evt)
        {
            this.evt = evt;
        }

        public void Apply(Province obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate { get { return evt; } }
    }

    public partial class ControllerChange : IHistory<Province>
    {
        private readonly DateTime evt;
        public ControllerChange(DateTime evt)
        {
            this.evt = evt;
        }

        public void Apply(Province obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate { get { return evt; } }
    }

    public class BuildingChange : IHistory<Province>
    {
        private DateTime evt;
        public string Building { get; set; }
        public BuildingChange(DateTime evt, string building)
        {
            this.evt = evt;
            this.Building = building;
        }

        public void Apply(Province obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine(Building, true);
        }
    }

    public class OwnerChange : IHistory<Province>
    {
        private DateTime evt;
        public string Owner { get; set; }
        public OwnerChange(DateTime evt, string owner)
        {
            this.evt = evt;
            this.Owner = owner;
        }

        public void Apply(Province obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("owner", Owner, ValueWrite.Quoted);
        }
    }
    public class AddCore : IHistory<Province>
    {
        private DateTime evt;
        public string Core { get; set; }
        public AddCore(DateTime evt, string core)
        {
            this.evt = evt;
            this.Core = core;
        }

        public void Apply(Province obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("add_core", Core, ValueWrite.Quoted);
        }
    }

    public class ReligionChange : IHistory<Province>
    {
        private DateTime evt;
        public string Religion { get; set; }
        public ReligionChange(DateTime evt, string religion)
        {
            this.evt = evt;
            this.Religion = religion;
        }

        public void Apply(Province obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("religion", Religion);
        }
    }

    public class CultureChange : IHistory<Province>
    {
        private DateTime evt;
        public string Culture { get; set; }
        public CultureChange(DateTime evt, string culture)
        {
            this.evt = evt;
            this.Culture = culture;
        }

        public void Apply(Province obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("religion", Culture);
        }
    }

    public class TradeGoodsChange : IHistory<Province>
    {
        private DateTime evt;
        public string TradeGoods { get; set; }
        public TradeGoodsChange(DateTime evt, string tradeGoods)
        {
            this.evt = evt;
            this.TradeGoods = tradeGoods;
        }

        public void Apply(Province obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("trade_goods", TradeGoods);
        }
    }

    public class ManpowerChange : IHistory<Province>
    {
        private DateTime evt;
        public double Manpower { get; set; }
        public ManpowerChange(DateTime evt, double manpower)
        {
            this.evt = evt;
            this.Manpower = manpower;
        }

        public void Apply(Province obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("manpower", Manpower);
        }
    }

    public partial class Advisor : IHistory<Province>
    {
        private DateTime evt;
        public Advisor(DateTime evt)
            : this()
        {
            this.evt = evt;
        }

        public void Apply(Province obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }
    }

    public class DiscoveredBy : IHistory<Province>
    {
        private DateTime evt;
        public string Country { get; set; }
        public DiscoveredBy(DateTime evt, string country)
        {
            this.evt = evt;
            this.Country = country;
        }

        public void Apply(Province obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("owner", Country, ValueWrite.Quoted);
        }
    }

    public class AddClaim : IHistory<Province>
    {
        private DateTime evt;
        public string Claim { get; set; }
        public AddClaim(DateTime evt, string claim)
        {
            this.evt = evt;
            this.Claim = claim;
        }

        public void Apply(Province obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("add_claim", Claim, ValueWrite.Quoted);
        }
    }
}
