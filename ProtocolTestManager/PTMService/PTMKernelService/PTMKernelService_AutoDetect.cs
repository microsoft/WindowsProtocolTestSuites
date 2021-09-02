// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    internal partial class PTMKernelService
    {
        private ReaderWriterLockSlim cacheLock = new ReaderWriterLockSlim();

        public int CreateAutoDetector(int configurationId)
        {
            var configuration = GetConfiguration(configurationId);
            var ptfConfigStorage = configuration.StorageRoot.GetNode(ConfigurationConsts.PtfConfig);

            var testSuite = configuration.TestSuite;

            cacheLock.EnterWriteLock();
            try
            {
                var autoDetector = AutoDetection.Create(configuration);
                AutoDetectionPool.AddOrUpdate(configurationId, _ => autoDetector, (_, _) => autoDetector);
            }
            finally
            {
                cacheLock.ExitWriteLock();
            }

            return configurationId;
        }

        public IAutoDetection GetAutoDetection(int configurationId)
        {
            cacheLock.EnterUpgradeableReadLock();
            try
            {
                if (!AutoDetectionPool.ContainsKey(configurationId))
                {
                    CreateAutoDetector(configurationId);
                }

                return AutoDetectionPool[configurationId];
            }
            finally
            {
                cacheLock.ExitUpgradeableReadLock();
            }
        }

        public PrerequisiteView GetPrerequisites(int configurationId)
        {
            return GetAutoDetection(configurationId).GetPrerequisites();
        }

        public bool SetPrerequisites(List<Property> prerequisiteProperties, int configurationId)
        {
            return GetAutoDetection(configurationId).SetPrerequisits(prerequisiteProperties);
        }

        public List<DetectingItem> GetDetectedSteps(int configurationId)
        {
            return GetAutoDetection(configurationId).GetDetectedSteps();
        }

        public DetectionOutcome GetDetectionOutcome(int configurationId)
        {
            return GetAutoDetection(configurationId).GetDetectionOutcome();
        }

        public void Reset(int configurationId)
        {
            GetAutoDetection(configurationId).Reset();
        }

        public void StartDetection(int configurationId, DetectionCallback callback)
        {
            GetAutoDetection(configurationId).StartDetection(callback);
        }

        public void StopDetection(int configurationId, Action callback)
        {
            GetAutoDetection(configurationId).StopDetection(callback);
        }

        public void ApplyDetectionResult(int configurationId)
        {
            var configuration = GetConfiguration(configurationId);
            var detector = GetAutoDetection(configurationId);

            // Get ruleGroupsBySelectedRules and ptfconfig properties values by detector.
            detector.ApplyDetectedRules(out IEnumerable<RuleGroup> ruleGroupsBySelectedRules, configuration.TargetFilterIndex, configuration.MappingFilterIndex);
            // Save profile.xml
            configuration.Rules = ruleGroupsBySelectedRules;

            IEnumerable<PropertyGroup> properties = GetConfigurationProperties(configurationId);
            detector.ApplyDetectedValues(ref properties);
            // Save ptfconfig files.
            SetConfigurationProperties(configurationId, properties);
        }

        public List<ResultItemMap> GetDetectionSummary(int configurationId)
        {
            return GetAutoDetection(configurationId).GetDetectionSummary();
        }

        public string GetDetectionLog(int configurationId)
        {
            return GetAutoDetection(configurationId).GetDetectionLog();
        }

        public IEnumerable<PropertyGroup> GetConfigurationProperties(int configurationId)
        {
            var configuration = GetConfiguration(configurationId);
            return configuration.GetProperties(GetAutoDetection(configurationId));
        }

        public void SetConfigurationProperties(int configurationId, IEnumerable<PropertyGroup> propertyGroupes)
        {
            var configuration = GetConfiguration(configurationId);
            configuration.SetProperties(propertyGroupes);
        }
    }
}