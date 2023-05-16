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
    /// Represents a capabilities file that can be used to configure tests filtering and display and
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
    public class CapabilitiesConfig
    {
        private readonly Dictionary<string, Dictionary<string, HashSet<string>>> testsByCategories;

        /// <summary>
        /// Creates a new instance of <see cref="CapabilitiesConfig"/>.
        /// </summary>
        /// <param name="testsByCategories">A dictionary of test cases by groups and categories representing the inner state of the
        /// <see cref="CapabilitiesConfig"/> file.</param>
        private CapabilitiesConfig(Dictionary<string, Dictionary<string, HashSet<string>>> testsByCategories)
        {
            this.testsByCategories = testsByCategories;
        }

        /// <summary>
        /// Parses a category identifier in the format '{groupName}.{categoryName}'. If only the group name
        /// is specified, an empty category name is returned.
        /// </summary>
        /// <param name="identifier">The identifier to parse. Should be in the form '{groupName}.{categoryName}'.</param>
        /// <returns>A tuple containing the group and the category.</returns>
        private static (string group, string category) ParseCategoryInfo(string identifier)
        {
            var identifierSeperator = '.';
            var group = string.Empty;
            var category = string.Empty;

            if (!string.IsNullOrWhiteSpace(identifier))
            {
                identifier = identifier.ToLowerInvariant();

                var identifierSeperatorIndex =
                    identifier.IndexOf(identifierSeperator, StringComparison.InvariantCulture);
                if (identifierSeperatorIndex == -1) // Only group name specified.
                {
                    group = identifier.Trim();
                    category = string.Empty;
                }
                else
                {
                    group = identifier.Substring(0, identifierSeperatorIndex).Trim();
                    category = identifier.Substring(identifierSeperatorIndex + 1).Trim();
                }
            }

            return (group, category);
        }

        /// <summary>
        /// Gets the categories in a given group within the capabilities file.
        /// </summary>
        /// <param name="group">The group to get the categories for.</param>
        /// <returns>The categories for the group.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the specified group is not found in the capabilities file or the specified
        /// group name is empty.
        /// </exception>
        private Dictionary<string, HashSet<string>> GetCategories(string group)
        {
            group = group?.Trim()?.ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(group))
            {
                throw new InvalidOperationException($"Capabilities file group names cannot be empty.");
            }

            if (!testsByCategories.ContainsKey(group))
            {
                throw new InvalidOperationException($"Group with name '{group}' does not exist in this capabilities file.");
            }

            return testsByCategories[group];
        }

        /// <summary>
        /// Gets test cases for a given group within the capabilities file.
        /// </summary>
        /// <param name="group">The group to get the test cases for.</param>
        /// <returns>The test cases in the specified group.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the specified group is not found in the capabilities file or the specified
        /// group name is empty.
        /// </exception>
        public string[] GetTestCases(string group)
        {
            var categories = GetCategories(group);

            return categories.SelectMany(g => g.Value).ToArray();
        }

        /// <summary>
        /// Gets test cases for a given group and category within the capabilities file.
        /// </summary>
        /// <param name="group">The group to get the test cases for.</param>
        /// <param name="category">The category within the group to get the test cases for.</param>
        /// <returns>The test cases within the category in the specified group.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the specified group or category is not found in the capabilities file or the specified
        /// group or category name is empty.
        /// </exception>
        public string[] GetTestCases(string group, string category)
        {
            if (string.IsNullOrWhiteSpace(group) || string.IsNullOrWhiteSpace(category))
            {
                throw new InvalidOperationException($"Capabilities file group or category names cannot be empty.");
            }

            var categories = GetCategories(group);
            if (!categories.ContainsKey(category))
            {
                throw new InvalidOperationException($"Category with name '{category}' does not exist within the group, '{group}' in this capabilities file.");
            }

            return categories[category].ToArray();
        }

        /// <summary>
        /// Gets test cases matching filters specified in the format {groupName}.{categoryName} or {groupName}.
        /// </summary>
        /// <param name="filters">The filters to use in retrieving the test cases.</param>
        /// <returns>The test cases matching the specified filters.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the specified group or category in any of the filters is not found in the capabilities 
        /// file.
        /// </exception>
        public string[] GetTestCases(params string[] filters)
        {
            return filters.Select(f => ParseCategoryInfo(f))
                          .SelectMany(r =>
                          {
                              return r.category == string.Empty ?
                                          GetTestCases(r.group) :
                                          GetTestCases(r.group, r.category);
                          }
            ).ToArray();
        }

        /// <summary>
        /// Creates a capabilities file from a test suite reference.
        /// </summary>
        /// <param name="ptmKernelService">The <see cref="IPTMKernelService"/> instance for managing the test suite operations.</param>
        /// <param name="testSuiteId">The Id of the test suite to create the file from.</param>
        /// <returns>The created file in Json format.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the given test suite does not exist or has been removed.
        /// </exception>
        public static JsonNode Create(IPTMKernelService ptmKernelService, int testSuiteId)
        {
            var testSuite = ptmKernelService.GetTestSuite(testSuiteId);

            if (testSuite != default(ITestSuite) || testSuite.Removed)
            {
                throw new InvalidOperationException($"The test suite with the Id, {testSuiteId} has been removed.");
            }

            var testCases = testSuite.GetTestCases(filter: null);

            var document = new JsonObject
            {
                ["capabilities"] = new JsonObject()
                {
                    ["metadata"] = new JsonObject
                    {
                        ["testsuite"] = "testSuite.Name",
                        ["version"] = "testSuite.Version",
                    },
                    ["categories"] = new JsonArray(),
                    ["testcases"] = new JsonArray(
                                    testCases.Select(c => new JsonObject
                                    {
                                        ["name"] = c.Name,
                                        ["categories"] = new JsonArray()
                                    }).ToArray()
                                )
                }
            };

            return document;
        }

        /// <summary>
        /// Creates a <see cref="CapabilitiesConfig"/>, given a Json document.
        /// </summary>
        /// <param name="json">The Json document to create the <see cref="CapabilitiesConfig"/> from.</param>
        /// <returns>The created <see cref="CapabilitiesConfig"/> instance.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the given Json document does not represent a valid capabilities file.
        /// </exception>
        public static CapabilitiesConfig Parse(JsonNode json)
        {
            var testsByCategories =
                new Dictionary<string, Dictionary<string, HashSet<string>>>();

            var document = json["capabilities"];

            var testSuite = document?["metadata"]?["testsuite"]?.ToString();
            var version = document?["metadata"]?["version"]?.ToString();

            if (string.IsNullOrEmpty(testSuite) || string.IsNullOrEmpty(version))
            {
                throw new InvalidOperationException("Test suite name and version must be specified for a capabilities file.");
            }

            var groups = document?["groups"] as JsonArray ?? new JsonArray();
            foreach (var group in groups)
            {
                var groupName = group?["name"]?.ToString()?.Trim()?.ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(groupName) || testsByCategories.ContainsKey(groupName))
                {
                    throw new InvalidOperationException($"Group names cannot be empty and must be unique within the capabilities file | Group: {groupName}.");
                }

                testsByCategories.Add(groupName, new());

                var categories = group["categories"] as JsonArray ?? new JsonArray();
                foreach (var category in categories)
                {
                    var categoryName = category["name"]?.ToString()?.Trim()?.ToLowerInvariant();
                    if (string.IsNullOrWhiteSpace(categoryName) || testsByCategories[groupName].ContainsKey(categoryName))
                    {
                        throw new InvalidOperationException($"Category names cannot be empty and must be unique within a group in capabilities file | Group: {groupName}, Category: {categoryName}.");
                    }

                    testsByCategories[groupName].Add(categoryName, new HashSet<string>());
                }
            }

            var testCases = document?["testcases"] as JsonArray ?? new JsonArray();
            foreach (var testCase in testCases)
            {
                var testName = testCase["name"]?.ToString()?.Trim();
                if (string.IsNullOrWhiteSpace(testName))
                {
                    throw new InvalidOperationException("Test case names cannot be empty.");
                }

                var testCategories = testCase["categories"] as JsonArray ?? new JsonArray();
                foreach (var testCategory in testCategories)
                {
                    var identifier = testCategory.AsValue().ToString();
                    (string group, string category) = ParseCategoryInfo(identifier);

                    if (!testsByCategories.ContainsKey(group) || !testsByCategories[group].ContainsKey(category))
                    {
                        throw new InvalidOperationException($"Test, '{testName}' has an invalid group or category name | Group: {group}, Category: {category}.");
                    }

                    if (!testsByCategories[group][category].Contains(testName))
                    {
                        testsByCategories[group][category].Add(testName);
                    }
                }
            }

            return new CapabilitiesConfig(testsByCategories);
        }

        /// <summary>
        /// Creates a <see cref="CapabilitiesConfig", given a Json string./>
        /// </summary>
        /// <param name="json">The Json string to create the <see cref="CapabilitiesConfig"/> from.</param>
        /// <returns>The created <see cref="CapabilitiesConfig"/> instance.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the given json string is not properly formatted or the the Json document
        /// does not represent a valid capabilities file.
        /// </exception>
        public static CapabilitiesConfig Parse(string json)
        {
            try
            {
                var document =
                    JsonNode.Parse(json);

                return Parse(document);
            }
            catch (JsonException)
            {
                throw new InvalidOperationException("The provided capabilities file is not a valid Json file.");
            }
        }
    }
}
