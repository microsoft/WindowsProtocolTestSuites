// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// Represents a reader for a capabilities file that can be used to configure tests filtering and display and
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
    public class CapabilitiesConfigReader
    {
        public static string MissingTestSuiteOrVersionMessage =>
            @"Test suite name and version must be specified for a capabilities file.";
        public static string EmptyOrDuplicateGroupNameMessage(string groupName) =>
            $"Group names cannot be empty and must be unique within the capabilities file | Group: {groupName}.";
        public static string EmptyOrDuplicateCategoryNameMessage(string groupName, string categoryName) =>
            $"Category names cannot be empty and must be unique within a group in capabilities file | Group: {groupName}, Category: {categoryName}.";
        public static string EmptyTestCaseNameMessage =>
            @"Test case names cannot be empty.";
        public static string InvalidGroupOrCategoryMessage(string testName, string groupName, string categoryName) =>
            $"Test, '{testName}' has an invalid group or category name | Group: {groupName}, Category: {categoryName}.";
        public static string InvalidJsonMessage =>
            @"The provided capabilities file is not a valid Json file.";
        public static string EmptyTestCaseFilterGroupOrCategoryNameMessage =>
            $"Capabilities file group or category names cannot be empty.";
        public static string EmptyTestCaseFilterGroupNameMessage =>
            $"Capabilities file group names cannot be empty.";
        public static string NonExistentGroupNameMessage(string groupName) =>
            $"Group with name '{groupName}' does not exist in this capabilities file.";
        public static string NonExistentCategoryNameMessage(string groupName, string categoryName) =>
            $"Category with name '{categoryName}' does not exist within the group, '{groupName}' in this capabilities file.";


        private readonly Dictionary<string, Dictionary<string, HashSet<string>>> testsByCategories;

        /// <summary>
        /// Creates a new instance of <see cref="CapabilitiesConfigReader"/>.
        /// </summary>
        /// <param name="testsByCategories">A dictionary of test cases by groups and categories representing the inner state of the
        /// capabilities file.</param>
        private CapabilitiesConfigReader(Dictionary<string, Dictionary<string, HashSet<string>>> testsByCategories)
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
                throw new InvalidOperationException(EmptyTestCaseFilterGroupNameMessage);
            }

            if (!testsByCategories.ContainsKey(group))
            {
                throw new InvalidOperationException(NonExistentGroupNameMessage(group));
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
                throw new InvalidOperationException(EmptyTestCaseFilterGroupOrCategoryNameMessage);
            }

            category = category?.Trim()?.ToLowerInvariant();
            var categories = GetCategories(group);
            if (!categories.ContainsKey(category))
            {
                throw new InvalidOperationException(NonExistentCategoryNameMessage(group, category));
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
        public string[] GetTestCases(string[] filters)
        {
            return filters.Select(f => ParseCategoryInfo(f))
                          .SelectMany(r =>
                          {
                              return r.category == string.Empty ?
                                          GetTestCases(r.group) :
                                          GetTestCases(r.group, r.category);
                          }
            ).Distinct()
             .ToArray();
        }

        /// <summary>
        /// Creates a <see cref="CapabilitiesConfigReader"/>, given a Json document.
        /// </summary>
        /// <param name="json">The Json document to create the <see cref="CapabilitiesConfigReader"/> from.</param>
        /// <returns>The created <see cref="CapabilitiesConfigReader"/> instance.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the given Json document does not represent a valid capabilities file.
        /// </exception>
        public static CapabilitiesConfigReader Parse(JsonNode json)
        {
            var testsByCategories =
                new Dictionary<string, Dictionary<string, HashSet<string>>>();

            var document = json["capabilities"];

            var testSuite = document?["metadata"]?["testsuite"]?.ToString();
            var version = document?["metadata"]?["version"]?.ToString();

            if (string.IsNullOrEmpty(testSuite) || string.IsNullOrEmpty(version))
            {
                throw new InvalidOperationException(MissingTestSuiteOrVersionMessage);
            }

            var groups = document?["groups"] as JsonArray ?? new JsonArray();
            foreach (var group in groups)
            {
                var groupName = group?["name"]?.ToString()?.Trim()?.ToLowerInvariant();
                if (string.IsNullOrWhiteSpace(groupName) || testsByCategories.ContainsKey(groupName))
                {
                    throw new InvalidOperationException(EmptyOrDuplicateGroupNameMessage(groupName));
                }

                testsByCategories.Add(groupName, new());

                var categories = group["categories"] as JsonArray ?? new JsonArray();
                foreach (var category in categories)
                {
                    var categoryName = category["name"]?.ToString()?.Trim()?.ToLowerInvariant();
                    if (string.IsNullOrWhiteSpace(categoryName) || testsByCategories[groupName].ContainsKey(categoryName))
                    {
                        throw new InvalidOperationException(EmptyOrDuplicateCategoryNameMessage(groupName, categoryName));
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
                    throw new InvalidOperationException(EmptyTestCaseNameMessage);
                }

                var testCategories = testCase["categories"] as JsonArray ?? new JsonArray();
                foreach (var testCategory in testCategories)
                {
                    var identifier = testCategory.AsValue().ToString();
                    (string group, string category) = ParseCategoryInfo(identifier);

                    if (!testsByCategories.ContainsKey(group) || !testsByCategories[group].ContainsKey(category))
                    {
                        throw new InvalidOperationException(InvalidGroupOrCategoryMessage(testName, group, category));
                    }

                    if (!testsByCategories[group][category].Contains(testName))
                    {
                        testsByCategories[group][category].Add(testName);
                    }
                }
            }

            return new CapabilitiesConfigReader(testsByCategories);
        }

        /// <summary>
        /// Creates a <see cref="CapabilitiesConfigReader" />, given a Json string.
        /// </summary>
        /// <param name="json">The Json string to create the <see cref="CapabilitiesConfigReader"/> from.</param>
        /// <returns>The created <see cref="CapabilitiesConfigReader"/> instance.</returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the given Json string is not properly formatted or the the Json document
        /// does not represent a valid capabilities file.
        /// </exception>
        public static CapabilitiesConfigReader Parse(string json)
        {
            try
            {
                var document =
                    JsonNode.Parse(json);

                return Parse(document);
            }
            catch (JsonException)
            {
                throw new InvalidOperationException(InvalidJsonMessage);
            }
        }

        /// <summary>
        /// Creates a <see cref="CapabilitiesConfigReader" />, given a path to a capabilities file.
        /// </summary>
        /// <param name="file">The Json string to create the <see cref="CapabilitiesConfigReader"/> from.</param>
        /// <returns>The created <see cref="CapabilitiesConfigReader"/> instance.</returns>
        public static CapabilitiesConfigReader Parse(FileInfo file)
        {
            using (var reader = file.OpenText())
            {
                var json = reader.ReadToEnd();

                return Parse(json);
            }
        }
    }
}
