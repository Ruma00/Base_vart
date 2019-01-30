using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DB_Vart_Main
{
    static class Program
    {
        public static Main_form form;
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            form = new Main_form();
            Application.Run(form);
        }

        public static bool CheckInputDate(string date)
        {
            List<string> pattern = new List<string>();
            pattern.Add(@"\d{2}\.\d{2}\.d{4}");
            pattern.Add(@"\d{2}\.\d{2}\.d{2}");
            pattern.Add(@"\d{2}/d{2}/d{4}");
            pattern.Add(@"\d{2}/d{2}/d{2}");
            pattern.Add(@"\d{2}[-]d{2}[-]d{4}");
            pattern.Add(@"\d{2}[-]d{2}[-]d{2}");

            foreach (string s in pattern)
                if (Regex.IsMatch(date, s))
                    return true;
            return false;
        }

        public static bool CheckInputNum(string number)
        {
            string pattern = @"\d{" + number.Length + "}";

            if (Regex.IsMatch(number, pattern))
                return true;
            return false;
        }
    }
}
