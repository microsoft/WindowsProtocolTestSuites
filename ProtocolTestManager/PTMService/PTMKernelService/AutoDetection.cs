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
using System.Runtime.Loader;
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

        private Exception detectedException = null;

        private List<DetectingItem> detectSteps;

        private ITestSuite TestSuite { get; set; }

        private IConfiguration Configuration { get; set; }

        private PtfConfig PtfConfig { get; set; }

        private CancellationTokenSource cts = null;

        private IValueDetector valueDetector = null;

        private PrerequisiteView prerequisiteView = null;

        private Task detectTask = null;

        private bool taskCanceled = false;

        private string detectorAssemblyFileName = string.Empty;

        private Assembly detectorAssembly = null;

        private AssemblyLoadContext alc = null;

        private string detectorInstanceTypeName = string.Empty;

        private DetectionStatus detectionStatus = DetectionStatus.NotStart;

        private string latestLogPath = string.Empty;

        private string latestDetectorInstanceId = string.Empty;

        private Dictionary<string, StreamWriter> detectLogs = new Dictionary<string, StreamWriter>();
        private Dictionary<string, int> detectStepIndexes = new Dictionary<string, int>();

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

        private AutoDetection(IConfiguration configuration)
        {
            TestSuite = configuration.TestSuite;
            Configuration = configuration;

            InitializeDetector();

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
                    Value = ((i.Value != null) && (i.Value.Count > 0)) ? i.Value[0] : null,
                    Choices = i.Value
                });
            }
        }

        public static AutoDetection Create(IConfiguration configuration)
        {
            var instance = new AutoDetection(configuration);

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
                                valueDetector = detectorAssembly.CreateInstance(detectorInstanceTypeName) as IValueDetector;
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
        /// <param name="detectorAssemblyFileName">File name</param>
        public void Load(string detectorAssemblyFileName)
        {
            // Get CustomerInterface
            Type interfaceType = typeof(IValueDetector);

            string assemblyDirPath = Directory.GetParent(detectorAssemblyFileName).FullName;
            alc = new CollectibleAssemblyLoadContext(detectorAssemblyFileName, AutoDetectionConsts.ignoredAssemblies, AutoDetectionConsts.mixedAssemblies);

            alc.Resolving += (context, assemblyName) =>
            {
                string assemblyPath = Path.Combine(assemblyDirPath, $"{assemblyName.Name}.dll");
                if (File.Exists(assemblyPath))
                {
                    return context.LoadFromAssemblyPath(assemblyPath);
                }

                return null;
            };

            Assembly assembly = alc.LoadFromAssemblyPath(detectorAssemblyFileName);

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

            detectorAssembly = assembly;
        }

        public void InitializeDetector()
        {
            var ptfConfigStorage = Configuration.StorageRoot.GetNode(ConfigurationConsts.PtfConfig);
            PtfConfig = new PtfConfig(ptfConfigStorage.GetFiles().ToList());

            UtilCallBackFunctions.GetPropertyValue = (string name) =>
            {
                var property = this.PtfConfig.GetPropertyNodeByName(name);
                if (property != null) return property.Value;
                return null;
            };

            UtilCallBackFunctions.GetPropertiesByFile = (filename) =>
            {
                if (!this.PtfConfig.FileProperties.ContainsKey(filename))
                    return null;
                return this.PtfConfig.FileProperties[filename];
            };

            detectorAssemblyFileName = TestSuite.GetDetectorAssembly();

            Load(detectorAssemblyFileName);
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
        public bool SetPrerequisits(List<Property> prerequisiteProperties)
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            foreach (var p in prerequisiteProperties)
            {
                properties.Add(p.Name, p.Value);
            };

            prerequisitesLocker.EnterWriteLock();
            try
            {
                prerequisiteView.Properties = prerequisiteProperties;
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
            try
            {
                if (!string.IsNullOrEmpty(latestLogPath) && File.Exists(latestLogPath))
                {
                    return File.ReadAllText(latestLogPath);
                }
                return string.Empty;
            }
            catch (IOException)
            {
                return $"{latestLogPath} is being processed, please wait and try again.";
            }
        }

        #region Detection
        /// <summary>
        /// Reset AutoDetection settings
        /// </summary>
        public void Reset()
        {
            CloseLogger();

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
            SetDetectStepCurrentStatus(DetectingStatus.Canceling);

            StopDetection();
        }

        #endregion

        /// <summary>
        /// Gets an object represents the detection summary.
        /// </summary>
        /// <returns>An object</returns>
        public List<ResultItemMap> GetDetectionSummary()
        {
            return ValueDetector.GetSUTSummary();
        }

        #region Apply Detection Summary to xml
        /// <summary>
        /// Apply the test case selection rules detected by the plug-in.
        /// </summary>
        /// <param name="ruleGroupsBySelectedRules">The rule groups by selected rules.</param>
        /// <param name="targetFilterIndex">Target filter index</param>
        /// <param name="mappingFilterIndex">Mapping filter index</param>
        public void ApplyDetectedRules(out IEnumerable<Common.Types.RuleGroup> ruleGroupsBySelectedRules, int targetFilterIndex, int mappingFilterIndex)
        {
            // Get the filter.
            var filter = TestSuite.GetTestCaseFilter();
            // create mapping table for the filter.
            CreateMappingTableForTestCaseFilter(filter, targetFilterIndex, mappingFilterIndex);
            // Update selected rules for the filter.
            foreach (var rule in ValueDetector.GetSelectedRules())
            {
                Kernel.Rule r = filter.FindRuleByName(rule.Name);
                if (r == null) throw new Exception(string.Format("Cannot find rule by name {0}.", rule.Name));
                switch (rule.Status)
                {
                    case Microsoft.Protocols.TestManager.Detector.RuleStatus.Selected:
                        r.SelectStatus = Kernel.RuleSelectStatus.Selected;
                        r.Status = RuleSupportStatus.Selected;
                        break;
                    case Microsoft.Protocols.TestManager.Detector.RuleStatus.NotSupported:
                        r.SelectStatus = Kernel.RuleSelectStatus.UnSelected;
                        r.Status = RuleSupportStatus.NotSupported;
                        break;
                    case Microsoft.Protocols.TestManager.Detector.RuleStatus.Unknown:
                        r.SelectStatus = Kernel.RuleSelectStatus.UnSelected;
                        r.Status = RuleSupportStatus.Unknown;
                        break;
                    default:
                        r.SelectStatus = Kernel.RuleSelectStatus.UnSelected;
                        r.Status = RuleSupportStatus.Default;
                        break;
                }
            }
            // Update filter to ruleGroups
            var ruleGroups = new List<Common.Types.RuleGroup>();
            foreach (var group in filter)
            {
                Common.Types.RuleGroup ruleGroup = new Common.Types.RuleGroup()
                {
                    Name = group.Name,
                    DisplayName = group.Name,
                    Rules = new List<Common.Types.Rule>()
                };
                AddItems(ruleGroup.Rules, group);
                ruleGroups.Add(ruleGroup);
            }
            // Update the selected rule groups.
            ruleGroupsBySelectedRules = ruleGroups;
        }

        public void CreateMappingTableForTestCaseFilter(TestCaseFilter filter, int targetFilterIndex, int mappingFilterIndex)
        {
            if (targetFilterIndex == -1 ||
                mappingFilterIndex == -1)
            {
                return;
            }
            else
            {
                Dictionary<string, List<Kernel.Rule>> featureMappingTableForKernel = new Dictionary<string, List<Kernel.Rule>>();
                Dictionary<string, List<Kernel.Rule>> reverseMappingTableForKernel = new Dictionary<string, List<Kernel.Rule>>();
                Kernel.RuleGroup targetFilterGroup = filter[targetFilterIndex];
                Kernel.RuleGroup mappingFilterGroup = filter[mappingFilterIndex];
                Dictionary<string, Kernel.Rule> mappingRuleTable = CreateRuleTableFromRuleGroupForKernel(mappingFilterGroup);
                Dictionary<string, Kernel.Rule> targetRuleTable = CreateRuleTableFromRuleGroupForKernel(targetFilterGroup);

                var testCaseList = TestSuite.GetTestCases(null);

                foreach (TestManager.Common.TestCaseInfo testCase in testCaseList)
                {
                    List<string> categories = testCase.Category.ToList();
                    foreach (string target in targetRuleTable.Keys)
                    {
                        if (categories.Contains(target))
                        {
                            Kernel.Rule currentRule;
                            foreach (string category in categories)
                            {
                                if (!category.Equals(target))
                                {
                                    mappingRuleTable.TryGetValue(category, out currentRule);
                                    if (currentRule == null)
                                    {
                                        continue;
                                    }
                                    UpdateMappingTableForKernel(featureMappingTableForKernel, target, currentRule);
                                    // Add item to reverse mapping table
                                    UpdateMappingTableForKernel(reverseMappingTableForKernel, category, targetRuleTable[target]);
                                }
                            }
                            break;
                        }
                    }
                }

                targetFilterGroup.featureMappingTable = featureMappingTableForKernel;
                targetFilterGroup.mappingRuleGroup = mappingFilterGroup;

                mappingFilterGroup.reverseFeatureMappingTable = reverseMappingTableForKernel;
                mappingFilterGroup.targetRuleGroup = targetFilterGroup;
            }
        }

        private Dictionary<string, Kernel.Rule> CreateRuleTableFromRuleGroupForKernel(Kernel.RuleGroup ruleGroup)
        {
            Dictionary<string, Kernel.Rule> ruleTable = new Dictionary<string, Kernel.Rule>();
            Stack<Kernel.Rule> ruleStack = new Stack<Kernel.Rule>();
            foreach (Kernel.Rule r in ruleGroup) ruleStack.Push(r);
            while (ruleStack.Count > 0)
            {
                Kernel.Rule r = ruleStack.Pop();
                if (r.CategoryList.Count != 0 &&
                    !ruleTable.ContainsKey(r.CategoryList[0]))
                {
                    ruleTable.Add(r.CategoryList[0], r);
                }
                foreach (Kernel.Rule childRule in r) ruleStack.Push(childRule);
            }
            return ruleTable;
        }

        private void UpdateMappingTableForKernel(Dictionary<string, List<Kernel.Rule>> mappingTable, string target, Kernel.Rule currentRule)
        {
            if (mappingTable.ContainsKey(target))
            {
                if (!mappingTable[target].Contains(currentRule))
                {
                    mappingTable[target].Add(currentRule);
                }
            }
            else
            {
                mappingTable[target] = new List<Kernel.Rule> { currentRule };
            }
        }

        private void AddItems(IList<Common.Types.Rule> displayRules, List<Kernel.Rule> rules)
        {
            foreach (var rule in rules)
            {
                Common.Types.Rule displayRule = new Common.Types.Rule()
                {
                    DisplayName = rule.Name,
                    Name = rule.Name,
                    Categories = rule.CategoryList.ToArray(),
                    SelectStatus = rule.SelectStatus == Kernel.RuleSelectStatus.Selected ? Common.Types.RuleSelectStatus.Selected : (rule.SelectStatus == Kernel.RuleSelectStatus.Partial ? Common.Types.RuleSelectStatus.Partial : Common.Types.RuleSelectStatus.UnSelected),
                };

                if (rule.Count > 0)
                {
                    AddItems(displayRule, rule);
                }
                displayRules.Add(displayRule);
            }
        }

        public void ApplyDetectedValues(ref IEnumerable<PropertyGroup> properties)
        {
            Dictionary<string, List<string>> propertiesByDetector;
            ValueDetector.GetDetectedProperty(out propertiesByDetector);
            List<PropertyGroup> updatedPropertyGroupList = new List<PropertyGroup>();
            foreach (var ptfconfigProperty in properties)
            {
                PropertyGroup newPropertyGroup = new PropertyGroup()
                {
                    Name = ptfconfigProperty.Name,
                    Items = ptfconfigProperty.Items,
                };

                foreach (var item in ptfconfigProperty.Items)
                {
                    var propertyFromDetctor = propertiesByDetector.Where(i => i.Key == item.Key);
                    if (propertyFromDetctor.Count() > 0)
                    {
                        var detectorPropertyValue = propertyFromDetctor.FirstOrDefault().Value;
                        var newProperty = newPropertyGroup.Items.Where(i => i.Key == item.Key).FirstOrDefault();
                        if (detectorPropertyValue.Count() == 1)
                        {
                            newProperty.Value = detectorPropertyValue[0];
                        }
                        else if (detectorPropertyValue.Count() > 0)
                        {
                            newProperty.Choices = detectorPropertyValue;
                            newProperty.Value = detectorPropertyValue[0];
                        }
                    }
                }

                updatedPropertyGroupList.Add(newPropertyGroup);
            }
            properties = updatedPropertyGroupList.ToArray();
        }

        #endregion

        #region Private Methods

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
        /// Gets a list of properties to hide.
        /// </summary>
        /// <param name="rules">Test case selection rules</param>
        /// <returns>A list of properties to hide.</returns>
        public List<string> GetHiddenPropertiesInValueDetectorAssembly(List<CaseSelectRule> rules)
        {
            return ValueDetector.GetHiddenProperties(rules);
        }

        private void AttachDetectLogCallback()
        {
            DetectLogCallback = (msg, style) =>
            {
                if (LogWriter != null)
                {
                    LogWriter.WriteLine("[{0}] {1}", DateTime.Now.ToString(), msg);
                    LogWriter.Flush();
                }
            };
        }

        private void StartDetection()
        {
            cts = new CancellationTokenSource();
            var token = cts.Token;

            token.Register(() =>
            {
                taskCanceled = true;
            });

            DetectContext context = new DetectContext((instanceId, stepId, logStyle) =>
             {
                 if (taskCanceled || !instanceId.Equals(latestDetectorInstanceId))
                 {
                     return;
                 }

                 var status = logStyle switch
                 {
                     LogStyle.Default => DetectingStatus.Detecting,
                     LogStyle.Error => DetectingStatus.Error,
                     LogStyle.StepFailed => DetectingStatus.Failed,
                     LogStyle.StepSkipped => DetectingStatus.Skipped,
                     LogStyle.StepNotFound => DetectingStatus.NotFound,
                     LogStyle.StepPassed => DetectingStatus.Finished,
                     _ => DetectingStatus.Finished,
                 };

                 StepIndex = stepId;
                 SetDetectStepCurrentStatus(status);
             }, token, TestSuite.StorageRoot.AbsolutePath);
            latestDetectorInstanceId = context.Id;

            detectTask = new Task(() =>
            {
                token.ThrowIfCancellationRequested();

                // mark detection status as InProgress
                SetDetectionStatus(DetectionStatus.InProgress);
                try
                {
                    var resultStatus = ValueDetector.RunDetection(context) ? DetectionStatus.Finished : DetectionStatus.Error;
                    SetDetectionStatus(resultStatus);
                    detectedException = null;

                    if (cts.IsCancellationRequested)
                    {
                        SetDetectStepCurrentStatus(DetectingStatus.Cancelled);
                    }
                }
                catch (Exception ex)
                {
                    SetDetectionStatus(DetectionStatus.Error);
                    detectedException = ex;
                }

                if ((StepIndex < GetDetectedSteps().Count - 1) && !cts.IsCancellationRequested)
                {
                    SetDetectStepCurrentStatus(DetectingStatus.Pending);
                }

                CloseLogger();
            }, token);

            latestLogPath = Path.Combine(TestSuite.StorageRoot.AbsolutePath, "Detector_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + ".log");
            LogWriter = new StreamWriter(latestLogPath);
            detectTask.Start();
        }

        private void StopDetection()
        {
            if (detectTask != null)
            {
                cts.Cancel();
                while (!taskCanceled)
                {
                    Thread.SpinWait(100);
                }
            }
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
            DetectLogCallback = null;

            if (LogWriter != null)
            {
                LogWriter.Close();
                LogWriter.Dispose();
            }

            if (detectLogs.ContainsKey(latestDetectorInstanceId))
            {
                detectLogs.Remove(latestDetectorInstanceId);
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
                    return detectStepIndexes[latestDetectorInstanceId];
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
                    detectStepIndexes[latestDetectorInstanceId] = value;
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
                    if (detectLogs.ContainsKey(latestDetectorInstanceId))
                    {
                        return detectLogs[latestDetectorInstanceId];
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
                    detectLogs[latestDetectorInstanceId] = value;
                }
                finally
                {
                    logLocker.ExitWriteLock();
                }
            }
        }

        private void SetDetectStepCurrentStatus(DetectingStatus detectingStatus)
        {
            if (StepIndex >= detectSteps.Count)
                return;

            stepsLocker.EnterWriteLock();
            try
            {
                detectSteps[StepIndex].DetectingStatus = detectingStatus;
            }
            finally
            {
                stepsLocker.ExitWriteLock();
            }
        }

        #endregion
    }
}