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
using WPFModernVerticalMenu.Classes;

namespace WPFModernVerticalMenu.Pages
{
    /// <summary>
    /// Логика взаимодействия для ListReports.xaml
    /// </summary>
    public partial class ListReports : Page
    {
        public List<Report> Reports { get; set; }
        public User currentUser { get; set; }
        public Dataclass data = new Dataclass();
        public ListReports()
        {
            InitializeComponent();
            currentUser = (User)Application.Current.Properties["CurrentUser"];
            currentUser = data.GetUserByEmail(currentUser.Email, "users.json");
            if (currentUser.Reports != null && currentUser.Reports.Count > 0)
            {
                foreach (var report in currentUser.Reports)
                {
                    listOfRequests.Items.Add(report);
                }
            }
            DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (currentUser.Reports != null && currentUser.Reports.Count > 0)
            {
                if (listOfRequests.SelectedItem != null)
                {
                    data.DeleteReport(currentUser, (Report)listOfRequests.SelectedItem);
                    listOfRequests.Items.Clear();
                    currentUser = (User)Application.Current.Properties["CurrentUser"];
                    currentUser = data.GetUserByEmail(currentUser.Email, "users.json");
                    foreach (var report in currentUser.Reports)
                    {
                        listOfRequests.Items.Add(report);
                    }
                    MessageBox.Show("Отчёт удалён");
                }
                else
                {
                    MessageBox.Show("Выберите отчёт");
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (currentUser.Reports != null && currentUser.Reports.Count > 0)
            {
                if (listOfRequests.SelectedItem != null)
                {
                    Frame MainFrame = (Frame)Application.Current.Properties["MainFrame"];
                    MainFrame.Navigate(new AboutReport((Report)listOfRequests.SelectedItem));
                }
                else
                {
                    MessageBox.Show("Выберите отчёт");
                }
            }
        }
    }
}
