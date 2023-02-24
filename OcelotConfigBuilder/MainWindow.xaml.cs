using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace OcelotConfigBuilder
{
    public partial class MainWindow : Window
    {
        public static Root Root = null!;

        private string filepath = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            InitializeData();

            Root.Routes.CollectionChanged += (o, e) =>
            {
                list.Items.Clear();
                foreach (var route in Root.Routes)
                {
                    list.Items.Add(route.ListBoxLine);
                }
            };
        }

        private void InitializeData()
        {
            OpenFileDialog ofd = new();
            ofd.ShowDialog();
            filepath = ofd.FileName;

            Root = JsonConvert.DeserializeObject<Root>(File.ReadAllText(filepath))!;
            foreach (var route in Root.Routes)
            {
                list.Items.Add(route.ListBoxLine);
            }
        }

        private void AddRoute(object sender, RoutedEventArgs e)
        {
            new RouteWindow().Show();
        }

        private void RemoveRoute(object sender, RoutedEventArgs e)
        {
            if (list.SelectedIndex == -1)
            {
                return;
            }

            Root.Routes.RemoveAt(list.SelectedIndex);
        }

        private void UpdateRoute(object sender, RoutedEventArgs e)
        {
            new RouteWindow(Root.Routes[list.SelectedIndex]).Show();
        }

        private void SaveRoutes(object sender, RoutedEventArgs e)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            File.WriteAllText(filepath, JsonConvert.SerializeObject(Root, Formatting.Indented, settings), Encoding.UTF8);
        }
    }
}
