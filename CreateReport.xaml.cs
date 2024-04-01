using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;


namespace Project
{
    /// <summary>
    /// Логика взаимодействия для CreateReport.xaml
    /// </summary>
    public partial class CreateReport : Page
    {
        public Frame MainFrame { get; set; }
        public CreateReport(Frame mf)
        {
            InitializeComponent();
            MainFrame = mf;
            CultureInfo russianCulture = new CultureInfo("ru-RU");
            DateTime now = DateTime.Now;
            string monthName = now.ToString("MMMM", russianCulture);
            tbDate.Text = monthName;
        }
    }
}
