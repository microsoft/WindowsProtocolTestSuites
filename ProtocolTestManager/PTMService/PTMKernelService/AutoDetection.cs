// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestManager.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    public class AutoDetection : IAutoDetection
    {
        private ReaderWriterLockSlim stepsLocker = new ReaderWriterLockSlim();
        private ReaderWriterLockSlim prerequisitesLocker = new ReaderWriterLockSlim();
        private ReaderWriterLockSlim detectorLocker = new ReaderWriterLockSlim();

        private StreamWriter logWriter;

        private int stepIndex;

        private List<DetectingItem> detectSteps;

        private ITestSuite TestSuite { get; set; }

        private CancellationTokenSource cts = null;

        private IValueDetector valueDetector = null;

        private PrerequisiteView prerequisiteView = null;

        private Task detectTask = null;

        private bool taskCanceled = false;

        private DetectionOutcome detectionResult = new DetectionOutcome(DetectionStatus.NotStart, null);

        private string detectorAssembly = string.Empty;

        private string detectorInstanceTypeName = string.Empty;

        /// <summary>
        /// Delegate of logging.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="style"></param>
        public delegate void DetectLog(string message, LogStyle style);

        /// <summary>
        /// Instance of DetectLog.
        /// </summary>
        public DetectLog DetectLogCallback;

        private AutoDetection(ITestSuite testSuite)
        {
            TestSuite = testSuite;

            InitializeDetector(TestSuite.Id);

            detectSteps = ValueDetector.GetDetectionSteps();

            Prerequisites p = GetPrerequisitsInValueDetectorAssembly();
            prerequisiteView = new PrerequisiteView()
            {
                Summary = p.Summary,
                Title = p.Title,
                Properties = new List<Property>()
            };
            foreach (var i in p.Properties)
            {
                prerequisiteView.Properties.Add(new Property()
                {
                    Name = i.Key,
                    Value = ((i.Value != null) && (i.Value.Count == 1)) ? i.Value[0] : null,
                    Choices = i.Value
                });
            }
        }

        public static AutoDetection Create(ITestSuite testSuite)
        {
            var instance = new AutoDetection(testSuite);

            return instance;
        }

        protected IValueDetector ValueDetector
        {
            get
            {
                detectorLocker.EnterUpgradeableReadLock();
                try
                {
                    if (valueDetector == null)
                    {
                        detectorLocker.EnterWriteLock();
                        try
                        {
                            if(valueDetector == null)
                            {
                                // Create an instance
                                Assembly assembly = Assembly.LoadFrom(detectorAssembly);
                                valueDetector = assembly.CreateInstance(detectorInstanceTypeName) as IValueDetector;
                            }
                        }
                        finally
                        {
                            detectorLocker.ExitWriteLock();
                        }
                    }
                }
                finally
                {
                    detectorLocker.ExitUpgradeableReadLock();
                }

                return valueDetector;
            }
        }

        /// <summary>
        /// Loads the auto-detect plug-in from assembly file.
        /// </summary>
        /// <param name="detectorAssembly">File name</param>
        public void Load(string detectorAssembly)
        {
            // Get CustomerInterface
            Type interfaceType = typeof(IValueDetector);

            Assembly assembly = Assembly.LoadFrom(detectorAssembly);

            Type[] types = assembly.GetTypes();

            // Find a class that implement Customer Interface
            foreach (Type type in types)
            {
                if (type.IsClass && interfaceType.IsAssignableFrom(type) == true)
                {
                    detectorInstanceTypeName = type.FullName;
                    break;
                }
            }
        }

        public void InitializeDetector(int testSuiteId)
        {
            var ptfconfig = LoadPtfconfig();

            UtilCallBackFunctions.GetPropertyValue = (string name) =>
            {
                var property = ptfconfig.GetPropertyNodeByName(name);
                if (property != null) return property.Value;
                return null;
            };

            UtilCallBackFunctions.GetPropertiesByFile = (filename) =>
            {
                if (!ptfconfig.FileProperties.ContainsKey(filename))
                    return null;
                return ptfconfig.FileProperties[filename];
            };

            detectorAssembly = TestSuite.GetDetectorAssembly();

            Load(detectorAssembly);
        }

        /// <summary>
        /// Gets the properties required for auto-detection.
        /// </summary>
        /// <returns>Prerequisites object.</returns>
        public PrerequisiteView GetPrerequisites()
        {
            prerequisitesLocker.EnterReadLock();
            try
            {
                return prerequisiteView;
            }
            finally
            {
                prerequisitesLocker.ExitReadLock();
            }
        }

        /// <summary>
        /// Sets the property values required for auto-detection.
        /// </summary>
        /// <returns>Returns true if succeeded, otherwise false.</returns>
        public bool SetPrerequisits(List<Property> prerequisitProperties)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            foreach (var p in prerequisitProperties)
            {
                properties.Add(p.Name, p.Value);
            };

            prerequisitesLocker.EnterWriteLock();
            try
            {
                prerequisiteView.Properties = prerequisitProperties;
            }
            finally
            {
                prerequisitesLocker.ExitWriteLock();
            }

            return SetPrerequisitesInValueDetectorAssembly(properties);
        }

        /// <summary>
        /// Gets a list of the detection steps.
        /// </summary>
        /// <returns>A list of the detection steps.</returns>
        public List<DetectingItem> GetDetectedSteps()
        {
            stepsLocker.EnterReadLock();
            try
            {
                return detectSteps;
            }
            finally
            {
                stepsLocker.ExitReadLock();
            }
        }

        public DetectionOutcome GetDetectionOutcome()
        {
            return detectionResult;
        }

        /// <summary>
        /// Reset AutoDetection settings
        /// </summary>
        public void Reset()
        {
            stepIndex = 0;
            if (detectTask != null)
            {
                StopAndCancelDetection(null);
            }

            if (valueDetector != null)
            {
                valueDetector.Dispose();
                valueDetector = null;
            }

            if (logWriter != null)
            {
                logWriter.Dispose();
            }
            string detectorLog = Path.Combine(TestSuite.StorageRoot.AbsolutePath, "Detector_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + ".log");
            logWriter = new StreamWriter(detectorLog);

            if (cts != null)
            {
                cts.Dispose();
            }
            cts = new CancellationTokenSource();

            UtilCallBackFunctions.WriteLog = (message, newline, style) =>
            {
                if (DetectLogCallback != null) DetectLogCallback(message, style);
            };
            detectionResult = new DetectionOutcome(DetectionStatus.NotStart, null);

            stepsLocker.EnterWriteLock();
            try
            {
                detectSteps.Clear();
                detectSteps = ValueDetector.GetDetectionSteps();
            }
            finally
            {
                stepsLocker.ExitWriteLock();
            }
        }

        /// <summary>
        /// Begins the auto-detection.
        /// </summary>
        /// <param name="DetectionEvent">Callback function when the detection finished.</param>
        public void StartDetection(DetectionCallback callback)
        {
            DetectLogCallback = (msg, style) =>
            {
                stepsLocker.EnterWriteLock();
                try
                {
                    if (stepIndex == detectSteps.Count) return;
                    var item = detectSteps[stepIndex];
                    item.Style = style;
                    switch (style)
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
                }
                finally
                {
                    stepsLocker.ExitWriteLock();
                }

                logWriter.WriteLine("[{0}] {1}", DateTime.Now.ToString(), msg);
                logWriter.Flush();
            };
            BeginDetection((outcome) =>
            {
                stepsLocker.EnterReadLock();
                try
                {
                    if (stepIndex < detectSteps.Count) detectSteps[stepIndex].DetectingStatus = DetectingStatus.Pending;
                }
                finally
                {
                    stepsLocker.ExitReadLock();
                }

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
            DetectLogCallback = null;

            stepsLocker.EnterWriteLock();
            try
            {
                detectSteps[stepIndex].DetectingStatus = DetectingStatus.Canceling;
            }
            finally
            {
                stepsLocker.ExitWriteLock();
            }

            StopAndCancelDetection(callback);

            stepsLocker.EnterReadLock();
            try
            {
                if (stepIndex < detectSteps.Count) detectSteps[stepIndex].DetectingStatus = DetectingStatus.Pending;
            }
            finally
            {
                stepsLocker.ExitReadLock();
            }

            if (logWriter != null)
            {
                logWriter.Close();
                logWriter.Dispose();
                logWriter = null;
            }
            stepIndex = 0;
            detectionResult.Status = DetectionStatus.Finished;
        }

        /// <summary>
        /// Gets an object represents the detection summary.
        /// </summary>
        /// <returns>An object</returns>
        public object GetDetectionSummary()
        {
            return GetDetectionSummaryInValueDetectorAssembly();
        }

        public void ApplyDetectionResult()
        {
            ApplyDetectedRules();
            ApplyDetectedValues();
        }

        /// <summary>
        /// Apply the test case selection rules detected by the plug-in.
        /// </summary>
        private void ApplyDetectedRules()
        {
            var ruleGroups = TestSuite.LoadTestCaseFilter();
            foreach (var rule in ValueDetector.GetSelectedRules())
            {
                
            }
        }

        private void ApplyDetectedValues()
        {
            var detectedValues = GetDetectedPropertyInValueDetectorAssembly();
        }

        private PtfConfig LoadPtfconfig()
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

        /// <summary>
        /// Gets the properties required for auto-detection.
        /// </summary>
        /// <returns>Prerequisites object.</returns>
        private Prerequisites GetPrerequisitsInValueDetectorAssembly()
        {
            return ValueDetector.GetPrerequisites();
        }

        private DetectionOutcome RunDetectionInValueDetectorAssembly()
        {
            detectionResult.Status = DetectionStatus.InProgress;
            try
            {
                detectionResult.Status = ValueDetector.RunDetection() ? DetectionStatus.Finished : DetectionStatus.Error;
            }
            catch (Exception exception)
            {
                detectionResult.Status = DetectionStatus.Error;
                detectionResult.Exception = exception;
                return new DetectionOutcome(DetectionStatus.Error, exception);
            }
            return new DetectionOutcome(DetectionStatus.Finished, null);
        }

        /// <summary>
        /// Begins the auto-detection.
        /// </summary>
        /// <param name="DetectionEvent">Callback function when the detection finished.</param>
        private void BeginDetection(DetectionCallback DetectionEvent)
        {
            if (detectTask != null)
            {
                return;
            }

            var token = cts.Token;

            token.Register(() =>
            {
                taskCanceled = true;
            });

            detectTask = Task.Factory.StartNew(() =>
            {
                token.ThrowIfCancellationRequested();

                var outcome = RunDetectionInValueDetectorAssembly();

                if (DetectionEvent != null)
                {
                    DetectionEvent(outcome);
                }
            }, token);
        }

        /// <summary>
        /// Stop the auto-detection
        /// </summary>
        private void StopAndCancelDetection(Action callback)
        {
            if (detectTask != null)
            {
                cts.Cancel();
                while (!taskCanceled)
                {
                    Thread.SpinWait(100);
                }

                if (callback != null)
                {
                    callback();
                }

                detectTask = null;
            }

            taskCanceled = false;
        }

        /// <summary>
        /// Sets the values of the properties required for auto-detection.
        /// </summary>
        /// <param name="properties">Name - value map.</param>
        /// <returns>Returns true if provided values are enough, otherwise returns false.</returns>
        private bool SetPrerequisitesInValueDetectorAssembly(Dictionary<string, string> properties)
        {
            return ValueDetector.SetPrerequisiteProperties(properties);
        }

        /// <summary>
        /// Gets an object represents the detection summary.
        /// </summary>
        /// <returns>An object</returns>
        private object GetDetectionSummaryInValueDetectorAssembly()
        {
            return ValueDetector.GetSUTSummary();
        }

        /// <summary>
        /// Gets the case selection rules suggested by the detector.
        /// </summary>
        /// <returns>A list of the rules</returns>
        private List<CaseSelectRule> GetRulesInValueDetectorAssembly()
        {
            return ValueDetector.GetSelectedRules();
        }

        /// <summary>
        /// Gets a list of properties to hide.
        /// </summary>
        /// <param name="rules">Test case selection rules</param>
        /// <returns>A list of properties to hide.</returns>
        private List<string> GetHiddenPropertiesInValueDetectorAssembly(List<CaseSelectRule> rules)
        {
            return ValueDetector.GetHiddenProperties(rules);
        }

        /// <summary>
        /// Gets the property values detected by the detector.
        /// </summary>
        /// <returns>Name - value / values map.</returns>
        private Dictionary<string, List<string>> GetDetectedPropertyInValueDetectorAssembly()
        {
            Dictionary<string, List<string>> dict;
            ValueDetector.GetDetectedProperty(out dict);
            return dict;
        }
    }
}