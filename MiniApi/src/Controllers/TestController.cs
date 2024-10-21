using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace MiniApi.Controllers
{
    public class TestController : ApiController
    {
        static int count;

        [HttpPost]
        public HttpResult Add()
        {
            return HttpResult.OK(++count);
        }

        [HttpPost]
        public HttpResult Subtract()
        {
            return HttpResult.OK(--count);
        }
    }
}
