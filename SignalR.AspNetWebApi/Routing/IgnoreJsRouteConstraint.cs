using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;

namespace SignalR.AspNetWebApi.Routing
{
    public class IgnoreJsRouteConstraint : IHttpRouteConstraint
    {
        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            // TODO
            //if (Path.GetExtension(request..AppRelativeCurrentExecutionFilePath).Equals(".js", StringComparison.OrdinalIgnoreCase))
            //{
            //    return false;
            //}

            return true;
        }
    }
}
