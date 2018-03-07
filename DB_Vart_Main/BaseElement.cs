using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DB_Vart_Main
{
    class BaseElement
    {
        public string Adress { get; set; }
        public string Surname { get; set; }
        public string Contract { get; set; }
        public string Debt { get; set; }
        public string Section { get; set; }
        public string Apartment { get; set; }
        public string Monthly_fee { get; set; }
        public string Notice { get; set; }

        public BaseElement() { }
    }
}
