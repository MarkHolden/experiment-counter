using ExperimentCounter.Models;
using ExperimentCounter.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ExperimentCounter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ITestService testService;

        public TestController(ITestService testService)
        {
            this.testService = testService;
        }

        /// <summary>
        /// Get tests from remote service and return processed results.
        /// </summary>
        /// <param name="seed">Sets the random number seed so you get the same results every time.</param>
        /// <param name="count">Sets the number of arrays to return.</param>
        /// <param name="length">Sets the length of the arrays returned.</param>
        /// <returns>Processed results of Experiments.</returns>
        [HttpGet]
        public async Task<IActionResult> Get(string seed, int? count, int? length)
        {
            try
            {
                var result = await this.testService.GetTestCounts(seed, count, length);
                return this.Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Submit tests and return processed results.
        /// </summary>
        /// <param name="tests">Experiment data.</param>
        /// <returns>Processed results of Experiments.</returns>
        [HttpPost]
        public IActionResult Post(Tests tests)
        {
            try
            {
                var result = this.testService.GetTestCounts(tests);
                return this.Ok(result);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}
