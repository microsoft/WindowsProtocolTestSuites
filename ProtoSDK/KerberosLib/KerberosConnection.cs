// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Net;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /// <summary>
    /// Maintains the connection with a client
    /// </summary>
    public class KerberosConnection
    {
        /// <summary>
        /// Represents the target ip endpoint while sending packets
        /// </summary>
        private IPEndPoint targetEndPoint;


        /// <summary>
        /// Represents the target ip endpoint while sending packets
        /// </summary>
        public IPEndPoint TargetEndPoint
        {
            get
            {
                return targetEndPoint;
            }
        }

        #region Constructor

        /// <summary>
        /// Create a KerberosConnection object
        /// </summary>
        /// <param name="ipEndPoint">Represents the target ip endpoint while sending packets</param>
        public KerberosConnection(IPEndPoint ipEndPoint)
        {
            targetEndPoint = ipEndPoint;
        }

        #endregion
    }
}
