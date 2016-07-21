// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Adts
{
    /// <summary>
    /// this is the context of AdtsLdapServer, <para/>
    /// which contains the security provider, 
    /// provides a friendly security interfaces,
    /// and LDAP operation Apis.
    /// </summary>
    public partial class AdtsLdapContext
    {
        #region Fields

        /// <summary>
        /// a SecurityLayer object that specifies the security layer.
        /// </summary>
        private AdtsLdapSecurityLayer security;

        /// <summary>
        /// a bool value that indicates whether using message security to encrypt/decrypt message.
        /// </summary>
        private bool usingMessageSecurity;

        #endregion

        #region Properties

        /// <summary>
        /// get/set a SecurityLayer object that specifies the security layer.
        /// </summary>
        public AdtsLdapSecurityLayer Security
        {
            get
            {
                return this.security;
            }
            set
            {
                this.security = value;
            }
        }


        /// <summary>
        /// get/set a bool value that indicates whether using message security to encrypt/decrypt message.
        /// </summary>
        public bool UsingMessageSecurity
        {
            get
            {
                return this.usingMessageSecurity;
            }
            set
            {
                this.usingMessageSecurity = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// set an int value that specifies the SignatureLength of SASL authentication; only for SASL.<para/>
        /// SASL encrypt message as LENGTH|SIGNATURE|MESSAGE, which need SignatureLength to decode the signature.<para/>
        /// the SignatureLength is initialized to a negative value.<para/>
        /// if the SignatureLength is negative, decoder will query the attributes from security context.<para/>
        /// normally, once security layer initializes, set the SignatureLength, then encode and decode packet.
        /// </summary>
        /// <param name="length">
        /// an int value that specifies the length of signature.
        /// </param>
        public void SetSaslSignatureLength(int length)
        {
            if (this.security == null)
            {
                return;
            }

            AdtsLdapSaslSecurityLayer saslSecurityLayer = this.security as AdtsLdapSaslSecurityLayer;
            if (saslSecurityLayer == null)
            {
                return;
            }

            saslSecurityLayer.SignatureLength = length;
        }

        #endregion
    }
}