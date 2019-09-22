// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// Json format test report
    /// </summary>
    public class JsonReport : TestReport
    {
        /// <summary>
        /// Constructor of JsonReport
        /// </summary>
        /// <param name="testCases">Executed test cases</param>
        public JsonReport(List<TestCase> testCases) : base(testCases)
        {
        }

        /// <summary>
        /// The file extension name for the report
        /// </summary>
        public override string FileExtension
        {
            get
            {
                return "json";
            }
        }

        /// <summary>
        /// The filter string to use in SaveFileDialog
        /// </summary>
        public override string FileDialogFilter
        {
            get
            {
                return "JSON text report (*.json)|*.json";
            }
        }

        /// <summary>
        /// Export report to a file
        /// </summary>
        /// <param name="filename">File name of the exported report</param>
        public override bool ExportReport(string filename)
        {
            if (this.testCases.Count() == 0)
            {
                return false;
            }

            using (StreamWriter sw = new StreamWriter(filename))
            {
                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = ShouldSerializeContractResolver.Instance
                };
                string report = JsonConvert.SerializeObject(testCases, Formatting.Indented, serializerSettings);
                sw.Write(report);
            }
            return true;
        }

        private class ShouldSerializeContractResolver : DefaultContractResolver
        {
            public new static readonly ShouldSerializeContractResolver Instance = new ShouldSerializeContractResolver();

            protected override JsonContract CreateContract(Type objectType)
            {
                JsonContract contract = base.CreateContract(objectType);

                if (objectType == typeof(TestCaseStatus))
                {
                    contract.Converter = new TestCaseStatusConverter();
                }

                return contract;
            }

            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                JsonProperty property = base.CreateProperty(member, memberSerialization);

                if (property.DeclaringType == typeof(TestCase))
                {
                    var ignoredProperties = new string[]
                    {
                        "ToolTipOnUI",
                        "IsChecked",
                        "LogUri",
                    };

                    if (ignoredProperties.Contains(property.PropertyName))
                    {
                        property.Ignored = true;
                    }
                }

                return property;
            }
        }

        private class TestCaseStatusConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return (objectType == typeof(TestCaseStatus));
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                TestCaseStatus status = (TestCaseStatus)value;
                string outcome = status == TestCaseStatus.Other ? "Inconclusive" : status.ToString();
                writer.WriteValue(outcome);
            }

            public override bool CanRead
            {
                get { return false; }
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }
        }
    }
}
