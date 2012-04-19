using System;
using System.Web.Http;
using System.Web.Http.Routing;
using SignalR.AspNetWebApi.Infrastructure;
using SignalR.Hubs;

namespace SignalR.AspNetWebApi.Routing
{
    public static class HttpRouteExtensions
    {
        public static IHttpRoute MapConnection<T>(this HttpRouteCollection routes, string name, string url) where T : PersistentConnection
        {
            return MapConnection(routes, name, url, typeof(T), Global.DependencyResolver);
        }

        public static IHttpRoute MapConnection<T>(this HttpRouteCollection routes, string name, string url, IDependencyResolver resolver) where T : PersistentConnection
        {
            return MapConnection(routes, name, url, typeof(T));
        }

        public static IHttpRoute MapConnection(this HttpRouteCollection routes, string name, string url, Type type)
        {
            return MapConnection(routes, name, url, type, Global.DependencyResolver);
        }
        
        public static IHttpRoute MapHubs(this HttpRouteCollection routes, string url)
        {
            return MapHubs(routes, url, Global.DependencyResolver);
        }

        public static IHttpRoute MapHubs(this HttpRouteCollection routes, string url, IDependencyResolver resolver)
        {
            string routeUrl = url;

            if(routes.ContainsKey("signalr.hubs"))
            {
                routes.Remove("signalr.hubs");
            }

            if (!routeUrl.EndsWith("/"))
            {
                routeUrl += "/{*operation}";
            }

            routeUrl = routeUrl.TrimStart('~').TrimStart('/');

            var locator = new Lazy<IAssemblyLocator>(() => new WebApiAssemblyLocator());
            resolver.Register(typeof(IAssemblyLocator), () => locator.Value);

            var constraints = new HttpRouteValueDictionary();
            // TODO: this does not work
            //constraints.Add("Incoming", new IncomingOnlyRouteConstraint());
            //constraints.Add("IgnoreJs", new IgnoreJsRouteConstraint());

            var route = new HttpRoute(routeUrl, null, constraints);
            routes.Add("signalr.hubs", route);

            return route;
        }

        public static IHttpRoute MapConnection(this HttpRouteCollection routes, string name, string url, Type type, IDependencyResolver resolver)
        {
            var constraints = new HttpRouteValueDictionary();
            // TODO: this does not work
            //constraints.Add("Incoming", new IncomingOnlyRouteConstraint());

            var route = new HttpRoute(url, null, constraints);
            routes.Add(name, route);

            return route;
        }
    }
}
