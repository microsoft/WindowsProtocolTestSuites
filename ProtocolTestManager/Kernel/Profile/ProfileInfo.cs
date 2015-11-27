// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// The information of the test profile.
    /// </summary>
    public class ProfileInfo
    {
        /// <summary>
        /// The test suite name of the profile.
        /// </summary>
        public string TestSuiteName { get; set; }
        /// <summary>
        /// The Version of the profile.
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// Serializes the object to stream.
        /// </summary>
        /// <param name="stream">Stream</param>
        public void WriteTo(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ProfileInfo));
            serializer.Serialize(stream, this);
        }

        /// <summary>
        /// Reads profile info from a stream.
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns>An instance of the ProfileInfo</returns>
        public static ProfileInfo Readfrom(Stream stream)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ProfileInfo));
            XmlReaderSettings settings = new XmlReaderSettings()
            {
                XmlResolver = null
            };
            ProfileInfo profileInfo;
            using (XmlReader reader = XmlReader.Create(stream, settings))
            {
                profileInfo = serializer.Deserialize(reader) as ProfileInfo;
            }
            return profileInfo;
        }

    }
}
