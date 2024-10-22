using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using WPFModernVerticalMenu.Classes;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFModernVerticalMenu.Pages
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Page
    {
        public Dataclass data = new Dataclass();
        public Authorization()
        {
            InitializeComponent();
        }

        private void btn_Reg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Frame MainFrame = (Frame)Application.Current.Properties["MainFrame"];
            MainFrame.Navigate(new System.Uri("Pages/Registration.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool userFound = false;
            List<User> userss = new List<User>();

            userss = data.LoadUsers("users.json");

            foreach (User user in userss)
            {
                if (user.Email == tbEmail.Text && user.Password == tbPassword.Text)
                {
                    MessageBox.Show($"{user.Name} вошёл в систему");
                    userFound = true;
                    Application.Current.Properties["CurrentUser"] = user;
                    break;
                }

            }

            if (!userFound)
            {
                MessageBox.Show("Вы ввели неверный Email/Логин или Пароль.");
            }

            if (userFound)
            {
                Frame MainFrame = (Frame)Application.Current.Properties["MainFrame"];
                if (MainFrame != null)
                {
                    MainFrame.Navigate(new System.Uri("Pages/CreateReport.xaml", UriKind.RelativeOrAbsolute));
                }

            }
        }
    }
}
