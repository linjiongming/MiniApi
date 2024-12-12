using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Security.Authentication;
using System.Threading;
using System.Web.Http.Controllers;

namespace System.Web.Http
{
    public class AuthAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.RequestContext.Configuration.Services.GetService(typeof(ITokenProvider)) is ITokenProvider tokenProvider)
            {
                if (!actionContext.Request.Headers.TryGetValues("token", out var values))
                    throw new AuthenticationException("Token cannot be null.");
                Thread.CurrentPrincipal = actionContext.RequestContext.Principal = tokenProvider.Validate(values.FirstOrDefault());
            }
        }
    }
}
