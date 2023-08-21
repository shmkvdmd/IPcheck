using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Net.NetworkInformation;
using System.Net.Http;

namespace IPcheck.View.UserControls
{
    /// <summary>
    /// Логика взаимодействия для InfoBar.xaml
    /// </summary>
    public partial class InfoBar : UserControl
    {
        public InfoBar()
        {
            InitializeComponent();
            CheckConnection();
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckConnection())
            {
                MessageBox.Show("Нет соединения");
            }
        }
        
        bool CheckConnection()
        {
            string line = "";
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                labelInfo.Content = "No connection";
                return false;
            }
            try
            {
                using (WebClient wc = new WebClient())
                    line = wc.DownloadString($"http://ipwho.is/?output=xml&lang=ru");
                Match match = Regex.Match(line, "<ip>(.*?)</ip>");
                labelInfo.Content = "Current IP: " + match.Groups[1].Value;
            }
            catch (WebException ex)
            {
                labelInfo.Content = "No connection";
                return false;
            }
            return true;
        }
    }
}
