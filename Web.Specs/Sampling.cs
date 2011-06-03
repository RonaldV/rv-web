using System.Net;

namespace RV.Web.Specs
{
    // TODO How to make the content de - serializer extensible 
    // ... set it through an extension method?
    // ... use a factory to create ... and setting the serializer?
    public class Sampling
    {
        public void Get<T>(string requestUri, T data)
        {
            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.ContentType = HttpContentType.JSON;
            request.Method = HttpMethod.GET; // isn't really needed when using Get, Put, Post, Delete, set automatically
            //request.GetRequestStream()
            //request.Credentials
            request.Content(data); // TODO don't let content be set this way?
            request.Send();
        }

        public void Test1<TResult, T>(string requestUri, T data)
        {
            var request = (HttpWebRequest)WebRequest.Create(requestUri);
            request.ContentType = HttpContentType.JSON;
            var result = request.Get<TResult>();
        }
    }
}
