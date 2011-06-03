using RV.Web;

namespace System.Net
{
    public static class HttpWebRequestExtensions
    {
        private readonly static HttpContentSerializer Serializer = new HttpContentSerializer();

        public static void Content(this HttpWebRequest httpWebRequest)
        {
            httpWebRequest.ContentLength = 0;
        }

        public static void Content<T>(this HttpWebRequest httpWebRequest, T content)
        {
            if (content == null)
            {
                Content(httpWebRequest);
                return;
            }
            Serializer.Execute(httpWebRequest, content);
        }

        public static HttpWebResponse Send(this HttpWebRequest httpWebRequest)
        {
            return (HttpWebResponse)httpWebRequest.GetResponse();
        }

        public static HttpWebResponse Send<T>(this HttpWebRequest httpWebRequest, T content)
        {
            httpWebRequest.Content(content);
            return (HttpWebResponse)httpWebRequest.GetResponse();
        }

        public static TResult Send<TResult>(this HttpWebRequest httpWebRequest)
        {
            var response = httpWebRequest.Send();
            response.EnsureSuccess();
            return response.Content<TResult>();
        }

        public static TResult Get<TResult>(this HttpWebRequest httpWebRequest)
        {
            httpWebRequest.Method = HttpMethod.GET;
            return httpWebRequest.Send<TResult>();
        }

        public static void Post<T>(this HttpWebRequest httpWebRequest, T content)
        {
            httpWebRequest.Method = HttpMethod.POST;
            httpWebRequest.Content(content);
            using (var response = httpWebRequest.Send())
            {
                response.EnsureSuccess();
            }
        }

        public static TResult Post<TResult, T>(this HttpWebRequest httpWebRequest, T content)
        {
            httpWebRequest.Method = HttpMethod.POST;
            httpWebRequest.Content(content);
            var response = httpWebRequest.Send();
            return response.Content<TResult>();
        }

        public static void Put<T>(this HttpWebRequest httpWebRequest, T content)
        {
            httpWebRequest.Method = HttpMethod.PUT;
            httpWebRequest.Content(content);
            using (var response = httpWebRequest.Send())
            {
                response.EnsureSuccess();
            }
        }

        public static TResult Put<TResult, T>(this HttpWebRequest httpWebRequest, T content)
        {
            httpWebRequest.Method = HttpMethod.PUT;
            httpWebRequest.Content(content);
            var response = httpWebRequest.Send();
            return response.Content<TResult>();
        }

        public static void Delete(this HttpWebRequest httpWebRequest)
        {
            httpWebRequest.Method = HttpMethod.DELETE;
            using (var response = httpWebRequest.Send())
            {
                response.EnsureSuccess();
            }
        }

        public static void Send<TResult>(this HttpWebRequest httpWebRequest, Action<TResult> successCallback, Action<Exception> errorCallback)
        {
            httpWebRequest.BeginGetResponse(asyncResult =>
            {
                try
                {
                    var response = (HttpWebResponse)httpWebRequest.EndGetResponse(asyncResult);
                    successCallback(response.ContentLength > 0
                        ? response.Content<TResult>()
                        : default(TResult));
                }
                catch (NotSupportedException ex)
                {
                    errorCallback(ex);
                }
                catch (WebException ex)
                {
                    errorCallback(ex);
                }
            }, null);
        }

        public static void Get<TResult>(this HttpWebRequest httpWebRequest, Action<TResult> successCallback, Action<Exception> errorCallback)
        {
            httpWebRequest.Method = HttpMethod.GET;
            httpWebRequest.Send(successCallback, errorCallback);
        }

        public static void Post<TResult, T>(this HttpWebRequest httpWebRequest, T content, Action<TResult> successCallback, Action<Exception> errorCallback)
        {
            httpWebRequest.Method = HttpMethod.POST;
            try
            {
                httpWebRequest.Content(content);
                httpWebRequest.Send(successCallback, errorCallback);
            }
            catch (WebException ex)
            {
                if (errorCallback != null)
                {
                    errorCallback(ex);
                }
            }
        }

        public static void Put<TResult, T>(this HttpWebRequest httpWebRequest, T content, Action<TResult> successCallback, Action<Exception> errorCallback)
        {
            httpWebRequest.Method = HttpMethod.PUT;
            try
            {
                httpWebRequest.Content(content);
                httpWebRequest.Send(successCallback, errorCallback);
            }
            catch (WebException ex)
            {
                if (errorCallback != null)
                {
                    errorCallback(ex);
                }
            }
        }

        public static void Delete<TResult>(this HttpWebRequest httpWebRequest, Action<TResult> successCallback, Action<Exception> errorCallback)
        {
            httpWebRequest.Method = HttpMethod.DELETE;
            httpWebRequest.Send(successCallback, errorCallback);
        }
    }
}
