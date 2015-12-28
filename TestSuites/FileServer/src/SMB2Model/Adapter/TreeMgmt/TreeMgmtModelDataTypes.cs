// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.TreeMgmt
{
    /// <summary>
    /// The configuration of server for negotiate
    /// </summary>
    public struct TreeMgmtServerConfig
    {
        public Platform Platform;
    }


    public enum ModelSharePath
    {
        /// <summary>
        /// Invalid Share Path
        /// </summary>
        InvalidSharePath,

        /// <summary>
        /// Valid Share Path, Server Name is NetBIOS, FQDN or IPAddress. And Share Name is valid with 80 bytes
        /// </summary>
        ValidSharePath,

        /// <summary>
        /// Share with STYPE_SPECIAL bit, for example, //serverName//C$
        /// </summary>
        SpecialSharePath
    }

    public enum ModelTreeId
    {
        /// <summary>
        /// TreeId with value -1 in Adapter, which is reserved TreeID.
        /// </summary>
        InvalidTreeId,

        /// <summary>
        /// Valid TreeId(not -1) and the TreeConnect with the Tree Id exist in Session
        /// </summary>
        ValidExistTreeId,

        /// <summary>
        /// Valid TreeId(not -1) and the TreeConnect with the Tree Id not exist in Session 
        /// </summary>
        ValidNotExistTreeId
    }

    public enum ModelSessionSecurityContext
    {
        /// <summary>
        /// Session Security Context with Administrator
        /// </summary>
        Admin,
        /// <summary>
        /// Session Security Context with no Administrator
        /// </summary>
        NonAdmin
    }
}
