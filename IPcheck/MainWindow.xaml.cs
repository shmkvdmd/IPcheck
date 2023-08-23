using IPcheck.View.UserControls;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace IPcheck
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        bool CheckCon()
        {
            return NetworkInterface.GetIsNetworkAvailable();
        }
        private async void btnGetIp_Click(object sender, RoutedEventArgs e)
        {
            CultureInfo culture = new CultureInfo("en-US");
            string line = "";
            if (!CheckCon())
            {
                MessageBox.Show("Отсутствует сетевое подключение.");
                conCheck.Background = Brushes.Red;
                return;
            }
            try
            {
                using (HttpClient hc = new HttpClient())
                    line = await hc.GetStringAsync($"http://ipwho.is/{clearableTextBox.InputText}");
                Match matchCountry =
                    Regex.Match(line, "\"continent\":\"(.*?)\",\"(.*?)\",\"country\":\"(.*?)\",\"(.*?)\",\"city\":\"(.*?)\",");
                Match matchError =
                    Regex.Match(line, "\"success\":(.*?),\"message\":\"(.*?)\"");
                Match matchTime =
                    Regex.Match(line, "\"utc\":\"(.*?)\",\"current_time\":\"(.*?)\"");
                Match matchConnection =
                    Regex.Match(line, "\"org\":\"(.*?)\",\"(.*?)\",\"domain\":\"(.*?)\"");
                Match matchCords =
                    Regex.Match(line, "\"latitude\":(.*?),\"longitude\":(.*?),");
                if (matchError.Groups[1].Value == "false")
                {
                    MessageBox.Show("Ошибка: " + matchError.Groups[2].Value);
                    conCheck.Background = Brushes.Red;
                }
                else
                {
                    conCheck.Background = Brushes.LightGreen;
                    labelCountryCity.Content = "Continent: " + matchCountry.Groups[1].Value +
                    "\nCountry: " + matchCountry.Groups[3].Value + "\nCity: " + matchCountry.Groups[5].Value;
                    labelTimezone.Content = "UTC: " + matchTime.Groups[1].Value + "\nCurrent time: " + matchTime.Groups[2].Value;
                    labelConnection.Content = "Org: " + matchConnection.Groups[1].Value + " | Domain: " + matchConnection.Groups[3].Value;
                    double latitude = Convert.ToDouble(matchCords.Groups[1].Value, culture);
                    double longitude = Convert.ToDouble(matchCords.Groups[2].Value, culture);
                    Location center = new Location(latitude, longitude);
                    myMap.Visibility = Visibility.Visible;
                    myMap.SetView(center, 10);
                }
            }
            catch (WebException ex)
            {
                conCheck.Background = Brushes.Red;
            }
        }
    }
}
