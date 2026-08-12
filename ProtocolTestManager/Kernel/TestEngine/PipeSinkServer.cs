// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Protocols.TestManager.Kernel
{
    // Use to get the output of the test host console.
    public class PipeSinkServer
    {
        private const int PendingListenerCount = 8;

        private static readonly object SyncRoot = new object();
        private static string pipeName;
        private static List<Listener> listeners = new List<Listener>();
        private static HashSet<NamedPipeServerStream> waitingServers =
            new HashSet<NamedPipeServerStream>();
        private static bool isRunning;

        public static void Start(string name)
        {
            lock (SyncRoot)
            {
                if (isRunning)
                {
                    throw new InvalidOperationException("The PTF pipe sink server is already running.");
                }

                var newWaitingServers = new HashSet<NamedPipeServerStream>();
                try
                {
                    for (int index = 0; index < PendingListenerCount; index++)
                    {
                        var server = CreateWaitingServer(name);
                        newWaitingServers.Add(server);
                    }

                    pipeName = name;
                    listeners = new List<Listener>();
                    waitingServers = newWaitingServers;
                    Listener.IgnoreLogs = false;
                    isRunning = true;
                }
                catch
                {
                    foreach (var server in newWaitingServers)
                    {
                        server.Dispose();
                    }

                    throw;
                }
            }
        }

        public static void Stop()
        {
            List<Listener> listenersToStop;
            List<NamedPipeServerStream> waitingServersToStop;

            lock (SyncRoot)
            {
                if (!isRunning)
                {
                    return;
                }

                isRunning = false;
                Listener.IgnoreLogs = true;
                waitingServersToStop = waitingServers.ToList();
                waitingServers.Clear();
                listenersToStop = listeners;
                listeners = new List<Listener>();
            }

            foreach (var server in waitingServersToStop)
            {
                server.Dispose();
            }

            // Listeners are stopped concurrently: each stop waits up to 5 seconds and a parallel
            // run can hold several connections, which would otherwise add up on the caller thread.
            if (listenersToStop.Count == 1)
            {
                listenersToStop[0].Stop();
            }
            else if (listenersToStop.Count > 1)
            {
                Task.WaitAll(listenersToStop.Select(listener => Task.Run(listener.Stop)).ToArray());
            }
        }

        private static void BeginWaitingForConnection()
        {
            if (!isRunning)
            {
                return;
            }

            var server = CreateWaitingServer(pipeName);
            waitingServers.Add(server);
        }

        private static NamedPipeServerStream CreateWaitingServer(string name)
        {
            var server = new NamedPipeServerStream(
                name,
                PipeDirection.InOut,
                NamedPipeServerStream.MaxAllowedServerInstances,
                PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous);
            try
            {
                server.BeginWaitForConnection(ServerCallback, server);
                return server;
            }
            catch
            {
                server.Dispose();
                throw;
            }
        }

        private static void ServerCallback(IAsyncResult result)
        {
            var server = (NamedPipeServerStream)result.AsyncState;

            try
            {
                server.EndWaitForConnection(result);

                lock (SyncRoot)
                {
                    if (!isRunning || !waitingServers.Remove(server))
                    {
                        // Reject delayed callbacks from a previous run (or a stale waiter)
                        // so they don't attach old connections to the new listener list.
                        server.Dispose();
                        return;
                    }

                    CleanUnusedListeners();
                    listeners.Add(new Listener(new StreamReader(server), server));
                    BeginWaitingForConnection();
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (IOException)
            {
                server.Dispose();

                lock (SyncRoot)
                {
                    bool wasPending = waitingServers.Remove(server);
                    if (isRunning && wasPending)
                    {
                        BeginWaitingForConnection();
                    }
                }
            }
        }

        private static void CleanUnusedListeners()
        {
            listeners.RemoveAll(listener =>
            {
                if (!listener.IsCompleted)
                {
                    return false;
                }

                listener.Stop();
                return true;
            });
        }

        public delegate void ParseLogMessageCallback(Guid connectionId, string message);

        public static ParseLogMessageCallback ParseLogMessage;
    }

    public class Listener
    {
        public static bool IgnoreLogs;

        private readonly Guid connectionId = Guid.NewGuid();
        private readonly StreamReader reader;
        private readonly NamedPipeServerStream serverStream;
        private readonly CancellationTokenSource cancellationTokenSource;
        private readonly Task listenerTask;

        public bool IsCompleted => listenerTask.IsCompleted;

        public Listener(StreamReader reader, NamedPipeServerStream server)
        {
            this.reader = reader;
            serverStream = server;
            cancellationTokenSource = new CancellationTokenSource();
            listenerTask = Task.Run(Run, cancellationTokenSource.Token);
        }

        public void Stop()
        {
            if (!listenerTask.Wait(TimeSpan.FromSeconds(5)))
            {
                cancellationTokenSource.Cancel();
            }

            reader.Dispose();
            serverStream.Dispose();
        }

        private void Run()
        {
            try
            {
                string line;
                while (!cancellationTokenSource.IsCancellationRequested &&
                    (line = reader.ReadLine()) != null)
                {
                    if (!IgnoreLogs)
                    {
                        PipeSinkServer.ParseLogMessage?.Invoke(connectionId, line);
                    }
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (IOException)
            {
            }
        }
    }
}
