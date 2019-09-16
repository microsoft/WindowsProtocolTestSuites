// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// XUnit format test report
    /// </summary>
    public class XUnitReport : TestReport
    {
        /// <summary>
        /// Constructor of XUnitReport
        /// </summary>
        /// <param name="testCases">Executed test cases</param>
        public XUnitReport(List<TestCase> testCases) : base(testCases)
        {
        }

        /// <summary>
        /// The file extension name for the report
        /// </summary>
        public override string FileExtension
        {
            get
            {
                return "xml";
            }
        }

        /// <summary>
        /// The filter string to use in SaveFileDialog
        /// </summary>
        public override string FileDialogFilter
        {
            get
            {
                return "xUnit report (*.xml)|*.xml";
            }
        }

        /// <summary>
        /// Export report to a file
        /// </summary>
        /// <param name="filename">File name of the exported report</param>
        public override bool ExportReport(string filename)
        {
            if (this.testCases.Count() == 0)
            {
                return false;
            }

            var groupedByAssembly = this.testCases.GroupBy(c => c.Assembly);

            XmlDocument xmlDoc = new XmlDocument();

            XmlDeclaration declaration;
            declaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(declaration);

            XmlNode rootNode = xmlDoc.CreateElement("assemblies");
            xmlDoc.AppendChild(rootNode);

            DateTimeOffset testStartTime = this.testCases.First().StartTime;

            foreach (var assemblyGroup in groupedByAssembly)
            {
                XmlNode assemblyNode = xmlDoc.CreateElement("assembly");

                XmlNode errorsNode = xmlDoc.CreateElement("errors");
                assemblyNode.AppendChild(errorsNode);

                var groupedByClass = assemblyGroup.GroupBy(c => c.ClassName);

                int totalInAssembly = 0;
                int passedInAssembly = 0;
                int failedInAssembly = 0;
                int skippedInAssembly = 0;
                TimeSpan assemblyDuration = new TimeSpan();
                DateTimeOffset assemblyStartTime = assemblyGroup.First().StartTime;

                foreach (var classGroup in groupedByClass)
                {
                    #region <collection> node
                    XmlNode collectionNode = xmlDoc.CreateElement("collection");
                    assemblyNode.AppendChild(collectionNode);

                    int totalInClass = 0;
                    int passedInClass = 0;
                    int failedInClass = 0;
                    int skippedInClass = 0;
                    TimeSpan classDuration = new TimeSpan();

                    foreach (TestCase testcase in classGroup)
                    {
                        #region <test> node
                        XmlNode caseNode = xmlDoc.CreateElement("test");
                        collectionNode.AppendChild(caseNode);

                        TimeSpan caseDuration = testcase.EndTime - testcase.StartTime;

                        // test case full name
                        XmlAttribute caseAttr = xmlDoc.CreateAttribute("name");
                        caseAttr.Value = testcase.FullName;
                        caseNode.Attributes.Append(caseAttr);

                        // test case class name
                        caseAttr = xmlDoc.CreateAttribute("type");
                        caseAttr.Value = testcase.ClassName;
                        caseNode.Attributes.Append(caseAttr);

                        // test case method name
                        caseAttr = xmlDoc.CreateAttribute("method");
                        caseAttr.Value = testcase.Name;
                        caseNode.Attributes.Append(caseAttr);

                        // test case duration
                        caseAttr = xmlDoc.CreateAttribute("time");
                        caseAttr.Value = $"{caseDuration.TotalSeconds:0.000}";
                        caseNode.Attributes.Append(caseAttr);

                        // test case result
                        caseAttr = xmlDoc.CreateAttribute("result");
                        string result = "";
                        if (testcase.Status == TestCaseStatus.Passed) result = "Pass";
                        if (testcase.Status == TestCaseStatus.Failed) result = "Fail";
                        if (testcase.Status == TestCaseStatus.Other) result = "Skip";
                        caseAttr.Value = result;
                        caseNode.Attributes.Append(caseAttr);

                        // standard output of test case
                        if (!String.IsNullOrEmpty(testcase.StdOut))
                        {
                            XmlNode stdoutNode = xmlDoc.CreateElement("output");
                            stdoutNode.InnerText = testcase.StdOut;
                            caseNode.AppendChild(stdoutNode);
                        }

                        // failure detail of test case
                        if (testcase.Status == TestCaseStatus.Failed)
                        {
                            XmlNode failureNode = xmlDoc.CreateElement("failure");
                            caseNode.AppendChild(failureNode);

                            if (!String.IsNullOrEmpty(testcase.ErrorMessage))
                            {
                                XmlNode errMsgNode = xmlDoc.CreateElement("message");
                                errMsgNode.InnerText = testcase.ErrorMessage;
                                failureNode.AppendChild(errMsgNode);
                            }

                            if (!String.IsNullOrEmpty(testcase.ErrorStackTrace))
                            {
                                XmlNode errStackNode = xmlDoc.CreateElement("stack-trace");
                                errStackNode.InnerText = testcase.ErrorStackTrace;
                                failureNode.AppendChild(errStackNode);
                            }
                        }

                        // test case category
                        XmlNode traitsNode = xmlDoc.CreateElement("traits");
                        caseNode.AppendChild(traitsNode);
                        foreach (string category in testcase.Category)
                        {
                            XmlNode traitNode = xmlDoc.CreateElement("trait");

                            XmlAttribute traitAttr = xmlDoc.CreateAttribute("name");
                            traitAttr.Value = "Category";
                            traitNode.Attributes.Append(traitAttr);

                            traitAttr = xmlDoc.CreateAttribute("value");
                            traitAttr.Value = category;
                            traitNode.Attributes.Append(traitAttr);

                            traitsNode.AppendChild(traitNode);
                        }
                        #endregion

                        if (testcase.Status == TestCaseStatus.Passed) passedInClass++;
                        if (testcase.Status == TestCaseStatus.Failed) failedInClass++;
                        if (testcase.Status == TestCaseStatus.Other) skippedInClass++;
                        totalInClass++;
                        classDuration += caseDuration;

                        if (testcase.StartTime < testStartTime) testStartTime = testcase.StartTime;
                        if (testcase.StartTime < assemblyStartTime) assemblyStartTime = testcase.StartTime;
                    }

                    XmlAttribute collectionAttr = xmlDoc.CreateAttribute("name");
                    collectionAttr.Value = $"Test Collection for {classGroup.Key}";
                    collectionNode.Attributes.Append(collectionAttr);

                    collectionAttr = xmlDoc.CreateAttribute("total");
                    collectionAttr.Value = totalInClass.ToString();
                    collectionNode.Attributes.Append(collectionAttr);

                    collectionAttr = xmlDoc.CreateAttribute("passed");
                    collectionAttr.Value = passedInClass.ToString();
                    collectionNode.Attributes.Append(collectionAttr);

                    collectionAttr = xmlDoc.CreateAttribute("failed");
                    collectionAttr.Value = failedInClass.ToString();
                    collectionNode.Attributes.Append(collectionAttr);

                    collectionAttr = xmlDoc.CreateAttribute("skipped");
                    collectionAttr.Value = skippedInClass.ToString();
                    collectionNode.Attributes.Append(collectionAttr);

                    collectionAttr = xmlDoc.CreateAttribute("time");
                    collectionAttr.Value = $"{classDuration.TotalSeconds:0.000}";
                    collectionNode.Attributes.Append(collectionAttr);
                    #endregion

                    totalInAssembly += totalInClass;
                    passedInAssembly += passedInClass;
                    failedInAssembly += failedInClass;
                    skippedInAssembly += skippedInClass;
                    assemblyDuration += classDuration;
                }

                XmlAttribute assemblyAttr = xmlDoc.CreateAttribute("name");
                assemblyAttr.Value = assemblyGroup.Key;
                assemblyNode.Attributes.Append(assemblyAttr);

                assemblyAttr = xmlDoc.CreateAttribute("run-date");
                assemblyAttr.Value = assemblyStartTime.ToLocalTime().ToString("yyyy-MM-dd");
                assemblyNode.Attributes.Append(assemblyAttr);

                assemblyAttr = xmlDoc.CreateAttribute("run-time");
                assemblyAttr.Value = assemblyStartTime.ToLocalTime().ToString("HH:mm:ss");
                assemblyNode.Attributes.Append(assemblyAttr);

                assemblyAttr = xmlDoc.CreateAttribute("total");
                assemblyAttr.Value = totalInAssembly.ToString();
                assemblyNode.Attributes.Append(assemblyAttr);

                assemblyAttr = xmlDoc.CreateAttribute("passed");
                assemblyAttr.Value = passedInAssembly.ToString();
                assemblyNode.Attributes.Append(assemblyAttr);

                assemblyAttr = xmlDoc.CreateAttribute("failed");
                assemblyAttr.Value = failedInAssembly.ToString();
                assemblyNode.Attributes.Append(assemblyAttr);

                assemblyAttr = xmlDoc.CreateAttribute("skipped");
                assemblyAttr.Value = skippedInAssembly.ToString();
                assemblyNode.Attributes.Append(assemblyAttr);

                assemblyAttr = xmlDoc.CreateAttribute("time");
                assemblyAttr.Value = $"{assemblyDuration.TotalSeconds:0.000}";
                assemblyNode.Attributes.Append(assemblyAttr);

                rootNode.AppendChild(assemblyNode);
            }

            XmlAttribute rootAttr = xmlDoc.CreateAttribute("timestamp");
            rootAttr.Value = testStartTime.ToLocalTime().ToString("MM/dd/yyyy HH:mm:ss");
            rootNode.Attributes.Append(rootAttr);

            xmlDoc.Save(filename);
            return true;
        }
    }
}
