// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Protocols.TestManager.Kernel;
using System.Threading;

namespace CodeCoverage
{
    [TestClass]
    public class TestDetector
    {
        [TestMethod]
        public void LoadDetectorAssembly()
        {
            Detector detector = new Detector();
            detector.Load(@"..\..\..\SampleDetector\bin\Debug\SampleDetector.dll");
        }

        [TestMethod]
        public void GetPreRequisit()
        {
            Detector detector = new Detector();
            detector.Load(@"..\..\..\SampleDetector\bin\Debug\SampleDetector.dll");
            Microsoft.Protocols.TestManager.Detector.Prerequisites prerequisits = detector.GetPrerequisits();
            Assert.AreEqual("Summary", prerequisits.Summary,"Verify Summary.");
            Assert.AreEqual(3, prerequisits.Properties.Count, "Verify propertie number.");
        }

        [TestMethod]
        public void TestAsyncDetection()
        {
            Detector detector = new Detector();
            detector.Load(@"..\..\..\SampleDetector\bin\Debug\SampleDetector.dll");
            Microsoft.Protocols.TestManager.Detector.Prerequisites prerequisits = detector.GetPrerequisits();
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            DetectionOutcome outcome = null;
            detector.BeginDetection((o) =>
                {
                    outcome = o;
                    autoEvent.Set();                    
                }
            );
            autoEvent.WaitOne();
            Assert.AreEqual(DetectionStatus.Error, outcome.Status, "Error should occur");
            Assert.IsNotNull(outcome.Exception, "Check exception");
        }
    }
}
