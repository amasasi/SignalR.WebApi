using System.Web.Http;
using SignalR.AspNetWebApi.Samples.Data;

namespace SignalR.AspNetWebApi.Samples.Api
{
    public class IdentityController : ApiController
    {
        public Identity Get()
        {
            return new Identity(Request.GetUserPrincipal().Identity);
        }
    }
}