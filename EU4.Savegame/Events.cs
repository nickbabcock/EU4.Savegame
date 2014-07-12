using Pdoxcl2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EU4.Savegame
{
    public class Events : IParadoxRead, IParadoxWrite
    {
        public IList<string> EventNames { get; set; }
        public IList<ParadoxId> EventIds { get; set; }

        public Events()
        {
            EventNames = new List<string>();
            EventIds = new List<ParadoxId>();
        }

        public void Write(ParadoxStreamWriter writer)
        {
            foreach (var evt in EventNames)
                writer.WriteLine("id", evt, ValueWrite.Quoted);

            foreach (var evt in EventIds)
                writer.Write("id", evt);
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            if (token != "id")
                throw new ApplicationException("Unrecognized token: " + token);

            if (parser.NextIsBracketed())
                EventIds.Add(parser.Parse(new ParadoxId()));
            else
                EventNames.Add(parser.ReadString());
        }
    }
}
