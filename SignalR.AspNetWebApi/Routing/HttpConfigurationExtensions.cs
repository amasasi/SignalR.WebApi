using System.Web.Http;
using SignalR.Hubs;

namespace SignalR.AspNetWebApi.Routing
{
    public static class HttpConfigurationExtensions
    {
        public static HttpConfiguration MapHubs(this HttpConfiguration config, string url)
        {
            var dispatcher = new HubDispatcher(url);

            config.Routes.MapHubs(url, Global.DependencyResolver);
            config.MessageHandlers.Add(new SignalRMessageHandler(Global.DependencyResolver, dispatcher, config.Routes));
            
            return config;
        }
    }
}