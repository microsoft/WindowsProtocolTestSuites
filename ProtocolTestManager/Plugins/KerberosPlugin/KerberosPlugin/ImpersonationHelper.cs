// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.KerberosPlugin
{
    public enum ImpersonationLevel
    {
        SecurityAnonymous = 0,
        SecurityIdentification = 1,
        SecurityImpersonation = 2,
        SecurityDelegation = 3
    }

    public enum LogonType
    {
        LOGON32_LOGON_INTERACTIVE = 2,
        LOGON32_LOGON_NETWORK = 3,
        LOGON32_LOGON_BATCH = 4,
        LOGON32_LOGON_SERVICE = 5,
        LOGON32_LOGON_UNLOCK = 7,
        LOGON32_LOGON_NETWORK_CLEARTEXT = 8,
        LOGON32_LOGON_NEW_CREDENTIALS = 9
    }

    public enum LogonProvider
    {
        LOGON32_PROVIDER_DEFAULT = 0,
        LOGON32_PROVIDER_WINNT35 = 1,
        LOGON32_PROVIDER_WINNT40 = 2,
        LOGON32_PROVIDER_WINNT50 = 3
    }

    public class ImersonationFailureException : Exception
    {
        public ImersonationFailureException(string msg)
            : base(msg)
        {

        }
    }

    public class ImpersonationHelper : IDisposable
    {
        [DllImport("advapi32.dll")]
        public static extern bool LogonUser(String lpszUserName,
            String lpszDomain,
            String lpszPassword,
            int dwLogonType,
            int dwLogonProvider,
            ref IntPtr phToken);

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool ImpersonateLoggedOnUser(IntPtr hToken);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool CloseHandle(IntPtr handle);

        bool disposed;

        /// <summary>
        /// Impersonate a specific user context
        /// </summary>
        public ImpersonationHelper(
            string userName,
            string domainName,
            string password,
            LogonType logonType = LogonType.LOGON32_LOGON_NEW_CREDENTIALS,
            LogonProvider logonProvider = LogonProvider.LOGON32_PROVIDER_WINNT50)
        {
            if (string.IsNullOrEmpty(userName))
            {
                throw new ArgumentNullException("userName");
            }

            if (string.IsNullOrEmpty(domainName))
            {
                domainName = ".";
            }

            IntPtr token = IntPtr.Zero;
            int errorCode = 0;

            if (LogonUser(userName, domainName, password, (int)logonType, (int)logonProvider, ref token))
            {
                if (!ImpersonateLoggedOnUser(token))
                {
                    errorCode = Marshal.GetLastWin32Error();
                }

                CloseHandle(token);
            }
            else
            {
                errorCode = Marshal.GetLastWin32Error();
            }

            if (errorCode != 0)
            {
                throw new ImersonationFailureException(string.Format("Impersonation failed with Win32 error code 0x{0:X}", errorCode));
            }

        }

        ~ImpersonationHelper()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                RevertToSelf();

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
}
