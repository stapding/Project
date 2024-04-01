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

namespace Project
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        public Frame MainFrame { get; set; }
        public Dataclass data = new Dataclass();

        public User currentUser { get; set; }
        public Profile(Frame mf)
        {
            InitializeComponent();
            MainFrame = mf;
            TextBlock userFI = (TextBlock)Application.Current.MainWindow.FindName("isRegistrated");
            MessageBox.Show(userFI.Text);
            currentUser = data.GetUserByEmail(userFI.Text);
            if (currentUser != null)
            {
                textNameUser.Text = currentUser.Name;
                textEmailUser.Text = $"Почта: {currentUser.Email}";
                textGUIDUser.Text = $"GUID: {currentUser.ID}";
                textIncomeUser.Text = $"Доход: {currentUser.Income} ₽";
                if (currentUser.FamilyID != null)
                {
                    List<Family> families = new List<Family>();
                    families = data.LoadFamilies(families, "families.json");
                    string currentFamilyID = currentUser.FamilyID;
                    Family currentFamily = families.FirstOrDefault(u => u.ID_family == currentFamilyID);
                    if (currentFamily.MembersID[0] == currentUser.ID)
                    {
                        TextBox newTB1 = new TextBox();
                        Style styleForTB = (Style)Application.Current.FindResource("tbox_AlertIconPlaceholder");
                        newTB1.Name = "tbAddUser";
                        newTB1.Style = styleForTB;
                        stackPanelFamily.Children.Add(newTB1);

                        Button newBTN1 = new Button();
                        Style styleForBTN = (Style)Application.Current.FindResource("roundedButton");
                        newBTN1.Style = styleForBTN;
                        newBTN1.Margin = new Thickness(5, 0, 0, 5);
                        newBTN1.Content = "Добавить";
                        newBTN1.Click += new RoutedEventHandler(Button_Click_Add);
                        stackPanelFamily.Children.Add(newBTN1);

                        Button newBTN2 = new Button();
                        newBTN2.Style = styleForBTN;
                        newBTN2.Margin = new Thickness(5, 0, 0, 5);
                        newBTN2.Content = "Удалить";
                        newBTN2.Click += new RoutedEventHandler(Button_Click_Delete);
                        stackPanelFamily.Children.Add(newBTN2);
                    }
                    else
                    {
                        textFamilyUser.Text = $"Семья: есть";
                    }
                }
                else
                {
                    Button newBtnCreate = new Button();
                    Style styleForBTN = (Style)Application.Current.FindResource("roundedButton");
                    newBtnCreate.Style = styleForBTN;
                    newBtnCreate.Margin = new Thickness(5, 0, 0, 5);
                    newBtnCreate.Content = "Создать";
                    newBtnCreate.Click += new RoutedEventHandler(Button_Click_Create);
                    stackPanelFamily.Children.Add(newBtnCreate);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Image files (*.png;*.jpeg)|*.png;*.jpeg|All files (*.*)|*.*";
            //if (openFileDialog.ShowDialog() == true)
            //{
            //    blockImageOfProfile.ImageSource = new BitmapImage(new Uri(openFileDialog.FileName));
            //}
            MainFrame.Navigate(new EditProfile(MainFrame));
        }

        private void Button_Click_Create(object sender, RoutedEventArgs e)
        {
            data.CreateFamily("families.json", currentUser);

            stackPanelFamily.Children.RemoveAt(1);

            TextBox newTB1 = new TextBox();
            Style styleForTB = (Style)Application.Current.FindResource("tbox_AlertIconPlaceholder");
            newTB1.Name = "tbAddUser";
            newTB1.Style = styleForTB;
            stackPanelFamily.Children.Add(newTB1);

            Button newBTN1 = new Button();
            Style styleForBTN = (Style)Application.Current.FindResource("roundedButton");
            newBTN1.Style = styleForBTN;
            newBTN1.Margin = new Thickness(5, 0, 0, 5);
            newBTN1.Content = "Добавить";
            newBTN1.Click += new RoutedEventHandler(Button_Click_Add);
            stackPanelFamily.Children.Add(newBTN1);

            Button newBTN2 = new Button();
            newBTN2.Style = styleForBTN;
            newBTN2.Margin = new Thickness(5, 0, 0, 5);
            newBTN2.Content = "Удалить";
            newBTN2.Click += new RoutedEventHandler(Button_Click_Delete);
            stackPanelFamily.Children.Add(newBTN2);
            TextBlock userFI = (TextBlock)Application.Current.MainWindow.FindName("isRegistrated");
            currentUser = data.GetUserByEmail(userFI.Text);
            MessageBox.Show(currentUser.FamilyID);
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            TextBox tbAddFamily = stackPanelFamily.Children[1] as TextBox;
            string currentAddUser = tbAddFamily.Text;
            string currentFamilyID = currentUser.FamilyID;
            data.AddToFamily(currentAddUser, currentFamilyID);
        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            TextBox tbAddFamily = stackPanelFamily.Children[1] as TextBox;
            string currentDeleteUser = tbAddFamily.Text;
            string currentFamilyID = currentUser.FamilyID;
            data.DeleteFromFamily(currentDeleteUser, currentFamilyID);
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (textGUIDUser.Text != null)
            {
                Clipboard.SetText(textGUIDUser.Text);
                MessageBox.Show("Скопировано!");
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock userFI = (TextBlock)Application.Current.MainWindow.FindName("isRegistrated");
            currentUser = data.GetUserByEmail(userFI.Text);
            textNameUser.Text = currentUser.Name;
            textIncomeUser.Text = $"Доход: {currentUser.Income} ₽";
            if (File.Exists(currentUser.ImageAvatar))
            {
                blockImageOfProfile.ImageSource = new BitmapImage(new Uri(currentUser.ImageAvatar));
            }
            else
            {
                blockImageOfProfile.ImageSource = new BitmapImage(new Uri("C:/Users/МОиБД/source/Project/Project/Images/NoAvatar.png"));
            }
        }
    }
}
