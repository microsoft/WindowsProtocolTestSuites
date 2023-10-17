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
using Microsoft.Protocols.TestManager.Common;

namespace Microsoft.Protocols.TestManager.PTMService.UnitTest.Kernel
{
    [TestClass]
    public class TestCaseInfoExtensionsTests
    {
        [TestMethod]
        public void ClassName_Should_Return_Fully_Qualified_ClassName_Without_MethodName()
        {
            // Arrange
            var testCaseInfo = new TestCaseInfo()
            {
                Name = "BVT_ValidateNegotiateInfo",
                FullName = "Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite.ValidateNegotiateInfo.BVT_ValidateNegotiateInfo",
            };

            // Act
            var className = testCaseInfo.GetClassName();

            // Assert
            Assert.AreEqual("Microsoft.Protocols.TestSuites.FileSharing.SMB2.TestSuite.ValidateNegotiateInfo", className);
        }

        [TestMethod]
        public void ClassName_Should_Return_Same_String_If_No_Namespace()
        {
            // Arrange
            var testCaseInfo = new TestCaseInfo()
            {
                Name = "BVT_ValidateNegotiateInfo",
                FullName = "BVT_ValidateNegotiateInfo",
            };

            // Act
            var className = testCaseInfo.GetClassName();

            // Assert
            Assert.AreEqual("BVT_ValidateNegotiateInfo", className);
        }

        [TestMethod]
        public void ClassName_Should_Return_Empty_String_If_Null_Input()
        {
            // Arrange
            TestCaseInfo testCaseInfo = null;

            // Act
            var className = testCaseInfo.GetClassName();

            // Assert
            Assert.AreEqual(string.Empty, className);
        }
    }
}