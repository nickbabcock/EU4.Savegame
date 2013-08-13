using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pdoxcl2Sharp;

namespace EU4.Savegame
{
    public class LedgerData : IParadoxRead, IParadoxWrite
    {
        private string name;
        private IList<int> ydata;
        private IList<int> xdata;

        public LedgerData()
        {
            this.ydata = new List<int>();
            this.xdata = new List<int>();
        }

        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        public IList<int> XData
        {
            get { return this.xdata; }
            set { this.xdata = value; }
        }

        public IList<int> YData
        {
            get { return this.ydata; }
            set { this.ydata = value; }
        }

        public void TokenCallback(ParadoxParser parser, string token)
        {
            switch (token)
            {
                case "name": this.name = parser.ReadString(); break;
                case "ledger_data_y": this.ydata = parser.ReadIntList(); break;
                case "ledger_data_x": this.xdata = parser.ReadIntList(); break;
            }
        }

        public void Write(ParadoxStreamWriter writer)
        {
            WriteLedgerList(writer, this.ydata, "ledger_data_y");
            WriteLedgerList(writer, this.xdata, "ledger_data_x");
            writer.WriteLine("name", this.name, ValueWrite.Quoted | ValueWrite.LeadingTabs);
        }

        /// <summary>
        /// Creates a list of the paired x and y data
        /// </summary>
        /// <returns>A list of data that is paired (x, y)</returns>
        public IList<Tuple<int, int>> GetData()
        {
            IList<Tuple<int, int>> result = new List<Tuple<int, int>>(this.xdata.Count);
            for (int i = 0; i < this.xdata.Count; i++)
            {
                result.Add(Tuple.Create(this.xdata[i], this.ydata[i]));
            }

            return result;
        }

        private static void WriteLedgerList(ParadoxStreamWriter writer, IList<int> list, string listName)
        {
            writer.WriteLine(listName + '=', ValueWrite.LeadingTabs);
            writer.WriteLine("{", ValueWrite.LeadingTabs);
            foreach (var val in list)
            {
                writer.Write(val.ToString());
                writer.Write(" ");
            }

            writer.Write("\t");
            writer.WriteLine("}", ValueWrite.LeadingTabs);
        }
    }
}
