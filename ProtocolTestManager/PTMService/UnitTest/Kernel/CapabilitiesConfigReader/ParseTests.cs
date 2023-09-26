// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Protocols.TestManager.PTMService.Abstractions;
using Microsoft.Protocols.TestManager.PTMService.PTMKernelService;
using Microsoft.Protocols.TestManager.PTMService.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestManager.Kernel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.PTMService.UnitTest.Kernel
{
    [TestClass]
    public class ParseTests
    {
        private JsonNode validJson;

        [TestInitialize]
        public void TestInitialize()
        {
            validJson = JsonNode.Parse(@"
            {
                ""capabilities"": {
                    ""metadata"": {
                        ""testsuite"": ""Test Suite 1"",
                        ""version"": ""1.0""
                    },
                    ""groups"": [
                        {
                            ""name"": ""Group 1-_"",
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
                                ""Group 1-_ .Category 1"",
                                ""Group 2.Category 3""
                            ]
                        },
                        {
                            ""name"": ""Test Case 2"",
                            ""categories"": [
                                ""Group 1-_ .Category 2""
                            ]
                        },
                        {
                            ""name"": ""Test Case 3"",
                            ""categories"": []
                        }
                    ]
                }
            }");
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        [TestMethod]
        public void Parse_WithValidInput_ReturnsCapabilitiesConfig()
        {
            // Arrange
            var json = validJson;

            // Act
            var capabilitiesConfig = CapabilitiesConfigReader.Parse(json);

            // Assert
            Assert.IsNotNull(capabilitiesConfig);
        }

        [TestMethod]
        public void Parse_WithMissingTestSuite_ThrowsInvalidOperationException()
        {
            // Arrange
            var json = JsonNode.Parse(@"
            {
                ""capabilities"": {
                    ""metadata"": {
                        ""version"": ""1.0""
                    },
                    ""groups"": [],
                    ""testcases"": []
                }
            }");

            // Act and Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() => 
                            CapabilitiesConfigReader.Parse(json));
            Assert.AreEqual(CapabilitiesConfigReader.MissingTestSuiteOrVersionMessage.ToLowerInvariant(), ex.Message.ToLowerInvariant());
        }

        [TestMethod]
        public void Parse_WithMissingVersion_ThrowsInvalidOperationException()
        {
            // Arrange
            var json = JsonNode.Parse(@"
            {
                ""capabilities"": {
                    ""metadata"": {
                        ""testsuite"": ""Test Suite 1""
                    },
                    ""groups"": [],
                    ""testcases"": []
                }
            }");

            // Act and Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() => 
                            CapabilitiesConfigReader.Parse(json));
            Assert.AreEqual(CapabilitiesConfigReader.MissingTestSuiteOrVersionMessage.ToLowerInvariant().ToLowerInvariant(), ex.Message.ToLowerInvariant());
        }

        [TestMethod]
        public void Parse_WithEmptyGroupName_ThrowsInvalidOperationException()
        {
            var groupName = string.Empty;

            // Arrange
            var json = JsonNode.Parse($@"
            {{
              ""capabilities"": {{
                ""metadata"": {{
                  ""testsuite"": ""Test Suite 1"",
                  ""version"": ""1.0""
                }},
                ""groups"": [
                  {{
                    ""name"": ""{groupName}"",
                    ""categories"": [
                      {{
                        ""name"": ""Category 1""
                      }},
                      {{
                        ""name"": ""Category 2""
                      }}
                    ]
                  }}
                ],
                ""testcases"": []
              }}
            }}");

            // Act and Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() => 
                                CapabilitiesConfigReader.Parse(json));
            Assert.AreEqual(CapabilitiesConfigReader.EmptyOrDuplicateGroupNameMessage(groupName).ToLowerInvariant(), ex.Message.ToLowerInvariant());
        }

        [TestMethod]
        public void Parse_WithInvalidGroupName_ThrowsInvalidOperationException()
        {
            var groupName = "Group**'']";

            // Arrange
            var json = JsonNode.Parse($@"
            {{
              ""capabilities"": {{
                ""metadata"": {{
                  ""testsuite"": ""Test Suite 1"",
                  ""version"": ""1.0""
                }},
                ""groups"": [
                  {{
                    ""name"": ""{groupName}"",
                    ""categories"": [
                      {{
                        ""name"": ""Category 1""
                      }},
                      {{
                        ""name"": ""Category 2""
                      }}
                    ]
                  }}
                ],
                ""testcases"": []
              }}
            }}");

            // Act and Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                                CapabilitiesConfigReader.Parse(json));
            Assert.AreEqual(CapabilitiesConfigReader.InvalidCharactersInGroupNameMessage(groupName).ToLowerInvariant(), ex.Message.ToLowerInvariant());
        }

        [TestMethod]
        public void Parse_WithDuplicateGroupName_ThrowsInvalidOperationException()
        {
            var groupName = "Group 1";

            // Arrange
            var json = JsonNode.Parse($@"
            {{
              ""capabilities"": {{
                ""metadata"": {{
                  ""testsuite"": ""Test Suite 1"",
                  ""version"": ""1.0""
                }},
                ""groups"": [
                  {{
                    ""name"": ""{groupName}"",
                    ""categories"": [
                      {{
                        ""name"": ""Category 1""
                      }},
                      {{
                        ""name"": ""Category 2""
                      }}
                    ]
                  }},
                  {{
                    ""name"": ""{groupName}"",
                    ""categories"": [
                      {{
                        ""name"": ""Category 3""
                      }}
                    ]
                  }}
                ],
                ""testcases"": []
              }}
            }}");

            // Act and Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                              CapabilitiesConfigReader.Parse(json));
            Assert.AreEqual(CapabilitiesConfigReader.EmptyOrDuplicateGroupNameMessage(groupName).ToLowerInvariant(), ex.Message.ToLowerInvariant());
        }

        [TestMethod]
        public void Parse_WithEmptyCategoryName_ThrowsInvalidOperationException()
        {
            var groupName = "Group 1";
            var categoryName = string.Empty;

            // Arrange
            var json = JsonNode.Parse($@"
            {{
              ""capabilities"": {{
                ""metadata"": {{
                  ""testsuite"": ""Test Suite 1"",
                  ""version"": ""1.0""
                }},
                ""groups"": [
                  {{
                    ""name"": ""{groupName}"",
                    ""categories"": [
                      {{
                        ""name"": ""{categoryName}""
                      }},
                      {{
                        ""name"": ""Category 2""
                      }}
                    ]
                  }}
                ],
                ""testcases"": []
              }}
            }}");

            // Act and Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() => 
                                    CapabilitiesConfigReader.Parse(json));
            Assert.AreEqual(CapabilitiesConfigReader.EmptyOrDuplicateCategoryNameMessage(groupName, categoryName).ToLowerInvariant(), ex.Message.ToLowerInvariant());
        }

        [TestMethod]
        public void Parse_WithInvalidCategoryName_ThrowsInvalidOperationException()
        {
            var groupName = "Group 1";
            var categoryName = "''**!Hello";

            // Arrange
            var json = JsonNode.Parse($@"
            {{
              ""capabilities"": {{
                ""metadata"": {{
                  ""testsuite"": ""Test Suite 1"",
                  ""version"": ""1.0""
                }},
                ""groups"": [
                  {{
                    ""name"": ""{groupName}"",
                    ""categories"": [
                      {{
                        ""name"": ""{categoryName}""
                      }},
                      {{
                        ""name"": ""Category 2""
                      }}
                    ]
                  }}
                ],
                ""testcases"": []
              }}
            }}");

            // Act and Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                                    CapabilitiesConfigReader.Parse(json));
            Assert.AreEqual(CapabilitiesConfigReader.InvalidCharactersInCategoryNameMessage(groupName, categoryName).ToLowerInvariant(), ex.Message.ToLowerInvariant());
        }

        [TestMethod]
        public void Parse_WithDuplicateCategoryNameWithinAGroup_ThrowsInvalidOperationException()
        {
            var groupName = "Group 1";
            var categoryName = "Category 2";

            // Arrange
            var json = JsonNode.Parse($@"
            {{
              ""capabilities"": {{
                ""metadata"": {{
                  ""testsuite"": ""Test Suite 1"",
                  ""version"": ""1.0""
                }},
                ""groups"": [
                  {{
                    ""name"": ""{groupName}"",
                    ""categories"": [
                      {{
                        ""name"": ""{categoryName}""
                      }},
                      {{
                        ""name"": ""{categoryName}""
                      }}
                    ]
                  }}
                ],
                ""testcases"": []
              }}
            }}");

            // Act and Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() => 
                                CapabilitiesConfigReader.Parse(json));
            Assert.AreEqual(CapabilitiesConfigReader.EmptyOrDuplicateCategoryNameMessage(groupName, categoryName).ToLowerInvariant(), ex.Message.ToLowerInvariant());
        }

        [TestMethod]
        public void Parse_WithEmptyTestCaseName_ThrowsInvalidOperationException()
        {
            var testCaseName = string.Empty;

            // Arrange
            var json = JsonNode.Parse($@"
            {{
              ""capabilities"": {{
                ""metadata"": {{
                  ""testsuite"": ""Test Suite 1"",
                  ""version"": ""1.0""
                }},
                ""groups"": [
                  {{
                    ""name"": ""Group 1"",
                    ""categories"": [
                      {{
                        ""name"": ""Category 1""
                      }}
                    ]
                  }}
                ],
                ""testcases"": [
                  {{
                    ""name"": ""{testCaseName}"",
                    ""categories"": [
                      ""Group 1.Category 1""
                    ]
                  }}
                ]
              }}
            }}");

            // Act and Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() => 
                                    CapabilitiesConfigReader.Parse(json));
            Assert.AreEqual(CapabilitiesConfigReader.EmptyTestCaseNameMessage.ToLowerInvariant(), ex.Message.ToLowerInvariant());
        }

        [TestMethod]
        public void Parse_WithInvalidTestCaseGroupOrCategoryName_ThrowsInvalidOperationException()
        {
            var testName = "Test Case 1";
            var groupName = "Group X";
            var categoryName = "Category Y";

            // Arrange
            var json = JsonNode.Parse($@"
            {{
              ""capabilities"": {{
                ""metadata"": {{
                  ""testsuite"": ""Test Suite 1"",
                  ""version"": ""1.0""
                }},
                ""groups"": [
                  {{
                    ""name"": ""Group 1"",
                    ""categories"": [
                      {{
                        ""name"": ""Category 1""
                      }}
                    ]
                  }}
                ],
                ""testcases"": [
                  {{
                    ""name"": ""Test Case 1"",
                    ""categories"": [
                      ""{groupName}.{categoryName}""
                    ]
                  }}
                ]
              }}
            }}");

            // Act and Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                                    CapabilitiesConfigReader.Parse(json));
            Assert.AreEqual(CapabilitiesConfigReader.InvalidGroupOrCategoryMessage(testName, groupName, categoryName).ToLowerInvariant(), ex.Message.ToLowerInvariant());
        }

        [TestMethod]
        public void Parse_WithInvalidJson_ThrowsInvalidOperationException()
        {
            // Arrange
            var json = $@"invalid-json";

            // Act and Assert
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                                    CapabilitiesConfigReader.Parse(json));
            Assert.AreEqual(CapabilitiesConfigReader.InvalidJsonMessage.ToLowerInvariant(), ex.Message.ToLowerInvariant());
        }

        [TestMethod]
        public void ExpandIdentifier_WithValidGroup_ReturnsAllCategoriesInTheGroup()
        {
            // Arrange
            var json = validJson;

            // Act
            var capabilitiesConfig = CapabilitiesConfigReader.Parse(json);
            var categories = capabilitiesConfig.ExpandIdentifier("Group 1-_").ToArray();

            //Assert
            Assert.AreEqual(categories.Count(), 2);
            Assert.AreEqual(categories[0].group.ToLowerInvariant(), "Group 1-_".ToLowerInvariant());
            Assert.AreEqual(categories[0].category.ToLowerInvariant(), "Category 1".ToLowerInvariant());
            Assert.AreEqual(categories[1].group.ToLowerInvariant(), "Group 1-_".ToLowerInvariant());
            Assert.AreEqual(categories[1].category.ToLowerInvariant(), "Category 2".ToLowerInvariant());
        }

        [TestMethod]
        public void ExpandIdentifier_WithValidGroupAndCategory_ReturnsTheSpecificCategorySpecified()
        {
            // Arrange
            var json = validJson;

            // Act
            var capabilitiesConfig = CapabilitiesConfigReader.Parse(json);
            var categories = capabilitiesConfig.ExpandIdentifier("Group 1-_.Category 1").ToArray();

            //Assert
            Assert.AreEqual(categories.Count(), 1);
            Assert.AreEqual(categories[0].group.ToLowerInvariant(), "Group 1-_".ToLowerInvariant());
            Assert.AreEqual(categories.ElementAt(0).category.ToLowerInvariant(), "Category 1".ToLowerInvariant());
        }

        [TestMethod]
        public void ExpandIdentifier_WithInvalidGroup_ThrowsInvalidOperationException()
        {
            // Arrange
            var json = validJson;

            // Act and Assert
            var capabilitiesConfig = CapabilitiesConfigReader.Parse(json);
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                                    capabilitiesConfig.ExpandIdentifier("GroupX.Category 1").ToArray());
            Assert.AreEqual(CapabilitiesConfigReader.UnknownGroupMessage("GroupX").ToLowerInvariant(), ex.Message.ToLowerInvariant());
        }

        [TestMethod]
        public void ExpandIdentifier_WithInvalidCategory_ThrowsInvalidOperationException()
        {
            // Arrange
            var json = validJson;

            // Act and Assert
            var capabilitiesConfig = CapabilitiesConfigReader.Parse(json);
            var ex = Assert.ThrowsException<InvalidOperationException>(() =>
                                    capabilitiesConfig.ExpandIdentifier("Group 1-_.CategoryX").ToArray());
            Assert.AreEqual(CapabilitiesConfigReader.UnknownCategoryMessage("CategoryX").ToLowerInvariant(), ex.Message.ToLowerInvariant());
        }

        [TestMethod]
        public void GetJson_ReturnsFullJson_WhenSkipTestsWithNoCategoryIsFalse()
        {
            // Arrange
            var json = validJson;

            // Act
            var capabilitiesConfig = CapabilitiesConfigReader.Parse(json);

            // Assert
            var testCasesNode =
                capabilitiesConfig.Json["capabilities"]["testcases"] as JsonArray;
            Assert.IsNotNull(capabilitiesConfig);
            Assert.AreEqual(capabilitiesConfig.Json, capabilitiesConfig.GetJson(skipTestsWithNoCategory: false));
            Assert.IsTrue(testCasesNode != null);
            Assert.IsTrue(testCasesNode.Count == 3);
        }

        [TestMethod]
        public void GetJson_ReturnsFilteredJson_WhenSkipTestsWithNoCategoryIsTrue()
        {
            // Arrange
            var json = validJson;

            // Act
            var capabilitiesConfig = CapabilitiesConfigReader.Parse(json);
            var filteredJson = capabilitiesConfig.GetJson(skipTestsWithNoCategory: true);

            // Assert
            var testCasesNode =
                filteredJson["capabilities"]["testcases"] as JsonArray;
            Assert.IsNotNull(capabilitiesConfig);
            Assert.IsTrue(testCasesNode != null);
            Assert.IsTrue(testCasesNode.Count == 2);
        }
    }
}
