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
        public List<string> Ins_category { get; set; }
        public Report(double total_income, List<double> values_category, string current_month, List<string> bad_category, List<string> ins_category)
        {
            Total_income = total_income;
            Values_category = values_category;
            Current_month = current_month;
            Bad_category = bad_category;
            Ins_category = ins_category;
        }
    }
}
