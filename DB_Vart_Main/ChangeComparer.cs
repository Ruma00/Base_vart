using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Vart_Main
{
    class ChangeComparer : IComparer<Dat>
    {
        public int Compare(Dat a, Dat b)
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
