using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Vart_Main
{
    class Dat
    {
        
        public int Fee { get; set; }
        public double FeeD { get; set; }
        public DateTime Date { get; set; }

        public Dat() { }

        public Dat(int fee, DateTime date)
        {
            Fee = fee;
            Date = date;
        }
    }
}
