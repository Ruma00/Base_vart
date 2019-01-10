using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Vart_Main
{
    class YearComparer : IComparer<PayInf>
    {
        public int Compare(PayInf a, PayInf b)
        {
            if (a.Date > b.Date)
            {
                return 1;
            }
            else if (a.Date < b.Date)
            {
                return -1;
            }

            return 0;
        }
    }
}
