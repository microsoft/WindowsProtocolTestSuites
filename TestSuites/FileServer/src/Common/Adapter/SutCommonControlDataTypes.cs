// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Dtyp;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter
{
    /// <summary>
    /// Represent a local or domain user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The user name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The user _SID.
        /// </summary>
        public _SID Sid { get; set; }
    }

    /// <summary>
    /// Represent a local or domain group.
    /// </summary>
    public class Group
    {
        /// <summary>
        /// The group name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The group _SID.
        /// </summary>
        public _SID Sid { get; set; }
    }

    /// <summary>
    /// Represent a local or domain group member.
    /// </summary>
    public class GroupMember
    {
        /// <summary>
        /// The group member name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The group member object class.
        /// </summary>
        public string ObjectClass { get; set; }

        /// <summary>
        /// The group member principal source.
        /// </summary>
        public string PrincipalSource { get; set; }

        /// <summary>
        /// The group _SID.
        /// </summary>
        public _SID Sid { get; set; }
    }

    /// <summary>
    /// _WindowsIdentity class contains the necessary information to represent a Windows user.
    /// </summary>
    public class _WindowsIdentity
    {
        /// <summary>
        /// The down-level logon name of the domain user.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The user _SID.
        /// </summary>
        public _SID User { get; set; }

        /// <summary>
        /// An _SID collection of groups.
        /// The user is a member of the groups.
        /// </summary>
        public List<_SID> Groups { get; set; }

        /// <summary>
        /// The owner _SID.
        /// </summary>
        public _SID Owner { get; set; }
    }

    public class _SIDConverter : JsonConverter<_SID>
    {
        public override _SID Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return new _SID(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, _SID value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.GetSddlForm());
        }
    }
}
