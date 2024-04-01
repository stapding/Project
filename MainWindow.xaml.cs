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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool RegStatus { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new Registration(MainFrame));
            RegStatus = false;
        }

        private void btn_Back_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (isRegistrated.Text == "T")
            {
                MainFrame.Navigate(new CreateReport(MainFrame));
            }
            else
            {
                MessageBox.Show("Для начала войдите в аккаунт или создайте его");
            }
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (MainFrame.Content.GetType() == typeof(Registration) || MainFrame.Content.GetType() == typeof(Authorization))
            {
                isRegistrated.Text = "F";
            }
        }
    }
}
