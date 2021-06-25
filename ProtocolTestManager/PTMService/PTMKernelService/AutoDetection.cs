// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Protocols.TestManager.Kernel;
using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    public class AutoDetection : IAutoDetection
    {
        private StreamWriter logWriter;

        private int stepIndex;

        private Detector detector = null;

        private List<DetectingItem> detectSteps;

        private ITestSuite TestSuite { get; set; }

        private AutoDetection(ITestSuite testSuite)
        {
            TestSuite = testSuite;

            InitializeDetector(TestSuite.Id);
        }

        public static AutoDetection Create(ITestSuite testSuite)
        {
            var instance = new AutoDetection(testSuite);

            return instance;
        }

        public void InitializeDetector(int testSuiteId)
        {
            var ptfconfig = LoadPtfconfig(testSuiteId);

            Microsoft.Protocols.TestManager.Detector.UtilCallBackFunctions.GetPropertyValue = (string name) =>
            {
                var property = ptfconfig.GetPropertyNodeByName(name);
                if (property != null) return property.Value;
                return null;
            };

            Microsoft.Protocols.TestManager.Detector.UtilCallBackFunctions.GetPropertiesByFile = (filename) =>
            {
                if (!ptfconfig.FileProperties.ContainsKey(filename))
                    return null;
                return ptfconfig.FileProperties[filename];
            };

            string detectorAssembly = TestSuite.GetDetectorAssembly();

            detector = new Detector();

            detector.Load(detectorAssembly);
        }

        /// <summary>
        /// Gets the properties required for auto-detection.
        /// </summary>
        /// <returns>Prerequisites object.</returns>
        public PrerequisiteView GetPrerequisites()
        {
            Prerequisites p = detector.GetPrerequisits();

            var prerequisits = new PrerequisiteView()
            {
                Summary = p.Summary,
                Title = p.Title,
                Properties = new List<Property>()
            };
            foreach (var i in p.Properties)
            {
                prerequisits.Properties.Add(new Property()
                {
                    Name = i.Key,
                    Choices = i.Value
                });
            }

            return prerequisits;
        }

        /// <summary>
        /// Sets the property values required for auto-detection.
        /// </summary>
        /// <returns>Returns true if succeeded, otherwise false.</returns>
        public bool SetPrerequisits(List<PrerequisiteProperty> prerequisitProperties)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            foreach (var p in prerequisitProperties)
            {
                properties.Add(p.PropertyName, p.Value);
            };
            return detector.SetPrerequisites(properties);
        }

        /// <summary>
        /// Gets a list of the detection steps.
        /// </summary>
        /// <returns>A list of the detection steps.</returns>
        public List<DetectingItem> GetDetectedSteps()
        {
            return detector.GetDetectSteps();
        }

        /// <summary>
        /// Begins the auto-detection.
        /// </summary>
        /// <param name="DetectionEvent">Callback function when the detection finished.</param>
        public void StartDetection(DetectionCallback callback)
        {
            stepIndex = 0;
            detectSteps = GetDetectedSteps();
            string detectorLog = Path.Combine(TestSuite.StorageRoot.AbsolutePath, "Detector_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + ".log");
            logWriter = new StreamWriter(detectorLog);
            detector.DetectLogCallback = (msg, style) => 
            {
                if (stepIndex == detectSteps.Count) return;
                var item = detectSteps[stepIndex];
                item.Style = style;
                switch(style)
                {
                    case LogStyle.Default:
                        detectSteps[stepIndex].DetectingStatus = DetectingStatus.Detecting;
                        break;
                    case LogStyle.Error:
                        stepIndex++;
                        item.DetectingStatus = DetectingStatus.Error;
                        break;
                    case LogStyle.StepFailed:
                        stepIndex++;
                        item.DetectingStatus = DetectingStatus.Failed;
                        break;
                    case LogStyle.StepSkipped:
                        stepIndex++;
                        item.DetectingStatus = DetectingStatus.Skipped;
                        break;
                    case LogStyle.StepNotFound:
                        stepIndex++;
                        item.DetectingStatus = DetectingStatus.NotFound;
                        break;
                    case LogStyle.StepPassed:
                        stepIndex++;
                        item.DetectingStatus = DetectingStatus.Finished;
                        break;
                    default:
                        item.DetectingStatus = DetectingStatus.Pending;
                        break;
                }
                logWriter.WriteLine("[{0}] {1}", DateTime.Now.ToString(), msg);
                logWriter.Flush();
            };
            detector.BeginDetection((outcome) => 
            {
                if (stepIndex < detectSteps.Count) detectSteps[stepIndex].DetectingStatus = DetectingStatus.Pending;
                callback(outcome);
                logWriter.Close();
                logWriter = null;
            });
        }

        /// <summary>
        /// Stop the auto-detection
        /// </summary>
        public void StopDetection(Action callback)
        {
            detector.DetectLogCallback = null;

            detectSteps[stepIndex].DetectingStatus = DetectingStatus.Canceling;
            
            detector.StopDetection(callback);
            
            if (stepIndex < detectSteps.Count) detectSteps[stepIndex].DetectingStatus = DetectingStatus.Pending;
            
            if (logWriter != null)
            {
                logWriter.Close();
                logWriter.Dispose();
                logWriter = null;
            }
            stepIndex = 0;
        }

        /// <summary>
        /// Gets an object represents the detection summary.
        /// </summary>
        /// <returns>An object</returns>
        public object GetDetectionSummary()
        {
            return detector.GetDetectionSummary();
        }

        private PtfConfig LoadPtfconfig(int testSuiteId)
        {
            try
            {
                var ptfConfigFiles = TestSuite.GetConfigurationFiles().ToList();
                
                var ptfconfig = new PtfConfig(ptfConfigFiles);

                return ptfconfig;
            }
            catch (Exception e)
            {
                throw new Exception(string.Format(AutoDetectionConsts.LoadPtfconfigError, e.Message));
            }
        }
    }
}