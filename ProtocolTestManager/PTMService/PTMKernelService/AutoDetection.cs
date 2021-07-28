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
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    public class AutoDetection : IAutoDetection
    {
        private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

        private StreamWriter logWriter;

        private int stepIndex;

        //private Detector detector = null;

        private List<DetectingItem> detectSteps;

        private ITestSuite TestSuite { get; set; }

        private CancellationTokenSource cts = new CancellationTokenSource();

        private IValueDetector valueDetector = null;

        private Task detectTask = null;

        private bool taskCanceled = false;

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
        }

        public static AutoDetection Create(ITestSuite testSuite)
        {
            var instance = new AutoDetection(testSuite);

            return instance;
        }

        public void Reset()
        {
            if (valueDetector != null) valueDetector.Dispose();
            valueDetector = null;
            UtilCallBackFunctions.WriteLog = (message, newline, style) =>
            {
                if (DetectLogCallback != null) DetectLogCallback(message, style);
            };
        }

        /// <summary>
        /// Loads the auto-detect plug-in from assembly file.
        /// </summary>
        /// <param name="detectorAssembly">File name</param>
        public void Load(string detectorAssembly)
        {
            Reset();

            // Get CustomerInterface
            Type interfaceType = typeof(IValueDetector);

            Assembly assembly = Assembly.LoadFrom(detectorAssembly);

            Type[] types = assembly.GetTypes();

            // Find a class that implement Customer Interface
            foreach (Type type in types)
            {
                if (type.IsClass && interfaceType.IsAssignableFrom(type) == true)
                {
                    // Create an instance
                    valueDetector = assembly.CreateInstance(type.FullName) as IValueDetector;
                }
            }
            if (valueDetector == null) throw new Exception(AutoDetectionConsts.LoadingAutoDetectorFailed);
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

            Load(detectorAssembly);
        }

        /// <summary>
        /// Gets the properties required for auto-detection.
        /// </summary>
        /// <returns>Prerequisites object.</returns>
        public PrerequisiteView GetPrerequisites()
        {
            Prerequisites p = GetPrerequisitsInValueDetectorAssembly();

            var prerequisites = new PrerequisiteView()
            {
                Summary = p.Summary,
                Title = p.Title,
                Properties = new List<Property>()
            };
            foreach (var i in p.Properties)
            {
                prerequisites.Properties.Add(new Property()
                {
                    Name = i.Key,
                    Choices = i.Value
                });
            }

            return prerequisites;
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
            return SetPrerequisitesInValueDetectorAssembly(properties);
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
            return SetPrerequisitesInValueDetectorAssembly(properties);
        }

        /// <summary>
        /// Gets a list of the detection steps.
        /// </summary>
        /// <returns>A list of the detection steps.</returns>
        public List<DetectingItem> GetDetectedSteps(bool resetSteps = false)
        {
            cacheLock.EnterWriteLock();
            try
            {
                if ((detectSteps == null) || resetSteps)
                {
                    detectSteps = valueDetector.GetDetectionSteps();
                }
                return detectSteps;
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
        }

        //public 

        /// <summary>
        /// Begins the auto-detection.
        /// </summary>
        /// <param name="DetectionEvent">Callback function when the detection finished.</param>
        public void StartDetection(DetectionCallback callback)
        {
            stepIndex = 0;
            GetDetectedSteps(true);
            string detectorLog = Path.Combine(TestSuite.StorageRoot.AbsolutePath, "Detector_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + ".log");
            logWriter = new StreamWriter(detectorLog);
            DetectLogCallback = (msg, style) =>
            {
                cacheLock.EnterWriteLock();
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
                    cacheLock.ExitWriteLock();
                }
                
                logWriter.WriteLine("[{0}] {1}", DateTime.Now.ToString(), msg);
                logWriter.Flush();
            };
            BeginDetection((outcome) =>
            {
                cacheLock.EnterReadLock();
                try
                {
                    if (stepIndex < detectSteps.Count) detectSteps[stepIndex].DetectingStatus = DetectingStatus.Pending;
                }
                finally
                {
                    cacheLock.ExitReadLock();
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

            cacheLock.EnterWriteLock();
            try
            {
                detectSteps[stepIndex].DetectingStatus = DetectingStatus.Canceling;
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }
            
            StopAndCancelDetection(callback);

            cacheLock.EnterReadLock();
            try
            {
                if (stepIndex < detectSteps.Count) detectSteps[stepIndex].DetectingStatus = DetectingStatus.Pending;
            }
            finally
            {
                cacheLock.ExitReadLock();
            }
            
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
            return GetDetectionSummaryInValueDetectorAssembly();
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

        /// <summary>
        /// Gets the properties required for auto-detection.
        /// </summary>
        /// <returns>Prerequisites object.</returns>
        private Prerequisites GetPrerequisitsInValueDetectorAssembly()
        {
            return valueDetector.GetPrerequisites();
        }

        private DetectionOutcome RunDetectionInValueDetectorAssembly()
        {
            try
            {
                valueDetector.RunDetection();
            }
            catch (Exception exception)
            {
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

            detectTask = new Task(() =>
            {
                token.ThrowIfCancellationRequested();

                var outcome = RunDetectionInValueDetectorAssembly();

                if (DetectionEvent != null)
                {
                    DetectionEvent(outcome);
                }
            }, token);

            detectTask.Start();
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
            return valueDetector.SetPrerequisiteProperties(properties);
        }

        /// <summary>
        /// Gets an object represents the detection summary.
        /// </summary>
        /// <returns>An object</returns>
        private object GetDetectionSummaryInValueDetectorAssembly()
        {
            return valueDetector.GetSUTSummary();
        }

        /// <summary>
        /// Gets the case selection rules suggested by the detector.
        /// </summary>
        /// <returns>A list of the rules</returns>
        private List<CaseSelectRule> GetRulesInValueDetectorAssembly()
        {
            return valueDetector.GetSelectedRules();
        }

        /// <summary>
        /// Gets a list of properties to hide.
        /// </summary>
        /// <param name="rules">Test case selection rules</param>
        /// <returns>A list of properties to hide.</returns>
        private List<string> GetHiddenPropertiesInValueDetectorAssembly(List<CaseSelectRule> rules)
        {
            return valueDetector.GetHiddenProperties(rules);
        }

        /// <summary>
        /// Gets the property values detected by the detector.
        /// </summary>
        /// <returns>Name - value / values map.</returns>
        private Dictionary<string, List<string>> GetDetectedPropertyInValueDetectorAssembly()
        {
            Dictionary<string, List<string>> dict;
            valueDetector.GetDetectedProperty(out dict);
            return dict;
        }
    }
}