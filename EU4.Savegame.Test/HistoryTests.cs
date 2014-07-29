using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Pdoxcl2Sharp;
using System.IO;

namespace EU4.Savegame.Test
{
    [TestFixture]
    public class HistoryTests
    {
        [Test]
        public void HistoryGrouping()
        {
            var history = new ParaHistory();
            history.Add(new ParaEvent(new DateTime(1492, 1, 1), "val3"));
            history.Add(new ParaEvent(new DateTime(1, 1, 1), "val1"));
            history.Add(new ParaEvent(new DateTime(1, 1, 1), "val2"));
            history.Add(new ParaEvent(new DateTime(1492, 1, 1), "val4"));
            history.Add(new ParaEvent(new DateTime(1560, 1, 1), "val5"));

            string actual = string.Empty;
            using (var mem = new MemoryStream())
            {
                using (var writer = new ParadoxSaver(mem))
                    history.Write(writer);
                actual = Globals.ParadoxEncoding.GetString(mem.ToArray());
            }

            string expected = @"string=val1
string=val2
1492.1.1=
{
	string=val3
	string=val4
}
1560.1.1=
{
	string=val5
}
";
            Assert.AreEqual(expected, actual);
        }
    }

    public class ParaObj : IParadoxWrite
    {
        public void Write(ParadoxStreamWriter writer)
        {
        }
    }


    public class ParaHistory : History<ParaObj>
    {
        protected override IHistory<ParaObj> InnerToken(ParadoxParser parser, string token, DateTime date)
        {
            return new ParaEvent(date, parser.ReadString());
        }
    }

    public class ParaEvent : IHistory<ParaObj>
    {
        private readonly DateTime eventDate;

        public string Val { get; set; }

        public ParaEvent(DateTime evt, string val)
        {
            eventDate = evt;
            Val = val;
        }

        public void Apply(ParaObj val, bool advance)
        {
            throw new NotImplementedException();
        }

        public DateTime EventDate
        {
            get { return eventDate; }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            writer.WriteLine("string", Val);
        }
    }


}
