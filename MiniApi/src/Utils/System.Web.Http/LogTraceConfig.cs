using Newtonsoft.Json;
using System.Data.Linq;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Filters;

namespace System.Web.Http
{
    public class LogTraceConfig
    {
        private static HttpConfiguration _config;

        public static void Configure(HttpConfiguration config)
        {
            _config = config;
            config.Filters.Add(new LoggerActionFilterAttribute());
            config.Filters.Add(new LoggerExceptionFilterAttribute());
            config.Services.Replace(typeof(IExceptionLogger), new ExceptionLogger());
        }

        public class LoggerActionFilterAttribute : ActionFilterAttribute
        {
            public override async Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
            {
                var request = actionContext.Request;

                var message = await request.GetLogMessageAsync();

                Trace.TraceInformation(message);

                await base.OnActionExecutingAsync(actionContext, cancellationToken);
            }

            public override async Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
            {
                var response = actionExecutedContext.Response;

                if (response == null) return;

                var request = actionExecutedContext.Request;

                var message = await response.GetLogMessageAsync();

                Trace.TraceInformation(message);

                await base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
            }
        }

        public class LoggerExceptionFilterAttribute : ExceptionFilterAttribute
        {
            public override void OnException(HttpActionExecutedContext actionExecutedContext)
            {
                var ex = actionExecutedContext.Exception;
                var request = actionExecutedContext.Request;
                var url = request.RequestUri.PathAndQuery;

                while (ex is AggregateException) ex = ex.InnerException;

                actionExecutedContext.Response = request.CreateResponse(HttpStatusCode.OK);

                HttpStatusCode statusCode;

                switch (ex)
                {
                    case ArgumentException argumentException:
                        statusCode = HttpStatusCode.BadRequest;
                        break;
                    case AuthenticationException authenticationException:
                        statusCode = HttpStatusCode.Unauthorized;
                        break;
                    case InvalidOperationException invalidOperationException:
                        if (invalidOperationException is DuplicateKeyException duplicateKeyException)
                            statusCode = HttpStatusCode.Conflict;
                        else
                            statusCode = HttpStatusCode.Forbidden;
                        break;
                    case CryptographicException cryptographicException
                        when DateTime.Now.Minute == 0 || DateTime.Now.Minute == 59:
                        statusCode = HttpStatusCode.ServiceUnavailable;
                        break;
                    default:
                        statusCode = HttpStatusCode.InternalServerError;
                        break;
                }

                var result = new HttpResult(statusCode, ex.Message, statusCode != HttpStatusCode.InternalServerError && ex.Data.Count > 0 ? ex.Data : null);

                var json = JsonConvert.SerializeObject(result, _config.Formatters.JsonFormatter.SerializerSettings);

                actionExecutedContext.Response.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var message = $"【{request.Method}] {url} OUT " + json;

                Trace.TraceInformation(message);
            }
        }

        public class ExceptionLogger : IExceptionLogger
        {
            public async Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
            {
                await Task.Run(() => Trace.TraceError(context.Exception.Message, context.Exception), cancellationToken);
            }
        }
    }
}