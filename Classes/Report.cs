using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFModernVerticalMenu.Classes
{
    public class Report
    {
        public double Total_income { get; set; }
        public double Total_expenses { get; set; }
        public List<double> Values_category { get; set; }
        public string Current_month { get; set; }
        public List<string> Bad_category { get; set; }
        public string Stringed_Bad_Categories { get; set; }
        public List<string> Ins_category { get; set; }
        public string Status { get; set; }
        public Report(double total_income, double total_expenses, List<double> values_category, string current_month, List<string> bad_category, string stringed_bad_categories, List<string> ins_category, string status)
        {
            Total_income = total_income;
            Total_expenses = total_expenses;
            Values_category = values_category;
            Current_month = current_month;
            Bad_category = bad_category;
            Stringed_Bad_Categories = stringed_bad_categories;
            Ins_category = ins_category;
            Status = status;
        }
    }
}
