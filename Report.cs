using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public class Report
    {
        public double Total_income { get; set; }
        public List<double> Values_category { get; set; }
        public string Current_month { get; set; }
        public List<string> Bad_category { get; set; } 
        public Report(double income, List<double> values, string month, List<string> bads)
        {
            Total_income = income;
            Values_category = values;
            Current_month = month;
            Bad_category = bads;
        }
    }
}
