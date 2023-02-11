using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OcelotConfigBuilder
{
    public class AuthenticationOptions
    {
        public string AuthenticationProviderKey { get; set; }
    }

    public class DownstreamHostAndPort
    {
        public string Host { get; set; }
        public string Port { get; set; }
    }

    public class Root
    {
        public ObservableCollection<Route> Routes { get; set; }
    }

    public class Route
    {
        public string DownstreamPathTemplate { get; set; }
        public string DownstreamScheme { get; set; }
        public List<DownstreamHostAndPort> DownstreamHostAndPorts { get; set; }
        public string UpstreamPathTemplate { get; set; }
        public List<string> UpstreamHttpMethod { get; set; }
        public AuthenticationOptions AuthenticationOptions { get; set; }
        public List<string> DelegatingHandlers { get; set; }

        public string ListBoxLine 
        {
            get => $"{UpstreamHttpMethod.First().ToUpper()} " +
                $"{DownstreamScheme}://{DownstreamHostAndPorts.First().Host}:{DownstreamHostAndPorts.First().Port}" +
                $"{(DownstreamPathTemplate.Contains('?') ? DownstreamPathTemplate.Remove(DownstreamPathTemplate.IndexOf('?')) : DownstreamPathTemplate)}";
        }
    }


}
