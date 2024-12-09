using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace MiniApi.Controllers
{
    public class TestController : ApiController
    {
        static int count;

        [HttpPost]
        public HttpResult Login(string username, string password)
        {
            if (username == "admin" && password == "123")
            {
                if (RequestContext.Configuration.Services.GetService(typeof(IAuthProvider)) is IAuthProvider authProvider)
                {
                    string token = authProvider.Authorize(new Claim(ClaimTypes.Name, username));
                    return HttpResult.OK(token);
                }
            }
            return HttpResult.Unauthorized();
        }

        [HttpPost]
        public HttpResult Add()
        {
            return HttpResult.OK(++count);
        }

        [HttpPost, Auth]
        public HttpResult Subtract()
        {
            return HttpResult.OK(--count);
        }
    }
}
