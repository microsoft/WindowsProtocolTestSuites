// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Protocols.TestManager.Detector;

namespace SampleDetector
{
    public class Detector : IValueDetector
    {
        public Detector()
        {
        }

        public void SelectEnvironment(string NetworkEnvironment)
        {
            throw new NotImplementedException();
        }

        public Prerequisites GetPrerequisites()
        {
            Prerequisites pre = new Prerequisites()
            {
                Summary = "Summary",
                Title = "Test",
                Properties = new Dictionary<string, List<string>>()
                {
                    {"Property1",new List<string>(){"Value1"}},
                    {"Property2",new List<string>(){"Value2","Value3"}},
                    {"Property3",new List<string>(){"Value4","Value5","Value6"}}
                }
            };
            return pre;
        }

        public bool SetPrerequisiteProperties(Dictionary<string, string> properties)
        {
            throw new NotImplementedException();
        }

        public List<DetectingItem> GetDetectionSteps()
        {
            throw new NotImplementedException();
        }

        public bool RunDetection()
        {
            throw new NotImplementedException();
        }

        public bool GetDetectedProperty(out Dictionary<string, List<string>> propertiesDic)
        {
            throw new NotImplementedException();
        }

        public List<CaseSelectRule> GetSelectedRules()
        {
            throw new NotImplementedException();
        }

        public object GetSUTSummary()
        {
            throw new NotImplementedException();
        }

        public List<string> GetHiddenProperties(List<CaseSelectRule> rules)
        {
            throw new NotImplementedException();
        }

        public bool CheckConfigrationSettings(Dictionary<string, string> properties)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
