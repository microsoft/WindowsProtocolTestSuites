using Microsoft.AspNetCore.Mvc;

namespace PTMService.Controllers
{
    /// <summary>
    /// Test suite info controller.
    /// </summary>
    [Route("api/testsuite")]
    [ApiController]
    public class TestSuiteInfoController : ControllerBase
    {
        /// <summary>
        /// Get test suites.
        /// </summary>
        /// <returns>An array containing test suites.</returns>
        [HttpGet]
        public TestSuite[] GetTestSuites()
        {
            System.Threading.Thread.Sleep(1000 * 5);
            return new TestSuite[]
            {
                new TestSuite
                {
                    Name = "File Server Test Suite",
                    Description="It is designed to test implementations of file server protocol family including [MS-SMB2], [MS-DFSC], [MS-SWN], [MS-FSRVP], [MS-FSA], [MS-FSCC], [MS-RSVD] and [MS-SQOS]",
                    Version = "4.21.1.0"
                },
            };
        }

        /// <summary>
        /// Get detail of a specific test suite.
        /// </summary>
        /// <param name="id">The test suite Id.</param>
        /// <returns>The detail of test suite.</returns>
        [Route("{id}")]
        [HttpGet]
        public TestSuite GetTestSuiteDetail(int id)
        {
            return new TestSuite
            {
                Name = "FileServer",
                TestCases = new TestCase[]
                {
                    new TestCase
                    {
                        Name= "Test",
                        Categories = new string[]
                        {
                            "BVT",
                        },
                    },
                },
            };
        }
    }
}
