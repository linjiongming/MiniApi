using Microsoft.IdentityModel.Tokens;
using System.Net.Http;
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
                if (RequestContext.Configuration.Services.GetService(typeof(ITokenProvider)) is ITokenProvider tokenProvider)
                {
                    TokenInfo tokenInfo = tokenProvider.Create(username);
                    return HttpResult.OK(tokenInfo);
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
