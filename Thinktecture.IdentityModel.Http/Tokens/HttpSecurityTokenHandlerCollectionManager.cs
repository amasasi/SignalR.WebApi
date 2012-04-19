using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using Microsoft.IdentityModel.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Thinktecture.IdentityModel.Http
{
    public class HttpSecurityTokenHandlerCollectionManager : SecurityTokenHandlerCollectionManager
    {
        protected List<string> _schemes = new List<string>();

        public HttpSecurityTokenHandlerCollectionManager() : base("")
        { }

        public IEnumerable<string> RegisteredSchemes 
        { 
            get 
            {
                return _schemes;
            } 
        }

        public void Add(string scheme, SecurityTokenHandlerCollection collection)
        {
            if (this.ContainsKey(scheme))
            {
                throw new ArgumentException("Scheme already registered.");
            }

            this[scheme] = collection;
            _schemes.Add(scheme);
        }

        public void AddSaml11SecurityTokenHandler(string scheme, SecurityTokenHandlerConfiguration configuration)
        {
            var collection = new SecurityTokenHandlerCollection(configuration)
            {
                new HttpSaml11SecurityTokenHandler(),
                new EncryptedSecurityTokenHandler()
            };

            Add(scheme, collection);
        }

        public void AddSaml2SecurityTokenHandler(string scheme, SecurityTokenHandlerConfiguration configuration)
        {
            var collection = new SecurityTokenHandlerCollection(configuration)
            {
                new HttpSaml2SecurityTokenHandler()
            };

            Add(scheme, collection);
        }

        public void AddBasicAuthenticationHandler(SecurityTokenHandler handler)
        {
            var collection = new SecurityTokenHandlerCollection { handler };

            Add("Basic", collection);
        }

        public void AddBasicAuthenticationHandler(HttpUserNameSecurityTokenHandler.ValidateUserNameCredentialDelegate validationDelegate)
        {
            var collection = new SecurityTokenHandlerCollection { new HttpUserNameSecurityTokenHandler(validationDelegate) };

            Add("Basic", collection);
        }

        public void AddSimpleWebTokenHandler(string scheme, string issuer, string audience, string signingKey)
        {
            var config = new SecurityTokenHandlerConfiguration();

            // issuer name registry
            var registry = new SymmetricSigningKeyIssuerNameRegistry();
            registry.AddTrustedIssuer(issuer, issuer);
            config.IssuerNameRegistry = registry;

            // issuer signing key resolver
            var issuerResolver = new SymmetricSigningKeyIssuerTokenResolver();
            issuerResolver.AddSigningKey(issuer, signingKey);
            config.IssuerTokenResolver = issuerResolver;

            // audience restriction
            config.AudienceRestriction.AllowedAudienceUris.Add(new Uri(audience));

            var collection = new SecurityTokenHandlerCollection(config)
            {
                new SwtSecurityTokenHandler()
            };

            Add(scheme, collection);
        }

        public IClaimsPrincipal ValidateToken(string scheme, string token)
        {
            if (this.ContainsKey(scheme))
            {
                return this[scheme].OfType<IHttpSecurityTokenHandler>().First().ValidateToken(token);
            }

            throw new SecurityTokenValidationException("Unknown scheme");
        }
    }
}
