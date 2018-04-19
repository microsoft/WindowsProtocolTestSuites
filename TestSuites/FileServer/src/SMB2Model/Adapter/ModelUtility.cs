// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smb2;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter
{
    /// <summary>
    /// This class is a utility of common methods for model
    /// </summary>
    public static class ModelUtility
    {
        /// <summary>
        /// Convert dialect from ModelDialectRevision to DialectRevision.
        /// </summary>
        /// <param name="dialect">ModelDialectRevision</param>
        /// <returns>DialectRevision</returns>
        public static DialectRevision GetDialectRevision(ModelDialectRevision dialect)
        {
            return (DialectRevision)(uint)dialect;
        }

        /// <summary>
        /// Convert dialect from DialectRevision to ModelDialectRevision.
        /// </summary>
        /// <param name="dialect">DialectRevision</param>
        /// <param name="treat311as302">Whether treat 311 as 302. We treat 311 as 302 in most SMB2 model cases witch have the same behavior in 311 and 302 to reduce the case number</param>
        /// <returns>ModelDialectRevision</returns>
        public static ModelDialectRevision GetModelDialectRevision(DialectRevision dialect, bool treat311as302 = true)
        {
            ModelDialectRevision revision = (ModelDialectRevision)(uint)dialect;

            if (Enum.IsDefined(typeof(ModelDialectRevision), revision) && (!treat311as302 || revision != ModelDialectRevision.Smb311))
            {
                return revision;
            }
            else if (dialect > DialectRevision.Smb302 && dialect != DialectRevision.Smb2Unknown)
            {
                return ModelDialectRevision.Smb302; // Model cases only test Dialect lower than 3.11
            }
            else
            {
                throw new ArgumentException("Unexpected dialect");
            }
        }

        /// <summary>
        /// Determine if a given dialect belongs to the SMB 3.x dialect family
        /// </summary>
        /// <param name="dialect">Dialect to be determined</param>
        /// <returns>Return true if given dialect belongs to the SMB 3.x dialect family, otherwise return false</returns>
        public static bool IsSmb3xFamily(DialectRevision dialect)
        {
            return dialect >= DialectRevision.Smb30 && dialect != DialectRevision.Smb2Unknown;
        }

        /// <summary>
        /// Determine if a given model dialect belongs to the SMB 3.x dialect family
        /// </summary>
        /// <param name="dialect">Model dialect to be determined</param>
        /// <returns>Return true if given model dialect belongs to the SMB 3.x dialect family, otherwise return false</returns>
        public static bool IsSmb3xFamily(ModelDialectRevision dialect)
        {
            return dialect >= ModelDialectRevision.Smb30;
        }

        /// <summary>
        /// Determine if a given dialect belongs to the SMB 2 dialect family
        /// </summary>
        /// <param name="dialect">Dialect to be determined</param>
        /// <returns>Return true if given dialect belongs to the SMB 2 dialect family, otherwise return false</returns>
        public static bool IsSmb2Family(DialectRevision dialect)
        {
            return dialect == DialectRevision.Smb2002 || dialect == DialectRevision.Smb21;
        }

        /// <summary>
        /// Determine if a given model dialect belongs to the SMB 2 dialect family
        /// </summary>
        /// <param name="dialect">Model dialect to be determined</param>
        /// <returns>Return true if given model dialect belongs to the SMB 2 dialect family, otherwise return false</returns>
        public static bool IsSmb2Family(ModelDialectRevision dialect)
        {
            return dialect == ModelDialectRevision.Smb2002 || dialect == ModelDialectRevision.Smb21;
        }
    }
}
