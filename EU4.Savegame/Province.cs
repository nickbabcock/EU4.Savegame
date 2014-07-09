using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pdoxcl2Sharp;

namespace EU4.Savegame
{
    public class Province : IParadoxRead, IParadoxWrite
    {
        public TopFlags Flags { get; set; }
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Controller { get; set; }
        public string OriginalColoniser { get; set; }
        public IList<string> Core { get; set; }
        public IList<string> Claims { get; set; }
        public string Trade { get; set; }
        public FabricateClaim FabricateClaim { get; set; }
        public IList<ParadoxId> Units { get; set; }
        public string Culture { get; set; }
        public string Religion { get; set; }
        public string Capital { get; set; }
        public bool IsCity { get; set; }
        public double? Colonysize { get; set; }
        public double? Garrison { get; set; }
        public double? Siege { get; set; }
        public double? BaseTax { get; set; }
        public double? OriginalTax { get; set; }
        public double? Manpower { get; set; }
        public double? RevoltRisk { get; set; }
        public string LikelyRebels { get; set; }
        public double? ForeignSupportForRebels { get; set; }
        public double? MissionaryProgress { get; set; }
        public bool? Hre { get; set; }
        public string TradeGoods { get; set; }
        public bool Blockade { get; set; }
        public double BlockadeEfficiency { get; set; }
        public ProvinceHistory History { get; set; }
        public int Patrol { get; set; }
        public IList<DateTime> DiscoveryDates { get; set; }
        public IList<DateTime> DiscoveryReligionDates { get; set; }
        public IList<string> DiscoveredBy { get; set; }
        public double NativeSize { get; set; }
        public int NativeFerocity { get; set; }
        public int NativeHostileness { get; set; }
        public int? Nationalism { get; set; }
        public IList<Modifier> Modifiers { get; set; }
        public IList<MilitaryConstruction> MilitaryConstructions { get; set; }
        public IList<MerchantConstruction> MerchantConstructions { get; set; }
        public IList<DiplomacyConstruction> DiplomacyConstructions { get; set; }
        public BuildingConstruction BuildingConstruction { get; set; }
        public BuildCoreConstruction BuildCoreConstruction { get; set; }
        public MissionaryConstruction MissionaryConstruction { get; set; }
        public MissionaryConstruction ColonyConstruction { get; set; }
        public double TradePower { get; set; }
        public ParadoxId RebelFaction { get; set; }
        public bool HreLiberated { get; set; }
        public bool UserChangedName { get; set; }
        public IList<string> Buildings { get; set; }

        public Province(int id)
        {
            Id = id;
            Core = new List<string>();
            Claims = new List<string>();
            Units = new List<ParadoxId>();
            Modifiers = new List<Modifier>();
            MilitaryConstructions = new List<MilitaryConstruction>();
            MerchantConstructions = new List<MerchantConstruction>();
            DiplomacyConstructions = new List<DiplomacyConstruction>();
            Buildings = new List<string>();
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            switch (token)
            {
                case "flags": Flags = parser.Parse(new TopFlags()); break;
                case "name": Name = parser.ReadString(); break;
                case "owner": Owner = parser.ReadString(); break;
                case "controller": Controller = parser.ReadString(); break;
                case "original_coloniser": OriginalColoniser = parser.ReadString(); break;
                case "core": Core.Add(parser.ReadString()); break;
                case "claim": Claims.Add(parser.ReadString()); break;
                case "trade": Trade = parser.ReadString(); break;
                case "fabricate_claim": FabricateClaim = parser.Parse(new FabricateClaim()); break;
                case "unit": Units.Add(parser.Parse(new ParadoxId())); break;
                case "culture": Culture = parser.ReadString(); break;
                case "religion": Religion = parser.ReadString(); break;
                case "capital": Capital = parser.ReadString(); break;
                case "is_city": IsCity = parser.ReadBool(); break;
                case "colonysize": Colonysize = parser.ReadDouble(); break;
                case "garrison": Garrison = parser.ReadDouble(); break;
                case "siege": Siege = parser.ReadDouble(); break;
                case "base_tax": BaseTax = parser.ReadDouble(); break;
                case "original_tax": OriginalTax = parser.ReadDouble(); break;
                case "manpower": Manpower = parser.ReadDouble(); break;
                case "revolt_risk": RevoltRisk = parser.ReadDouble(); break;
                case "likely_rebels": LikelyRebels = parser.ReadString(); break;
                case "foreign_support_for_rebels": ForeignSupportForRebels = parser.ReadDouble(); break;
                case "missionary_progress": MissionaryProgress = parser.ReadDouble(); break;
                case "hre": Hre = parser.ReadBool(); break;
                case "trade_goods": TradeGoods = parser.ReadString(); break;
                case "blockade": Blockade = parser.ReadBool(); break;
                case "blockade_efficiency": BlockadeEfficiency = parser.ReadDouble(); break;
                case "history": History = parser.Parse(new ProvinceHistory()); break;
                case "patrol": Patrol = parser.ReadInt32(); break;
                case "discovery_dates": DiscoveryDates = parser.ReadDateTimeList(); break;
                case "discovery_religion_dates": DiscoveryReligionDates = parser.ReadDateTimeList(); break;
                case "discovered_by": DiscoveredBy = parser.ReadStringList(); break;
                case "native_size": NativeSize = parser.ReadDouble(); break;
                case "native_ferocity": NativeFerocity = parser.ReadInt32(); break;
                case "native_hostileness": NativeHostileness = parser.ReadInt32(); break;
                case "nationalism": Nationalism = parser.ReadInt32(); break;
                case "modifier": Modifiers.Add(parser.Parse(new Modifier())); break;
                case "military_construction": MilitaryConstructions.Add(parser.Parse(new MilitaryConstruction())); break;
                case "merchant_construction": MerchantConstructions.Add(parser.Parse(new MerchantConstruction())); break;
                case "diplomacy_construction": DiplomacyConstructions.Add(parser.Parse(new DiplomacyConstruction())); break;
                case "building_construction": BuildingConstruction = parser.Parse(new BuildingConstruction()); break;
                case "build_core_construction": BuildCoreConstruction = parser.Parse(new BuildCoreConstruction()); break;
                case "missionary_construction": MissionaryConstruction = parser.Parse(new MissionaryConstruction()); break;
                case "colony_construction": ColonyConstruction = parser.Parse(new MissionaryConstruction()); break;
                case "trade_power": TradePower = parser.ReadDouble(); break;
                case "rebel_faction": RebelFaction = parser.Parse(new ParadoxId()); break;
                case "hre_liberated": HreLiberated = parser.ReadBool(); break;
                case "user_changed_name": UserChangedName = parser.ReadBool(); break;
                default:
                    if (parser.ReadBool())
                        Buildings.Add(token);
                    break;
            }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            if (Flags != null)
            {
                writer.Write("flags", Flags);
            }
            if (Name != null)
            {
                writer.WriteLine("name", Name, ValueWrite.Quoted);
            }
            if (Owner != null)
            {
                writer.WriteLine("owner", Owner, ValueWrite.Quoted);
            }
            if (Controller != null)
            {
                writer.WriteLine("controller", Controller, ValueWrite.Quoted);
            }
            if (OriginalColoniser != null)
            {
                writer.WriteLine("original_coloniser", OriginalColoniser, ValueWrite.Quoted);
            }
            foreach (var val in Core)
            {
                writer.WriteLine("core", val, ValueWrite.Quoted);
            }
            foreach (var val in Claims)
            {
                writer.WriteLine("claim", val, ValueWrite.Quoted);
            }
            if (Trade != null)
            {
                writer.WriteLine("trade", Trade, ValueWrite.Quoted);
            }
            if (FabricateClaim != null)
            {
                writer.Write("fabricate_claim", FabricateClaim);
            }
            foreach (var val in Units)
            {
                writer.Write("unit", val);
            }
            if (Culture != null)
            {
                writer.WriteLine("culture", Culture);
            }
            if (Religion != null)
            {
                writer.WriteLine("religion", Religion);
            }
            if (Capital != null)
            {
                writer.WriteLine("capital", Capital, ValueWrite.Quoted);
            }
            writer.WriteLine("is_city", IsCity);
            if (Colonysize.HasValue)
            {
                writer.WriteLine("colonysize", Colonysize.Value);
            }
            if (Garrison.HasValue)
            {
                writer.WriteLine("garrison", Garrison.Value);
            }
            if (Siege.HasValue)
            {
                writer.WriteLine("siege", Siege.Value);
            }
            if (BaseTax.HasValue)
            {
                writer.WriteLine("base_tax", BaseTax.Value);
            }
            if (OriginalTax.HasValue)
            {
                writer.WriteLine("original_tax", OriginalTax.Value);
            }
            if (Manpower.HasValue)
            {
                writer.WriteLine("manpower", Manpower.Value);
            }
            if (RevoltRisk.HasValue)
            {
                writer.WriteLine("revolt_risk", RevoltRisk.Value);
            }
            if (LikelyRebels != null)
            {
                writer.WriteLine("likely_rebels", LikelyRebels);
            }
            if (ForeignSupportForRebels.HasValue)
            {
                writer.WriteLine("foreign_support_for_rebels", ForeignSupportForRebels.Value);
            }
            if (MissionaryProgress.HasValue)
            {
                writer.WriteLine("missionary_progress", MissionaryProgress.Value);
            }
            if (Hre.HasValue)
            {
                writer.WriteLine("hre", Hre.Value);
            }
            if (TradeGoods != null)
            {
                writer.WriteLine("trade_goods", TradeGoods);
            }
            writer.WriteLine("blockade", Blockade);
            writer.WriteLine("blockade_efficiency", BlockadeEfficiency);
            foreach (var building in Buildings)
            {
                writer.WriteLine(building, true);
            }
            if (History != null)
            {
                writer.Write("history", History);
            }
            writer.WriteLine("patrol", Patrol);
            if (DiscoveryDates != null)
            {
                writer.Write("discovery_dates={ ");
                foreach (var val in DiscoveryDates)
                {
                    writer.Write(val);
                    writer.Write(" ");
                }
                writer.WriteLine("}");
            }
            if (DiscoveryReligionDates != null)
            {
                writer.Write("discovery_religion_dates={ ");
                foreach (var val in DiscoveryReligionDates)
                {
                    writer.Write(val);
                    writer.Write(" ");
                }
                writer.WriteLine("}");
            }
            if (DiscoveredBy != null)
            {
                writer.Write("discovered_by={ ");
                foreach (var val in DiscoveredBy)
                {
                    writer.Write(val);
                    writer.Write(" ");
                }
                writer.WriteLine("}");
            }
            writer.WriteLine("native_size", NativeSize);
            writer.WriteLine("native_ferocity", NativeFerocity);
            writer.WriteLine("native_hostileness", NativeHostileness);
            if (Nationalism.HasValue)
            {
                writer.WriteLine("nationalism", Nationalism.Value);
            }
            foreach (var val in Modifiers)
            {
                writer.Write("modifier", val);
            }
            foreach (var val in MilitaryConstructions)
            {
                writer.Write("military_construction", val);
            }
            foreach (var val in MerchantConstructions)
            {
                writer.Write("merchant_construction", val);
            }
            foreach (var val in DiplomacyConstructions)
            {
                writer.Write("diplomacy_construction", val);
            }
            if (BuildingConstruction != null)
            {
                writer.Write("building_construction", BuildingConstruction);
            }
            if (BuildCoreConstruction != null)
            {
                writer.Write("build_core_construction", BuildCoreConstruction);
            }
            if (MissionaryConstruction != null)
            {
                writer.Write("missionary_construction", MissionaryConstruction);
            }
            if (ColonyConstruction != null)
            {
                writer.Write("colony_construction", ColonyConstruction);
            }
            writer.WriteLine("trade_power", TradePower);
            if (RebelFaction != null)
            {
                writer.Write("rebel_faction", RebelFaction);
            }
            writer.WriteLine("hre_liberated", HreLiberated);
            writer.WriteLine("user_changed_name", UserChangedName);
        }
    }
}
