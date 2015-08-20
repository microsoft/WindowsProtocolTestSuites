// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk;
using Microsoft.Protocols.TestTools.StackSdk.Proxy;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Xml;

namespace Microsoft.Protocols.TestTools.StackSdk.Proxy
{
    #region proxy of log, this will merge to ptf in future.

    /// <summary>
    /// the proxy for IConfigurationData
    /// </summary>
    public class StackSdkConfigurationData
    {
        private System.Xml.XmlNode xmlNode = null;

        private System.Xml.XmlReader xmlReader = null;

        ~StackSdkConfigurationData()
        {
            if (xmlReader != null)
            {
                xmlReader.Close();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="xmlFileName">the xml file path</param>
        public StackSdkConfigurationData(string xmlFileName)
        {
            System.Xml.XmlDocument node = new System.Xml.XmlDocument();
            node.XmlResolver = null;
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.DtdProcessing = DtdProcessing.Prohibit;
            settings.XmlResolver = null;
            this.xmlReader = System.Xml.XmlReader.Create(xmlFileName, settings);
            node.Load(this.xmlReader);
            this.xmlNode = node;

            // read config for MilliSecondsToGetLog
            System.Xml.XmlNode milliSecondsToGetLogNode = this.xmlNode.SelectSingleNode(@"//MilliSecondsToGetLog");
            if (milliSecondsToGetLogNode != null)
            {
                this.milliSecondsToGetLog = Convert.ToInt32(milliSecondsToGetLogNode.InnerText, CultureInfo.InvariantCulture);
            }

            // init configs
            this.logConfigs = new Dictionary<string, LogConfigBase>();

            // read all file log configs
            ReadFileLogConfig();

            // read all udp configs
            ReadUDPLogConfig();

            // read all console configs
            ReadConsoleLogConfig();

            // read all trace configs
            ReadTraceLogConfig();
        }

        private void ReadTraceLogConfig()
        {
            System.Xml.XmlNode udp = this.xmlNode.SelectSingleNode("//Trace");
            if (udp != null)
            {

                System.Xml.XmlNodeList configs = udp.SelectNodes("Config");
                foreach (System.Xml.XmlNode config in configs)
                {
                    //id="capture" category="" level="DEBUG" module="capture"
                    System.Xml.XmlAttribute id = config.Attributes["id"];
                    System.Xml.XmlAttribute category = config.Attributes["category"];
                    System.Xml.XmlAttribute level = config.Attributes["level"];
                    System.Xml.XmlAttribute module = config.Attributes["module"];
                    if (id == null)
                    {
                        throw new StackException("config error: console config should have id.");
                    }

                    // create a config instance
                    TraceLogConfig traceLogConfig = new TraceLogConfig();
                    traceLogConfig.Category = category.Value;
                    traceLogConfig.Level = (LogLevel)Enum.Parse(typeof(LogLevel), level.Value);
                    traceLogConfig.Module = module.Value;

                    if (this.logConfigs.ContainsKey(id.Value))
                    {
                        throw new StackException("the id of config must be unique.");
                    }
                    this.logConfigs.Add(id.Value, traceLogConfig);
                }
            }
        }

        private void ReadConsoleLogConfig()
        {
            System.Xml.XmlNode udp = this.xmlNode.SelectSingleNode("//Console");
            if (udp != null)
            {
                System.Xml.XmlNodeList configs = udp.SelectNodes("Config");
                foreach (System.Xml.XmlNode config in configs)
                {
                    //id="capture" category="" level="DEBUG" module="capture"
                    System.Xml.XmlAttribute id = config.Attributes["id"];
                    System.Xml.XmlAttribute category = config.Attributes["category"];
                    System.Xml.XmlAttribute level = config.Attributes["level"];
                    System.Xml.XmlAttribute module = config.Attributes["module"];
                    if (id == null)
                    {
                        throw new StackException("config error: console config should have id.");
                    }

                    // create a config instance
                    ConsoleLogConfig consoleLogConfig = new ConsoleLogConfig();
                    consoleLogConfig.Category = category.Value;
                    consoleLogConfig.Level = (LogLevel)Enum.Parse(typeof(LogLevel), level.Value);
                    consoleLogConfig.Module = module.Value;

                    if (this.logConfigs.ContainsKey(id.Value))
                    {
                        throw new StackException("the id of config must be unique.");
                    }
                    this.logConfigs.Add(id.Value, consoleLogConfig);
                }
            }
        }

        private void ReadUDPLogConfig()
        {
            System.Xml.XmlNode udp = this.xmlNode.SelectSingleNode("//UDP");
            if (udp != null)
            {
                System.Xml.XmlNodeList configs = udp.SelectNodes("Config");
                foreach (System.Xml.XmlNode config in configs)
                {
                    //id="capture" category="" level="DEBUG" module="capture" protocolType="IPv4" remoteUDPAddress="127.0.0.1" remoteUDPPort="1024"
                    System.Xml.XmlAttribute id = config.Attributes["id"];
                    System.Xml.XmlAttribute category = config.Attributes["category"];
                    System.Xml.XmlAttribute level = config.Attributes["level"];
                    System.Xml.XmlAttribute module = config.Attributes["module"];
                    System.Xml.XmlAttribute protocolType = config.Attributes["protocolType"];
                    System.Xml.XmlAttribute remoteUDPAddress = config.Attributes["remoteUDPAddress"];
                    System.Xml.XmlAttribute remoteUDPPort = config.Attributes["remoteUDPPort"];
                    if (id == null || protocolType == null || remoteUDPAddress == null || remoteUDPPort == null)
                    {
                        throw new StackException("config error: dns config should have id, protocolType, remoteUDPAddress and remoteUDPPort.");
                    }

                    // create a config instance
                    UDPLogConfig udpLogConfig = new UDPLogConfig();
                    udpLogConfig.Category = category.Value;
                    udpLogConfig.Level = (LogLevel)Enum.Parse(typeof(LogLevel), level.Value);
                    udpLogConfig.Module = module.Value;
                    udpLogConfig.ProtoType = (ProtocolType)Enum.Parse(typeof(ProtocolType), protocolType.Value);
                    foreach (IPAddress address in System.Net.Dns.GetHostAddresses(remoteUDPAddress.Value))
                    {
                        if (
                            (udpLogConfig.ProtoType == ProtocolType.IPv4 && address.AddressFamily == AddressFamily.InterNetwork)
                            ||
                            (udpLogConfig.ProtoType == ProtocolType.IPv6 && address.AddressFamily == AddressFamily.InterNetworkV6)
                            )
                        {
                            udpLogConfig.RemoteUdpAddress = address;
                            break;
                        }
                    }
                    udpLogConfig.RemoteUdpPort = Convert.ToInt32(remoteUDPPort.Value, CultureInfo.InvariantCulture);

                    if (this.logConfigs.ContainsKey(id.Value))
                    {
                        throw new StackException("the id of config must be unique.");
                    }
                    this.logConfigs.Add(id.Value, udpLogConfig);
                }
            }
        }

        private void ReadFileLogConfig()
        {
            System.Xml.XmlNode file = this.xmlNode.SelectSingleNode("//File");
            if (file != null)
            {
                System.Xml.XmlNodeList configs = file.SelectNodes("Config");
                foreach (System.Xml.XmlNode config in configs)
                {
                    //id="stack" category="" level="DEBUG" module="stack" fileName="stack.log"
                    System.Xml.XmlAttribute id = config.Attributes["id"];
                    System.Xml.XmlAttribute category = config.Attributes["category"];
                    System.Xml.XmlAttribute level = config.Attributes["level"];
                    System.Xml.XmlAttribute module = config.Attributes["module"];
                    System.Xml.XmlAttribute fileName = config.Attributes["fileName"];
                    if (id == null || fileName == null)
                    {
                        throw new StackException("config error: file config should have id and fileName.");
                    }

                    // create a config instance
                    FileLogConfig fileLogConfig = new FileLogConfig();
                    fileLogConfig.Category = category.Value;
                    fileLogConfig.Level = (LogLevel)Enum.Parse(typeof(LogLevel), level.Value);
                    fileLogConfig.Module = module.Value;
                    fileLogConfig.FileName = fileName.Value;
                    if (this.logConfigs.ContainsKey(id.Value))
                    {
                        throw new StackException("the id of config must be unique.");
                    }
                    this.logConfigs.Add(id.Value, fileLogConfig);
                }
            }
        }

        private Dictionary<string, LogConfigBase> logConfigs;

        internal Dictionary<string, LogConfigBase> LogConfigs
        {
            get
            {
                return this.logConfigs;
            }
        }

        private int milliSecondsToGetLog;


        /// <summary>
        /// The time to wait for GetLog() call
        /// </summary>
        public int MilliSecondsToGetLog
        {
            get
            {
                return this.milliSecondsToGetLog;
            }
        }
    }

    /// <summary>
    /// the proxy of testsite, to replace the default testsite
    /// </summary>
    public class StackSdkTestSite
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileName">The file path name</param>
        public StackSdkTestSite(string fileName)
        {
            this.config = new StackSdkConfigurationData(fileName);
        }

        /// <summary>
        /// the config for log system
        /// </summary>
        private StackSdkConfigurationData config;

        /// <summary>
        /// the config for log system
        /// </summary>
        public StackSdkConfigurationData Config
        {
            get
            {
                return this.config;
            }
        }
    }

    #endregion

}

namespace Microsoft.Protocols.TestTools.StackSdk
{
    /// <summary>
    /// the log level in StackSdk.
    /// </summary>
    [FlagsAttribute]
    public enum LogLevel : int
    {
        /// <summary>
        /// Default value
        /// </summary>
        NONE = 0x00,

        /// <summary>
        ///  The DEBUG Level designates fine-grained informational events that are most useful 
        ///  to debug an application. 
        /// </summary>
        DEBUG = 0x01,

        /// <summary>
        ///  The INFO level designates informational messages that highlight the progress of 
        ///  the application at coarse-grained level. 
        /// </summary>
        INFO = 0x02,

        /// <summary>
        ///  The WARN level designates potentially harmful situations.
        /// </summary>
        WARN = 0x04,

        /// <summary>
        ///  The ERROR level designates error events that might still allow the application to 
        ///  continue running.
        /// </summary>
        ERROR = 0x08,

        /// <summary>
        ///  The FATAL level designates very severe error events that will presumably lead the 
        ///  application to abort. 
        /// </summary>
        FATAL = 0x10
    }

    /// <summary>
    /// the log type
    /// </summary>
    [FlagsAttribute]
    public enum LogType : int
    {
        /// <summary>
        /// Default value
        /// </summary>
        NONE = 0x00,

        /// <summary>
        /// Indicate that the log will be put in a file
        /// </summary>
        FILE = 0x01,

        /// <summary>
        /// Indicate that the log will be write to a Udp stream
        /// </summary>
        UDP = 0x02,

        /// <summary>
        /// Indicate that the log will be write to console
        /// </summary>
        CONSOLE = 0x04,

        /// <summary>
        /// Indicate that the log will be write to trace
        /// </summary>
        TRACE = 0x08
    }

    /// <summary>
    /// the config for logger
    /// </summary>
    public class LogConfig
    {
        private LogConfigBase realLogConfig;

        internal LogConfigBase RealLogConfig
        {
            get
            {
                return this.realLogConfig;
            }
        }

        /// <summary>
        /// create LogConfig instance
        /// </summary>
        /// <param name="fileName">the xml config file name</param>
        /// <param name="configId">the config id</param>
        /// <returns>the logconfig instance</returns>
        public LogConfig(string fileName, string configId)
        {
            LogManagement.Instance().Initialize(fileName);
            LogConfigBase config = LogManagement.Instance().Site.Config.LogConfigs[configId];

            if (config == null)
            {
                throw new StackException(string.Format(CultureInfo.InvariantCulture, "the config can not be found, id={0}", configId));
            }

            this.realLogConfig = config;
        }

    }

    /// <summary>
    /// the config for logger
    /// </summary>
    internal abstract class LogConfigBase : IDisposable
    {
        public abstract LogType Type
        {
            get;
        }

        private string module;

        public string Module
        {
            get
            {
                return this.module;
            }
            set
            {
                this.module = value;
            }
        }

        private LogLevel level;

        public LogLevel Level
        {
            get
            {
                return this.level;
            }
            set
            {
                this.level = value;
            }
        }

        private string category;

        public string Category
        {
            get
            {
                return this.category;
            }
            set
            {
                this.category = value;
            }
        }

        /// <summary>
        /// create destination with special type
        /// </summary>
        internal abstract ILogDistination CreateDistination();

        #region IDisposable

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                }

                //Note disposing has been done.
                this.disposed = true;
            }
        }

        #endregion
    }

    /// <summary>
    /// the config for file logger
    /// </summary>
    internal class FileLogConfig : LogConfigBase
    {
        private string fileName;

        public string FileName
        {
            get
            {
                return this.fileName;
            }
            set
            {
                this.fileName = value;
            }
        }

        public override LogType Type
        {
            get
            {
                return LogType.FILE;
            }
        }

        /// <summary>
        /// create destination with special type
        /// </summary>
        internal override ILogDistination CreateDistination()
        {
            return new LogToFile(this.Type);
        }
    }

    /// <summary>
    /// the config for UDP logger
    /// </summary>
    internal class UDPLogConfig : LogConfigBase
    {
        private IPAddress remoteUdpAddress;

        public IPAddress RemoteUdpAddress
        {
            get
            {
                return this.remoteUdpAddress;
            }
            set
            {
                this.remoteUdpAddress = value;
            }
        }


        private int remoteUdpPort;

        public int RemoteUdpPort
        {
            get
            {
                return this.remoteUdpPort;
            }
            set
            {
                this.remoteUdpPort = value;
            }
        }

        private ProtocolType protoType;

        public ProtocolType ProtoType
        {
            get
            {
                return this.protoType;
            }
            set
            {
                this.protoType = value;
            }
        }

        public override LogType Type
        {
            get
            {
                return LogType.UDP;
            }
        }

        /// <summary>
        /// create destination with special type
        /// </summary>
        internal override ILogDistination CreateDistination()
        {
            return new LogToUdp(this.Type);
        }
    }

    /// <summary>
    /// the config for console logger
    /// </summary>
    internal class ConsoleLogConfig : LogConfigBase
    {
        public override LogType Type
        {
            get
            {
                return LogType.CONSOLE;
            }
        }

        /// <summary>
        /// create destination with special type
        /// </summary>
        internal override ILogDistination CreateDistination()
        {
            return new LogToConsole(this.Type);
        }
    }

    /// <summary>
    /// the config for trace logger
    /// </summary>
    internal class TraceLogConfig : LogConfigBase
    {
        public override LogType Type
        {
            get
            {
                return LogType.TRACE;
            }
        }

        /// <summary>
        /// create destination with special type
        /// </summary>
        internal override ILogDistination CreateDistination()
        {
            return new LogToTrace(this.Type);
        }
    }

    /// <summary>
    /// the log message entity
    /// </summary>
    internal class LogMessage : IDisposable
    {
        private bool disposed;

        private string message;

        public string Message
        {
            get
            {
                return this.message;
            }
            set
            {
                this.message = value;
            }
        }

        private LogLevel logLevel;

        public LogLevel LogLevel
        {
            get
            {
                return this.logLevel;
            }
            set
            {
                this.logLevel = value;
            }
        }

        private DateTime date;

        public DateTime Date
        {
            get
            {
                return this.date;
            }
            set
            {
                this.date = value;
            }
        }

        private Logger logActor;

        public Logger LogActor
        {
            get
            {
                return this.logActor;
            }
            set
            {
                this.logActor = value;
            }
        }

        public LogMessage(Logger logger, string message, LogLevel level)
        {
            this.LogActor = logger;
            this.Message = message;
            this.LogLevel = level;
            this.Date = DateTime.Now;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    //Release managed resource.
                    this.message = null;
                }

                //Note disposing has been done.
                this.disposed = true;
            }
        }

    }

    /// <summary>
    /// add log to log management
    /// </summary>
    public class Logger : IDisposable
    {
        private bool disposed;

        #region attributes from config

        /// <summary>
        /// The module who generates the log information
        /// </summary>
        public string Module
        {
            get
            {
                return this.Config.Module;
            }
            set
            {
                this.Config.Module = value;
            }
        }


        /// <summary>
        /// The category of the log information
        /// </summary>
        public string Category
        {
            get
            {
                return this.Config.Category;
            }
            set
            {
                this.Config.Category = value;
            }
        }

        #endregion

        #region log attributes

        private LogConfigBase config;

        internal LogConfigBase Config
        {
            get
            {
                return this.config;
            }
            set
            {
                this.config = value;
            }
        }

        #endregion

        #region constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logConfig">The log config</param>
        public Logger(LogConfig logConfig)
        {
            this.Config = logConfig.RealLogConfig;

            LogManagement.Instance().AddLogger(this);
        }

        #endregion

        #region IDisposable

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
                    //Release managed resource.
                    LogManagement.Instance().RemoveLogger(this);
                    if (LogManagement.Instance().LoggerCount == 0)
                    {
                        //LogManagement.Finalize();
                    }
                }

                //Note disposing has been done.
                this.disposed = true;
            }
        }

        #endregion

        #region utility members

        private void WriteMessage(string logMsg, LogLevel level)
        {
            LogLevel configLevel = this.Config.Level;
            if (configLevel < level)
            {
                LogMessage log = new LogMessage(this, logMsg, level);
                LogManagement.Instance().AddLog(log);
            }
        }


        /// <summary>
        /// write the logMsg to log as INFO type
        /// </summary>
        /// <param name="logMsg">The message</param>
        public void Info(string logMsg)
        {
            WriteMessage(logMsg, LogLevel.INFO);
        }


        /// <summary>
        /// write the logMsg to log as WARN type
        /// </summary>
        /// <param name="logMsg">The message</param>
        public void Warning(string logMsg)
        {
            WriteMessage(logMsg, LogLevel.WARN);
        }


        /// <summary>
        /// write the logMsg to log as DEBUG type
        /// </summary>
        /// <param name="logMsg">The message</param>
        public void Debug(string logMsg)
        {
            WriteMessage(logMsg, LogLevel.DEBUG);
        }


        /// <summary>
        /// write the logMsg to log as ERROR type
        /// </summary>
        /// <param name="logMsg">The message</param>
        public void Error(string logMsg)
        {
            WriteMessage(logMsg, LogLevel.ERROR);
        }


        /// <summary>
        /// write the logMsg to log as FATAL type
        /// </summary>
        /// <param name="logMsg">The message</param>
        public void Fatal(string logMsg)
        {
            WriteMessage(logMsg, LogLevel.FATAL);
        }


        /// <summary>
        /// Set the log level
        /// </summary>
        /// <param name="level">The log level</param>
        public void SetLevel(LogLevel level)
        {
            this.Config.Level = level;
        }

        #endregion
    }

    /// <summary>
    /// static class, invisible to external users
    /// </summary>
    public class LogManagement : IDisposable
    {
        #region members for log

        private StackSdkTestSite site;

        /// <summary>
        /// The StackSdkTestSite
        /// </summary>
        public StackSdkTestSite Site
        {
            get
            {
                return this.site;
            }
        }

        private List<Logger> loggerList;
        private QueueManager logMsgList;
        private Dictionary<LogType, ILogDistination> logDistinationMap;

        /// <summary>
        /// The log thread which is used to capture log and write it the  
        /// log destination.
        /// </summary>
        public LogThread logThread;

        #endregion

        #region the singleton instance

        private static LogManagement instance;

        /// <summary>
        /// The only instance of LogManagement
        /// </summary>
        /// <returns></returns>
        public static LogManagement Instance()
        {
            if (instance == null)
            {
                instance = new LogManagement();
            }
            return instance;
        }

        private LogManagement()
        {
        }

        #endregion

        #region initialize this class with site

        /// <summary>
        /// Initialize the LogManagement with the specified fileName
        /// </summary>
        /// <param name="fileName">The file name</param>
        public void Initialize(string fileName)
        {
            // to prevent multiply initialize
            if (this.site != null)
            {
                return;
            }

            this.site = new StackSdkTestSite(fileName);

            loggerList = new List<Logger>();
            logMsgList = new QueueManager();
            logDistinationMap = new Dictionary<LogType, ILogDistination>();
            logThread = new LogThread();
        }

        #endregion

        #region access the loggers

        internal int LoggerCount
        {
            get { return loggerList.Count; }
        }

        internal void AddLogger(Logger logger)
        {
            lock (loggerList)
            {
                loggerList.Add(logger);
            }
        }

        internal void RemoveLogger(Logger logger)
        {
            lock (loggerList)
            {
                loggerList.Remove(logger);
            }
        }

        #endregion

        #region get the distination of special config

        internal ILogDistination GetDistination(LogConfigBase config)
        {
            lock (logDistinationMap)
            {
                ILogDistination distination = null;
                if (!logDistinationMap.ContainsKey(config.Type))
                {
                    distination = config.CreateDistination();
                    logDistinationMap.Add(config.Type, distination);
                }
                else
                {
                    distination = logDistinationMap[config.Type];
                }
                return distination;
            }
        }

        #endregion

        #region access the logs

        internal void AddLog(LogMessage log)
        {
            lock (logMsgList)
            {
                logMsgList.AddObject(log);
            }
        }

        internal LogMessage GetLog(TimeSpan timeout)
        {
            lock (logMsgList)
            {
                return logMsgList.GetObject(ref timeout) as LogMessage;
            }
        }

        #endregion

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
                    if (logThread != null)
                    {
                        logThread.Dispose();
                        logThread = null;
                    }
                    if (logMsgList != null)
                    {
                        logMsgList.Dispose();
                        logMsgList = null;
                    }
                    if (this.logDistinationMap != null)
                    {
                        foreach (ILogDistination distination in this.logDistinationMap.Values)
                        {
                            distination.Dispose();
                        }
                    }
                }

                //Note disposing has been done.
                this.disposed = true;
            }
        }

        #endregion
    }

    /// <summary>
    /// the log thread, to write log to destinations.
    /// </summary>
    public class LogThread : IDisposable
    {
        private bool disposed;

        /// <summary>
        /// an event indicates whether this thread should be terminated
        /// </summary>
        public System.Threading.ManualResetEvent exitEvent;
        private Thread logProcThread;
        private int milliSecondsToGetLog;


        /// <summary>
        /// Constructor
        /// </summary>
        public LogThread()
        {
            this.exitEvent = new ManualResetEvent(false);

            this.milliSecondsToGetLog = 100;
            if (LogManagement.Instance().Site != null)
            {
                this.milliSecondsToGetLog = LogManagement.Instance().Site.Config.MilliSecondsToGetLog;
            }

            this.logProcThread = new Thread(ProcessLoop);
            this.logProcThread.Name = "log proc thread";
            this.logProcThread.Start();
        }

        private void ProcessLoop()
        {
            TimeSpan waitForExitTimeSpan = new TimeSpan(0);

            while (true)
            {
                try
                {
                    LogMessage log = LogManagement.Instance().GetLog(new TimeSpan(0, 0, 0, 0, this.milliSecondsToGetLog));

                    if (log != null)
                    {
                        Logger logger = log.LogActor;
                        ILogDistination distination = LogManagement.Instance().GetDistination(logger.Config);

                        distination.Write(log);

                        // to write all log
                        continue;
                    }
                }
                // just catch timeout exception, if timeout, do nothing.
                catch (TimeoutException)
                {
                }

                // thread need to exit
                if (this.exitEvent.WaitOne(waitForExitTimeSpan, false))
                {
                    break;
                }
            }
        }


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
                    // wait for log thread to exit.
                    this.exitEvent.Set();
                    this.logProcThread.Join();

                    // to disposed event
                    this.exitEvent.Close();
                }

                //Note disposing has been done.
                this.disposed = true;
            }
        }
    }

    /// <summary>
    /// interface for all log destination.
    /// to write log to actual device.
    /// </summary>
    internal interface ILogDistination : IDisposable
    {
        void Write(LogMessage logMessage);
    }

    /// <summary>
    /// write log to file.
    /// </summary>
    internal class LogToFile : ILogDistination
    {
        private bool disposed;
        private LogType type;
        private System.Collections.ObjectModel.Collection<string> fileList;

        public LogToFile(LogType type)
        {
            this.fileList = new System.Collections.ObjectModel.Collection<string>();
            this.type = type;
            if (this.type != LogType.FILE)
            {
            }
        }

        public void Write(LogMessage logMessage)
        {
            if (logMessage == null)
            {
                return;
            }

            lock (this.fileList)
            {
                FileLogConfig config = logMessage.LogActor.Config as FileLogConfig;

                // write xml information head
                if (!this.fileList.Contains(config.FileName))
                {
                    StreamWriter sw = new StreamWriter(config.FileName, false);
                    this.fileList.Add(config.FileName);
                    sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                    sw.WriteLine("<TestLog "
                        + "xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" "
                        + "xsi:schemaLocation=\"http://schemas.microsoft.com/windows/ProtocolsTest/2007/07/TestLog "
                        + "http://schemas.microsoft.com/windows/ProtocolsTest/2007/07/TestLog.xsd\" "
                        + "xmlns=\"http://schemas.microsoft.com/windows/ProtocolsTest/2007/07/TestLog\">");
                    sw.WriteLine("<LogEntries>");
                    sw.Close();
                }

                // write log information
                if (config != null)
                {

                    StreamWriter sw = new StreamWriter(config.FileName, true);
                    sw.WriteLine("<LogEntry kind=\"{0}\" timeStamp=\"{1}\" module=\"{2}\" category=\"{3}\">",
                        logMessage.LogLevel,
                        string.Format(CultureInfo.InvariantCulture, "{0}.{1}Z", logMessage.Date.ToString("s", CultureInfo.InvariantCulture), logMessage.Date.Millisecond),
                        logMessage.LogActor.Module,
                        logMessage.LogActor.Category);
                    sw.WriteLine("<Message>{0}</Message>", logMessage.Message);
                    sw.WriteLine("</LogEntry>");
                    sw.Close();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            //Note disposing has been done.
            this.disposed = true;

            if (!disposing)
            {
                return;
            }

            lock (this.fileList)
            {
                foreach (string fileName in this.fileList)
                {
                    StreamWriter sw = new StreamWriter(fileName, true);
                    sw.WriteLine("</LogEntries>");
                    sw.WriteLine("</TestLog>");
                    sw.Close();
                }
                this.fileList.Clear();
            }
        }
    }

    /// <summary>
    /// write log to console
    /// </summary>
    internal class LogToConsole : ILogDistination
    {
        private LogType type;

        public LogToConsole(LogType type)
        {
            this.type = type;
            if (this.type != LogType.CONSOLE)
            {
            }
        }

        public void Write(LogMessage logMessage)
        {
            if (logMessage != null)
            {
                Console.WriteLine(logMessage.Message);
            }
        }

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                }

                //Note disposing has been done.
                this.disposed = true;
            }
        }
    }

    /// <summary>
    /// write log to trace, the output window of vs.
    /// </summary>
    internal class LogToTrace : ILogDistination
    {
        private LogType type;

        public LogToTrace(LogType type)
        {
            this.type = type;
            if (this.type != LogType.TRACE)
            {
            }
        }

        public void Write(LogMessage logMessage)
        {
            if (logMessage != null)
            {
                System.Diagnostics.Debug.WriteLine(logMessage.Message);
            }
        }

        private bool disposed;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                }

                //Note disposing has been done.
                this.disposed = true;
            }
        }
    }

    /// <summary>
    /// write log to remote server through udp.
    /// </summary>
    internal class LogToUdp : ILogDistination
    {
        private bool disposed;
        private LogType type;
        private Dictionary<IPAddress, UdpClient> udpList;

        public LogToUdp(LogType type)
        {
            this.type = type;
            this.udpList = new Dictionary<IPAddress, UdpClient>();
            if (this.type != LogType.UDP)
            {
            }
        }

        public void Write(LogMessage logMessage)
        {
            if (logMessage != null)
            {
                UDPLogConfig config = logMessage.LogActor.Config as UDPLogConfig;

                UdpClient udpClient = null;

                if (!udpList.ContainsKey(config.RemoteUdpAddress))
                {
                    udpClient = new UdpClient();
                    udpList.Add(config.RemoteUdpAddress, udpClient);
                }
                else
                {
                    udpClient = udpList[config.RemoteUdpAddress];
                }

                IPEndPoint remoteEndPoint = new IPEndPoint(config.RemoteUdpAddress, config.RemoteUdpPort);
                byte[] sendBytes = Encoding.Unicode.GetBytes(logMessage.Message);
                udpClient.Send(sendBytes, sendBytes.Length, remoteEndPoint);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    udpList.Clear();
                    udpList = null;
                }

                //Note disposing has been done.
                this.disposed = true;
            }
        }
    }

    #region Simple Logger

    /// <summary>
    /// The logging handler delegate of the SimpleLogger class.
    /// </summary>
    /// <param name="logMsg">The string content which is being logging.</param>
    public delegate void LoggingHandler(string logMsg);

    /// <summary>
    /// The simple logger is used for SDK to report to the debugging information directly by invoking a callback delegate provided by users.
    /// The SimpleLogger is a thread-safe singleton, which could not be initialized twice globally.
    /// </summary>
    public class SimpleLogger
    {
        #region Private Members

        private static object locker = new object();
        private LoggingHandler handler = null;
        private static SimpleLogger instance = null;

        #endregion

        #region Private singleton constructors
        private SimpleLogger()
        {
        }

        private SimpleLogger(LoggingHandler handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException("handler");
            }
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Initialize the simple logger by passing in a logging handler.
        /// </summary>
        /// <param name="handler">The logging handler must be type of LoggingHandler</param>
        public static void Init(LoggingHandler handler)
        {
            lock (locker)
            {

                if (null != instance)
                {
                    throw new InvalidOperationException("SimpleLogger has been initialized.");
                }

                instance = new SimpleLogger(handler);
            }
        }

        /// <summary>
        /// Log a string by passing to the logging call delegate.
        /// </summary>
        /// <param name="logMsg">The content to be logged.</param>
        public static void Log(string logMsg)
        {
            lock (locker)
            {
                // Bypass the logging events if the logger has not been initialized yet. 
                if (instance != null)
                {
                    instance.handler(logMsg);
                }
            }
        }

        /// <summary>
        /// Log a formatted string by passing to the logging call delegate.
        /// </summary>
        /// <param name="logMsgFormat">The logging message format.</param>
        /// <param name="args">The input arguments.</param>
        public static void Log(string logMsgFormat, params object[] args)
        {
            Log(String.Format(CultureInfo.InvariantCulture, logMsgFormat, args));
        }
        #endregion
    }

    #endregion
}