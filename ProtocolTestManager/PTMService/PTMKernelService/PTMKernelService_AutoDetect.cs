// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;
using Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel;

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    internal partial class PTMKernelService
    {
        public int CreateAutoDetector(int configurationId)
        {
            var configuration = GetConfiguration(configurationId);

            var testSuite = configuration.TestSuite;

            var autoDetector = AutoDetection.Create(testSuite);

            AutoDetectionPool.AddOrUpdate(configurationId, _ => autoDetector, (_, _) => autoDetector);

            return configurationId;
        }

        public IAutoDetection GetAutoDetection(int configurationId)
        {
            if (!AutoDetectionPool.ContainsKey(configurationId))
            {
                CreateAutoDetector(configurationId);
            }

            return AutoDetectionPool[configurationId];
        }

        public PrerequisiteView GetPrerequisites(int configurationId)
        {
            var detector = GetAutoDetection(configurationId);

            var prerequisitView = detector.GetPrerequisites();

            return prerequisitView;
        }

        public bool SetPrerequisites(List<PrerequisiteProperty> prerequisitProperties, int configurationId)
        {
            var detector = GetAutoDetection(configurationId);

            return detector.SetPrerequisits(prerequisitProperties);
        }

        public List<DetectingItem> GetDetectedSteps(int configurationId)
        {
            var detector = GetAutoDetection(configurationId);

            return detector.GetDetectedSteps();
        }

        public void StartDetection(int configurationId, DetectionCallback callback)
        {
            var detector = GetAutoDetection(configurationId);

            detector.StartDetection(callback);
        }

        public void StopDetection(int configurationId, Action callback)
        {
            var detector = GetAutoDetection(configurationId);

            detector.StopDetection(callback);
        }

        public object GetDetectionSummary(int configurationId)
        {
            var detector = GetAutoDetection(configurationId);

            return detector.GetDetectionSummary();
        }
    }
}