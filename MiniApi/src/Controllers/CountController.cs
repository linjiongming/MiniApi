using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MiniApi.Controllers
{
    public class CountController : ApiController
    {
        private static int count;

        [HttpPost]
        public async Task<HttpResult> AddAsync(int num = 1)
        {
            if (num < 1)
            {
                throw new ArgumentException($"Parameter '{nameof(num)}' cannot be zero or negative").AddData(nameof(num), num);
            }
            await Task.Run(() => count += num);
            return HttpResult.OK(count);
        }
    }
}
