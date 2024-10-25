using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
    /// Логика взаимодействия для EditProfile.xaml
    /// </summary>
    public partial class EditProfile : Page
    {
        public User currentUser { get; set; }
        public Dataclass data = new Dataclass();
        public EditProfile()
        {
            InitializeComponent();
            currentUser = (User)Application.Current.Properties["CurrentUser"];
            currentUser = data.GetUserByEmail(currentUser.Email);
            tbAvatar.Text = currentUser.ImageAvatar;
            tbName.Text = currentUser.Name;
            tbIncome.Text = currentUser.Income.ToString();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg";
            if (openFileDialog.ShowDialog() == true)
            {
                if (File.Exists(openFileDialog.FileName))
                {
                    tbAvatar.Text = openFileDialog.FileName;
                }
                else
                {
                    MessageBox.Show("Выбранный файл не существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (data.ValidateIncome(tbIncome.Text) && data.ValidateName(tbName.Text))
            {
                // Проверяем, что путь указывает на файл с допустимым расширением
                string filePath = tbAvatar.Text;
                string fileExtension = System.IO.Path.GetExtension(filePath).ToLower();

                if (fileExtension == ".png" || fileExtension == ".jpeg" || fileExtension == ".jpg")
                {
                    List<User> users = data.LoadUsers("users.json");
                    foreach (User user in users)
                    {
                        if (user.ID == currentUser.ID)
                        {
                            user.Income = double.Parse(tbIncome.Text);
                            user.Name = tbName.Text;
                            user.ImageAvatar = tbAvatar.Text;
                            break;
                        }
                    }

                    using (FileStream fs1 = new FileStream("users.json", FileMode.Truncate))
                    {
                        JsonSerializer.Serialize(fs1, users);
                    }

                    MessageBox.Show("Данные пользователя обновлены!");
                }
                else
                {
                    MessageBox.Show("Указанный файл должен быть формата PNG, JPEG или JPG.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
