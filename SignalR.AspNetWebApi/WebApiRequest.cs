using System;
using System.Collections.Specialized;
using System.Net.Http;
using SignalR.Hosting;

namespace SignalR.AspNetWebApi
{
    internal class WebApiRequest : IRequest
    {
        private readonly HttpRequestMessage httpRequestMessage;
        private readonly Lazy<IRequestCookieCollection> cookies;
        private readonly Lazy<NameValueCollection> form;
        private readonly Lazy<NameValueCollection> headers;
        private readonly Lazy<NameValueCollection> queryString;

        public WebApiRequest(HttpRequestMessage httpRequestMessage)
        {
            this.httpRequestMessage = httpRequestMessage;

            this.cookies = new Lazy<IRequestCookieCollection>(() => httpRequestMessage.Headers.ParseCookies());
            this.form = new Lazy<NameValueCollection>(() => httpRequestMessage.Content.ReadAsNameValueCollection());
            this.headers = new Lazy<NameValueCollection>(() => httpRequestMessage.Headers.ParseHeaders());
            this.queryString = new Lazy<NameValueCollection>(() => Url.ParseQueryString());
        }

        public IRequestCookieCollection Cookies
        {
            get
            {
                return cookies.Value;
            }
        }

        public NameValueCollection Form
        {
            get
            {
                return form.Value;
            }
        }

        public NameValueCollection Headers
        {
            get
            {
                return headers.Value;
            }
        }

        public NameValueCollection QueryString
        {
            get
            {
                return queryString.Value;
            }
        }

        public Uri Url
        {
            get
            {
                return httpRequestMessage.RequestUri;
            }
        }
    }
}
