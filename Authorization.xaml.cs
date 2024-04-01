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

namespace Project
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        public Frame MainFrame { get; set; }
        public Authorization(Frame mf)
        {
            InitializeComponent();
            MainFrame = mf;
        }

        private void btn_Reg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Navigate(new Registration(MainFrame));
        }
    }
}
