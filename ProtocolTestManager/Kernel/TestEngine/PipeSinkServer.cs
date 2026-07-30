// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.Kernel
{
    // Use to get the output of QTAgent console.
    public class PipeSinkServer
    {
        static string PipeName;
        static List<Listener> listeners;
        static readonly object listenersLock = new object();
        static NamedPipeServerStream waitingServer = null;
        public static void serverCallback(IAsyncResult result)
        {
            // Should clean these unused listener, otherwise, these NamedPipeServerStreams will be kept at last.
            CleanUnusedListener();
            NamedPipeServerStream server = (NamedPipeServerStream)result.AsyncState;
            server.EndWaitForConnection(result);
            StreamReader reader = new StreamReader(server);
            Listener listener = new Listener(reader, server);
            lock (listenersLock)
            {
                listeners.Add(listener);
            }
            waitingServer = new NamedPipeServerStream(PipeName, PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
            waitingServer.BeginWaitForConnection(new AsyncCallback(serverCallback), waitingServer);
        }

        /// <summary>
        /// Start PipeSink of PTF.
        /// </summary>
        public static void Start(string pipeName)
        {
            listeners = new List<Listener>();
            PipeName = pipeName;
            if (waitingServer == null)
            {
                waitingServer = new NamedPipeServerStream(PipeName, PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
                waitingServer.BeginWaitForConnection(new AsyncCallback(serverCallback), waitingServer);
            }
            Listener.IgnoreLogs = false;
        }

        /// <summary>
        /// Stop PipeSink
        /// </summary>
        public static void Stop()
        {
            Listener.IgnoreLogs = true;
            lock (listenersLock)
            {
                if (listeners != null)
                {
                    foreach (var listener in listeners)
                    {
                        listener.Stop();
                    }

                    listeners.Clear();
                }
            }
        }

        public delegate void ParseLogMessageCallback(Guid connectionId, string message);
        public static ParseLogMessageCallback ParseLogMessage;
        
        private static void CleanUnusedListener()
        {
            lock (listenersLock)
            {
                listeners?.RemoveAll(listener =>
                {
                    if (!listener.IsCompleted)
                    {
                        return false;
                    }

                    listener.Stop();
                    return true;
                });
            }
        }
    
    }

    /// <summary>
    /// Listener of NamedPipe.
    /// </summary>
    public class Listener
    {
        public static bool IgnoreLogs;
        private readonly Guid connectionId = Guid.NewGuid();
        StreamReader SR;
        NamedPipeServerStream serverStream;
        CancellationTokenSource source;
        Task task;

        public bool IsCompleted => task?.IsCompleted ?? true;
        public Listener(StreamReader reader, NamedPipeServerStream server)
        {
            serverStream = server;
            SR = reader;
            source = new CancellationTokenSource();
            task = new Task(Run, source.Token);
            task.Start();
        }
        /// <summary>
        /// Stop to get the stream.
        /// </summary>
        public void Stop()
        {
            if (task != null && task.Status == TaskStatus.Running)
            {
                source.Cancel();
            }

            if(SR != null)
                SR.Close();

            if(serverStream != null)
                serverStream.Close();
        }
        /// <summary>
        /// Start to get the output.
        /// </summary>
        public void Run()
        {

            string line;
            while ((line = SR.ReadLine()) != null)
            {
                if (!IgnoreLogs && PipeSinkServer.ParseLogMessage != null)
                    PipeSinkServer.ParseLogMessage(connectionId, line);
            }
        }
    }
}