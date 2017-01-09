// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using Microsoft.Protocols.TestTools.StackSdk.Networking.Rpce;

namespace Microsoft.Protocols.TestTools.StackSdk.ActiveDirectory.Drsr
{
    public class DrsrClientSessionContext : IDisposable, ICloneable
    {

        private IntPtr? drsHandle;
        private DRS_EXTENSIONS_INT? serverExtensions;

        //rpc handle
        private IntPtr? rpcHandle;

        private DrsrDomainFunctionLevel domainFunLevel;


        #region Properties

        ///// <summary>
        ///// Gets RPC transport context.
        ///// </summary>
        //[CLSCompliant(false)]
        //public RpceClientContext RpceTransportContext
        //{
        //    get
        //    {
        //        return rpceTransportContext;
        //    }
        //    set
        //    {
        //        rpceTransportContext = value;
        //    }
        //}

        /// <summary>
        /// Get and Set the RPC handle.
        /// </summary>
        [CLSCompliant(false)]
        public IntPtr RPCHandle
        {
            get
            {
                return rpcHandle.Value;
            }
            set
            {
                rpcHandle = value;
            }
        }

        /// <summary>
        /// Get and Set the Server Extensions
        /// </summary>
        [CLSCompliant(false)]
        public DRS_EXTENSIONS_INT? ServerExtensions
        {
            get
            {
                return serverExtensions;
            }
            set
            {
                serverExtensions = value;
            }
        }

        /// <summary>
        /// Get and set for DRSR handle
        /// </summary>
        [CLSCompliant(false)]
        public IntPtr DRSHandle
        {
            get
            {
                if (drsHandle!=null && drsHandle.HasValue)
                {
                    return drsHandle.Value;
                }
                else
                {
                    return IntPtr.Zero;
                }
            }
            set
            {
                drsHandle = value;
            }
        }

        /// <summary>
        /// Get and set for domainFunLevel
        /// </summary>
        [CLSCompliant(false)]
        public DrsrDomainFunctionLevel DomainFunLevel
        {
            get
            {
                return domainFunLevel;
            }
            set
            {
                domainFunLevel = value;
            }
        }

        #endregion


        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        internal DrsrClientSessionContext(DrsrDomainFunctionLevel domainFunLevel)
        {
            this.DomainFunLevel = domainFunLevel;
        }

        /// <summary>
        /// Constructor
        /// set dcFunLevel
        /// </summary>
        internal DrsrClientSessionContext()
        {
            this.DomainFunLevel = DrsrDomainFunctionLevel.DS_BEHAVIOR_WIN2003;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Dispose method.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Dispose method.
        /// </summary>
        /// <param name="disposing">
        /// True to release both managed and unmanaged resources.<para/>
        /// False to release unmanaged resources only.
        /// </param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        protected virtual void Dispose(bool disposing)
        {

            // Release unmanaged resources.
        }


        /// <summary>
        /// finalizer
        /// </summary>
        ~DrsrClientSessionContext()
        {
            Dispose(false);
        }

        #endregion

        public object Clone()
        {
            DrsrClientSessionContext ret = new DrsrClientSessionContext();
            ret.domainFunLevel = this.domainFunLevel;
            ret.drsHandle = this.drsHandle;
            ret.rpcHandle = this.rpcHandle;
            if (this.serverExtensions.HasValue)
            {
                DRS_EXTENSIONS_INT ext = new DRS_EXTENSIONS_INT();
                ext.cb = this.serverExtensions.Value.cb;
                ext.ConfigObjGUID = this.serverExtensions.Value.ConfigObjGUID;
                ext.dwFlags = this.serverExtensions.Value.dwFlags;
                ext.dwFlagsExt = this.serverExtensions.Value.dwFlagsExt;
                ext.dwReplEpoch = this.serverExtensions.Value.dwReplEpoch;
                ext.Pid = this.serverExtensions.Value.Pid;
                ext.SiteObjGuid = this.serverExtensions.Value.SiteObjGuid;
                ret.serverExtensions = ext;
            }
            return ret;
        }
    }
}
