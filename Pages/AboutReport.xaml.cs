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
using OxyPlot;
using OxyPlot.Series;
using System.Windows.Markup;
using OxyPlot.Axes;

namespace WPFModernVerticalMenu.Pages
{
    /// <summary>
    /// Логика взаимодействия для AboutReport.xaml
    /// </summary>
    public partial class AboutReport : Page
    {
        public Report CurrentReport { get; set; }
        public List<double> Values_category { get; set; }
        public List<string> Ins_category { get; set;}

        public List<string> Bad_statuses { get; set; }
        public List<string> Bad_category { get; set; }
        public List<double> Total_expenses_month { get; set; }
        public List<string> Ins_month { get; set; }
        public Dataclass data = new Dataclass();
        public User currentUser { get; set; }
        public AboutReport(Report currentReport)
        {
            InitializeComponent();

            CurrentReport = currentReport;
            Values_category = currentReport.Values_category;
            Ins_category = currentReport.Ins_category;
            Bad_category = currentReport.Bad_category;

            Total_expenses_month = new List<double>();
            Ins_month = new List<string>();
            Bad_statuses = new List<string>();

            currentUser = (User)Application.Current.Properties["CurrentUser"];
            currentUser = data.GetUserByEmail(currentUser.Email, "users.json");
            foreach (Report report in currentUser.Reports)
            {
                Total_expenses_month.Add(report.Total_expenses);
                Ins_month.Add(report.Current_month);
                if (report.Status == "Плохо")
                {
                    Bad_statuses.Add(report.Current_month);
                }
            }

            mainTextMonth.Text = $"Отчёт за {currentReport.Current_month}";
            incomeTextMonth.Text = $"Доход за месяц: {currentReport.Total_income} ₽";
            expensesTextMonth.Text = $"Траты за месяц: {currentReport.Total_expenses} ₽";
            if (currentReport.Total_income > currentReport.Total_expenses)
            {
                remainderTextMonth.Text = $"Остаток: {currentReport.Total_income - currentReport.Total_expenses} ₽";
            }
            else
            {
                remainderTextMonth.Text = "Остаток: Нет :(";
            }
            badTextMonth.Text = $"Траты превышены в: {currentReport.Stringed_Bad_Categories}";
            // Установка текста для статуса
            statusTextMonth.Inlines.Clear(); // Очищаем текущие inlines, если они были установлены ранее
            statusTextMonth.Inlines.Add(new Run("Статус: ") { Foreground = new SolidColorBrush(Colors.White) });

            // Создаем новый Run для самого статуса
            Run statusRun = new Run($" {currentReport.Status}");
            switch (currentReport.Status)
            {
                case "Хорошо":
                    statusRun.Foreground = new SolidColorBrush(Colors.Green);
                    break;
                case "Плохо":
                    statusRun.Foreground = new SolidColorBrush(Colors.Red);
                    break;
                default:
                    statusRun.Foreground = new SolidColorBrush(Colors.White);
                    break;
            }
            statusTextMonth.Inlines.Add(statusRun);

            CreatePieChart_Category();
            CreatePieChart_Month();

            CreateBarChart_Category();
            CreateBarChart_Month();

            CreateLineChart_Category();
            CreateLineChart_Month();
        }

        private void CreatePieChart_Category()
        {
            var pieSeries = new PieSeries
            {
                StrokeThickness = 1.0,
                InsideLabelPosition = 0.8,
                AngleSpan = 360,
                StartAngle = 0
            };

            for (int i = 0; i < Values_category.Count; i++)
            {
                pieSeries.Slices.Add(new PieSlice(Ins_category[i], Values_category[i]));
            }

            var plotModel = new PlotModel { Title = "Категории (круговая)" };
            plotModel.Series.Add(pieSeries);
            pieChartCategory.Model = plotModel;
        }

        private void CreatePieChart_Month()
        {
            var pieSeries = new PieSeries
            {
                StrokeThickness = 1.0,
                InsideLabelPosition = 0.8,
                AngleSpan = 360,
                StartAngle = 0
            };

            for (int i = 0; i < Total_expenses_month.Count; i++)
            {
                pieSeries.Slices.Add(new PieSlice(Ins_month[i], Total_expenses_month[i]));
            }

            var plotModel = new PlotModel { Title = "Месяца (круговая)" };
            plotModel.Series.Add(pieSeries);
            pieChartMonth.Model = plotModel;
        }

        private void CreateBarChart_Category()
        {
            var plotModel = new PlotModel { Title = "Категории (столбчатая)" };

            // Настройка осей
            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "CategoryAxis",
                ItemsSource = Ins_category
            };
            plotModel.Axes.Add(categoryAxis);

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                MinimumPadding = 0,
                AbsoluteMinimum = 0
            };
            plotModel.Axes.Add(valueAxis);

            // Создание серии столбцов
            var barSeries = new BarSeries
            {
                StrokeColor = OxyColors.Black,
                StrokeThickness = 1,
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "{0}",
                IsStacked = false,
                BaseValue = 0
            };

            for (int i = 0; i < Values_category.Count; i++)
            {
                var barItem = new BarItem
                {
                    Value = Values_category[i],
                    Color = Bad_category.Contains(Ins_category[i]) ? OxyColors.Red : OxyColors.Green
                };

                barSeries.Items.Add(barItem);
            }

            plotModel.Series.Add(barSeries);
            barChartCategory.Model = plotModel;
        }

        private void CreateBarChart_Month()
        {
            var plotModel = new PlotModel { Title = "Месяца (столбчатая)" };

            // Настройка осей
            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Left,
                Key = "CategoryAxis",
                ItemsSource = Ins_month
            };
            plotModel.Axes.Add(categoryAxis);

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Bottom,
                MinimumPadding = 0,
                AbsoluteMinimum = 0
            };
            plotModel.Axes.Add(valueAxis);

            // Создание серии столбцов
            var barSeries = new BarSeries
            {
                StrokeColor = OxyColors.Black,
                StrokeThickness = 1,
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "{0}",
                IsStacked = false,
                BaseValue = 0
            };

            for (int i = 0; i < Total_expenses_month.Count; i++)
            {
                var barItem = new BarItem
                {
                    Value = Total_expenses_month[i],
                    Color = Bad_statuses.Contains(Ins_month[i]) ? OxyColors.Red : OxyColors.Green
                };

                barSeries.Items.Add(barItem);
            }

            plotModel.Series.Add(barSeries);
            barChartMonth.Model = plotModel;
        }

        private void CreateLineChart_Category()
        {
            var plotModel = new PlotModel { Title = "Категории (линейная)" };

            // Настройка осей
            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Key = "CategoryAxis"
            };
            categoryAxis.Labels.AddRange(Ins_category);
            plotModel.Axes.Add(categoryAxis);

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                MinimumPadding = 0,
                AbsoluteMinimum = 0
            };
            plotModel.Axes.Add(valueAxis);

            // Создание серии линий
            var lineSeries = new LineSeries
            {
                StrokeThickness = 2,
                MarkerSize = 4,
                MarkerType = MarkerType.Circle,
                CanTrackerInterpolatePoints = false,
                Title = "Values"
            };

            // Цвет точек в зависимости от наличия в Bad_category
            for (int i = 0; i < Values_category.Count; i++)
            {
                var point = new DataPoint(i, Values_category[i]);

                // Проверяем, содержится ли категория в списке плохих категорий
                if (Bad_category.Contains(Ins_category[i]))
                {
                    lineSeries.Points.Add(point);
                    lineSeries.MarkerFill = OxyColors.Green; // Красный цвет для плохих категорий
                }
                else
                {
                    lineSeries.Points.Add(point);
                    lineSeries.MarkerFill = OxyColors.Green; // Зелёный цвет для обычных категорий
                }
            }

            plotModel.Series.Add(lineSeries);
            lineChartCategory.Model = plotModel;
        }

        private void CreateLineChart_Month()
        {
            var plotModel = new PlotModel { Title = "Месяца (линейная)" };

            // Настройка осей
            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Key = "CategoryAxis"
            };
            categoryAxis.Labels.AddRange(Ins_month);
            plotModel.Axes.Add(categoryAxis);

            var valueAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                MinimumPadding = 0,
                AbsoluteMinimum = 0
            };
            plotModel.Axes.Add(valueAxis);

            // Создание серии линий
            var lineSeries = new LineSeries
            {
                StrokeThickness = 2,
                MarkerSize = 4,
                MarkerType = MarkerType.Circle,
                CanTrackerInterpolatePoints = false,
                Title = "Values"
            };

            // Цвет точек в зависимости от наличия в Bad_category
            for (int i = 0; i < Ins_month.Count; i++)
            {
                var point = new DataPoint(i, Total_expenses_month[i]);

                // Проверяем, содержится ли категория в списке плохих категорий
                if (Bad_statuses.Contains(Ins_month[i]))
                {
                    lineSeries.Points.Add(point);
                    lineSeries.MarkerFill = OxyColors.Green; // Красный цвет для плохих категорий
                }
                else
                {
                    lineSeries.Points.Add(point);
                    lineSeries.MarkerFill = OxyColors.Green; // Зелёный цвет для обычных категорий
                }
            }

            plotModel.Series.Add(lineSeries);
            lineChartMonth.Model = plotModel;
        }
    }
}
