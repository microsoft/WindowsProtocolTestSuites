// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;

namespace Microsoft.Protocols.TestSuites.FileSharing.ServerFailover.Adapter
{
    public interface ISutControlAdapter : IAdapter
    {

        #region Non-Windows only, for FileServer Failover

        /// <summary>
        /// Disable the endpoint with specified IP address to trigger failover.
        /// It's used in FileServerFailover cases for Non-Windows
        /// </summary>
        /// <param name="IPAddr">IP address which to disable</param>
        [MethodHelp("Disable the endpoint with specified IP address to trigger failover.\r\nPlease click Continue after failover is finished successfully.")]
        void TriggerFailover(string ipAddress);

        /// <summary>
        /// Restore all endpoints to initial state
        /// It's used in FileServerFailover cases for Non-Windows
        /// </summary>
        [MethodHelp("Restore all endpoints to initial state. \r\nPlease press Continue after all endpoints are back to initial state.")]
        void RestoreToInitialState();

        #endregion

        #region Windows only
        /// <summary>
        /// Get status string of cluster service on a specific cluster node
        /// It's a Windows behavior. Non-windows implementation does not need to implement this method.
        /// </summary>
        /// <param name="nodeName">Name of the cluster node</param>
        /// <returns>
        /// Status string of cluster service on a specific cluster node
        /// Return "Running" when the service is running, otherwise return "NotRunning"
        /// </returns>
        [MethodHelp("Get the status of the cluster service on a specific cluster node. \r\nThe return value should be Running.\r\nNote: If it's not running right now, wait and then fill in Running when it becomes running.")]
        string GetClusterNodeStatus(string nodeName);
        #endregion

        /// <summary>
        /// Enable a specific cluster node by rebooting the specified node
        /// It's used in SWN cases for both Windows and Non-Windows
        /// It's used in FileServerFailover cases for Windows
        /// </summary>
        /// <param name="nodeName">Computer name or IP address of the cluster node</param>
        /// <returns>Return true if success, otherwise return false.</returns>
        [MethodHelp("Enable a specific cluster node. \r\nIn Windows, it can be enabled by reboot the specified node\r\nThe return True if the node is rebooted, otherwise False.")]
        bool EnableClusterNode(string nodeName);

        /// <summary>
        /// Disable a specific cluster node by stopping cluster service
        /// It's used in SWN cases for Windows or Non-Windows
        /// It's used in FileServerFailover cases for Windows
        /// </summary>
        /// <param name="nodeName">Computer name or IP address of the cluster node</param>
        /// <returns>Return true if success, otherwise return false.</returns>
        [MethodHelp("Disable a specific cluster node. \r\nIn Windows, it can be disabled by stopping the cluster service: ClusSvc\r\nThe return True if the service is refreshed, otherwise False.")]
        bool DisableClusterNode(string nodeName);

        /// <summary> 
        /// Get name of owner node for a specific cluster resource, such as file server owner, cluster owner
        /// It's used in SWN/FSRVP cases for both Windows and Non-Windows.
        /// It's used in FileServer Failover cases for Windows.
        /// </summary>
        /// <param name="resName">Name of cluster resource</param>
        /// <returns>Name/IP of owner node for the cluster resource</returns>
        [MethodHelp("Get the owner node name for a specific cluster resource. \r\nThe return value is the name of owner node for the specified cluster resource")]
        string GetClusterResourceOwner(string resName);

        /// <summary>
        /// Notify the witness client that the file server will be moved to the new node.
        /// It's used in SWN cases for both Windows and Non-Windows
        /// </summary>
        /// <param name="clientName">Name of the SWN client</param>
        /// <param name="nodeName">Name of the node to be moved</param>
        [MethodHelp("Notify the witness client that the file server will be moved to the new node.")]
        void MoveSmbWitnessClient(string clientName, string nodeName);

        /// <summary>
        /// Move the cluster resource to a specific node. 
        /// It's used in SWN cases for both Windows and Non-Windows
        /// </summary>
        /// <param name="resName">Name of cluster resource</param>
        /// <param name="nodeName">Name of the node to be moved</param>
        /// <returns>Return true if success, otherwise return false.</returns>
        [MethodHelp("Move the cluster resource to a specific node. \r\nThe return the result with True if the move success, otherwise False")]
        bool MoveClusterResourceOwner(string resName, string nodeName);

        /// <summary>
        /// Refresh Net adapter according to IP address. 
        /// It's used in SWN cases for both Windows and Non-Windows
        /// </summary>
        /// <param name="IPAddress">IP address</param>
        /// <param name="nodeName">Node name</param>
        [MethodHelp("Refresh Net adapter according to IP address. \r\n")]
        void RefreshNetAdapter(string IPAddress, string nodeName);

        /// <summary>
        /// Flush DNS of the driver computer
        /// </summary>
        [MethodHelp("Flush DNS of the driver computer\r\n")]
        void FlushDNS();
    }
}
