// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using System.Linq;

namespace Microsoft.Protocols.TestManager.PTMService.PTMService.Controllers
{
    /// <summary>
    /// Test suite configuration controller.
    /// </summary>
    [Route("api/configuration")]
    [ApiController]
    public class TestSuiteConfigurationController : PTMServiceControllerBase
    {
        /// <summary>
        /// Constructor of test suite configuration controller.
        /// </summary>
        /// <param name="ptmKernelService">The PTM kernel service.</param>
        public TestSuiteConfigurationController(IPTMKernelService ptmKernelService)
            : base(ptmKernelService)
        {
        }

        /// <summary>
        /// Get configurations.
        /// </summary>
        /// <param name="testSuiteId">The optional test suite Id.</param>
        /// <returns>The configurations.</returns>
        [HttpGet]
        public Configuration[] GetConfigurations(int? testSuiteId)
        {
            var configurations = PTMKernelService.QueryConfigurations(testSuiteId);

            var result = configurations.Select(configuration => new Configuration
            {
                Id = configuration.Id,
                Name = configuration.Name,
                TestSuiteId = configuration.TestSuite.Id,
                Description = configuration.Description,
            });

            return result.ToArray();
        }

        /// <summary>
        /// Create a default configuration.
        /// </summary>
        /// <param name="request">The configuration to create.</param>
        /// <returns>The configuration Id.</returns>
        [HttpPost]
        public int CreateConfiguration(Configuration request)
        {
            int result = PTMKernelService.CreateConfiguration(request.Name, request.TestSuiteId, request.Description);

            return result;
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
                    Name = "All",
                    DisplayName ="All",
                    Rules = new Rule[]
                    {
                        new Rule
                        {
                            Type = RuleType.Selector,
                            Name = "All",
                            DisplayName = "All",
                            Categories = new string [] { "All", },
                        },
                    },
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
        public PropertyGetItemGroup[] GetProperties(int id)
        {
            var configuration = PTMKernelService.GetConfiguration(id);

            var result = configuration.Properties.Select(group => new PropertyGetItemGroup
            {
                Name = group.Name,
                Items = group.Items.Select(property => new PropertyGetItem
                {
                    Key = property.Key,
                    Name = property.Name,
                    Description = property.Description,
                    Choices = property.Choices?.ToArray(),
                    Value = property.Value,
                }).ToArray(),
            }).ToArray();

            return result;
        }

        /// <summary>
        /// Set properties of configuration.
        /// </summary>
        /// <param name="id">The configuration Id.</param>
        /// <param name="properties">The properties to set.</param>
        /// <returns>The action result.</returns>
        [Route("{id}/property")]
        [HttpPut]
        public IActionResult SetProperties(int id, PropertySetItemGroup[] properties)
        {
            var configuration = PTMKernelService.GetConfiguration(id);

            configuration.Properties = properties.Select(group => new PropertyGroup
            {
                Name = group.Name,
                Items = group.Items.Select(item => new Property
                {
                    Key = item.Key,
                    Value = item.Value,
                }),
            });

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
            var configuration = PTMKernelService.GetConfiguration(id);

            var result = configuration.Adapters.ToArray();

            return result;
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
            var configuration = PTMKernelService.GetConfiguration(id);

            configuration.Adapters = adapters;

            return Ok();
        }

        /// <summary>
        /// Get all applicable test cases of configuration.
        /// </summary>
        /// <param name="id">The configuration Id.</param>
        /// <returns>All the applicable test cases.</returns>
        [Route("{id}/test")]
        [HttpGet]
        public string[] GetTests(int id)
        {
            var configuration = PTMKernelService.GetConfiguration(id);

            var result = configuration.GetApplicableTestCases().ToArray();

            return result;
        }
    }
}
