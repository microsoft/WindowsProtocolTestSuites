// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Kernel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.PTMService.UnitTest.Kernel
{
    [TestClass]
    public class TestCasesTests
    {
        private CapabilitiesConfigReader capabilitiesConfigReader;

        [TestInitialize]
        public void TestInitialize()
        {
            var json = JsonNode.Parse(@"
            {
                ""capabilities"": {
                    ""metadata"": {
                        ""testsuite"": ""Test Suite 1"",
                        ""version"": ""1.0""
                    },
                    ""groups"": [
                        {
                            ""name"": ""Group 1"",
                            ""categories"": [
                                {
                                    ""name"": ""Category 1""
                                },
                                {
                                    ""name"": ""Category 2""
                                }
                            ]
                        },
                        {
                            ""name"": ""Group 2"",
                            ""categories"": [
                                {
                                    ""name"": ""Category 3""
                                }
                            ]
                        }
                    ],
                    ""testcases"": [
                        {
                            ""name"": ""Test Case 1"",
                            ""categories"": [
                                ""Group 1.Category 1"",
                                ""Group 2.Category 3""
                            ]
                        },
                        {
                            ""name"": ""Test Case 2"",
                            ""categories"": [
                                ""Group 1.Category 2""
                            ]
                        },
                        {
                            ""name"": ""Test Case 3"",
                            ""categories"": [
                                ""Group 1.Category 1""
                            ]
                        }
                    ]
                }
            }");

            capabilitiesConfigReader = CapabilitiesConfigReader.Parse(json);
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public void GetTestCases_ValidGroupAndCategoryIsCaseInsensitive_ReturnsTestCases()
        {
            // Arrange
            string group = "Group 1";
            string category = "Category 1";
            string group2 = "group 1";
            string category2 = "category 1";

            // Act
            string[] testCases = capabilitiesConfigReader.GetTestCases(group, category);
            string[] testCases2 = capabilitiesConfigReader.GetTestCases(group2, category2);

            // Assert
            Assert.AreEqual(2, testCases.Length);
            Assert.IsTrue(testCases.Contains("Test Case 1"));
            Assert.IsTrue(testCases.Contains("Test Case 3"));

            Assert.AreEqual(2, testCases2.Length);
            Assert.IsTrue(testCases2.Contains("Test Case 1"));
            Assert.IsTrue(testCases2.Contains("Test Case 3"));
        }

        [TestMethod]
        public void GetTestCases_ValidGroupIsCaseInsensitive_ReturnsAllTestCasesInGroup()
        {
            // Arrange
            string group = "Group 1";
            string group2 = "group 1";

            // Act
            string[] testCases = capabilitiesConfigReader.GetTestCases(group);
            string[] testCases2 = capabilitiesConfigReader.GetTestCases(group2);

            // Assert
            Assert.AreEqual(3, testCases.Length);
            Assert.IsTrue(testCases.Contains("Test Case 1"));
            Assert.IsTrue(testCases.Contains("Test Case 2"));
            Assert.IsTrue(testCases.Contains("Test Case 3"));

            Assert.AreEqual(3, testCases2.Length);
            Assert.IsTrue(testCases2.Contains("Test Case 1"));
            Assert.IsTrue(testCases2.Contains("Test Case 2"));
            Assert.IsTrue(testCases2.Contains("Test Case 3"));
        }

        [TestMethod]
        public void GetTestCases_ValidGroupAndCategoryFiltersIsCaseInsensitive_ReturnsTestCases()
        {
            // Arrange
            string[] filter = new string[] { "Group 1.Category 1" };
            string[] filter2 = new string[] { "group 1.category 1" };

            // Act
            string[] testCases = capabilitiesConfigReader.GetTestCases(filter);
            string[] testCases2 = capabilitiesConfigReader.GetTestCases(filter2);

            // Assert
            Assert.AreEqual(2, testCases.Length);
            Assert.IsTrue(testCases.Contains("Test Case 1"));
            Assert.IsTrue(testCases.Contains("Test Case 3"));

            Assert.AreEqual(2, testCases2.Length);
            Assert.IsTrue(testCases2.Contains("Test Case 1"));
            Assert.IsTrue(testCases2.Contains("Test Case 3"));
        }

        public void GetTestCases_ValidGroupFiltersIsCaseInsensitive_ReturnsAllTestCasesInGroup()
        {
            // Arrange
            string[] filter = new string[] { "Group 1" };
            string[] filter2 = new string[] { "group 1" };

            // Act
            string[] testCases = capabilitiesConfigReader.GetTestCases(filter);
            string[] testCases2 = capabilitiesConfigReader.GetTestCases(filter2);

            // Assert
            Assert.AreEqual(3, testCases.Length);
            Assert.IsTrue(testCases.Contains("Test Case 1"));
            Assert.IsTrue(testCases.Contains("Test Case 2"));
            Assert.IsTrue(testCases.Contains("Test Case 3"));

            Assert.AreEqual(3, testCases2.Length);
            Assert.IsTrue(testCases2.Contains("Test Case 1"));
            Assert.IsTrue(testCases2.Contains("Test Case 2"));
            Assert.IsTrue(testCases2.Contains("Test Case 3"));
        }

        [TestMethod]
        public void GetTestCases_EmptyGroup_ThrowsInvalidOperationException()
        {
            // Arrange
            string group = string.Empty;
            string category = "Category 1";

            // Act and Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                            capabilitiesConfigReader.GetTestCases(group, category));
            Assert.AreEqual(CapabilitiesConfigReader.EmptyTestCaseFilterGroupOrCategoryNameMessage.ToLowerInvariant(), ex.Message.ToLowerInvariant());
        }

        [TestMethod]
        public void GetTestCases_EmptyCategory_ThrowsInvalidOperationException()
        {
            // Arrange
            string group = "Group 1";
            string category = string.Empty;

            // Act and Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                            capabilitiesConfigReader.GetTestCases(group, category));
            Assert.AreEqual(CapabilitiesConfigReader.EmptyTestCaseFilterGroupOrCategoryNameMessage.ToLowerInvariant(), ex.Message.ToLowerInvariant());
        }

        [TestMethod]
        public void GetTestCases_NonExistentGroup_ThrowsInvalidOperationException()
        {
            // Arrange
            string group = "Group Non-Existent";
            string category = "Category 1";

            // Act and Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                            capabilitiesConfigReader.GetTestCases(group, category));
            Assert.AreEqual(CapabilitiesConfigReader.NonExistentGroupNameMessage(group).ToLowerInvariant(), ex.Message.ToLowerInvariant());
        }

        [TestMethod]
        public void GetTestCases_NonExistentCategory_ThrowsInvalidOperationException()
        {
            // Arrange
            string group = "Group 1";
            string category = "Category Non-Existent";

            // Act and Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                            capabilitiesConfigReader.GetTestCases(group, category));
            Assert.AreEqual(CapabilitiesConfigReader.NonExistentCategoryNameMessage(group, category).ToLowerInvariant(), ex.Message.ToLowerInvariant());
        }

        [TestMethod]
        public void GetTestCases_GetCategories_ReturnsValidCategories()
        {
            // Arrange
            string testCase = "Test Case 1";

            //Act
            var categories =
                capabilitiesConfigReader.GetCategoriesFor(testCase);

            //Assert
            Assert.AreEqual(categories.Count(), 2);
            Assert.AreEqual(categories.ElementAt(0), "Group 1.Category 1");
            Assert.AreEqual(categories.ElementAt(1), "Group 2.Category 3");
        }
    }
}
