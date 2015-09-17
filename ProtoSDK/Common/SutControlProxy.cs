// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.ServiceModel;
using System.IO;
using System.ComponentModel;
using System.Globalization;

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// This class acts as an proxy to control a remote SUT.
    /// It provides methods to start a process on the SUT, in both sync and async way.
    /// It also provides methods to query or kill an SUT process.
    /// Note: 
    /// 1. To make this proxy work, an SUT control service must has been
    /// installed and started on the target remote SUT machine.
    /// 2. No authentication is needed to control the remote SUT.
    /// Any application on any machine that knows this service can manipulate
    /// any process on the controlled machine. This unsafe service is designed
    /// only for protocol test use, restricted in an isolated private network.
    /// </summary>
    public class SutControlProxy
    {
        #region constant variables

        /// <summary>
        /// SUT control service address has WCF net.tcp binding format:
        /// "net.tcp://{address}:{port}/SutControlService/", for example
        /// "net.tcp://sut01:8000/SutControlService/".
        /// configured in address attribute of endpoint element in
        /// SUT control service's App.config file.
        /// </summary>
        private const string ServiceAddressFormat = "net.tcp://{0}:{1}/SutControlService/";

        /// <summary>
        /// 8000 is the default value of SUT control service port,
        /// configured in address attribute of endpoint element in
        /// SUT control service's App.config file.
        /// </summary>
        private const uint DefaultServicePort = 8000;

        #endregion

        #region private variables

        /// <summary>
        /// The SUT control service's local stub.
        /// </summary>
        private ISutControlService service;

        #endregion

        #region constructors

        /// <summary>
        /// Constructor of SutControlProxy with default port 8000.
        /// </summary>
        /// <param name="sutAddress">the address of the SUT,
        /// can be either IP address or computer name.
        /// </param>
        /// <exception cref="ArgumentNullException">sutAddress is null</exception>
        public SutControlProxy(string sutAddress)
            : this(sutAddress, DefaultServicePort)
        {
        }


        /// <summary>
        /// Constructor of SutControlProxy.
        /// </summary>
        /// <param name="sutAddress">the address of the SUT,
        /// can be either IP address or computer name.
        /// </param>
        /// <param name="port">the port on SUT machine to provide SUT control service.</param>
        /// <exception cref="ArgumentNullException">sutAddress is null</exception>
        public SutControlProxy(string sutAddress, uint port)
        {
            AssertIsNotNull("sutAddress", sutAddress);

            InitializeLogger();

            #region logging
            Info("Construct SutControlProxy, address={0}, port={1}.", sutAddress, port);
            #endregion

            // SUT control service address
            string sutControlServiceAddress = string.Format(CultureInfo.InvariantCulture, ServiceAddressFormat, sutAddress, port);
            EndpointAddress endpointAddress = new EndpointAddress(sutControlServiceAddress);

            // channel factory
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            ChannelFactory<ISutControlService> factory = new ChannelFactory<ISutControlService>(binding);

            // create service channel
            service = factory.CreateChannel(endpointAddress);
        }


        /// <summary>
        /// Constructor of SutControlProxy, used in unit test
        /// or in case caller has obtained an ISutControlService instance.
        /// </summary>
        /// <param name="service">service that implemented ISutControlService interface.</param>
        /// <exception cref="ArgumentNullException">service is null</exception>
        public SutControlProxy(ISutControlService service)
        {
            InitializeLogger();

            #region assert
            AssertIsNotNull("service", service);
            #endregion

            this.service = service;
        }


        #endregion

        #region public methods

        /// <summary>
        /// Start a process on SUT.
        /// This method executes asynchronously. After calling this method,
        /// please wait and call <seealso cref="GetExecutionResults"/> method to check the status.
        /// </summary>
        /// <param name="command">the process's filename to start with.
        /// The filename must be of executable file type.
        /// The filename can use an absolute path, or relative path,
        /// or without path if the path has been defined in SUT's "PATH" environment variable.
        /// In any of above case, the ".exe" extension can be ignored.
        /// <example>
        /// "c:\windows\system32\cmd.exe";
        /// "cmd.exe";
        /// "cmd";
        /// "..\myApp.exe"
        /// "..\myApp"
        /// </example>
        /// </param>
        /// <param name="arguments">process specified command-line arguments</param>
        /// <returns>processId, the unique identifier for the process</returns>
        /// <exception cref="ArgumentNullException">command is null</exception>
        /// <exception cref="FileNotFoundException">command, as a file name, is not found on SUT machine.</exception>
        public int StartProcess(
            string command,
            string arguments)
        {
            return StartProcessInternal(command, arguments, null, null, null);
        }


        /// <summary>
        /// Start a process on SUT using specified domain\userName password.
        /// This method executes asynchronously. After calling this method,
        /// please wait and call <seealso cref="GetExecutionResults"/> method to check the status.
        /// </summary>
        /// <param name="command">the process's filename to start with.
        /// The filename must be of executable file type.
        /// The filename can use an absolute path, or relative path,
        /// or without path if the path has been defined in SUT's "PATH" environment variable.
        /// In any of above case, the ".exe" extension can be ignored.
        /// <example>
        /// "c:\windows\system32\cmd.exe";
        /// "cmd.exe";
        /// "cmd";
        /// "..\myApp.exe"
        /// "..\myApp"
        /// </example>
        /// </param>
        /// <param name="arguments">process specified command-line arguments</param>
        /// <param name="userName">user name to start process with.</param>
        /// <param name="password">password to start process with.</param>
        /// <param name="domain">domain to start process with.</param>
        /// <returns>processId, the unique identifier for the process</returns>
        /// <exception cref="ArgumentNullException">
        /// command is null, or userName is null, or password is null, or domain is null.
        /// </exception>
        /// <exception cref="FileNotFoundException">command, as a file name, is not found on SUT machine.</exception>
        public int StartProcess(
            string command,
            string arguments,
            string userName,
            string password,
            string domain)
        {
            ValidateCredential(userName, password, domain);

            return StartProcessInternal(command, arguments, userName, password, domain);
        }


        /// <summary>
        /// Start a process on SUT, wait for the process exit and return its results.
        /// This method executes synchronously, blocks current thread till process exits or timeouts.
        /// </summary>
        /// <param name="command">the process's filename to start with.
        /// The filename must be of executable file type.
        /// The filename can use an absolute path, or relative path,
        /// or without path if the path has been defined in SUT's "PATH" environment variable.
        /// In any of above case, the ".exe" extension can be ignored.
        /// <example>
        /// "c:\windows\system32\cmd.exe";
        /// "cmd.exe";
        /// "cmd";
        /// "..\myApp.exe"
        /// "..\myApp"
        /// </example>
        /// </param>
        /// <param name="arguments">process specified command-line arguments</param>
        /// <param name="timeout">if the process doesn't exit after waiting specified time span,
        /// a TimeoutException is thrown.
        /// timeout should either be 
        /// (1) TimeSpan.MaxValue which presents infinity, or
        /// (2) its total milliseconds is greater than 0 and less than or equals to Int32.MaxValue. </param>
        /// <param name="exitCode">exit code of the process</param>
        /// <param name="standardOutput">
        /// content that the process generates to the standard output stream.
        /// Empty string returns if no content generated.</param>
        /// <param name="standardError">
        /// content that the process generates to the standard error stream.
        /// Empty string returns if no content generated.</param>
        /// <returns>processId, the unique identifier for the process</returns>
        /// <exception cref="ArgumentNullException">command is null</exception>
        /// <exception cref="FileNotFoundException">command, as a file name, is not found on SUT machine.</exception>
        /// <exception cref="TimeoutException">
        /// Process doesn't exit after waiting as long as timeout specified.
        /// </exception>
        public int StartProcessWaitForExit(
            string command,
            string arguments,
            TimeSpan timeout,
            out int exitCode,
            out string standardOutput,
            out string standardError)
        {
            return StartProcessWaitForExitInternal(command, arguments, null, null, null, 
                timeout, out exitCode, out standardOutput, out standardError);
        }


        /// <summary>
        /// Start a process on SUT using specified domain\userName password,
        /// wait for the process exit and return its results.
        /// This method executes synchronously, blocks current thread till process exits or timeouts.
        /// </summary>
        /// <param name="command">the process's filename to start with.
        /// The filename must be of executable file type.
        /// The filename can use an absolute path, or relative path,
        /// or without path if the path has been defined in SUT's "PATH" environment variable.
        /// In any of above case, the ".exe" extension can be ignored.
        /// <example>
        /// "c:\windows\system32\cmd.exe";
        /// "cmd.exe";
        /// "cmd";
        /// "..\myApp.exe"
        /// "..\myApp"
        /// </example>
        /// </param>
        /// <param name="arguments">process specified command-line arguments</param>
        /// <param name="userName">user name to start process with.</param>
        /// <param name="password">password to start process with.</param>
        /// <param name="domain">domain to start process with.</param>
        /// <param name="timeout">if the process doesn't exit after waiting specified time span,
        /// a TimeoutException is thrown.
        /// timeout should either be 
        /// (1) TimeSpan.MaxValue which presents infinity, or
        /// (2) its total milliseconds is greater than 0 and less than or equals to Int32.MaxValue. </param>
        /// <param name="exitCode">exit code of the process</param>
        /// <param name="standardOutput">
        /// content that the process generates to the standard output stream.
        /// Empty string returns if no content generated.</param>
        /// <param name="standardError">
        /// content that the process generates to the standard error stream.
        /// Empty string returns if no content generated.</param>
        /// <returns>processId, the unique identifier for the process</returns>
        /// <exception cref="ArgumentNullException">
        /// command is null, or userName is null, or password is null, or domain is null.
        /// </exception>
        /// <exception cref="FileNotFoundException">command, as a file name, is not found on SUT machine.</exception>
        /// <exception cref="TimeoutException">
        /// Process doesn't exit after waiting as long as timeout specified.
        /// </exception>
        public int StartProcessWaitForExit(
            string command,
            string arguments,
            string userName,
            string password,
            string domain,
            TimeSpan timeout,
            out int exitCode,
            out string standardOutput,
            out string standardError)
        {
            ValidateCredential(userName, password, domain);

            return StartProcessWaitForExitInternal(command, arguments, userName, password, domain,
                timeout, out exitCode, out standardOutput, out standardError);
        }


        /// <summary>
        /// Get execution results of the process specified by processId
        /// This method executes asynchronously.
        /// If the process has exited, this method returns true, with valid exitCode.
        /// If the process is still running, this method returns false.
        /// standardOutput and standardError will always been output
        /// as long as process outputs stream, no matter the process is running or exited.
        /// </summary>
        /// <param name="processId">the unique identifier for the process</param>
        /// <param name="exitCode">exit code of the process if process has exited.
        /// Note: if this method returns false, this parameter is assigned zero
        /// which means process has not exited, thus has not provided exit code yet.</param>
        /// <param name="standardOutput">
        /// content that the process generates to the standard output stream,
        /// since last query time.
        /// Empty string returns if no content generated.</param>
        /// <param name="standardError">
        /// content that the process generates to the standard error stream,
        /// since last query time.
        /// Empty string returns if no content generated.</param>
        /// <returns>true if process has exited, otherwise returns false.</returns>
        /// <exception cref="FaultException" >
        /// FaultException is thrown by SutControlService to indicate errors on SUT machine.
        /// It encapsulate the details in ExceptionDetail, such as exception type, message and stack trace.
        /// If you want to catch and analyze it, here's example:
        /// 
        ///    catch (FaultException faultException)
        ///    {
        ///        if (faultException.Detail.Type == typeof(InvalidOperationException).FullName)
        ///        {
        ///            
        ///            throw new InvalidOperationException();
        ///        }
        ///    }
        /// </exception>
        public bool GetExecutionResults(
            int processId,
            out int exitCode,
            out string standardOutput,
            out string standardError)
        {
            #region logging
            Info("GetExecutionResults, processId={0}", processId);
            #endregion

            return service.GetExecutionResults(processId, out exitCode, out standardOutput, out standardError);
        }

        /// <summary>
        /// Kill the process specified by processId.
        /// Perform no action if the process has already exited.
        /// This method executes synchronously, blocks current thread till process exits or timeouts.
        /// </summary>
        /// <param name="processId">the unique identifier for the process</param>
        /// <param name="timeout">if the process doesn't exit after waiting specified time span,
        /// a TimeoutException is thrown.
        /// timeout should either be 
        /// (1) TimeSpan.MaxValue which presents infinity, or
        /// (2) its total milliseconds is greater than 0 and less than or equals to Int32.MaxValue.
        /// </param>
        /// <exception cref="FaultException" >
        /// FaultException is thrown by SutControlService to indicate errors on SUT machine.
        /// It encapsulate the details in ExceptionDetail, such as exception type, message and stack trace.
        /// If you want to catch and analyze it, here's example:
        /// 
        ///    catch (FaultException faultException)
        ///    {
        ///        if (faultException.Detail.Type == typeof(InvalidOperationException).FullName)
        ///        {
        ///            
        ///            throw new InvalidOperationException();
        ///        }
        ///    }
        /// </exception>
        public void KillProcess(int processId, TimeSpan timeout)
        {
            #region logging
            Info("GetExecutionResults, processId={0}", processId);
            #endregion

            ValidateTimeout(timeout);

            service.KillProcess(processId, timeout);
        }

        /// <summary>
        /// Clear information proxy collected for process of specified processId,
        /// so the unused memory can be recycled.
        /// After calling this method, the processId is disposed and not valid to use any more.
        /// It's safe to call this method more than once.
        /// </summary>
        /// <param name="processId">the unused process Id</param>
        public void ClearProcess(int processId)
        {
            service.ClearProcess(processId);
        }

        #endregion

        #region private methods

        /// <summary>
        /// Initialize Logger used in this proxy.
        /// </summary>
        private void InitializeLogger()
        {
            // to pass FxCop, initialize logger, set configId "stack".
            //logger = new Logger(new LogConfig("SutControlProxyLog", "stack"));
        }


        /// <summary>
        /// Write Debug level log.
        /// </summary>
        /// <param name="message">message to be logged, can be "xxx {0} {1} xxx" format.</param>
        /// <param name="args">arguments formatted into message.</param>
        private void Debug(string message, params object[] args)
        {
            // remain this interface for furture use.
            // to avoid fxcop warning CA1801 (argument not used),
            // add following line:
            Console.WriteLine(message, args);
        }


        /// <summary>
        /// Write Info level log.
        /// </summary>
        /// <param name="message">message to be logged, can be "xxx {0} {1} xxx" format.</param>
        /// <param name="args">arguments formatted into message.</param>
        private void Info(string message, params object[] args)
        {
            // remain this interface for furture use.
            // to avoid fxcop warning CA1801 (argument not used),
            // add following line:
            Console.WriteLine(message, args);
        }


        /// <summary>
        /// If object is null, throw ArgumentNullException.
        /// </summary>
        /// <param name="objectName">name of the checked object.</param>
        /// <param name="value">object value to be checked.</param>
        /// <exception cref="ArgumentNullException">object value is null.</exception>
        private void AssertIsNotNull(string objectName, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(objectName);
            }
        }


        /// <summary>
        /// Validate command, assert it is not null.
        /// </summary>
        /// <param name="command">command file name</param>
        /// <exception cref="ArgumentNullException">command is null</exception>
        private void ValidateCommand(string command)
        {
            #region assert
            AssertIsNotNull("command", command);
            #endregion

            #region logging
            Info("ValidateCommand, command={0}", command);
            #endregion
        }


        /// <summary>
        /// Validate userName, password and domain, assert any of them is not null.
        /// </summary>
        /// <param name="userName">userName on SUT machine</param>
        /// <param name="password">password on SUT machine</param>
        /// <param name="domain">domain on SUT machine</param>
        /// <exception cref="ArgumentNullException">userName is null, or password is null, or domain is null.</exception>
        private void ValidateCredential(string userName, string password, string domain)
        {
            #region assert
            AssertIsNotNull("userName", userName);
            AssertIsNotNull("password", password);
            AssertIsNotNull("domain", domain);
            #endregion

            #region logging
            // sensitive information must be output only in debug.
            Debug("ValidateCredential, userName={0}, password={1}, domain={2}", userName, password, domain);
            #endregion
        }


        /// <summary>
        /// timeout should either be 
        /// (1) TimeSpan.MaxValue which presents infinity, or
        /// (2) its total milliseconds is greater than 0 and less than or equals to or equals to Int32.MaxValue.
        /// </summary>
        /// <param name="timeout">timeout waiting for process exit.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// throw if timeout is less than zero
        /// </exception>
        private void ValidateTimeout(TimeSpan timeout)
        {

            if (timeout == TimeSpan.MaxValue)
            {
                #region logging
                // TimeSpan.MaxValue is a special value which presents infinity,
                // so we don't output its exact year, month, day, hour...
                Info("ValidateTimeout timeout=TimeSpan.MaxValue");
                #endregion

                return;
            }

            #region logging
            // TimeSpan.MaxValue is a special value, meaning infinite timeout.
            // so we don't output its exact year, month, day, hour...
            Info("ValidateTimeout timeout={0}", timeout);
            #endregion

            int timeoutMillis = (int)timeout.TotalMilliseconds;

            #region assert
            if (timeoutMillis < 0)
            {
                throw new ArgumentOutOfRangeException("timeout",
                    timeout,
                    "timeout should either be"
                    + "(1) TimeSpan.MaxValue, or"
                    + "(2) its total milliseconds is greater than 0 and less than or equals to Int32.MaxValue.");
            }
            #endregion
        }


        /// <summary>
        /// Start a process on SUT using specified domain\userName password.
        /// This method executes asynchronously. After calling this method,
        /// please wait and call <seealso cref="GetExecutionResults"/> method to check the status.
        /// </summary>
        /// <param name="command">the process's filename to start with.
        /// The filename must be of executable file type.
        /// The filename can use an absolute path, or relative path,
        /// or without path if the path has been defined in SUT's "PATH" environment variable.
        /// In any of above case, the ".exe" extension can be ignored.
        /// <example>
        /// "c:\windows\system32\cmd.exe";
        /// "cmd.exe";
        /// "cmd";
        /// "..\myApp.exe"
        /// "..\myApp"
        /// </example>
        /// </param>
        /// <param name="arguments">process specified command-line arguments</param>
        /// <param name="userName">user name to start process with.</param>
        /// <param name="password">password to start process with.</param>
        /// <param name="domain">domain to start process with.</param>
        /// <returns>processId, the unique identifier for the process</returns>
        /// <exception cref="FaultException" >
        /// FaultException is thrown by SutControlService to indicate errors on SUT machine.
        /// It encapsulates the details in ExceptionDetail, such as exception type, message and stack trace.
        /// If you want to catch and analyze it, here's example:
        /// 
        ///    catch (FaultException faultException)
        ///    {
        ///        if (faultException.Detail.Type == typeof(InvalidOperationException).FullName)
        ///        {
        ///            throw new InvalidOperationException();
        ///        }
        ///    }
        /// </exception>
        private int StartProcessInternal(
            string command,
            string arguments,
            string userName,
            string password,
            string domain)
        {
            ValidateCommand(command);

            #region logging
            Info("StartProcess, arguments='{0}'", (arguments == null ? "" : arguments));
            #endregion

            int processId = service.StartProcess(command, arguments, userName, password, domain);

            #region logging
            Info("StartProcess, returned processId={0}", processId);
            #endregion

            return processId;
        }


        /// <summary>
        /// Start a process on SUT using specified domain\userName password,
        /// wait for the process exit and return its results.
        /// This method executes synchronously, blocks current thread till process exits or timeouts.
        /// </summary>
        /// <param name="command">the process's filename to start with.
        /// The filename must be one of the executable file types.
        /// The filename can use an absolute path, or relative path,
        /// or without path if the path has been defined in SUT's "PATH" environment variable.
        /// In any of above case, the ".exe" extension can be ignored.
        /// <example>
        /// "c:\windows\system32\cmd.exe";
        /// "cmd.exe";
        /// "cmd";
        /// "..\myApp.exe"
        /// "..\myApp"
        /// </example>
        /// </param>
        /// <param name="arguments">process specified command-line arguments</param>
        /// <param name="userName">user name to start process with.</param>
        /// <param name="password">password to start process with.</param>
        /// <param name="domain">domain to start process with.</param>
        /// <param name="timeout">if the process doesn't exit after waiting specified time span,
        /// a TimeoutException is thrown.
        /// timeout should either be 
        /// (1) TimeSpan.MaxValue which presents infinity, or
        /// (2) its total milliseconds is greater than 0 and less than or equals to Int32.MaxValue. </param>
        /// <param name="exitCode">exit code of the process</param>
        /// <param name="standardOutput">
        /// content that the process generates to the standard output stream.
        /// Empty string returns if no content generated.</param>
        /// <param name="standardError">
        /// content that the process generates to the standard error stream.
        /// Empty string returns if no content generated.</param>
        /// <returns>processId, the unique identifier for the process</returns>
        /// <exception cref="FaultException" >
        /// FaultException is thrown by SutControlService to indicate errors on SUT machine.
        /// It encapsulate the details in ExceptionDetail, such as exception type, message and stack trace.
        /// If you want to catch and analyze it, here's example:
        /// 
        ///    catch (FaultException faultException)
        ///    {
        ///        if (faultException.Detail.Type == typeof(InvalidOperationException).FullName)
        ///        {
        ///            throw new InvalidOperationException();
        ///        }
        ///    }
        /// </exception>
        private int StartProcessWaitForExitInternal(
            string command,
            string arguments,
            string userName,
            string password,
            string domain,
            TimeSpan timeout,
            out int exitCode,
            out string standardOutput,
            out string standardError)
        {
            ValidateCommand(command);
            ValidateTimeout(timeout);

            int processId = service.StartProcessWaitForExit(command, arguments, userName, password, domain, timeout,
                out exitCode, out standardOutput, out standardError);

            #region logging
            Info("StartProcessWaitForExit, returned processId={0}", processId);
            #endregion

            return processId;
        }


        #endregion
    }
}