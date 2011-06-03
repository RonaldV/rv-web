using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace RV.Web
{
    public class HttpContentSerializer
    {
        public virtual void Execute<T>(HttpWebRequest request, T content)
        {
            if (request.ContentType == HttpContentType.XML)
            {
                using (var requestStream = request.GetRequestStream())
                {
                    var serializer = new DataContractSerializer(typeof(T));
                    serializer.WriteObject(requestStream, content);
                }
            }
            else if (request.ContentType == HttpContentType.JSON)
            {
                using (var requestStream = request.GetRequestStream())
                {
                    var serializer = new DataContractJsonSerializer(typeof(T));
                    serializer.WriteObject(requestStream, content);
                }
            }
            else if (request.ContentType == HttpContentType.Text)
            {
                using (var requestStream = request.GetRequestStream())
                using (var writer = new StreamWriter(requestStream))
                {
                    writer.Write(content);
                }
            }
            else
            {
                throw new NotSupportedException("ContentType");
            }
        }
    }
}
