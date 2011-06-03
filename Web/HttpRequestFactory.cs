using System;
using System.Net;

namespace RV.Web
{
    public class HttpRequestFactory
    {
        private readonly string baseUri;
        private readonly string contentType;

        public HttpRequestFactory(string baseUri, string contentType)
        {
            if (string.IsNullOrEmpty(contentType))
            {
                throw new ArgumentException("contentType can not be null or empty", "contentType");
            }
            this.baseUri = baseUri ?? string.Empty;
            this.contentType = contentType;
        }

        public virtual HttpWebRequest Create(params string[] segments)
        {
            var request = (HttpWebRequest)WebRequest.Create(string.Concat(baseUri, string.Join("/", segments)));
            request.ContentType = contentType;

            return request;
        }
    }
}
