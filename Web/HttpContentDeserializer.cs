using System;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace RV.Web
{
    public class HttpContentDeserializer
    {
        public virtual T Execute<T>(HttpWebResponse response)
        {
            // TODO check status
            if (response.ContentType == HttpContentType.XML)
            {
                using (var stream = response.GetResponseStream())
                {
                    if (stream == null)
                    {
                        return default(T);
                    }
                    var serializer = new DataContractSerializer(typeof(T));
                    return (T)serializer.ReadObject(stream);
                }
            }
            if (response.ContentType == HttpContentType.JSON)
            {
                using (var stream = response.GetResponseStream())
                {
                    if (stream == null)
                    {
                        return default(T);
                    }
                    var serializer = new DataContractJsonSerializer(typeof(T));
                    return (T)serializer.ReadObject(stream);
                }
            }
            throw new NotSupportedException("ContentType");
        }
    }
}
