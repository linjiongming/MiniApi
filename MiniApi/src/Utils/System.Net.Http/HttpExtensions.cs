using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace System.Net.Http
{
    public static class HttpExtensions
    {
        public static bool IsStringContent(this HttpContent content)
        {
            return content.Headers.ContentType != null &&
                (content.Headers.ContentType.MediaType.Equals("application/json", StringComparison.OrdinalIgnoreCase) ||
                content.Headers.ContentType.MediaType.StartsWith("text", StringComparison.OrdinalIgnoreCase));
        }

        public static async Task<string> ReadContentAsStringAsync(this HttpRequestMessage request)
        {
            var stream = new MemoryStream();
            {
                var context = (HttpContextBase)request.Properties["MS_HttpContext"];
                context.Request.InputStream.Seek(0, SeekOrigin.Begin);
                await context.Request.InputStream.CopyToAsync(stream);
                string requestBody = Encoding.UTF8.GetString(stream.ToArray());
                return requestBody;
            }
        }

        public static IEnumerable<KeyValuePair<string, IEnumerable<string>>> GetSpecialHeaders(this HttpRequestMessage request)
        {
            return request.Headers.Where(x => !x.Key.StartsWith("x-", StringComparison.OrdinalIgnoreCase) && !_commonRequestHeaders.Contains(x.Key, StringComparer.OrdinalIgnoreCase));
        }

        public static IEnumerable<KeyValuePair<string, IEnumerable<string>>> GetSpecialHeaders(this HttpResponseMessage response)
        {
            return response.Headers.Where(x => !_commonResponseHeaders.Contains(x.Key, StringComparer.OrdinalIgnoreCase));
        }

        public static async Task<string> GetLogMessageAsync(this HttpRequestMessage request)
        {
            var message = $"[{request.Method}] {request.RequestUri.PathAndQuery} IN "
                + string.Join(Environment.NewLine, request.GetSpecialHeaders().Select(x => $"{x.Key}:{string.Join(",", x.Value)}"));

            if (request.Content.IsStringContent())
            {
                var json = await request.ReadContentAsStringAsync();
                message += json;
            }

            return message;
        }

        public static async Task<string> GetLogMessageAsync(this HttpResponseMessage response)
        {
            var request = response.RequestMessage;

            var message = $"{request.Method}] {request.RequestUri.PathAndQuery} OUT "
                + string.Join(Environment.NewLine, response.GetSpecialHeaders().Select(x => $"{x.Key}:{string.Join(",", x.Value)}"));

            if (response.Content.IsStringContent())
            {
                var json = await response.Content.ReadAsStringAsync();
                message += json;
            }

            return message;
        }

        static readonly string[] _commonRequestHeaders = new string[]
        {
        /*Standard request fields*/
        "A-IM",
        "Accept",
        "Accept-Charset",
        "Accept-Datetime",
        "Accept-Encoding",
        "Accept-Language",
        "Access-Control-Request-Method",
        "Access-Control-Request-Headers",
        //"Authorization",
        "Cache-Control",
        "Connection",
        "Content-Encoding",
        "Content-Length",
        "Content-MD5",
        "Content-Type",
        "Cookie",
        "Date",
        "Expect",
        "Forwarded",
        "From",
        "Host",
        "HTTP2-Settings",
        "If-Match",
        "If-Modified-Since",
        "If-None-Match",
        "If-Range",
        "If-Unmodified-Since",
        "Max-Forwards",
        "Origin",
        "Pragma",
        "Prefer",
        "Proxy-Authorization",
        "Range",
        "Referer",
        "TE",
        "Trailer",
        "Transfer-Encoding",
        "Upgrade",
        "User-Agent",
        "Via",
        "Warning",

        /*Common non-standard request fields*/
        "DNT",
        "Front-End-Https",
        "Proxy-Connection",
        "Save-Data",
        "Sec-GPC",
        "Sec-Ch-Ua",
        "Sec-Ch-Ua-Mobile",
        "Sec-Ch-Ua-Platform",
        "Sec-Fetch-Site",
        "Sec-Fetch-Mode",
        "Sec-Fetch-Dest",
        "Upgrade-Insecure-Requests",
        "X-ATT-DeviceId",
        "X-Csrf-Token",
        "X-Forwarded-For",
        "X-Forwarded-Host",
        "X-Forwarded-Proto",
        "X-Http-Method-Override",
        "X-Request-ID",
        "X-Correlation-ID, Correlation-ID",
        "X-Requested-With",
        "X-UIDH",
        "X-Wap-Profile",

        /*Android common request fields*/
        "client-ip",
        "disguised-host",
        "was-default-hostname",
        "x-appservice-proto",
        "x-arr-log-id",
        "x-arr-ssl",
        "x-forwarded-tlsversion",
        "x-ms-coldstart",
        "x-original-url",
        "x-platform",
        "x-site-deployment-id",
        "x-version",
        "x-waws-unencoded-url",

        /*Customize common request fields*/
        "Postman-Token",
        //"Token",
        };

        static readonly string[] _commonResponseHeaders = new string[]
        {
        /*Standard response fields*/
        "Accept-CH",
        "Access-Control-Allow-Origin",
        "Access-Control-Allow-Credentials",
        "Access-Control-Expose-Headers",
        "Access-Control-Max-Age",
        "Access-Control-Allow-Methods",
        "Access-Control-Allow-Headers",
        "Accept-Patch",
        "Accept-Ranges",
        "Age",
        "Allow",
        "Alt-Svc",
        "Cache-Control",
        "Connection",
        "Content-Disposition",
        "Content-Encoding",
        "Content-Language",
        "Content-Length",
        "Content-Location",
        "Content-MD5",
        "Content-Range",
        "Content-Type",
        "Date",
        "Delta-Base",
        "ETag",
        "Expires",
        "IM",
        "Last-Modified",
        "Link",
        "Location",
        "P3P",
        "Pragma",
        "Preference-Applied",
        "Proxy-Authenticate",
        "Public-Key-Pins",
        "Retry-After",
        "Server",
        "Set-Cookie",
        "Strict-Transport-Security",
        "Trailer",
        "Transfer-Encoding",
        "Tk",
        "Upgrade",
        "Vary",
        "Via",
        "Warning",
        "WWW-Authenticate",
        "X-Frame-Options",

        /*Common non-standard response fields*/
        "Content-Security-Policy",
        "X-Content-Security-Policy",
        "X-WebKit-CSP",
        "Expect-CT",
        "NEL",
        "Permissions-Policy",
        "Refresh",
        "Report-To",
        "Status",
        "Timing-Allow-Origin",
        "X-Content-Duration",
        "X-Content-Type-Options",
        "X-Powered-By",
        "X-Redirect-By",
        "X-Request-ID",
        "X-Correlation-ID",
        "X-UA-Compatible",
        "X-XSS-Protection",
        };
    }
}
