using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class CountryHistory : History<Country>
    {
        protected override IHistory<Country> InnerToken(ParadoxParser parser, string token, DateTime date)
        {
            switch (token)
            {
                case "leader": return parser.Parse(new Leader(date));
                case "monarch": return parser.Parse(new Monarch(date));
                case "heir": return parser.Parse(new Heir(date));
                case "government": return new GovernmentChange(date, parser.ReadString());
                case "national_focus": return new NationalFocusChange(date, parser.ReadString());
                case "primary_culture": return new PrimaryCultureChange(date, parser.ReadString());
                case "religion": return new CountryReligionChange(date, parser.ReadString());
                case "technology_group": return new TechnologyGroupChange(date, parser.ReadString());
                case "unit_type": return new UnitTypeChange(date, parser.ReadString());
                case "mercantilism": return new MercantilismChange(date, parser.ReadDouble());
                case "elector": return new ElectorChange(date, parser.ReadBool());
                case "capital": return new CountryCapitalChange(date, parser.ReadInt32());
                case "fixed_capital": return new FixedCapitalChange(date, parser.ReadInt32());
                case "adm_tech": return new AdmTechChange(date, parser.ReadInt32());
                case "dip_tech": return new DipTechChange(date, parser.ReadInt32());
                case "mil_tech": return new MilTechChange(date, parser.ReadInt32());
                case "decision": return new DecisionChange(date, parser.ReadString());
                case "union": return new UnionChange(date, parser.ReadInt32());
                case "faction": return new AddFaction(date, parser.ReadString());
                case "add_accepted_culture": return new AddAcceptedCulture(date, parser.ReadString());
                case "remove_accepted_culture": return new RemoveAcceptedCulture(date, parser.ReadString());
                case "historical_friend": return new HistoricalFriendChange(date, parser.ReadString());
                case "historical_rival": return new HistoricalRivalChange(date, parser.ReadString());
                case "bureaucrats": return new BureaucratsChange(date, parser.ReadString());
                case "enuchs": return new EnuchsChange(date, parser.ReadString());
                case "temples": return new TemplesChange(date, parser.ReadString());
                case "changed_tag_from": return new ChangedTagFrom(date, parser.ReadString());
                default:
                    if (parser.ReadBool())
                        return new IdeaChange(date, token);
                    throw new ApplicationException("Unrecognized token: " + token);
            }
        }
    }

    public partial class Leader : IHistory<Country>
    {
        private DateTime evt;
        public Leader(DateTime evt)
        {
            this.evt = evt;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }
    }

    public partial class Monarch : IHistory<Country>
    {
        private DateTime evt;
        public Monarch(DateTime evt)
        {
            this.evt = evt;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }
    }

    public partial class Heir : IHistory<Country>
    {
        private DateTime evt;
        public Heir(DateTime evt)
        {
            this.evt = evt;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }
    }

    public class NationalFocusChange : IHistory<Country>
    {
        private DateTime evt;
        public string NationalFocus { get; set; }
        public NationalFocusChange(DateTime evt, string nationalFocus)
        {
            this.evt = evt;
            this.NationalFocus = nationalFocus;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("national_focus", NationalFocus);
        }
    }

    public class GovernmentChange : IHistory<Country>
    {
        private DateTime evt;
        public string Government { get; set; }
        public GovernmentChange(DateTime evt, string government)
        {
            this.evt = evt;
            this.Government = government;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("government", Government);
        }
    }

    public class IdeaChange : IHistory<Country>
    {
        private DateTime evt;
        public string Idea { get; set; }
        public IdeaChange(DateTime evt, string idea)
        {
            this.evt = evt;
            this.Idea = idea;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine(Idea, true);
        }
    }

    public class PrimaryCultureChange : IHistory<Country>
    {
        private DateTime evt;
        public string PrimaryCulture { get; set; }
        public PrimaryCultureChange(DateTime evt, string primaryCulture)
        {
            this.evt = evt;
            this.PrimaryCulture = primaryCulture;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("primary_culture", PrimaryCulture);
        }
    }

    public class CountryReligionChange : IHistory<Country>
    {
        private DateTime evt;
        public string Religion { get; set; }
        public CountryReligionChange(DateTime evt, string religion)
        {
            this.evt = evt;
            this.Religion = religion;
        }

        public void Apply(Country obj, bool advance)
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

    public class TechnologyGroupChange : IHistory<Country>
    {
        private DateTime evt;
        public string TechnologyGroup { get; set; }
        public TechnologyGroupChange(DateTime evt, string technologyGroup)
        {
            this.evt = evt;
            this.TechnologyGroup = technologyGroup;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("technology_group", TechnologyGroup);
        }
    }
    public class UnitTypeChange : IHistory<Country>
    {
        private DateTime evt;
        public string UnitType { get; set; }
        public UnitTypeChange(DateTime evt, string unitType)
        {
            this.evt = evt;
            this.UnitType = unitType;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("unit_type", UnitType);
        }
    }

    public class MercantilismChange : IHistory<Country>
    {
        private DateTime evt;
        public double Mercantilism { get; set; }
        public MercantilismChange(DateTime evt, double mercantilism)
        {
            this.evt = evt;
            this.Mercantilism = mercantilism;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("mercantilism", Mercantilism);
        }
    }

    public class ElectorChange : IHistory<Country>
    {
        private DateTime evt;
        public bool IsElector { get; set; }
        public ElectorChange(DateTime evt, bool isElector)
        {
            this.evt = evt;
            this.IsElector = isElector;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("elector", IsElector);
        }
    }

    public class CountryCapitalChange : IHistory<Country>
    {
        private DateTime evt;
        public int Capital { get; set; }
        public CountryCapitalChange(DateTime evt, int capital)
        {
            this.evt = evt;
            this.Capital = capital;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("capital", Capital);
        }
    }

    public class FixedCapitalChange : IHistory<Country>
    {
        private DateTime evt;
        public int FixedCapital { get; set; }
        public FixedCapitalChange(DateTime evt, int fixedCapital)
        {
            this.evt = evt;
            this.FixedCapital = fixedCapital;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("fixed_capital", FixedCapital);
        }
    }

    public class AdmTechChange : IHistory<Country>
    {
        private DateTime evt;
        public int Tech { get; set; }
        public AdmTechChange(DateTime evt, int tech)
        {
            this.evt = evt;
            this.Tech = tech;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("adm_tech", Tech);
        }
    }

    public class DipTechChange : IHistory<Country>
    {
        private DateTime evt;
        public int Tech { get; set; }
        public DipTechChange(DateTime evt, int tech)
        {
            this.evt = evt;
            this.Tech = tech;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("dip_tech", Tech);
        }
    }

    public class MilTechChange : IHistory<Country>
    {
        private DateTime evt;
        public int Tech { get; set; }
        public MilTechChange(DateTime evt, int tech)
        {
            this.evt = evt;
            this.Tech = tech;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("mil_tech", Tech);
        }
    }

    public class UnionChange : IHistory<Country>
    {
        private DateTime evt;
        public int Union { get; set; }
        public UnionChange(DateTime evt, int union)
        {
            this.evt = evt;
            this.Union = union;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("union", Union);
        }
    }

    public class DecisionChange : IHistory<Country>
    {
        private DateTime evt;
        public string Decision { get; set; }
        public DecisionChange(DateTime evt, string decision)
        {
            this.evt = evt;
            this.Decision = decision;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("decision", Decision, ValueWrite.Quoted);
        }
    }

    public class HistoricalFriendChange : IHistory<Country>
    {
        private DateTime evt;
        public string Friend { get; set; }
        public HistoricalFriendChange(DateTime evt, string friend)
        {
            this.evt = evt;
            this.Friend = friend;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("historical_friend", Friend, ValueWrite.Quoted);
        }
    }

    public class HistoricalRivalChange : IHistory<Country>
    {
        private DateTime evt;
        public string Rival { get; set; }
        public HistoricalRivalChange(DateTime evt, string rival)
        {
            this.evt = evt;
            this.Rival = rival;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("historical_rival", Rival, ValueWrite.Quoted);
        }
    }

    public class BureaucratsChange : IHistory<Country>
    {
        private DateTime evt;
        public string Faction { get; set; }
        public BureaucratsChange(DateTime evt, string faction)
        {
            this.evt = evt;
            this.Faction = faction;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("bureaucrats", Faction, ValueWrite.Quoted);
        }
    }

    public class EnuchsChange : IHistory<Country>
    {
        private DateTime evt;
        public string Faction { get; set; }
        public EnuchsChange(DateTime evt, string faction)
        {
            this.evt = evt;
            this.Faction = faction;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("enuchs", Faction, ValueWrite.Quoted);
        }
    }

    public class TemplesChange : IHistory<Country>
    {
        private DateTime evt;
        public string Faction { get; set; }
        public TemplesChange(DateTime evt, string faction)
        {
            this.evt = evt;
            this.Faction = faction;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("temples", Faction, ValueWrite.Quoted);
        }
    }

    public class ChangedTagFrom : IHistory<Country>
    {
        private DateTime evt;
        public string Tag { get; set; }
        public ChangedTagFrom(DateTime evt, string tag)
        {
            this.evt = evt;
            this.Tag = tag;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("changed_tag_from", Tag, ValueWrite.Quoted);
        }
    }

    public class AddFaction : IHistory<Country>
    {
        private DateTime evt;
        public string Faction { get; set; }
        public AddFaction(DateTime evt, string faction)
        {
            this.evt = evt;
            this.Faction = faction;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("faction", Faction);
        }
    }

    public class AddAcceptedCulture : IHistory<Country>
    {
        private DateTime evt;
        public string Culture { get; set; }
        public AddAcceptedCulture(DateTime evt, string culture)
        {
            this.evt = evt;
            this.Culture = culture;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("add_accepted_culture", Culture);
        }
    }

    public class RemoveAcceptedCulture : IHistory<Country>
    {
        private DateTime evt;
        public string Culture { get; set; }
        public RemoveAcceptedCulture(DateTime evt, string culture)
        {
            this.evt = evt;
            this.Culture = culture;
        }

        public void Apply(Country obj, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return evt; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("remove_accepted_culture", Culture);
        }
    }
}
