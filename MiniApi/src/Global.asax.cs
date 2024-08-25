using System.IO;
using System.Web.Http;

namespace MiniApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(CorsConfig.Configure);
            GlobalConfiguration.Configure(NLogConfig.Configure);
            GlobalConfiguration.Configure(FormatterConfig.Configure);
            GlobalConfiguration.Configure(SwaggerConfig.Configure);
        }
    }
}
