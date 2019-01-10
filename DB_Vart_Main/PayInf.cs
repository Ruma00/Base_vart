using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Vart_Main
{
    class PayInf
    {
        public DateTime Date { get; set; }
        public double Pay { get; set; }
        public string Notice { get; set; }

        public PayInf(DateTime date, double pay)
        {
            Date = date;
            Pay = pay;
            Notice = "";
        }

        public PayInf(DateTime date, double pay, string notice)
        {
            Date = date;
            Pay = pay;
            Notice = notice;
        }
    }
}
