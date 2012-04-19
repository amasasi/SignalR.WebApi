using System.Collections.Generic;
using SignalR.Hubs;

namespace SignalR.AspNetWebApi.Infrastructure
{
    public class WebApiAssemblyLocator : DefaultAssemblyLocator
    {
        public override IEnumerable<System.Reflection.Assembly> GetAssemblies()
        {
            // TODO: check whether Web API offers similar functionality as MVC does
            return base.GetAssemblies();
        }
    }
}
