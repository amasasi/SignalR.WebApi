using System.IO;
using System.Xml;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens.Saml2;

namespace Thinktecture.IdentityModel.Http
{
    public class HttpSaml2SecurityTokenHandler : Saml2SecurityTokenHandler, IHttpSecurityTokenHandler
    {
        public HttpSaml2SecurityTokenHandler()
            : base()
        { }

        public HttpSaml2SecurityTokenHandler(SecurityTokenHandlerConfiguration configuration)
            : base()
        {
            Configuration = configuration;
        }

        public IClaimsPrincipal ValidateToken(string token)
        {
            var securityToken = ReadToken(new XmlTextReader(new StringReader(token)));
            return ClaimsPrincipal.CreateFromIdentities(ValidateToken(securityToken));
        }
    }
}
