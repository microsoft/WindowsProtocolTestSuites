// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Kernel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.Protocols.TestManager.PTMService.UnitTest.Kernel
{
    [TestClass]
    public class FileServerExecutionPlanBuilderTests
    {
        private const string BinPath = @"C:\FileServer\Bin";

        [TestMethod]
        public void Build_ParallelDisabled_ReturnsSingleSequentialUnit()
        {
            var tests = new[]
            {
                CreateTest("Smb2Test", "MS-SMB2_ServerTestSuite.dll"),
                CreateTest("DfscTest", "MS-DFSC_ServerTestSuite.dll"),
            };

            var plan = new FileServerExecutionPlanBuilder().Build(tests, false);

            Assert.AreEqual(1, plan.Stages.Count);
            Assert.AreEqual(1, plan.Stages[0].Units.Count);
            Assert.AreEqual(1, plan.MaxDegreeOfParallelism);
            CollectionAssert.AreEquivalent(
                tests.Select(test => test.FullName).ToList(),
                plan.Stages[0].Units[0].TestCases.Select(test => test.FullName).ToList());
        }

        [TestMethod]
        public void Build_Smb2ModelTests_CreatesFourBalancedRoundRobinShards()
        {
            var tests = Enumerable.Range(0, 10)
                .Select(index => CreateTest($"ModelTest{index}", "MS-SMB2Model_ServerTestSuite.dll"))
                .ToList();

            var plan = new FileServerExecutionPlanBuilder().Build(tests, true);
            var stage = plan.Stages.Single();

            Assert.AreEqual("SMB2Model shards", stage.Name);
            Assert.AreEqual(4, stage.Units.Count);
            CollectionAssert.AreEqual(
                new[] { 3, 3, 2, 2 },
                stage.Units.Select(unit => unit.TestCases.Count).ToArray());
            CollectionAssert.AreEqual(
                new[] { "ModelTest0", "ModelTest4", "ModelTest8" },
                stage.Units[0].TestCases.Select(test => test.Name).ToArray());
        }

        [TestMethod]
        public void Build_MixedTests_PreservesBaselineStageOrder()
        {
            var tests = new[]
            {
                CreateTest("ModelTest", "MS-SMB2Model_ServerTestSuite.dll"),
                CreateTest("Smb2Test", "MS-SMB2_ServerTestSuite.dll"),
                CreateTest("AppInstanceId_Encryption", "MS-SMB2Model_ServerTestSuite.dll", "AppInstanceId"),
                CreateTest("FsaTest", "MS-FSA_ServerTestSuite.dll"),
                CreateTest("FsaModelTest", "MS-FSAModel_ServerTestSuite.dll"),
                CreateTest("DfscTest", "MS-DFSC_ServerTestSuite.dll"),
                CreateTest("AuthTest", "Auth_ServerTestSuite.dll"),
            };

            var plan = new FileServerExecutionPlanBuilder().Build(tests, true);

            CollectionAssert.AreEqual(
                new[]
                {
                    "SMB2Model shards",
                    "SMB2",
                    "AppInstanceId",
                    "FSA and FSAModel",
                    "Remaining FileServer assemblies",
                },
                plan.Stages.Select(stage => stage.Name).ToArray());
            Assert.AreEqual(2, plan.Stages[3].Units.Count);
            Assert.AreEqual(2, plan.Stages[4].Units.Count);
            Assert.AreEqual(FileServerExecutionPlanBuilder.DefaultMaxDegreeOfParallelism, plan.MaxDegreeOfParallelism);
        }

        [TestMethod]
        public void Build_AppInstanceIdTests_AreNotIncludedInModelShards()
        {
            var modelAppTest = CreateTest(
                "AppInstanceId_Encryption",
                "MS-SMB2Model_ServerTestSuite.dll",
                "AppInstanceId");
            var smb2AppTest = CreateTest(
                "AppInstanceId_DirectoryLeasing_NoLeaseInReOpen",
                "MS-SMB2_ServerTestSuite.dll",
                "AppInstanceId");
            var modelTest = CreateTest("ModelTest", "MS-SMB2Model_ServerTestSuite.dll");
            var smb2Test = CreateTest("Smb2Test", "MS-SMB2_ServerTestSuite.dll");

            var plan = new FileServerExecutionPlanBuilder().Build(
                new[] { modelAppTest, smb2AppTest, modelTest, smb2Test },
                true);

            Assert.AreEqual("SMB2Model shards", plan.Stages[0].Name);
            Assert.AreEqual(modelTest.FullName, plan.Stages[0].Units[0].TestCases.Single().FullName);
            Assert.AreEqual("SMB2", plan.Stages[1].Name);
            Assert.AreEqual(smb2Test.FullName, plan.Stages[1].Units[0].TestCases.Single().FullName);
            Assert.AreEqual("AppInstanceId", plan.Stages[2].Name);
            CollectionAssert.AreEquivalent(
                new[] { modelAppTest.FullName, smb2AppTest.FullName },
                plan.Stages[2].Units.SelectMany(unit => unit.TestCases).Select(test => test.FullName).ToArray());
        }

        [TestMethod]
        public void Build_TestWithoutAssembly_ThrowsArgumentException()
        {
            var test = CreateTest("TestWithoutAssembly", "MS-SMB2_ServerTestSuite.dll");
            test.Assembly = null;

            Assert.ThrowsException<ArgumentException>(
                () => new FileServerExecutionPlanBuilder().Build(new[] { test }, true));
        }

        private static TestCase CreateTest(string name, string assemblyName, params string[] categories)
        {
            return new TestCase
            {
                Name = name,
                FullName = $"FileServer.Tests.{name}",
                Assembly = Path.Combine(BinPath, assemblyName),
                Category = new List<string>(categories),
            };
        }
    }
}
