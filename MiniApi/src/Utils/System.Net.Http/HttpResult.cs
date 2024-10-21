using System.Collections.Generic;

namespace System.Net.Http
{

    public class HttpResult
    {
        const int HttpStatusCode_MultiStatus = 207;

        public int Code { get; set; }

        public string Msg { get; set; }

        public object Data { get; set; }

        public HttpResult(int code, string msg = null, object data = null)
        {
            Code = code;
            Msg = msg ?? ((HttpStatusCode)(code > 1000 ? code - 1000 : code)).ToString();
            Data = data;
        }

        public HttpResult(HttpStatusCode code, string msg = null, object data = null)
        {
            Code = 1000 + (int)code;
            Msg = msg ?? code.ToString();
            Data = data;
        }

        public static HttpResult OK(string msg = null, object data = null) => new HttpResult(HttpStatusCode.OK, msg, data);
        public static HttpResult OK(object data) => new HttpResult(HttpStatusCode.OK, null, data);
        public static HttpResult Created(string msg = null, object data = null) => new HttpResult(HttpStatusCode.Created, msg, data);
        public static HttpResult Created(object data) => new HttpResult(HttpStatusCode.Created, null, data);
        public static HttpResult MultiStatus(IEnumerable<HttpResult> results, string msg = null) => new HttpResult(HttpStatusCode_MultiStatus, msg, results);
        public static HttpResult MultiStatus(string msg, params HttpResult[] results) => new HttpResult(HttpStatusCode_MultiStatus, msg, results);
        public static HttpResult MultiStatus(params HttpResult[] results) => new HttpResult(HttpStatusCode_MultiStatus, null, results);
        public static HttpResult BadRequest(string msg = null, object data = null) => new HttpResult(HttpStatusCode.BadRequest, msg, data);
        public static HttpResult BadRequest(object data) => new HttpResult(HttpStatusCode.BadRequest, null, data);
        public static HttpResult Unauthorized(string msg = null, object data = null) => new HttpResult(HttpStatusCode.Unauthorized, msg, data);
        public static HttpResult Unauthorized(object data) => new HttpResult(HttpStatusCode.Unauthorized, null, data);
        public static HttpResult Forbidden(string msg = null, object data = null) => new HttpResult(HttpStatusCode.Forbidden, msg, data);
        public static HttpResult Forbidden(object data) => new HttpResult(HttpStatusCode.Forbidden, null, data);
        public static HttpResult NotFound(string msg = null, object data = null) => new HttpResult(HttpStatusCode.NotFound, msg, data);
        public static HttpResult NotFound(object data) => new HttpResult(HttpStatusCode.NotFound, null, data);
        public static HttpResult Conflict(string msg = null, object data = null) => new HttpResult(HttpStatusCode.Conflict, msg, data);
        public static HttpResult Conflict(object data) => new HttpResult(HttpStatusCode.Conflict, null, data);
        public static HttpResult InternalServerError(string msg = null, object data = null) => new HttpResult(HttpStatusCode.InternalServerError, msg, data);
        public static HttpResult InternalServerError(object data) => new HttpResult(HttpStatusCode.InternalServerError, null, data);
    }
}