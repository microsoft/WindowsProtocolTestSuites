using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PTMService.Controllers
{
    /// <summary>
    /// Test suite management controller.
    /// </summary>
    [Route("api/management/testsuite")]
    [ApiController]
    public class TestSuiteManagementController : ControllerBase
    {
        /// <summary>
        /// Install request.
        /// </summary>
        public class InstallRequest
        {
            /// <summary>
            /// Test suite name.
            /// </summary>
            public string TestSuiteName { get; set; }

            /// <summary>
            /// Test suite package.
            /// </summary>
            public IFormFile Package { get; set; }
        }

        /// <summary>
        /// Install a test suite by uploading its package.
        /// </summary>
        /// <param name="request">The install request.</param>
        /// <returns>The action result.</returns>
        [HttpPost]
        public IActionResult Install([FromForm] InstallRequest request)
        {
            return Ok();
        }
    }
}
