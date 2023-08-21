using System.Net;
using System.Text.RegularExpressions;
using System.Windows;

namespace IPcheck
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnGetIP_Click(object sender, RoutedEventArgs e)
        {
            string line = "";
            using (WebClient wc = new WebClient())
                line = wc.DownloadString($"http://ipwho.is/{adress.Text}?output=xml&lang=ru");
            Match match = Regex.Match(line, "<country>(.*?)</country>(.*?)<city>(.*?)</city>");
            labelInfo.Content = match.Groups[1].Value + "\n" + match.Groups[3].Value;
        }
    }
}
