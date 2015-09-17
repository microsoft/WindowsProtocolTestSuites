// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Text;
using System.Management;
using System.Collections.Generic;
using System.Globalization;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// Wrapper the information of remote commandLine call
    /// </summary>
    public class RemoteCommandLine : IDisposable
    {
        private ManagementClass classInstance;
        private string serverFullName;
        private Dictionary<uint, string> processIdAndFileMap;
        internal string remoteLogPath;
        private bool isCreateShareFolder;

        /// <summary>
        /// the constructor of class CommandRequest.
        /// </summary>
        /// <param name="serverName">server full Name prepared to connect</param>
        /// <param name="remoteFolderPath">the folder going to create in remote server, 
        /// if the path already exist in the remote machine, this method fails </param>
        public RemoteCommandLine(string serverName, string remoteFolderPath)
        {
            processIdAndFileMap = new Dictionary<uint, string>();
            serverFullName = serverName;
            remoteLogPath = remoteFolderPath;
        }

        #region IDisposable Members

        private bool disposed;


        /// <summary>
        /// Release all resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        /// <summary>
        /// Release all resources
        /// </summary>
        /// <param name="disposing">Indicates user or GC calling this function</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Delete all the files under remoteLogPath.
                    classInstance.InvokeMethod("Create", new object[] { "cmd /c del/q " + remoteLogPath });

                    classInstance.Dispose();
                }

                //Note disposing has been done.
                this.disposed = true;
            }
        }

        #endregion
        
        /// <summary>
        /// //// Run a command on the remote machine in synchronous mode.
        /// </summary>
        /// <param name="args">The command to be executed. For example, 
        /// to run an exe, the commandLine is like “c:\test.exe –a –b”;</param>
        /// <param name="outputResult">The standard and error output text of the command. </param>
        /// <returns>Return the result of the command. 0 means successful.</returns>
        public uint SyncExecute(string args, out string outputResult)
        {
            uint returnValue = 1;
            if (isCreateShareFolder == false)
            {
                Connect(null, null);
            }
            UInt32 processId;
            string fileName = RandomString(20) + ".txt";
            string commandLine = "cmd /c " + args + " 2> " + remoteLogPath + "\\" + fileName + " 1>&2";

            ManagementBaseObject inParams = classInstance.GetMethodParameters("Create");
            inParams["CommandLine"] = commandLine;
            InvokeMethodOptions methodOption = new InvokeMethodOptions(null, System.TimeSpan.MaxValue);
            ManagementBaseObject outParams = classInstance.InvokeMethod("Create", inParams, methodOption);
            processId = (UInt32)outParams["ProcessId"];
            processIdAndFileMap.Add(processId, fileName);

            // wating until this process ends, then take out the result,and return success.
            while (GetRunningResult(processId, out outputResult) != 0) ;
            returnValue = 0;
            return returnValue;
        }

        /// <summary>
        /// Run a command on the remote machine in asynchronous mode. After execute this request, use
        /// the method GetReturnValue to examine if the command has completed.
        /// </summary>
        /// <param name="args">The command to be executed. For example, 
        /// to run an exe, the commandLine is like “c:\test.exe –a –b”</param>
        /// <returns>The processId of the process.if the execute fails, return processId=0</returns>
        public uint AsynExecute(string args)
        {
            UInt32 processId;
            if (isCreateShareFolder == false)
            {
                Connect(null, null);
            }
            else
            {
                processId = 0;
            }

            string fileName = RandomString(20) + ".txt";
            string commandLine = "cmd /c " + args + " 2> " + remoteLogPath + "\\" + fileName + " 1>&2";

            ManagementBaseObject inParams = classInstance.GetMethodParameters("Create");
            inParams["CommandLine"] = commandLine;
            InvokeMethodOptions methodOption = new InvokeMethodOptions(null, System.TimeSpan.MaxValue);

            ManagementBaseObject outParams = classInstance.InvokeMethod("Create", inParams, methodOption);
            processId = (UInt32)outParams["ProcessId"];
            processIdAndFileMap.Add(processId, fileName);

            return (uint)processId;
        }



        /// <summary>
        /// Get the result of the previous remote execution. This method is used after AsynExecute.
        /// </summary>
        /// <param name="processId">the processId the previous command returned</param>
        /// <param name="outputResult">the output result file</param>
        /// <returns>
        /// return 0 means the process has been ended 
        /// return 1 means the process is not end.
        /// return 2 means the given processId never exists 
        /// </returns>
        public uint GetRunningResult(uint processId, out string outputResult)
        {
            uint result = 1;
            string fileName;
            outputResult = "";

            if (false == processIdAndFileMap.TryGetValue(processId, out fileName))
            {
                // cannot find the processId in map
                result = 2;
                return result;
            }
            else
            {
                ObjectQuery theQuery = new ObjectQuery("SELECT * FROM Win32_Process WHERE processId='" + processId + "'");
                ManagementObjectSearcher theSearcher = new ManagementObjectSearcher(classInstance.Scope, theQuery);
                ManagementObjectCollection theCollection = theSearcher.Get();
                // if theCollection.Count == 0, means th process ended.
                if (theCollection.Count == 0)
                {
                    outputResult = ReadFile(fileName);
                    classInstance.InvokeMethod("Create", new object[] { "cmd /c del/q" + remoteLogPath + "\\" + fileName });
                    processIdAndFileMap.Remove(processId);
                    result = 0;
                }
                return result;
            }
        }


        /// <summary>
        /// Connect the remote machine with the given user account.
        /// </summary>        
        /// <param name="logonUser">The authenticated account which has administrative access of the remote machine.</param>
        /// <param name="logonPassword">The password of the logonUser when logon the remote machine.</param>
        public void Connect(string logonUser, string logonPassword)
        {
            // get the machine name from serverFullName
            string serverHostName = serverFullName.Substring(0, serverFullName.IndexOf(".", StringComparison.OrdinalIgnoreCase));
            string domain = serverFullName.Substring(serverFullName.IndexOf(".", StringComparison.OrdinalIgnoreCase) + 1, serverFullName.Length - serverHostName.Length - 1);
            ConnectionOptions connOP = new ConnectionOptions();
            connOP.Username = logonUser;
            connOP.Password = logonPassword;
            connOP.Impersonation = ImpersonationLevel.Delegate;
            connOP.Authority = "Kerberos:" + domain + "\\" + serverHostName;
            connOP.EnablePrivileges = true;
            connOP.Authentication = AuthenticationLevel.PacketPrivacy;
            ManagementPath mngPath = new ManagementPath(@"\\" + serverHostName + @"\root\cimv2:Win32_Process");
            ManagementScope scope = new ManagementScope(mngPath, connOP);
            scope.Connect();
            classInstance = new ManagementClass(scope, mngPath, null);
            //create the share folder after connect success.
            CreateRemoteLogFolder();
            isCreateShareFolder = true;
        }


        /// <summary>
        /// The method helps to read the temp file in remote machine
        /// </summary>
        /// <param name="fileName">the temp file prepared to read</param>
        /// <returns>the outputresult string</returns>
        private string ReadFile(string fileName)
        {
            string[] dir = remoteLogPath.Split("\\".ToCharArray());
            string sharePath = "";
            string output;
            for (int i = 1; i < dir.Length; i++)
            {
                sharePath = sharePath + "\\" + dir[i];
            }
            output = File.ReadAllText(@"\\" + serverFullName + sharePath + "\\" + fileName);
            return output;
        }

        /// <summary>
        /// Create a share folder in remote computer with the name user sets
        /// if the path is already exited and the folder is not shared, this method share that folder user gives.
        /// if the path is already exited and the folder already shared, this method use the existing folder as share folder.
        /// if the path given is illegal, or the disk was not exist, the method throw a exception.
        /// </summary>
        private void CreateRemoteLogFolder()
        {
            object[] cmdline = { "cmd /c md " + remoteLogPath };
            classInstance.InvokeMethod("Create", cmdline);
            ManagementPath mngPath = new ManagementPath(@"\\" + serverFullName + @"\root\cimv2:Win32_Share");
            ManagementClass managementClass = new ManagementClass(classInstance.Scope, mngPath, null);
            ManagementBaseObject inParams = managementClass.GetMethodParameters("Create");
            ManagementBaseObject outParams;
            string[] dir = remoteLogPath.Split("\\".ToCharArray());
            // Set the input parameters 
            string remoteFolderName = dir[1];
            string sharePath = dir[0] + "\\" + dir[1];
            inParams["Description"] = remoteFolderName;
            inParams["Name"] = remoteFolderName;
            inParams["Path"] = sharePath;
            inParams["Type"] = 0x0; // Disk Drive 
            // Invoke the method on the ManagementClass object
            outParams = managementClass.InvokeMethod("Create", inParams, null);
            // Check to see if the method invocation was successful 0 means the foler alreay exits, 22 means the foler is already shared
            //if the path given is illegal, or the disk was not exist, the method throw an exception.
            int returnValue = Convert.ToInt32(outParams.Properties["ReturnValue"].Value, CultureInfo.InvariantCulture);
            if (returnValue != 0 && returnValue != 22)
            {
                throw new StackException("Unable to share directory.");
            }
        }


        /// <summary>
        /// Generates a random string with the given length
        /// </summary>
        /// <param name="size">Size of the string</param>       
        /// <returns>Random string</returns>
        private string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            return builder.ToString();
        }
    }
}