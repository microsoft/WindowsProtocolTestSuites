// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Xml;

namespace Microsoft.Protocols.TestManager.Kernel
{
    /// <summary>
    /// An interface of node.
    /// </summary>
    public interface IAdapterConfig
    {
        string DisplayName { get; }
        string Name { get; }
        [SuppressMessage("Microsoft.Design", "CA1059:MembersShouldNotExposeCertainConcreteTypes", Justification = "By Design")]
        XmlNode CreateXmlNode(XmlDocument doc);
        event ContentModifiedEventHandler ContentModified;
    }
}
