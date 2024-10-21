using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using Swashbuckle.Application;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Routing;

namespace MiniApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            // webapi
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // cors
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
            var constraints = new { httpMethod = new HttpMethodConstraint(HttpMethod.Options) };
            config.Routes.IgnoreRoute("OPTIONS", "*pathInfo", constraints);

            // formatters
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Include,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            config.Formatters.JsonFormatter.UseDataContractJsonSerializer = false;

            // swagger
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
            config
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", assemblyName);
                    c.PrettyPrint();
                    c.RootUrl(x =>
                    {
                        var idx = x.RequestUri.AbsoluteUri.IndexOf("swagger", StringComparison.InvariantCultureIgnoreCase);
                        return x.RequestUri.AbsoluteUri.Substring(0, idx - 1);
                    });
                    var commentsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, assemblyName + ".XML");
                    c.IncludeXmlComments(commentsFile);
                })
                .EnableSwaggerUi();

            // logging
            LogTraceConfig.Configure(config);

            app.UseWebApi(config);
        }
    }
}
