// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;
using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    public class SpngAccountCredential: AccountCredential
    {
        #region Fields
        /// <summary>
        /// Server principal name.
        /// </summary>
        private string principal;
        /// <summary>
        /// a null-terminated string that indicates the target of the context. The name is security-package specific. 
        /// </summary>
        private string targetName;
        #endregion

        #region Properties
        /// <summary>
        /// Property of server principal name.
        /// </summary>
        public string Principal
        {
            get
            {
                return this.principal;
            }
            set
            {
                this.principal = value;
            }
        }

        /// <summary>
        /// a null-terminated string that indicates the target of the context. The name is security-package specific. 
        /// </summary>
        public string TargetName
        {
            get
            {
                return this.targetName;
            }
            set
            {
                this.targetName = value;
            }
        }
        #endregion


        #region Methods
        /// <summary>
        /// This function allows applications to acquire a handle to preexisting credentials associated with the user 
        /// on whose behalf the call is made. These preexisting credentials are established through a system logon not 
        /// described here. However, this is different from login to the network and does not imply gathering of 
        /// credentials.
        /// </summary>
        /// <param name="targetName">
        /// a null-terminated string that indicates the target of the context. The name is security-package specific. 
        /// </param>
        /// <param name="domain">the domain of user to autenticate</param>
        /// <param name="userName">the name of user to autenticate</param>
        /// <param name="password">the password of user to autenticate</param>
        /// <exception cref="ArgumentNullException">the targetName must not be null</exception>
        /// <exception cref="ArgumentNullException">the principal must not be null</exception>
        public SpngAccountCredential(string principal, string targetName, string domain,
            string userName, string password)
            : base(domain, userName, password)
        {
            if (principal == null)
            {
                throw new ArgumentNullException("targetName");
            }
            if (targetName == null)
            {
                throw new ArgumentNullException("domain");
            }

            this.principal = principal;
            this.targetName = targetName;
        }
        #endregion
    }
}
