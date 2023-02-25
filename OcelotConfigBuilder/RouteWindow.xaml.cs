using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;

namespace OcelotConfigBuilder
{
    public partial class RouteWindow : Window
    {
        private bool _isUpdating;
        private Route temp = null!;

        public RouteWindow()
        {
            InitializeComponent();
            _isUpdating = false;
        }

        public RouteWindow(Route route)
        {
            InitializeComponent();

            downstream.Text = route.DownstreamPathTemplate.Split('?').First();
            if (route.DownstreamPathTemplate.Contains('?'))
            {
                parameters.Text = '?' + route.DownstreamPathTemplate.Split('?').Last();
            }
            radio_http.IsChecked = route.DownstreamScheme == "http";
            radio_https.IsChecked = route.DownstreamScheme == "https";
            host.Text = route.DownstreamHostAndPorts.First().Host;
            port.Text = route.DownstreamHostAndPorts.First().Port;
            upstream.Text = route.UpstreamPathTemplate.Split('?').First();
            method.SelectedIndex = route.UpstreamHttpMethod.First() switch
            {
                "GET" => 0,
                "POST" => 1,
                "PUT" => 2,
                "DELETE" => 3,
                _ => -1,
            };
            radio_yes.IsChecked = route.AuthenticationOptions != null;
            radio_no.IsChecked = route.AuthenticationOptions == null;

            _isUpdating = true;
            temp = route;
        }

        private void SaveRoute(object sender, RoutedEventArgs e)
        {
            if (_isUpdating)
            {
                MainWindow.Root.Routes.Remove(temp);
            }

            Route route = new()
            {
                DownstreamPathTemplate = $"{downstream.Text}{parameters.Text}",
                DownstreamScheme = (bool)radio_http!.IsChecked! ? "http" : "https",
                DownstreamHostAndPorts = new() { new() { Host = host.Text, Port = port.Text } },
                UpstreamPathTemplate = $"{upstream.Text}{parameters.Text}",
                UpstreamHttpMethod = new() { ((ComboBoxItem)method.SelectedValue)!.Content!.ToString()! },
                AuthenticationOptions = (bool)radio_yes!.IsChecked! ? new() { AuthenticationProviderKey = "oidc" } : null!,
                DelegatingHandlers = (bool)radio_yes!.IsChecked! ? new() { "HttpDelegatingHandler" } : null!
            };

            MainWindow.Root.Routes.Add(route);

            Close();
        }
    }
}
