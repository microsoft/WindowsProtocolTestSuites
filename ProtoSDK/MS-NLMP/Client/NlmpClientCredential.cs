// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Protocols.TestTools.StackSdk.Security.Sspi;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp
{
    /// <summary>
    /// this class holds the credential of nlmp, such as the domain, username, password and so on.
    /// </summary>
    public class NlmpClientCredential : AccountCredential
    {
        #region Fields

        /// <summary>
        /// a null-terminated string that indicates the target of the context. The name is security-package specific. 
        /// </summary>
        private string targetName;

        #endregion

        #region Properties

        /// <summary>
        /// a null-terminated string that indicates the target of the context. The name is security-package specific. 
        /// </summary>
        public string TargetName
        {
            get
            {
                return this.targetName;
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public NlmpClientCredential()
            : base(string.Empty, string.Empty,string.Empty)
        {
        }


        /// <summary>
        /// This function allows applications to acquire a handle to preexisting credentials associated with the user 
        /// on whose behalf the call is made. These preexisting credentials are established through a system logon 
        /// not described here. However, this is different from login to the network and does not imply gathering of 
        /// credentials.
        /// </summary>
        /// <param name="targetName">
        /// a null-terminated string that indicates the target of the context. The name is security-package specific. 
        /// </param>
        /// <exception cref="ArgumentNullException">the targetName must not be null.</exception>
        [SuppressMessage("Microsoft.Maintainability", "CA1500:VariableNamesShouldNotMatchFieldNames")]
        public NlmpClientCredential(string targetName)
            : base(string.Empty, string.Empty, string.Empty)
        {
            if (targetName == null)
            {
                throw new ArgumentNullException("targetName");
            }

            this.targetName = targetName;
        }


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
        /// <exception cref="ArgumentNullException">the domain must not be null</exception>
        /// <exception cref="ArgumentNullException">the userName must not be null</exception>
        /// <exception cref="ArgumentNullException">the password must not be null</exception>
        [SuppressMessage("Microsoft.Maintainability", "CA1500:VariableNamesShouldNotMatchFieldNames")]
        public NlmpClientCredential(string targetName, string domain, string userName, string password)
            : base(NlmpUtility.UpperCase(domain), userName, password)
        {
            if (targetName == null)
            {
                throw new ArgumentNullException("targetName");
            }
            if (domain == null)
            {
                throw new ArgumentNullException("domain");
            }
            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            this.targetName = targetName;
        }


        #endregion
    }
}
