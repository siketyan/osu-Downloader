using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace osu_Downloader.Utilities
{
    public class CookieWebClient
    {
        public string Cookies { get; private set; }

        private readonly CookieContainer _cookies = new CookieContainer();

        private HttpWebRequest CreateRequest(Uri uri)
        {
            var request = WebRequest.CreateHttp(uri);
            request.AllowAutoRedirect = false;
            request.CookieContainer = _cookies;
            SetHeaders(request);

            return request;
        }

        private string DecodeResponse(HttpWebResponse response)
        {
            foreach (Cookie cookie in response.Cookies)
            {
                _cookies.Add(
                    new Uri(
                        response.ResponseUri.GetLeftPart(UriPartial.Authority)
                    ), 
                    cookie
                );
            }

            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                var location = response.Headers[HttpResponseHeader.Location];
                if (!string.IsNullOrEmpty(location))
                    return Get(new Uri(location));
            }

            var stream = response.GetResponseStream();
            var buffer = new MemoryStream();
            var block = new byte[65536];
            var blockLength = 0;

            do
            {
                blockLength = stream.Read(block, 0, block.Length);
                buffer.Write(block, 0, blockLength);
            }
            while (blockLength == block.Length);

            return Encoding.UTF8.GetString(buffer.GetBuffer());
        }

        public string Get(Uri uri)
        {
            var request = CreateRequest(uri);
            var response = (HttpWebResponse)request.GetResponse();

            return DecodeResponse(response);
        }

        private void SetHeaders(HttpWebRequest request)
        {
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers[HttpRequestHeader.AcceptLanguage] = "ja-JP,ja,en-US,en;q=0.8,de-DE;q=0.5,de;q=0.3";
            request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip, deflate";
            request.Headers[HttpRequestHeader.CacheControl] = "no-cache";
        }

        public string Post(Uri uri, byte[] data)
        {
            var request = CreateRequest(uri);
            request.Method = "POST";
            request.GetRequestStream().Write(data, 0, data.Length);

            var response = (HttpWebResponse)request.GetResponse();
            Cookies = response.Headers[HttpResponseHeader.SetCookie];

            return DecodeResponse(response);
        }
    }
}