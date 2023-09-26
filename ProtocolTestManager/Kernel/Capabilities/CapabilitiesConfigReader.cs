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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.Kernel
{
    file record TestCaseInfo(string name, string[] categories);

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
        public static string InvalidCharactersInGroupNameMessage(string groupName) =>
            $"Group names can contain only alphanumeric, hyphen, underscore, space and period characters. | Group: {groupName}.";
        public static string EmptyOrDuplicateCategoryNameMessage(string groupName, string categoryName) =>
            $"Category names cannot be empty and must be unique within a group in capabilities file | Group: {groupName}, Category: {categoryName}.";
        public static string InvalidCharactersInCategoryNameMessage(string groupName, string categoryName) =>
            $"Category names can contain only alphanumeric, hyphen, underscore, space and period characters. | Group: {groupName}, Category: {categoryName}.";
        public static string EmptyTestCaseNameMessage =>
            @"Test case names cannot be empty.";
        public static string InvalidGroupOrCategoryMessage(string testName, string groupName, string categoryName) =>
            $"Test, '{testName}' has an invalid group or category name | Group: {groupName}, Category: {categoryName}.";
        public static string UnknownGroupMessage(string groupName) =>
            $"Group: {groupName} does not exist.";
        public static string UnknownCategoryMessage(string categoryName) =>
            $"Category: {categoryName} does not exist.";
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
        private readonly Dictionary<string, HashSet<string>> filtersByTest;

        /// <summary>
        /// Creates a new instance of <see cref="CapabilitiesConfigReader"/>.
        /// </summary>
        /// <param name="testsByCategories">A dictionary of test cases by groups and categories representing the inner state of the
        /// capabilities file.</param>
        /// <param name="filtersByTest">A dictionary of categories by test case representing the inner state of the
        /// capabilities file.</param>
        /// <param name="json"><see cref="JsonNode"/> representing the source Json.</param>
        private CapabilitiesConfigReader(Dictionary<string, Dictionary<string, HashSet<string>>> testsByCategories,
            Dictionary<string, HashSet<string>> filtersByTest, JsonNode json)
        {
            this.testsByCategories = testsByCategories;
            this.filtersByTest = filtersByTest;
            this.Json = json;
        }

        private static bool ContainsInvalidCharacters(string name)
        {
            var regex = new Regex($"^[A-Za-z0-9-_ ]+$"); // Allow alphanumeric, hyphen, underscores and space(s).

            if (!regex.IsMatch(name))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Parses a category identifier in the format '{groupName}.{categoryName}'. If only the group name
        /// is specified, an empty category name is returned.
        /// </summary>
        /// <param name="identifier">The identifier to parse. Should be in the form '{groupName}.{categoryName}'.</param>
        /// <returns>A tuple containing the group and the category.</returns>
        private static (string group, string category) ParseCategoryInfo(string identifier)
        {
            var identifierSeparator = '.';
            var group = string.Empty;
            var category = string.Empty;

            if (!string.IsNullOrWhiteSpace(identifier))
            {
                var identifierSeparatorIndex =
                    identifier.IndexOf(identifierSeparator, StringComparison.InvariantCulture);
                if (identifierSeparatorIndex == -1) // Only group name specified.
                {
                    group = identifier.Trim();
                    category = string.Empty;
                }
                else
                {
                    group = identifier.Substring(0, identifierSeparatorIndex).Trim();
                    category = identifier.Substring(identifierSeparatorIndex + 1).Trim();
                }
            }

            return (group, category);
        }

        /// <summary>
        /// Expands an identifier for a category from the format {GroupName}.{CategoryName}
        /// into an enumerable of tuples of the constituent group and categor(ies). If no category
        /// is explicitly specified, all the categories in the specified group are returned.
        /// </summary>
        /// <param name="identifier">The identifier to expand.</param>
        /// <returns>An enumerable of tuples of group and category names, matching the specified identifier. </returns>
        /// <exception cref="InvalidOperationException">Thrown when either the specified group or category name does
        /// not exist within this capabilities file.</exception>
        public IEnumerable<(string group, string category)> ExpandIdentifier(string identifier)
        {
            (string group, string category) = ParseCategoryInfo(identifier);
            var groupLowerCase = group.ToLowerInvariant();
            var categoryLowerCase = category.ToLowerInvariant();
            var categories = Enumerable.Empty<string>();

            if (!testsByCategories.ContainsKey(groupLowerCase))
            {
                throw new InvalidOperationException(UnknownGroupMessage(group));
            }

            if(string.IsNullOrWhiteSpace(category))
            {
                // If no category specified, return all the categories in the group.
                categories = testsByCategories[groupLowerCase].Select(c => c.Key).ToArray();
            }
            else
            {
                if (!testsByCategories[groupLowerCase].ContainsKey(categoryLowerCase))
                {
                    throw new InvalidOperationException(UnknownCategoryMessage(category));
                }

                categories = new string[] { category };
            }

            foreach (var c in categories)
            {
                yield return (group, c);
            }
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
        /// Returns the categories for the specified test case.
        /// </summary>
        /// <param name="testCase">The test case to return the categories for.</param>
        /// <returns>An enumerable of categories for the test case in the format: {GroupName}.{CategoryName}.</returns>
        public IEnumerable<string> GetCategoriesFor(string testCase)
        {
            if (!filtersByTest.ContainsKey(testCase))
            {
                return Enumerable.Empty<string>();
            }

            return filtersByTest[testCase].ToArray();
        }

        /// <summary>
        /// Json node read by this reader.
        /// </summary>
        public JsonNode Json
        {
            get;
        }

        /// <summary>
        /// Gets the Json representation of this capabilities file.
        /// </summary>
        /// <param name="skipTestsWithNoCategory">Specifies if to exclude test cases with no categories
        /// from the returned Json. The default is false.</param>
        /// <returns>The Json representation of this capabilities file.</returns>
        public JsonNode GetJson(bool skipTestsWithNoCategory = false)
        {
            if(skipTestsWithNoCategory)
            {
                var capabilities = Json["capabilities"];
                var testCases = 
                    (JsonSerializer.Deserialize<TestCaseInfo[]>(capabilities["testcases"]))
                        .Where(t => t.categories.Length > 0)
                        .ToArray();

                var document = new JsonObject
                {
                    ["capabilities"] = new JsonObject()
                    {
                        ["metadata"] = JsonNode.Parse(capabilities["metadata"].ToJsonString()),
                        ["groups"] = JsonNode.Parse(capabilities["groups"].ToJsonString()),
                        ["testcases"] = JsonNode.Parse(JsonSerializer.Serialize(testCases))
                    }
                };

                return document;
            }
            else
            {
                return Json;
            }
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
            var filtersByTest = new Dictionary<string, HashSet<string>>();

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

                if (ContainsInvalidCharacters(groupName))
                {
                    throw new InvalidOperationException(InvalidCharactersInGroupNameMessage(groupName));
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

                    if (ContainsInvalidCharacters(categoryName))
                    {
                        throw new InvalidOperationException(InvalidCharactersInCategoryNameMessage(groupName, categoryName));
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
                    (string group, string category) = ParseCategoryInfo(identifier.Trim().ToLowerInvariant());

                    if (!testsByCategories.ContainsKey(group) || !testsByCategories[group].ContainsKey(category))
                    {
                        throw new InvalidOperationException(InvalidGroupOrCategoryMessage(testName, group, category));
                    }

                    if (!testsByCategories[group][category].Contains(testName))
                    {
                        testsByCategories[group][category].Add(testName);
                    }

                    if(!filtersByTest.ContainsKey(testName))
                    {
                        filtersByTest.Add(testName, new HashSet<string>(new string[] { identifier }));
                    }
                    else if (!filtersByTest[testName].Contains(identifier))
                    {
                        filtersByTest[testName].Add(identifier);
                    }
                }
            }

            return new CapabilitiesConfigReader(testsByCategories, filtersByTest, json);
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
