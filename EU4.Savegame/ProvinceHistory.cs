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
                case "name": return parser.Parse(new ProvinceNameChange(date));
                case "owner": return new OwnerChange(date, parser.ReadString());
                case "capital": return new CapitalNameChange(date, parser.ReadString());
                case "discovered_by": return new DiscoveredBy(date, parser.ReadString());
                case "add_core": return new AddCore(date, parser.ReadString());
                case "add_claim": return new AddClaim(date, parser.ReadString());
                case "remove_core": return new RemoveCore(date, parser.ReadString());
                case "remove_claim": return new RemoveClaim(date, parser.ReadString());
                case "religion": return new ReligionChange(date, parser.ReadString());
                case "trade_goods": return new TradeGoodsChange(date, parser.ReadString());
                case "culture": return new CultureChange(date, parser.ReadString());
                case "manpower": return new ManpowerChange(date, parser.ReadDouble());
                case "citysize": return new CitysizeChange(date, parser.ReadDouble());
                case "colonysize": return new ColonysizeChange(date, parser.ReadDouble());
                case "revolt_risk": return new RevoltRiskChange(date, parser.ReadDouble());
                case "native_size": return new NativeSizeChange(date, parser.ReadDouble());
                case "native_ferocity": return new NativeFerocityChange(date, parser.ReadInt32());
                case "native_hostileness": return new NativeHostilenessChange(date, parser.ReadInt32());
                case "advisor": return parser.Parse(new Advisor(date));
                case "hre": return new HreChange(date, parser.ReadBool());
                case "is_city": return new IsCityChange(date, parser.ReadBool());
                case "base_tax": return new BaseTaxChange(date, parser.ReadDouble());
                default:
                    if (parser.ReadBool())
                        return new BuildingChange(date, token);
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

    public partial class ProvinceNameChange : IHistory<Province>
    {
        private readonly DateTime evt;
        public ProvinceNameChange(DateTime evt)
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

    public class CapitalNameChange : IHistory<Province>
    {
        private DateTime evt;
        public string Capital { get; set; }
        public CapitalNameChange(DateTime evt, string capital)
        {
            this.evt = evt;
            this.Capital = capital;
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
            writer.WriteLine("capital", Capital, ValueWrite.Quoted);
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

    public class RemoveCore : IHistory<Province>
    {
        private DateTime evt;
        public string Core { get; set; }
        public RemoveCore(DateTime evt, string core)
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
            writer.WriteLine("remove_core", Core, ValueWrite.Quoted);
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

    public class CitysizeChange : IHistory<Province>
    {
        private DateTime evt;
        public double Citysize { get; set; }
        public CitysizeChange(DateTime evt, double citysize)
        {
            this.evt = evt;
            this.Citysize = citysize;
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
            writer.WriteLine("citysize", Citysize);
        }
    }

    public class ColonysizeChange : IHistory<Province>
    {
        private DateTime evt;
        public double Colonysize { get; set; }
        public ColonysizeChange(DateTime evt, double colonysize)
        {
            this.evt = evt;
            this.Colonysize = colonysize;
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
            writer.WriteLine("colonysize", Colonysize);
        }
    }

    public class RevoltRiskChange : IHistory<Province>
    {
        private DateTime evt;
        public double RevoltRisk { get; set; }
        public RevoltRiskChange(DateTime evt, double revoltRisk)
        {
            this.evt = evt;
            this.RevoltRisk = revoltRisk;
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
            writer.WriteLine("revolt_risk", RevoltRisk);
        }
    }

    public class BaseTaxChange : IHistory<Province>
    {
        private DateTime evt;
        public double BaseTax { get; set; }
        public BaseTaxChange(DateTime evt, double baseTax)
        {
            this.evt = evt;
            this.BaseTax = baseTax;
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
            writer.WriteLine("base_tax", BaseTax);
        }
    }

    public class HreChange : IHistory<Province>
    {
        private DateTime evt;
        public bool IsHre { get; set; }
        public HreChange(DateTime evt, bool isHre)
        {
            this.evt = evt;
            this.IsHre = isHre;
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
            writer.WriteLine("hre", IsHre);
        }
    }

    public class IsCityChange : IHistory<Province>
    {
        private DateTime evt;
        public bool IsCity { get; set; }
        public IsCityChange(DateTime evt, bool isCity)
        {
            this.evt = evt;
            this.IsCity = isCity;
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
            writer.WriteLine("is_city", IsCity);
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

    public class RemoveClaim : IHistory<Province>
    {
        private DateTime evt;
        public string Claim { get; set; }
        public RemoveClaim(DateTime evt, string claim)
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
            writer.WriteLine("remove_claim", Claim, ValueWrite.Quoted);
        }
    }

    public class NativeSizeChange : IHistory<Province>
    {
        private DateTime evt;
        public double Size { get; set; }
        public NativeSizeChange(DateTime evt, double size)
        {
            this.evt = evt;
            this.Size = size;
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
            writer.WriteLine("native_size", Size);
        }
    }

    public class NativeFerocityChange : IHistory<Province>
    {
        private DateTime evt;
        public int Ferocity { get; set; }
        public NativeFerocityChange(DateTime evt, int ferocity)
        {
            this.evt = evt;
            this.Ferocity = ferocity;
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
            writer.WriteLine("native_ferocity", Ferocity);
        }
    }

    public class NativeHostilenessChange : IHistory<Province>
    {
        private DateTime evt;
        public int Hostileness { get; set; }
        public NativeHostilenessChange(DateTime evt, int hostileness)
        {
            this.evt = evt;
            this.Hostileness = hostileness;
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
            writer.WriteLine("native_hostileness", Hostileness);
        }
    }
}
