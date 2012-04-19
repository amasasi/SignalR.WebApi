using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using SignalR.Hosting;

namespace SignalR.AspNetWebApi
{
    public class SignalRMessageHandler : DelegatingHandler
    {
        private readonly PersistentConnection connection;
        private readonly HttpRouteCollection routes;
        private readonly IDependencyResolver resolver;

        public SignalRMessageHandler(IDependencyResolver resolver, PersistentConnection connection, HttpRouteCollection routes)
        {
            this.resolver = resolver;
            this.connection = connection;
            this.routes = routes;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var containsSignalRHubsRoute = routes.ContainsKey("signalr.hubs");

            if (!containsSignalRHubsRoute)
            {
                return base.SendAsync(request, cancellationToken);
            }

            var signalRHubRoute = routes["signalr.hubs"];
            var isSignalRRequest = request.RequestUri.LocalPath.Contains(signalRHubRoute.RouteTemplate.Split('/')[0]);

            if (isSignalRRequest)
            {
                var tcs = new TaskCompletionSource<HttpResponseMessage>();
                var req = new WebApiRequest(request);
                var response = new HttpResponseMessage();
                var resp = new WebApiResponse(cancellationToken, response, () => tcs.TrySetResult(response));
                var host = new HostContext(req, resp, Thread.CurrentPrincipal);

                connection.Initialize(resolver);
                connection.ProcessRequestAsync(host).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        tcs.TrySetException(task.Exception);
                    }
                    else if (task.IsCanceled)
                    {
                        tcs.TrySetCanceled();
                    }
                    else
                    {
                        tcs.TrySetResult(response);
                    }
                });

                return tcs.Task;
            }
                
            return base.SendAsync(request, cancellationToken);
        }
    }
}
