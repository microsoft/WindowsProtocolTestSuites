// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Protocols.TestManager.Detector;
using Microsoft.Protocols.TestManager.PTMService.Common.Types;

namespace Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel
{
    public interface IAutoDetection
    {
        void InitializeDetector();

        PrerequisiteView GetPrerequisites();

        bool SetPrerequisits(List<Property> prerequisiteProperties);

        List<DetectingItem> GetDetectedSteps();

        DetectionOutcome GetDetectionOutcome();

        void StartDetection(DetectionCallback callback);

        void StopDetection(Action callback);

        List<ResultItemMap> GetDetectionSummary();

        void Reset();

        void ApplyDetectedValues(ref IEnumerable<PropertyGroup> properties);

        void ApplyDetectedRules(out IEnumerable<Common.Types.RuleGroup> ruleGroupsBySelectedRules, int targetFilterIndex, int mappingFilterIndex);

        string GetDetectionLog();

        List<string> GetHiddenPropertiesInValueDetectorAssembly(List<CaseSelectRule> rules);
    }
}