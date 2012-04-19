using System.Collections.Generic;
using System.Web.Http.Routing;

namespace SignalR.AspNetWebApi.Routing
{
    public class IncomingOnlyRouteConstraint : IHttpRouteConstraint
    {
        public bool Match(System.Net.Http.HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            if (routeDirection == HttpRouteDirection.UriResolution)
            {
                return true;
            }

            return false;
        }
    }
}
