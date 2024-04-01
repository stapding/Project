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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        public Dataclass data = new Dataclass();
        public Frame MainFrame { get; set; }
        public Registration(Frame mf)
        {
            InitializeComponent();
            MainFrame = mf;
        }

        private void btn_Auth_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainFrame.Navigate(new Authorization(MainFrame));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string email = tbEmail.Text;
            string password = tbPassword.Text;
            string againpassword = tbPasswordAgain.Text;
            string name = tbName.Text;
            string income = tbIncome.Text;
            if (!data.ValidateFields(email, password, againpassword, name, income))
            {
                MessageBox.Show("Поля пустые");
                return;
            }
            if (!data.ValidateEmail(email))
            {
                MessageBox.Show("Неверный формат Email'а");
                return;
            }
            if (!data.ValidatePassword(password, againpassword))
            {
                MessageBox.Show("Неверный формат пароля или пароли не совпадают");
                return;
            }
            if (!data.ValidateName(name))
            {
                MessageBox.Show("Имя должно быть < 20 символов");
                return;
            }
            if (!data.ValidateIncome(income))
            {
                MessageBox.Show("Неверный формат дохода");
                return;
            }
            double convertedIncome = Double.Parse(income);
            data.RegistrateUser(email, password, name, convertedIncome, "users.json");
            MainFrame.Navigate(new Authorization(MainFrame));
        }
    }
}
