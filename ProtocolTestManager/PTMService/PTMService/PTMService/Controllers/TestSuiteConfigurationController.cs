using Microsoft.AspNetCore.Mvc;

namespace PTMService.Controllers
{
    /// <summary>
    /// Test suite configuration controller.
    /// </summary>
    [Route("api/configuration")]
    [ApiController]
    public class TestSuiteConfigurationController : ControllerBase
    {
        /// <summary>
        /// Get configurations.
        /// </summary>
        /// <returns>The configurations.</returns>
        [HttpGet]
        public Configuration[] GetConfigurations()
        {
            return new Configuration[]
            {
                new Configuration
                {
                    Id = 1123,
                }
            };
        }

        /// <summary>
        /// Create a default configuration.
        /// </summary>
        /// <param name="request">The configuration to create.</param>
        /// <returns>The configuration Id.</returns>
        [HttpPost]
        public int CreateConfiguration(Configuration request)
        {
            return 123;
        }

        /// <summary>
        /// Get rules of configuration.
        /// </summary>
        /// <param name="id">The configuration Id.</param>
        /// <returns>The rule groups.</returns>
        [Route("{id}/rule")]
        [HttpGet]
        public RuleGroup[] GetRules(int id)
        {
            return new RuleGroup[]
            {
                new RuleGroup
                {
                    Name = "Kind",
                },
            };
        }

        /// <summary>
        /// Set rules of configuration.
        /// </summary>
        /// <param name="id">The configuration Id.</param>
        /// <param name="rules">The rule groups to set.</param>
        /// <returns>The action result.</returns>
        [Route("{id}/rule")]
        [HttpPut]
        public IActionResult SetRules(int id, RuleGroup[] rules)
        {
            return Ok();
        }

        /// <summary>
        /// Get properties of configuration.
        /// </summary>
        /// <param name="id">The configuration Id.</param>
        /// <returns>The properties.</returns>
        [Route("{id}/property")]
        [HttpGet]
        public PropertyGroup[] GetProperties(int id)
        {
            return new PropertyGroup[]
            {
                new PropertyGroup
                {
                    Name = "SMB2",
                    Items = new Property[]
                    {
                        new Property
                        {
                            Name = "SUTAddress",
                        },
                    },
                },
            };
        }

        /// <summary>
        /// Set properties of configuration.
        /// </summary>
        /// <param name="id">The configuration Id.</param>
        /// <param name="properties">The properties to set.</param>
        /// <returns>The action result.</returns>
        [Route("{id}/property")]
        [HttpPut]
        public IActionResult SetProperties(int id, PropertyGroup[] properties)
        {
            return Ok();
        }

        /// <summary>
        /// Get adapters of configuration.
        /// </summary>
        /// <param name="id">The configuration Id.</param>
        /// <returns>The adapters.</returns>
        [Route("{id}/adapter")]
        [HttpGet]
        public Adapter[] GetAdapters(int id)
        {
            return new Adapter[]
            {
                new Adapter
                {
                    Name = "SUTControlAdapter",
                    Kind = AdapterKind.PowerShell,
                },
            };
        }

        /// <summary>
        /// Set adapters of configuration.
        /// </summary>
        /// <param name="id">The configuration Id.</param>
        /// <param name="adapters">The adapters to set.</param>
        /// <returns>The action result.</returns>
        [Route("{id}/adapter")]
        [HttpPut]
        public IActionResult SetAdapters(int id, Adapter[] adapters)
        {
            return Ok();
        }
    }
}
