using System.Collections.Generic;

namespace SignalR.AspNetWebApi.Samples.Data
{
    public class Claims : List<Claim>
    {
        public Claims()
        { }

        public Claims(IEnumerable<Claim> claims)
            : base(claims)
        { }
    }

    public class Claim
    {
        public string ClaimType { get; set; }
        public string Value { get; set; }
        public string Issuer { get; set; }
        public string OriginalIssuer { get; set; }
    }
}