using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pdoxcl2Sharp;

namespace EU4.Savegame
{
    public abstract class HistoricEvent : IParadoxWrite
    {
        private DateTime eventDate;
        public HistoricEvent(DateTime eventDate)
        {
            this.eventDate = eventDate;
        }

        public DateTime EventDate
        {
            get { return this.eventDate; }
            set { this.eventDate = value; }
        }

        public abstract void Write(ParadoxStreamWriter writer);
    }
}
