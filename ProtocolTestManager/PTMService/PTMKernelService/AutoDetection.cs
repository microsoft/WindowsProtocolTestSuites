// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestManager.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private ReaderWriterLockSlim statusLocker = new ReaderWriterLockSlim();

        private Dictionary<int, int> detectStepIndexes = new Dictionary<int, int>();
        private Dictionary<int, StreamWriter> logStreams = new Dictionary<int, StreamWriter>();
        private Exception detectedException = null;

        private List<DetectingItem> detectSteps;

        private ITestSuite TestSuite { get; set; }

        private CancellationTokenSource cts = null;

        private IValueDetector valueDetector = null;

        private PrerequisiteView prerequisiteView = null;

        private Task detectTask = null;

        private bool taskCanceled = false;

        private string detectorAssembly = string.Empty;

        private string detectorInstanceTypeName = string.Empty;

        private DetectionStatus detectionStatus = DetectionStatus.NotStart;

        private string latestLogPath = string.Empty;

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
                            if (valueDetector == null)
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

        #region Get/Set Prerequisites

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

        #endregion

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
            return new DetectionOutcome(GetDetectionStatus(), detectedException);
        }

        public string GetDetectionLog()
        {
            if(!string.IsNullOrEmpty(latestLogPath) && File.Exists(latestLogPath))
            {
                return File.ReadAllText(latestLogPath);
            }
            return string.Empty;
        }

        #region Detection
        /// <summary>
        /// Reset AutoDetection settings
        /// </summary>
        public void Reset()
        {
            CloseLogger();

            if (detectTask != null)
            {
                detectTask.Wait(5000); // wait 2 seconds to check if it can completed
                detectTask = null;
            }

            if (valueDetector != null)
            {
                valueDetector.Dispose();
                valueDetector = null;
            }

            if (cts != null)
            {
                cts.Dispose();
            }
            
            UtilCallBackFunctions.WriteLog = (message, newline, style) =>
            {
                if (DetectLogCallback != null) DetectLogCallback(message, style);
            };
            
            stepsLocker.EnterWriteLock();
            try
            {
                detectSteps = ValueDetector.GetDetectionSteps();
                detectStepIndexes.Clear();
            }
            finally
            {
                stepsLocker.ExitWriteLock();
            }
            SetDetectionStatus(DetectionStatus.NotStart);
            taskCanceled = false;
            detectedException = null;
        }

        /// <summary>
        /// Begins the auto-detection.
        /// </summary>
        /// <param name="DetectionEvent">Callback function when the detection finished.</param>
        public void StartDetection(DetectionCallback callback)
        {
            if (GetDetectionStatus() == DetectionStatus.InProgress)
            {
                return;
            }

            // attach detect log callback to update detect step status
            AttachDetectLogCallback();

            // start detection
            StartDetection();
        }

        /// <summary>
        /// Stop the auto-detection
        /// </summary>
        public void StopDetection(Action callback)
        {
            stepsLocker.EnterWriteLock();
            try
            {
                detectSteps[StepIndex].DetectingStatus = DetectingStatus.Canceling;
            }
            finally
            {
                stepsLocker.ExitWriteLock();
            }

            //StopAndCancelDetection(callback);
            StopDetection();

            CloseLogger();
        }

        #endregion

        /// <summary>
        /// Gets an object represents the detection summary.
        /// </summary>
        /// <returns>An object</returns>
        public object GetDetectionSummary()
        {
            return ValueDetector.GetSUTSummary();
        }

        #region Apply Detection Summary to xml

        public void ApplyDetectionResult()
        {
            ApplyDetectedRules();
            ApplyDetectedValues();
        }

        private void ApplyDetectedRules()
        {
            var ruleGroups = TestSuite.LoadTestCaseFilter();
            foreach (var rule in ValueDetector.GetSelectedRules())
            {

            }
        }

        private void ApplyDetectedValues()
        {
            Dictionary<string, List<string>> detectedValues;
            ValueDetector.GetDetectedProperty(out detectedValues);
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Apply the test case selection rules detected by the plug-in.
        /// </summary>

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

        private void AttachDetectLogCallback()
        {
            DetectLogCallback = (msg, style) =>
            {
                stepsLocker.EnterWriteLock();
                try
                {
                    if (StepIndex == detectSteps.Count) return;
                    var item = detectSteps[StepIndex];
                    item.Style = style;

                    if(style != LogStyle.Default)
                    {
                        StepIndex++;
                    }

                    item.DetectingStatus = style switch
                    {
                        LogStyle.Default => DetectingStatus.Detecting,
                        LogStyle.Error => DetectingStatus.Error,
                        LogStyle.StepFailed => DetectingStatus.Failed,
                        LogStyle.StepSkipped => DetectingStatus.Skipped,
                        LogStyle.StepNotFound => DetectingStatus.NotFound,
                        LogStyle.StepPassed => DetectingStatus.Finished,
                        _ => DetectingStatus.Finished,
                    };
                }
                finally
                {
                    stepsLocker.ExitWriteLock();
                }

                if (LogWriter != null)
                {
                    LogWriter.WriteLine("[{0}] {1}", DateTime.Now.ToString(), msg);
                    LogWriter.Flush();
                }
            };
        }

        private async void StartDetection()
        {
            cts = new CancellationTokenSource();
            var token = cts.Token;

            token.Register(() =>
            {
                taskCanceled = true;
            });

            detectTask = new Task(() =>
            {
                token.ThrowIfCancellationRequested();

                // mark detection status as InProgress
                SetDetectionStatus(DetectionStatus.InProgress);
                try
                {
                    var resultStatus = ValueDetector.RunDetection() ? DetectionStatus.Finished : DetectionStatus.Error;
                    SetDetectionStatus(resultStatus);
                }
                catch (Exception ex)
                {
                    SetDetectionStatus(DetectionStatus.Error);
                    detectedException = ex;
                }

                CloseLogger();
            }, token);

            StepIndex = 0;
            latestLogPath = Path.Combine(TestSuite.StorageRoot.AbsolutePath, "Detector_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + ".log");
            LogWriter = new StreamWriter(latestLogPath);

            detectTask.Start();
        }

        private async void StopDetection()
        {
            if (detectTask != null)
            {
                cts.Cancel();
                while (!taskCanceled)
                {
                    Thread.SpinWait(100);
                }

                detectTask = null;
            }
            
            taskCanceled = false;
        }

        private void SetDetectionStatus(DetectionStatus status)
        {
            statusLocker.EnterWriteLock();
            try
            {
                detectionStatus = status;
            }
            finally
            {
                statusLocker.ExitWriteLock();
            }
        }

        private DetectionStatus GetDetectionStatus()
        {
            statusLocker.EnterReadLock();
            try
            {
                return detectionStatus;
            }
            finally
            {
                statusLocker.ExitReadLock();
            }
        }

        private void CloseLogger()
        {
            if (LogWriter != null)
            {
                LogWriter.Close();
                LogWriter.Dispose();

                if(detectTask!=null && logStreams.ContainsKey(detectTask.Id))
                {
                    logStreams.Remove(detectTask.Id);
                }
            }
        }

        private ReaderWriterLockSlim stepIndexLocker = new ReaderWriterLockSlim();
        private int StepIndex
        {
            get
            {
                stepIndexLocker.EnterReadLock();
                try
                {
                    if ((detectTask != null) && detectStepIndexes.ContainsKey(detectTask.Id))
                    {
                        return detectStepIndexes[detectTask.Id];
                    }
                    else
                    {
                        return 0;
                    }
                }
                finally
                {
                    stepIndexLocker.ExitReadLock();
                }
            }
            set
            {
                stepIndexLocker.EnterWriteLock();
                try
                {
                    if ((detectTask != null) && detectStepIndexes.ContainsKey(detectTask.Id))
                    {
                        detectStepIndexes[detectTask.Id] = value;
                    }
                    else if (detectTask != null)
                    {
                        detectStepIndexes.Add(detectTask.Id, 0);
                    }
                }
                finally
                {
                    stepIndexLocker.ExitWriteLock();
                }
            }
        }

        private ReaderWriterLockSlim logLocker = new ReaderWriterLockSlim();
        private StreamWriter LogWriter
        {
            get
            {
                logLocker.EnterReadLock();
                try
                {
                    if ((detectTask != null) && logStreams.ContainsKey(detectTask.Id))
                    {
                        return logStreams[detectTask.Id];
                    }

                    return null;
                }
                finally
                {
                    logLocker.ExitReadLock();
                }
            }
            set
            {
                logLocker.EnterWriteLock();
                try
                {
                    if ((detectTask != null) && logStreams.ContainsKey(detectTask.Id))
                    {
                        logStreams[detectTask.Id] = value;
                    }
                    else if (detectTask != null)
                    {
                        logStreams.Add(detectTask.Id, value);
                    }
                }
                finally
                {
                    logLocker.ExitWriteLock();
                }
            }
        }
        #endregion
    }
}