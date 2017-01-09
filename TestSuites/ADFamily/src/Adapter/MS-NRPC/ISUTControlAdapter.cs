// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.ActiveDirectory.Nrpc
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    using Microsoft.Protocols.TestTools;

    /// <summary>
    /// This interface is a SUT Control Adapter interface.
    /// </summary>
    public interface INrpcServerSutControlAdapter : IAdapter
    {
        /// <summary>
        /// Restart the Netlogon service on the Endpoint and TDC.
        /// </summary>
        [MethodHelp(@"This method is used to restart the Netlogon service on the Endpoint and TDC")]
        void RestartNetlogonService(string endpointName, string tdcName);


        /// <summary>
        /// Stop the Netlogon service on the PDC.
        /// </summary>
        [MethodHelp(@"This method is used to stop the Netlogon service on the PDC")]
        void StopNetlogonService();


        /// <summary>
        /// Start the Netlogon service on the PDC.
        /// </summary>
        [MethodHelp(@"This method is used to start the Netlogon service on the PDC")]
        void StartNetlogonService();


        /// <summary>
        /// Pause the Netlogon service on the PDC or TDC (Trust DC).
        /// </summary>
        /// <param name="sutType">The computer to be set.</param>
        [MethodHelp(@"This method is used to pause the Netlogon service on the PDC or TDC")]
        void PauseNetlogonService(ComputerType sutType);


        /// <summary>
        /// Resume the Netlogon service on the PDC or TDC.
        /// </summary>
        /// <param name="sutType">The computer to be set.</param>
        [MethodHelp(@"This method is used to resume the Netlogon service on the PDC or TDC")]
        void ResumeNetlogonService(ComputerType sutType);


        /// <summary>
        /// Restore the Netlogon service on the PDC and TDC.
        /// </summary>
        [MethodHelp(@"This method is used to restore the Netlogon service on the PDC and TDC")]
        void RestoreNetlogonService(string pdcName, string tdcName);


        /// <summary>
        /// Configures the server.
        /// </summary>
        /// <param name="allowDes">
        /// Whether the server MUST reject incoming clients using DES encryption in ECB mode.
        /// </param>
        /// <param name="refusePasswordChange">whether the server refuses client password changes.</param>
        [MethodHelp(@"This method is used to configure the ADM 'AllowDes' & 'RefusePasswordChange' on the PDC")]
        void ConfigServer(bool allowDes, bool refusePasswordChange);


        /// <summary>
        ///  Configures Server for RejectMD5Client.
        /// </summary>
        /// <param name="rejectMD5Clients">
        ///  whether the server rejects incoming clients that are using MD5 encryption.
        /// </param>
        [MethodHelp(@"This method is used to configure the 'RejectMD5Client' on PDC")]
        void ConfigServerRejectMD5Client(bool rejectMD5Clients);

        /// <summary>
        /// Enable or Disable the non-DC computer account.
        /// </summary>
        /// <param name="isEnable">If true enable the computer account, otherwise disable the computer account.</param>
        [MethodHelp(@"This method is used to enable or disable the non-DC computer account on the PDC")]
        void ChangeNonDCMachineAccountStatus(bool isEnable);


        /// <summary>
        /// This method is used to get the OperatingSystem attribute of the client machine object.
        /// </summary>
        /// <returns>OperatingSystem attribute value.</returns>
        ///Disable CA1024 because the method in ISutControlAdapter could not be a property. 
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [MethodHelp(@"This method is used to get the OperatingSystem attribute of the client machine object")]
        string GetOperatingSystemAttribute();


        /// <summary>
        /// This method is used to get the LastLogon attribute of the Administrator 'users object on the PDC.
        /// </summary>
        /// <returns>LastLogon attribute value.</returns>
        ///Disable CA1024 because the method in ISutControlAdapter could not be a property. 
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [MethodHelp(@"This method is used to get the LastLogon attribute 
                    of the Administrator 'users object on the PDC")]
        long GetLastLogonAttribute();


        /// <summary>
        /// This method is used to get the PwdLastSet attribute of the Administrator 'users object on the PDC.
        /// </summary>
        /// <returns>PwdLastSet attribute value.</returns>
        ///Disable CA1024 because the method in ISutControlAdapter could not be a property. 
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [MethodHelp(@"This method is used to get the PwdLastSet attribute 
                    of the Administrator 'users object on the PDC")]
        long GetPwdLastSetAttribute(string username, string password);


        /// <summary>
        /// This method is used to get the SamAccountName attribute of the Administrator 'users object on the PDC
        /// </summary>
        /// <returns>SamAccountName attribute value</returns>
        ///Disable CA1024 because the method in ISutControlAdapter could not be a property. 
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [MethodHelp(@"This method is used to get the SamAccountName attribute 
                    of the Administrator 'users object on the PDC")]
        string GetSamAccountNameAttribute();  


        /// <summary>
        /// This method is used to get the LogonCount attribute of the Administrator 'users object on the PDC
        /// </summary>
        /// <returns>LogonCount attribute value</returns>
        ///Disable CA1024 because the method in ISutControlAdapter could not be a property. 
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [MethodHelp(@"This method is used to get the LogonCount attribute 
                    of the Administrator 'users object on the PDC")]
        string GetLogonCountAttribute();


        /// <summary>
        /// This method is used to get the BadPwdCount attribute of the Administrator 'users object on the PDC
        /// </summary>
        /// <returns>BadPwdCount attribute value</returns>
        ///Disable CA1024 because the method in ISutControlAdapter could not be a property. 
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [MethodHelp(@"This method is used to get the BadPwdCount attribute 
                    of the Administrator 'users object on the PDC")]
        string GetBadPwdCountAttribute();


        /// <summary>
        /// This method is used to get the ServicePrincipalName attribute of the client machine object.
        /// </summary>
        /// <returns>ServicePrincipalName attribute value</returns>
        ///Disable CA1024 because the method in ISutControlAdapter could not be a property. 
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [MethodHelp(@"This method is used to get the ServicePrincipalName attribute of the client machine object.")]
        string GetServicePrincipalNameAttribute();


        /// <summary>
        /// This method is used to get the attribute of the DnsHostName which belong to client machine object.
        /// </summary>
        /// <returns>DnsHostName attribute value.</returns>
        ///Disable CA1024 because the method in ISutControlAdapter could not be a property. 
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [MethodHelp(@"This method is used to get the attribute 
                    of the DnsHostName which belong to client machine object")]
        string GetDnsHostNameAttributeOfClient();

        /// <summary>
        /// This method is used to set the attribute of the DnsHostName which belong to client machine object.
        /// </summary>
        /// <param name="dnsHostName">DnsHostName attribute value.</param>
        /// <returns>Return true is success,; otherwise, return false.</returns>
        ///Disable CA1024 because the method in ISutControlAdapter could not be a property. 
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [MethodHelp(@"This method is used to get the attribute 
                    of the DnsHostName which belong to client machine object")]
        bool SetDnsHostNameAttributeOfClient(string dnsHostName);

        /// <summary>
        /// This method is used to get the ProfilePath attribute of the Administrator 'users object on the PDC.
        /// </summary>
        /// <returns>ProfilePath attribute value.</returns>
        ///Disable CA1024 because the method in ISutControlAdapter could not be a property. 
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [MethodHelp(@"This method is used to get the ProfilePath attribute 
                    of the Administrator 'users object on the PDC")]
        string GetProfilePathAttribute();


        /// <summary>
        /// This method is used to get the HomeDirectory attribute of the Administrator 'users object on the PDC.
        /// </summary>
        /// <returns>HomeDirectory attribute value.</returns>
        ///Disable CA1024 because the method in ISutControlAdapter could not be a property. 
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [MethodHelp(@"This method is used to get the HomeDirectory attribute 
                    of the Administrator 'users object on the PDC")]
        string GetHomeDirectoryAttribute();


        /// <summary>
        /// This method is used to get the ScriptPath attribute of the Administrator 'users object on the PDC.
        /// </summary>
        /// <returns>ScriptPath attribute value.</returns>
        ///Disable CA1024 because the method in ISutControlAdapter could not be a property. 
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [MethodHelp(@"This method is used to get the ScriptPath attribute 
                    of the Administrator 'users object on the PDC")]
        string GetScriptPathAttribute();


        /// <summary>
        /// This method is used to get the HomeDrive attribute of the Administrator 'users object on the PDC.
        /// </summary>
        /// <returns>HomeDrive attribute value.</returns>
        ///Disable CA1024 because the method in ISutControlAdapter could not be a property. 
        [SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        [MethodHelp(@"This method is used to get the HomeDrive attribute 
                    of the Administrator 'users object on the PDC")]
        string GetHomeDriveAttribute();
    }
}
