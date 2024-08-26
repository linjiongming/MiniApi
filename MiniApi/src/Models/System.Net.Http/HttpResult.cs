using System.Collections.Generic;

namespace System.Net.Http
{
    /// <summary>
    /// Uniform response wrapper
    /// </summary>
    public class HttpResult
    {
        const int HttpStatusCode_MultiStatus = 207;

        /// <summary>
        /// Status Code (Http Status Code + 1000)
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Message
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// Response Data
        /// </summary>
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

        public static HttpResult OK(string msg = null) => new HttpResult(HttpStatusCode.OK, msg);
        public static HttpResult Created(string msg = null) => new HttpResult(HttpStatusCode.Created, msg);
        public static HttpResult MultiStatus(IEnumerable<HttpResult> results, string msg = null) => new HttpResult(HttpStatusCode_MultiStatus, msg, results);
        public static HttpResult MultiStatus(string msg, params HttpResult[] results) => new HttpResult(HttpStatusCode_MultiStatus, msg, results);
        public static HttpResult MultiStatus(params HttpResult[] results) => new HttpResult(HttpStatusCode_MultiStatus, null, results);
        public static HttpResult BadRequest(string msg = null) => new HttpResult(HttpStatusCode.BadRequest, msg);
        public static HttpResult Unauthorized(string msg = null) => new HttpResult(HttpStatusCode.Unauthorized, msg);
        public static HttpResult Forbidden(string msg = null) => new HttpResult(HttpStatusCode.Forbidden, msg);
        public static HttpResult NotFound(string msg = null) => new HttpResult(HttpStatusCode.NotFound, msg);
        public static HttpResult Conflict(string msg = null) => new HttpResult(HttpStatusCode.Conflict, msg);
        public static HttpResult InternalServerError(string msg = null) => new HttpResult(HttpStatusCode.InternalServerError, msg);

        public static HttpResult<T> OK<T>(string msg, T data) => new HttpResult<T>(HttpStatusCode.OK, msg, data);
        public static HttpResult<T> OK<T>(T data) => new HttpResult<T>(HttpStatusCode.OK, null, data);
        public static HttpResult<T> Created<T>(string msg, T data) => new HttpResult<T>(HttpStatusCode.Created, msg, data);
        public static HttpResult<T> Created<T>(T data) => new HttpResult<T>(HttpStatusCode.Created, null, data);
        public static HttpResult<IEnumerable<HttpResult<T>>> MultiStatus<T>(IEnumerable<HttpResult<T>> results, string msg = null) => new HttpResult<IEnumerable<HttpResult<T>>>(HttpStatusCode_MultiStatus, msg, results);
        public static HttpResult<IEnumerable<HttpResult<T>>> MultiStatus<T>(string msg, params HttpResult<T>[] results) => new HttpResult<IEnumerable<HttpResult<T>>>(HttpStatusCode_MultiStatus, msg, results);
        public static HttpResult<IEnumerable<HttpResult<T>>> MultiStatus<T>(params HttpResult<T>[] results) => new HttpResult<IEnumerable<HttpResult<T>>>(HttpStatusCode_MultiStatus, null, results);
        public static HttpResult<T> BadRequest<T>(string msg, T data) => new HttpResult<T>(HttpStatusCode.BadRequest, msg, data);
        public static HttpResult<T> BadRequest<T>(T data) => new HttpResult<T>(HttpStatusCode.BadRequest, null, data);
        public static HttpResult<T> Unauthorized<T>(string msg, T data) => new HttpResult<T>(HttpStatusCode.Unauthorized, msg, data);
        public static HttpResult<T> Unauthorized<T>(T data) => new HttpResult<T>(HttpStatusCode.Unauthorized, null, data);
        public static HttpResult<T> Forbidden<T>(string msg, T data) => new HttpResult<T>(HttpStatusCode.Forbidden, msg, data);
        public static HttpResult<T> Forbidden<T>(T data) => new HttpResult<T>(HttpStatusCode.Forbidden, null, data);
        public static HttpResult<T> NotFound<T>(string msg, T data) => new HttpResult<T>(HttpStatusCode.NotFound, msg, data);
        public static HttpResult<T> NotFound<T>(T data) => new HttpResult<T>(HttpStatusCode.NotFound, null, data);
        public static HttpResult<T> Conflict<T>(string msg, T data) => new HttpResult<T>(HttpStatusCode.Conflict, msg, data);
        public static HttpResult<T> Conflict<T>(T data) => new HttpResult<T>(HttpStatusCode.Conflict, null, data);
        public static HttpResult<T> InternalServerError<T>(string msg, T data) => new HttpResult<T>(HttpStatusCode.InternalServerError, msg, data);
        public static HttpResult<T> InternalServerError<T>(T data) => new HttpResult<T>(HttpStatusCode.InternalServerError, null, data);
    }

    /// <summary>
    /// Uniform response wrapper
    /// </summary>
    public class HttpResult<T> : HttpResult
    {
        /// <summary>
        /// Response Data
        /// </summary>
        public new T Data { get; set; }

        public HttpResult(int code, string msg = null, T data = default(T)) : base(code, msg, data)
        {
            Data = data;
        }

        public HttpResult(HttpStatusCode code, string msg = null, T data = default(T)) : base(code, msg, data)
        {
            Data = data;
        }
    }
}