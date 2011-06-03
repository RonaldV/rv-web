using System.IO;
using RV.Web;

namespace System.Net
{
    public static class HttpWebResponseExtensions
    {
        private readonly static HttpContentDeserializer Deserializer = new HttpContentDeserializer();

        public static T Content<T>(this HttpWebResponse response)
        {
            return Deserializer.Execute<T>(response);
        }

        public static string Content(this HttpWebResponse response)
        {
            using (var stream = response.GetResponseStream())
            {
                if (stream == null) return null;

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public static bool IsSuccessful(this HttpWebResponse response)
        {
            return response.StatusCode == HttpStatusCode.OK
                || response.StatusCode == HttpStatusCode.Created
                || response.StatusCode == HttpStatusCode.Accepted;
        }

        public static void EnsureSuccess(this HttpWebResponse response)
        {
            if (!response.IsSuccessful())
            {
                throw new WebException("Request unsuccessful", null, WebExceptionStatus.ReceiveFailure, response);
            }
        }
    }
}
