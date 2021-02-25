using Microsoft.AspNetCore.Mvc;

namespace PTMService.Controllers
{
    /// <summary>
    /// Test result controller.
    /// </summary>
    [Route("api/testresult")]
    [ApiController]
    public class TestResultController : ControllerBase
    {
        /// <summary>
        /// List response.
        /// </summary>
        public class ListResponse
        {
            /// <summary>
            /// The total page count.
            /// </summary>
            public int PageCount { get; set; }

            /// <summary>
            /// The test results.
            /// </summary>
            public TestResult[] TestResults { get; set; }
        }

        /// <summary>
        /// List test results.
        /// </summary>
        /// <param name="max">Maximum count per page.</param>
        /// <param name="page">Page number.</param>
        /// <returns>The list response.</returns>
        [HttpGet]
        public ListResponse List(int max, int page)
        {
            return new ListResponse
            {
                PageCount = 10,
                TestResults = new TestResult[]
                {
                    new TestResult
                    {
                        Id = 321,
                        ConfigurationId = 1234,
                    },
                },
            };
        }

        /// <summary>
        /// Get detail of a specific test result.
        /// </summary>
        /// <param name="id">The Id of test result.</param>
        /// <returns>The test result.</returns>
        [Route("{id}")]
        [HttpGet]
        public TestResult Get(int id)
        {
            return new TestResult
            {
                Id = 321,
                ConfigurationId = 1234,
                NotRun = 55,
            };
        }

        /// <summary>
        /// Get result of a specific test case.
        /// </summary>
        /// <param name="id">The Id of test result.</param>
        /// <param name="testCaseId">The Id of test case.</param>
        /// <returns>The test case result.</returns>
        [Route("{id}/testcase")]
        [HttpGet]
        public TestCaseResult GetTestCaseResult(int id, int testCaseId)
        {
            return new TestCaseResult
            {
                Id = 12345,
                Name = "CreateFileTestCaseS8",
                State = TestCaseState.Passed,
            };
        }
    }
}
