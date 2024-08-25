using Swashbuckle.Application;
using System;
using System.IO;
using System.Reflection;
using System.Web.Http;

namespace MiniApi
{
    public static class SwaggerConfig
    {
        public static void Configure(HttpConfiguration config)
        {
            // Use swagger/ui/index to inspect API docs
            config
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "MiniApi");
                    c.PrettyPrint();
                    c.RootUrl(x =>
                    {
                        var idx = x.RequestUri.AbsoluteUri.IndexOf("swagger", StringComparison.InvariantCultureIgnoreCase);
                        return x.RequestUri.AbsoluteUri.Substring(0, idx - 1);
                    });

                    // This code allow you to use XML-comments
                    var commentsFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bin", Assembly.GetExecutingAssembly().GetName().Name + ".XML");
                    c.IncludeXmlComments(commentsFile);
                })
                .EnableSwaggerUi();
        }
    }
}