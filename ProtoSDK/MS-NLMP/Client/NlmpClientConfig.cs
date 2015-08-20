// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Nlmp
{
    /// <summary>
    /// the config information for nlmp client. e.g. the version of nlmp.
    /// </summary>
    public class NlmpClientConfig
    {
        #region Fields

        /// <summary>
        /// the version of nlmp. this value must be NTLMv1 or NTLMv2. LM is not supported by sdk.
        /// </summary>
        private NlmpVersion nlmpVersion;

        #endregion

        #region Properties

        /// <summary>
        /// the version of nlmp. this value must be NTLMv1 or NTLMv2. LM is not supported by sdk.
        /// </summary>
        public NlmpVersion Version
        {
            get
            {
                return this.nlmpVersion;
            }
            set
            {
                this.nlmpVersion = value;
            }
        }


        #endregion

        #region Constructors

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="nlmpVersion">
        /// the version of NLMP. this value must be NTLMv1 or NTLMv2. LM is not supported by sdk.
        /// </param>
        /// <exception cref="ArgumentException">the nlmpversion must be v1 or v2</exception>
        public NlmpClientConfig(NlmpVersion nlmpVersion)
        {
            if (nlmpVersion != NlmpVersion.v1 && nlmpVersion != NlmpVersion.v2)
            {
                throw new ArgumentException(
                    string.Format("the nlmpVersion is invalid value:{0}, it must be v1 or v2", nlmpVersion),
                    "nlmpVersion");
            }

            this.Version = nlmpVersion;
        }


        #endregion
    }
}
