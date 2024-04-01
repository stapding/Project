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
        public Dataclass data = new Dataclass();
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            bool userFound = false;
            List<User> userss = new List<User>();

            userss = data.LoadUsers(userss, "users.json");

            foreach (User user in userss)
            {
                if (user.Email == tbEmail.Text && user.Password == tbPassword.Text)
                {
                    MessageBox.Show($"{user.Name} вошёл в систему");
                    TextBlock userFI = (TextBlock)Application.Current.MainWindow.FindName("isRegistrated");
                    userFI.Text = user.Email;
                    userFound = true;
                    break;
                }

            }

            if (!userFound)
            {
                MessageBox.Show("Вы ввели неверный Email/Логин или Пароль.");
            }

            if (userFound)
            {
                if (MainFrame != null)
                {
                    MainFrame.Navigate(new CreateReport(MainFrame));
                }

            }
        }
    }
}
