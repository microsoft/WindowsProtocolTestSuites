using Microsoft.AspNetCore.Mvc;

namespace PTMService.Controllers
{
    /// <summary>
    /// The test suite run controller.
    /// </summary>
    [Route("api/run")]
    [ApiController]
    public class TestSuiteRunController : ControllerBase
    {
        /// <summary>
        /// Run request.
        /// </summary>
        public class RunRequest
        {
            /// <summary>
            /// Configuration Id.
            /// </summary>
            public int ConfigurationId { get; set; }

            /// <summary>
            /// Selected test cases.
            /// </summary>
            public string[] SelectedTestCases { get; set; }
        }

        /// <summary>
        /// Run test suite.
        /// </summary>
        /// <param name="request">The run request.</param>
        /// <returns>The test result Id.</returns>
        [HttpPost]
        public int Run(RunRequest request)
        {
            return 1234;
        }

        /// <summary>
        /// Abort a running test suite.
        /// </summary>
        /// <param name="id">The test result Id.</param>
        /// <returns>The action result.</returns>
        [Route("{id}")]
        [HttpPut]
        public IActionResult Abort(int id)
        {
            return Ok();
        }
    }
}
