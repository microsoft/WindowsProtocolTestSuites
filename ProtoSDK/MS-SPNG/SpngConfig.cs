// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.Security.SspiLib;

namespace Microsoft.Protocols.TestTools.StackSdk.Security.Spng
{
    /// <summary>
    /// Base class of SpngClientConfig and SpngServerConfig
    /// </summary>
    public abstract class SpngConfig : SecurityConfig
    {
        /// <summary>
        /// Security context attribute flags
        /// </summary>
        private uint contextAttributes;

        /// <summary>
        /// MechType list
        /// </summary>
        private MechTypeList mechList;

        /// <summary>
        /// Hint string. Contains the string "not_defined_in_RFC4178@please_ignore
        /// </summary>
        private NegHints hints;

        /// <summary>
        /// Spng OID array, int[] formatted.
        /// </summary>
        private int[] spngOidIntArray;

        /// <summary>
        /// SPNG OID, default value: { 0x01, 0x03, 0x6, 0x01, 0x05, 0x05, 0x02 }
        /// </summary>
        public int[] SpngOidIntArray
        {
            get
            {
                return spngOidIntArray;
            }
            set
            {
                spngOidIntArray = value;
            }
        }

        /// <summary>
        /// Supported MechType list
        /// </summary>
        public MechTypeList MechList
        {
            get
            {
                return this.mechList;
            }
            set
            {
                this.mechList = value;
            }
        }

        /// <summary>
        /// Asn.1 formatted security context flags
        /// </summary>
        public ContextFlags Asn1ContextAttributes
        {
            get
            {
                //transform the contextAttributes to ContextFlags
                if(this.contextAttributes == 0)
                {
                    return null;
                }
                else
                {
                    return SpngUtility.ConvertUintToContextFlags(this.contextAttributes);
                }
            }
        }

        /// <summary>
        /// Uint security context flags
        /// </summary>
        public uint ContextAttributes
        {
            get
            {
                return this.contextAttributes;
            }
            set
            {
                this.contextAttributes = value;
            }
        }

        /// <summary>
        /// Hints string: not_defined_in_RFC4178@please_ignore
        /// </summary>
        public NegHints Hints
        {
            get
            {
                return this.hints;
            }
            set
            {
                this.hints = value;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="flags">Security context attributes</param>
        /// <param name="mechList">Supported MechType list</param>
        protected SpngConfig(uint flags, MechTypeList mechList)
            : base(SecurityPackageType.Negotiate)
        {
            this.mechList = mechList;
            this.contextAttributes = flags;

            this.spngOidIntArray = SspiLib.Consts.SpngOidInt;
            //as in TD, the hintAddress is never present.
            this.hints = new NegHints(new Asn1.Asn1GeneralString("not_defined_in_RFC4178@please_ignore"), null);
        }
    }
}
