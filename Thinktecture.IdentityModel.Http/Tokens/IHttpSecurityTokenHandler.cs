using Microsoft.IdentityModel.Claims;

namespace Thinktecture.IdentityModel.Http
{
    interface IHttpSecurityTokenHandler
    {
        IClaimsPrincipal ValidateToken(string token);
    }
}
