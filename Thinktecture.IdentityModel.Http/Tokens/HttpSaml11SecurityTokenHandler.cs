using System.IO;
using System.Xml;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml11;

namespace Thinktecture.IdentityModel.Http
{
    public class HttpSaml11SecurityTokenHandler : Saml11SecurityTokenHandler, IHttpSecurityTokenHandler
    {
        public HttpSaml11SecurityTokenHandler() 
            : base()
        { }

        public HttpSaml11SecurityTokenHandler(SecurityTokenHandlerConfiguration configuration) 
            : base()
        {
            Configuration = configuration;
        }

        public IClaimsPrincipal ValidateToken(string token)
        {
            var securityToken = ContainingCollection.ReadToken(new XmlTextReader(new StringReader(token)));
            return ClaimsPrincipal.CreateFromIdentities(ValidateToken(securityToken));
        }
    }
}
