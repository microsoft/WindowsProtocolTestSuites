// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestManager.Detector
{
    /// <summary>
    /// Prerequisites for auto-detect.
    /// </summary>
    public class Prerequisites
    {
        /// <summary>
        /// The Title of the set prerequisites window.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Summary information to be shown in the windows.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// The properties needed for the auto-detection.
        /// Key: Property name.
        /// Value: Default value(One item) / Possible values (Multiple items).
        /// </summary>
        public Dictionary<string, List<string>> Properties { get; set; }
    }

    /// <summary>
    /// Class rule status
    /// </summary>
    public class CaseSelectRule
    {
        /// <summary>
        /// Rule name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Rule status.
        /// </summary>
        public RuleStatus Status { get; set; }
    }
    /// <summary>
    /// Rule status
    /// </summary>
    public enum RuleStatus 
    { 
        /// <summary>
        /// Indicates the rule is selected.
        /// </summary>
        Selected, 

        /// <summary>
        /// Indicates the rule is not supported.
        /// </summary>
        NotSupported, 

        /// <summary>
        /// Indicates the rule is unknown.
        /// </summary>
        Unknown, 

        /// <summary>
        /// Indicates the rule is default.
        /// </summary>
        Default 
    }

    /// <summary>
    /// Interface for property value auto-detect.
    /// </summary>
    public interface IValueDetector : IDisposable
    {
        /// <summary>
        /// Sets selected test environment.
        /// </summary>
        /// <param name="NetworkEnvironment">The network environment</param>
        void SelectEnvironment(string NetworkEnvironment);

        /// <summary>
        /// Gets the prerequisites for auto-detection.
        /// </summary>
        /// <returns>A instance of Prerequisites class.</returns>
        Prerequisites GetPrerequisites();

        /// <summary>
        /// Sets the values for the required properties.
        /// </summary>
        /// <param name="properties">Property name and values.</param>
        /// <returns>
        /// Return true if no other property needed. Return false means there are 
        /// other property required. PTF Tool will invoke GetPrerequisites and 
        /// pop up a dialog to set the value of the properties.
        /// </returns>
        bool SetPrerequisiteProperties(Dictionary<string, string> properties);

        /// <summary>
        /// Adds Detection steps to the log shown when detecting
        /// </summary>
        List<DetectingItem> GetDetectionSteps();

        /// <summary>
        /// Runs property autodetection.
        /// </summary>
        /// <returns>Return true if the function is succeeded.</returns>
        bool RunDetection();

        /// <summary>
        /// Gets the detect result.
        /// </summary>
        /// <returns>Return true if the property value is successfully got.</returns>
        bool GetDetectedProperty(out Dictionary<string, List<string>> propertiesDic);

        /// <summary>
        /// Gets selected rules
        /// </summary>
        /// <returns>Selected rules</returns>
        List<CaseSelectRule> GetSelectedRules();

        /// <summary>
        /// Gets a summary of the detect result.
        /// </summary>
        /// <returns>Detect result.</returns>
        object GetSUTSummary();

        /// <summary>
        /// Gets the list of properties that will be hidder in the configure page.
        /// </summary>
        /// <param name="rules">Selected rules.</param>
        /// <returns>The list of properties whick will not be shown in the configure page.</returns>
        List<string> GetHiddenProperties(List<CaseSelectRule> rules);

        /// <summary>
        /// Returns false if check failed and set failed property in dictionary
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        bool CheckConfigrationSettings(Dictionary<string, string> properties);

    }
}

