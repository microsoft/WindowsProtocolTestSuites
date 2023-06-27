// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    /// <summary>
    /// Represents a writer for a capabilities file that can be used to configure tests filtering and display and
    /// test results grouping within PTM Service and PTM CLI.
    /// </summary>
    /// <example>
    /// The capabilities file has the following structure:
    /// <code>
    ///{
    /// "capabilities": {
    ///    "metadata": {
    ///     "testsuite": "FileServer",
    ///     "version": "4.9.22"
    /// },
    /// "groups": [
    ///  {
    ///    "name": "Priority",
    ///      "categories": [
    ///      {
    ///        "name": "BVT"
    ///      }
    ///    ]
    ///  }
    /// ],
    /// "testcases": [
    ///  {
    ///    "name": "BVT_Negotiate_Compatible_2002",
    ///    "categories": ["Priority.BVT"]
    ///  }
    /// ]
    /// }
    ///}
    /// </code>
    /// </example>
    public class CapabilitiesConfigWriter
    {
        public static string RemovedOrNullTestSuiteMessage(int testSuiteId) =>
            $"The test suite with the Id, {testSuiteId} has been removed.";

        /// <summary>
        /// Creates a capabilities file from a test suite reference.
        /// </summary>
        /// <param name="ptmKernelService">The <see cref="IPTMKernelService"/> instance for managing the test suite operations.</param>
        /// <param name="testSuiteId">The Id of the test suite to create the file from.</param>
        /// <returns>A tuple of the created file in Json format and the test suite used in creating the file.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the given test suite does not exist or has been removed.
        /// </exception>
        public static (JsonNode, ITestSuite) Create(IPTMKernelService ptmKernelService, int testSuiteId)
        {
            var testSuite = ptmKernelService.GetTestSuite(testSuiteId);

            if (testSuite == default(ITestSuite) || testSuite.Removed)
            {
                throw new InvalidOperationException(RemovedOrNullTestSuiteMessage(testSuiteId));
            }

            var testCases = testSuite.GetTestCases(filter: null);

            var document = new JsonObject
            {
                ["capabilities"] = new JsonObject()
                {
                    ["metadata"] = new JsonObject
                    {
                        ["testsuite"] = testSuite.Name,
                        ["version"] = testSuite.Version,
                    },
                    ["groups"] = new JsonArray(),
                    ["testcases"] = new JsonArray(
                                    testCases.Select(c => new JsonObject
                                    {
                                        ["name"] = c.Name,
                                        ["categories"] = new JsonArray()
                                    }).ToArray()
                                )
                }
            };

            return (document, testSuite);
        }
    }
}
