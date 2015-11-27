// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.FileSharing.FSA.Model
{
    class Helper
    {
        #region Requirement Capture
        /// <summary>
        /// Requirement Capture in model 
        /// </summary>
        /// <param name="id">Requirement ID</param>
        /// <param name="description">Requirement Description</param>
        public static void CaptureRequirement(int id, string description)
        {
            Requirement.Capture(RequirementId.Make("MS-FSA", id, description));
            Requirement.Capture(description);
        }
        #endregion
    }
}
