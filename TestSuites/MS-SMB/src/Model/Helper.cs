// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Modeling;
using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocol.TestSuites.Smb
{
    public sealed class ModelHelper
    {
        /// <summary>
        /// The function MakeReqId makes the requirements using the Requirement ID and Requirement Description.
        /// Requirements Captured in Model need to be shown.
        /// </summary>
        /// <param name="id">Requirement ID obtained from the Requirements Specification spreadsheet.</param>
        /// <param name="description">Description of the requirement as captured from TD.</param>
        /// <returns>String.</returns>
        public static string MakeReqId(int id, string description)
        {
            return RequirementId.Make("MS-SMB", id, description);
        }


        /// <summary>
        /// The function RequiresCapture captures the requirement covered in Model.
        /// </summary>
        /// <param name="id">Requirement ID obtained from the Requirements Specification spreadsheet.</param>
        /// <param name="description">Description of the requirement as captured from TD.</param>
        public static void CaptureRequirement(int id, string description)
        {
            Requirement.Capture(MakeReqId(id, description));
        }


        #region Constructor

        /// <summary>
        /// ModelHelper private constructor.
        /// </summary>
        private ModelHelper()
        { }

        #endregion 
    }
}
