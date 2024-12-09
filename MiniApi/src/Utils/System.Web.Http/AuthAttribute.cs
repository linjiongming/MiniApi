using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace System.Web.Http
{
    public class AuthAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (actionContext.RequestContext.Configuration.Services.GetService(typeof(IAuthProvider)) is IAuthProvider authProvider)
            {
                if (!actionContext.Request.Headers.TryGetValues("token", out var values))
                    throw new AuthenticationException("Token cannot be null.");
                Thread.CurrentPrincipal = actionContext.RequestContext.Principal = authProvider.Authenticate(values.FirstOrDefault());
            }
        }
    }
    public interface IAuthProvider
    {
        string Authorize(params Claim[] claims);
        IPrincipal Authenticate(string token);
    }
}
