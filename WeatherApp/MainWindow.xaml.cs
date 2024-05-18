using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


namespace WeatherApp
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<WeatherData> weatherDataList = new ObservableCollection<WeatherData>();

        public MainWindow()
        {
            InitializeComponent();
            dgWeatherData.ItemsSource = weatherDataList;
        }

        private void AddData_Click(object sender, RoutedEventArgs e)
        {
            int days = int.Parse(txtDays.Text);
            Random random = new Random();
            for (int i = 0; i < days; i++)
            {
                weatherDataList.Add(new WeatherData { Date = DateTime.Today.AddDays(-i), Temperature = random.Next(-20, 40) });
            }
            UpdateStatistics();
        }

        private void Sort_Click(object sender, RoutedEventArgs e)
        {
            weatherDataList = new ObservableCollection<WeatherData>(weatherDataList.OrderBy(w => w.Date));
            dgWeatherData.ItemsSource = null;
            dgWeatherData.ItemsSource = weatherDataList;
            UpdateStatistics();
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            // Пример фильтрации по температуре
            double filterTemp = 10; // Температура для фильтрации
            weatherDataList = new ObservableCollection<WeatherData>(weatherDataList.Where(w => w.Temperature > filterTemp));
            dgWeatherData.ItemsSource = null;
            dgWeatherData.ItemsSource = weatherDataList;
            UpdateStatistics();
        }

        private void UpdateStatistics()
        {
            double averageTemp = weatherDataList.Average(w => w.Temperature);
            double maxTemp = weatherDataList.Max(w => w.Temperature);
            double minTemp = weatherDataList.Min(w => w.Temperature);
            int equalTemperatures = weatherDataList.GroupBy(w => w.Temperature).Count(g => g.Count() > 1);
            int anomalies = CountAnomalies();

            tbAverageTemperature.Text = $"Average Temperature: {averageTemp}";
            tbMaxTemperature.Text = $"Max Temperature: {maxTemp}";
            tbMinTemperature.Text = $"Min Temperature: {minTemp}";
            tbEqualTemperatures.Text = $"Equal Temperatures: {equalTemperatures}";
            tbAnomalies.Text = $"Anomalies: {anomalies}";
        }

        private int CountAnomalies()
        {
            if (weatherDataList.Count < 2) return 0;

            int count = 0;
            for (int i = 1; i < weatherDataList.Count; i++)
            {
                if (Math.Abs(weatherDataList[i].Temperature - weatherDataList[i - 1].Temperature) >= 10)
                {
                    count++;
                }
            }
            return count;
        }
    }

    public class WeatherData
    {
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
    }
}