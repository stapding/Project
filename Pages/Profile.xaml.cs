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
using WPFModernVerticalMenu.Classes;
using System.Windows.Shapes;
using System.IO;

namespace WPFModernVerticalMenu.Pages
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        public Dataclass data = new Dataclass();
        public User currentUser { get; set; }
        public Profile()
        {
            InitializeComponent();
            currentUser = (User)Application.Current.Properties["CurrentUser"];
            currentUser = data.GetUserByEmail(currentUser.Email);
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
            Frame MainFrame = (Frame)Application.Current.Properties["MainFrame"];
            MainFrame.Navigate(new System.Uri("Pages/EditProfile.xaml", UriKind.RelativeOrAbsolute));
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
            currentUser = (User)Application.Current.Properties["CurrentUser"];
            currentUser = data.GetUserByEmail(currentUser.Email);
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
            currentUser = (User)Application.Current.Properties["CurrentUser"];
            currentUser = data.GetUserByEmail(currentUser.Email);
            textNameUser.Text = currentUser.Name;
            textIncomeUser.Text = $"Доход: {currentUser.Income} ₽";
            if (File.Exists(currentUser.ImageAvatar))
            {
                blockImageOfProfile.ImageSource = new BitmapImage(new Uri(currentUser.ImageAvatar));
            }
        }
    }
}
