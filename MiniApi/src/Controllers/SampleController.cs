using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using TestApi.Models;

namespace MiniApi.Controllers
{
    /// <summary>
    /// Sample Controller Description
    /// </summary>
    public class SampleController : ApiController
    {
        private static readonly SampleModel _model = new SampleModel();

        /// <summary>
        /// Addition caculate
        /// </summary>
        /// <param name="num">Number cannot be zero</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResult<SampleModel>> Add(uint num)
        {
            if (num == 0)
            {
                throw new ArgumentException($"Parameter '{nameof(num)}' cannot be zero").AddData(nameof(num), num);
            }
            await _model.AddAsync(num);
            return HttpResult.OK(_model);
        }

        /// <summary>
        /// Subtraction caculate
        /// </summary>
        /// <param name="num">Number cannot be zero</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResult<SampleModel>> Subtract(uint num)
        {
            if (num == 0)
            {
                throw new ArgumentException($"Parameter '{nameof(num)}' cannot be zero").AddData(nameof(num), num);
            }
            await _model.SubtractAsync(num);
            return HttpResult.OK(_model);
        }

        /// <summary>
        /// Multiplication caculate
        /// </summary>
        /// <param name="num">Number cannot be zero</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResult<SampleModel>> Multiply(uint num)
        {
            if (num == 0)
            {
                throw new ArgumentException($"Parameter '{nameof(num)}' cannot be zero").AddData(nameof(num), num);
            }
            await _model.MultiplyAsync(num);
            return HttpResult.OK(_model);
        }

        /// <summary>
        /// Division caculate
        /// </summary>
        /// <param name="num">Number cannot be zero</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<HttpResult<SampleModel>> Divide(uint num)
        {
            if (num == 0)
            {
                throw new ArgumentException($"Parameter '{nameof(num)}' cannot be zero").AddData(nameof(num), num);
            }
            await _model.DivideAsync(num);
            return HttpResult.OK(_model);
        }
    }
}
