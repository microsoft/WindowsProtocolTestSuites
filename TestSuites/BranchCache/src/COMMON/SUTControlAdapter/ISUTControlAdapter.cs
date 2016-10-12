// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.BranchCache
{
    using Microsoft.Protocols.TestTools;

    /// <summary>
    /// Interface of SUTControlAdapter
    /// </summary>
    public interface ISUTControlAdapter : IAdapter
    {
        /// <summary>
        /// Clear Branch Cache buffers.
        /// </summary>
        /// <param name="computerName"> The computer name of the hosted cache server.</param>
        /// <param name="user"> The user name used to communicate with the remote computer.</param>
        /// <param name="password"> The password for the user parameter.</param>
        /// <returns> Returns "true" if success; otherwise, returns "false".</returns>
        [MethodHelp("This method is used to clear Branch Cache buffers.")]
        bool ClearBranchCacheBuffer(string computerName, string user, string password);

        /// <summary>
        /// Clear the buffer of the application which uses Branch Cache.
        /// </summary>
        /// <param name="computerName"> The remote computer name.</param>
        /// <param name="user"> The user name.</param>
        /// <param name="password"> The password.</param>
        /// <returns> Returns "true" if success; otherwise, returns "false".</returns>
        [MethodHelp("This method is used to clear the buffer of the application which uses branch cache.")]
        bool ClearApplicationBuffer(string computerName, string user, string password);

        /// <summary>
        /// Set the request timer.
        /// </summary>
        /// <param name="computerName"> The remote computer name.</param>
        /// <param name="user"> The user name.</param>
        /// <param name="password"> The password.</param>
        /// <param name="timerInMilliseconds"> The value.</param>
        /// <returns> Returns "true" if success; otherwise, returns "false".</returns>
        [MethodHelp("This method is used to set the request timer on the remote computer.")]
        bool SetRequestTimer(string computerName, string user, string password, string timerInMilliseconds);

        /// <summary>
        /// Used to set the backoff timer.
        /// </summary>
        /// <param name="computerName"> The remote computer name.</param>
        /// <param name="user"> The user name.</param>
        /// <param name="password"> The password.</param>
        /// <param name="timerInMilliseconds"> The value.</param>
        /// <returns> Returns "true" if success; otherwise, returns "false".</returns>
        [MethodHelp("This method is used to set the backoff timer on the remote computer.")]
        bool SetBackoffTimer(string computerName, string user, string password, string timerInMilliseconds);

        /// <summary>
        /// Trigger the remote peer to get content cache metadata from the content server.
        /// </summary>
        /// <param name="peerServerName"> The computer name of the remote peer.</param>
        /// <param name="contentServerName"> The computer name of the content server.</param>
        /// <param name="fileName"> The file name of the content to request.</param>
        /// <param name="user"> The user name used to communicate with the remote peer.</param>
        /// <param name="password"> The password for the user parameter.</param>
        /// <returns> Returns "true" if success; otherwise, returns "false".</returns>
        [MethodHelp("This method is used to trigger the remote peer to get content cache metadata from the content server.")]
        bool TriggerPeerGetContentCache(string peerServerName, string contentServerName, string fileName, string user, string password);

        /// <summary>
        /// Restart a Branch Cache service running on the remote computer.
        /// </summary>
        /// <param name="remoteComputerName"> The computer name of the remote computer.</param>
        /// <param name="serviceName"> The name of the service running on the remote computer.</param>
        /// <param name="user"> The user account used to communicated with the remote computer.</param>
        /// <param name="password"> The password for the user paramter.</param>
        /// <returns> Returns "true" if success; otherwise, returns "false".</returns>
        [MethodHelp("This method is used to restart a service running on the remote computer.")]
        bool RestartService(string remoteComputerName, string serviceName, string user, string password);

        /// <summary>
        /// Start a Branch Cache service running on the remote computer.
        /// </summary>
        /// <param name="computerName"> The computer name of the specified computer.</param>
        /// <param name="serviceName">The name of the service running on the specified computer.</param>
        /// <param name="user"> The user account used to communicated with the specified computer.</param>
        /// <param name="password"> The password for the user paramter.</param>
        /// <returns> Returns "true" if success; otherwise, returns "false".</returns>
        [MethodHelp("This method is used to start a service running on the remote computer.")]
        bool StartService(string computerName, string serviceName, string user, string password);

        /// <summary>
        /// Stop a Branch Cache service running on the remote computer.
        /// </summary>
        /// <param name="computerName"> The computer name of the specified computer.</param>
        /// <param name="serviceName">The name of the service running on the specified computer.</param>
        /// <param name="user"> The user account used to communicated with the specified computer.</param>
        /// <param name="password"> The password for the user paramter.</param>
        /// <returns> Returns "true" if success; otherwise, returns "false".</returns>
        [MethodHelp("This method is used to stop a service running on the remote computer.")]
        bool StopService(string computerName, string serviceName, string user, string password);

        /// <summary>
        /// Get the status of the execution of hash algorithms to check if the content server has finished hash computation.
        /// </summary>
        /// <param name="sutName">Indicates the name of SUT.</param>
        /// <param name="userName">Indicates the user name to logon SUT. </param>
        /// <param name="password">Indicates the password of user to logon SUT.</param>
        /// <param name="sutScriptPath">Indicates script path on the SUT.</param>
        [MethodHelp("Get the status of the execution of hash algorithms.")]
        bool IsHashComputationFinished(
            string sutName,
            string userName,
            string password,
            string sutScriptPath);

        /// <summary>
        /// Set the Hosted Cache Authentication.
        /// </summary>
        /// <param name="isAuthentication">Is enable the authentication in hosted cache or not</param>
        /// <param name="sutComputer">The hosted cache computer name</param>
        /// <param name="userName">The userName of hosted Cache</param>
        /// <param name="password">The PassWord of hosted Cache</param>
        [MethodHelp("Set the hosted cache clientauthentication enabled or not.")]
        void SetHostedCacheAuthentication(
            bool isAuthentication,
            string sutComputer,
            string userName,
            string password);

        /// <summary>
        /// Used to set the request port.
        /// </summary>
        /// <param name="computerName"> The remote computer name.</param>
        /// <param name="user"> The user account used to communicated with the remote computer.</param>
        /// <param name="password"> The password for the user paramter.</param>
        /// <param name="port"> The value of port.</param>
        /// <returns> Returns "true" if success; otherwise, returns "false".</returns>
        [MethodHelp("This method is used to set the request port on the remote computer.")]
        bool SetRequestPort(string computerName, string user, string password, string port);

        /// <summary>
        /// Trigger peer client to send MSG_GETBLKLIST request.
        /// </summary>
        /// <param name="SUTName">The computer name of the remote computer.</param>
        /// <param name="ContentComputerName">The computer name of the content server.</param>
        /// <param name="path">The path of the content file.</param>
        /// <param name="usrInVM">The user account used to communicated with the remote computer.</param>
        /// <param name="pwdInVM">The password for the user paramter.</param>
        [MethodHelp("Trigger peer client send MSGGETBLKLIST.")]
        void TriggerPeerClientSendMSGGETBLKLIST(string SUTName, string ContentComputerName, string path, string usrInVM, string pwdInVM);

        /// <summary>
        /// End task and restart the Branch Cache service.
        /// </summary>
        /// <param name="SUTName">The computer name of the remote computer.</param>
        /// <param name="Service">The service name.</param>
        /// <param name="Processname">The process name.</param>
        /// <param name="usrInVM">The user account used to communicated with the remote computer.</param>
        /// <param name="pwdInVM">The password for the user paramter.</param>
        /// <param name="path">The path of the content file.</param>
        [MethodHelp("End Task and restart the service.")]
        void EndTaskandRestartService(string SUTName, string Service, string Processname, string usrInVM, string pwdInVM, string path);

        /// <summary>
        /// Modify hosted cache protocol port.
        /// </summary>
        /// <param name="hostedClientComputerName">The hosted cache client computer name.</param>
        /// <param name="hostedServerComputerName">The hosted cache server computer name.</param>
        /// <param name="newPort">The new port.</param>
        [MethodHelp("Modify hosted cache protocol port.")]
        void ModifyHostedCacheProtocolPort(string hostedClientComputerName, string hostedServerComputerName, string newPort);
    }
}
