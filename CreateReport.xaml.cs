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
using System.IO;
using System.Text.Json;

namespace Project
{
    /// <summary>
    /// Логика взаимодействия для CreateReport.xaml
    /// </summary>
    public partial class CreateReport : Page
    {
        public Frame MainFrame { get; set; }
        public Dataclass data = new Dataclass();
        public User currentUser { get; set; }
        public CreateReport(Frame mf)
        {
            InitializeComponent();
            MainFrame = mf;
            CultureInfo russianCulture = new CultureInfo("ru-RU");
            DateTime now = DateTime.Now;
            string monthName = now.ToString("MMMM", russianCulture);
            tbDate.Text = monthName;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string[] months = { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь" };
            string month = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(tbDate.Text.ToLower());
            if (months.Contains(month))
            {
                double total_income = 0;
                if (currentUser.FamilyID != null)
                {
                    List<User> users = new List<User>();
                    users = data.LoadUsers("users.json");
                    foreach (User user in users)
                    {
                        if (user.FamilyID == currentUser.FamilyID)
                        {
                            total_income += user.Income;
                        }
                    }
                }
                else
                {
                    total_income += currentUser.Income;
                }
                if (data.ValidateIncome(tbInnerIncome.Text))
                {
                    total_income += Double.Parse(tbInnerIncome.Text);
                }
                int index = 2;
                List<string> categories = new List<string>();
                List<double> values = new List<double>();
                foreach (TextBox value in stkpnlTB.Children.OfType<TextBox>().Skip(2))
                {
                    if (!String.IsNullOrWhiteSpace(value.Text) && data.ValidateIncome(value.Text))
                    {
                        StackPanel stackPanel = (StackPanel)stkpnlText.Children[index];
                        TextBlock nameCategory = (TextBlock)stackPanel.Children[0];
                        categories.Add(nameCategory.Text);
                        values.Add(Double.Parse(value.Text));
                    }
                    index++;
                }

                if (currentUser.Reports != null)
                {
                    foreach (Report report in currentUser.Reports)
                    {
                        if (report.Current_month == month)
                        {
                            MessageBox.Show("У вас уже есть отчёт за этот месяц!");
                            return;
                        }
                    }
                }


                if (values.Count != 0)
                {
                    List<string> bad_categories = new List<string>();
                    index = 0;
                    foreach (string category in categories)
                    {
                        switch (category)
                        {
                            case "Автомобиль":
                                if (values[index] > total_income * 0.15)
                                    bad_categories.Add(category);
                                break;
                            case "Бизнес":
                                break;
                            case "Благотворительность, подарки, помощь":
                                if (values[index] > total_income * 0.10)
                                    bad_categories.Add(category);
                                break;
                            case "Дети":
                                if (values[index] > total_income * 0.20)
                                    bad_categories.Add(category);
                                break;
                            case "Домашние животные":
                                if (values[index] > total_income * 0.05)
                                    bad_categories.Add(category);
                                break;
                            case "Здоровье и красота":
                                if (values[index] > total_income * 0.10)
                                    bad_categories.Add(category);
                                break;
                            case "Ипотека, долги, кредиты":
                                if (values[index] > total_income * 0.35)
                                    bad_categories.Add(category);
                                break;
                            case "Квартира и связь":
                                if (values[index] > total_income * 0.10)
                                    bad_categories.Add(category);
                                break;
                            case "Налоги и страхование":
                                if (values[index] > total_income * 0.20)
                                    bad_categories.Add(category);
                                break;
                            case "Образование":
                                if (values[index] > total_income * 0.10)
                                    bad_categories.Add(category);
                                break;
                            case "Одежда и аксессуары":
                                if (values[index] > total_income * 0.05)
                                    bad_categories.Add(category);
                                break;
                            case "Отдых и развлечения":
                                if (values[index] > total_income * 0.10)
                                    bad_categories.Add(category);
                                break;
                            case "Питание":
                                if (values[index] > total_income * 0.15)
                                    bad_categories.Add(category);
                                break;
                            case "Разное":
                                if (values[index] > total_income * 0.10)
                                    bad_categories.Add(category);
                                break;
                            case "Ремонт и мебель":
                                if (values[index] > total_income * 0.05)
                                    bad_categories.Add(category);
                                break;
                            case "Товары для дома":
                                if (values[index] > total_income * 0.03)
                                    bad_categories.Add(category);
                                break;
                            case "Транспорт":
                                if (values[index] > total_income * 0.10)
                                    bad_categories.Add(category);
                                break;
                            case "Хобби":
                                if (values[index] > total_income * 0.05)
                                    bad_categories.Add(category);
                                break;
                            case "Бытовая техника, компьютер, материалы":
                                if (values[index] > total_income * 0.05)
                                    bad_categories.Add(category);
                                break;
                        }
                        index++;
                    }
                    List<User> users = new List<User>();
                    users = data.LoadUsers("users.json");
                    foreach (User user in users)
                    {
                        if (user.ID == currentUser.ID)
                        {
                            if (user.Reports != null)
                            {
                                user.Reports.Add(new Report(total_income, values, month, bad_categories, categories));
                                break;
                            }
                            else
                            {
                                List<Report> reports = new List<Report>();
                                reports.Add(new Report(total_income, values, month, bad_categories, categories));
                                user.Reports = reports;
                                break;
                            }

                        }
                    }
                    System.IO.FileStream fs1 = new FileStream("users.json", FileMode.Truncate);
                    JsonSerializer.Serialize(fs1, users);
                    fs1.Close();
                    MessageBox.Show("Отчёт добавлен!");
                    TextBlock userFI = (TextBlock)Application.Current.MainWindow.FindName("isRegistrated");
                    currentUser = data.GetUserByEmail(userFI.Text);
                }
                else
                {
                    MessageBox.Show("Хотя бы одна категория должна быть заполнена!");
                    return;
                }
            }



        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TextBlock userFI = (TextBlock)Application.Current.MainWindow.FindName("isRegistrated");
            currentUser = data.GetUserByEmail(userFI.Text);
        }
    }
}
