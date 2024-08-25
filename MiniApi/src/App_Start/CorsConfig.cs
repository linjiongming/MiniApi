using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Routing;

namespace MiniApi
{
    public static class CorsConfig
    {
        public static void Configure(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);

            var constraints = new { httpMethod = new HttpMethodConstraint(HttpMethod.Options) };
            config.Routes.IgnoreRoute("OPTIONS", "*pathInfo", constraints);
        }
    }
}