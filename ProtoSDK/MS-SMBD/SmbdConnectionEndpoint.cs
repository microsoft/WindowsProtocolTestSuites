// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rdma;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Smbd
{
    /// <summary>
    /// The implementation-dependent representation used to access the RDMA connection.
    /// </summary>
    public class SmbdConnectionEndpoint : IDisposable
    {
        private const int THREAD_COUNT = 2;
        #region Fields
        private int receiveIndex = 0;
        private ReceiveEntry[] receiveEntries;


        private RdmaProviderInfo[] rdmaProvidersList;
        private RdmaAdapter rdmaAdapter;
        private RdmaCompletionQueue rdmaCompletionQueue;
        private RdmaConnector rdmaConnector;
        private RdmaEndpoint rdmaEndpoint;
        private uint rdmaMaxInlineData;

        private NtStatus initializeStatus;
        private bool isConnected;

        private uint inboundEntries;
        private uint outboundEntries;
        private uint inboundSegment;
        private uint outboundSegment;
        private uint inboundReadLimit;
        private uint outboundReadLimit;
        private uint inboundDataSize;
        private uint completionQueueDepth;


        private List<SmbdMemoryWindow> memoryWindowList;
        /// <summary>
        /// Receive and Invalid notification request list. The result of the request is from peer.
        /// </summary>
        private List<SmbdRequest> receiveRequestList;
        /// <summary>
        /// Result list of Send, SendAndInvalidate, Bind, Invalidate, Read, and Write requests
        /// </summary>
        private SyncFilterQueue<SmbdRequestResult> otherRequestResult;
        /// <summary>
        /// Result list of Receive request
        /// </summary>
        private SyncFilterQueue<SmbdRequestResult> receiveRequestResult;

        private Object locker;
        private SmbdLogEvent logEndpointEvent;

        /// <summary>
        /// Semaphore for waiting disconnect
        /// </summary>
        private Semaphore disconnectSemaphore;

        /// <summary>
        /// The main thread should wait util all the listen thread is started.
        /// 
        /// In main thread, WaitOne should be invoked to wait all thread is launched.
        /// In other thread, Release should be invoked to notify that the thread is launched.
        /// </summary>
        private Semaphore threadStartSemaphore;

        /// <summary>
        /// When client does RDMA notification, the request(Send, Send and Invalid, Receive, Invalid, Read and Write request)
        /// should be recorded.
        /// 
        /// In the NotifyCallback thread, it will invoke WaitOne to wait for client to submit work item.
        /// In the main thread, it will invoke Release to notify that the work item has been submitted.
        /// </summary>
        private Semaphore rdmaNotificationSemaphore;
        private int requestCount;
        #endregion

        #region Properties
        public bool IsConnected { get { return isConnected; } }
        /// <summary>
        /// Count of receive request queued in Receive request result list
        /// </summary>
        public int ReceiveRequestsCount { get { return receiveRequestResult.Count; } }
        /// <summary>
        /// Count of receive has posted.
        /// </summary>
        public int ReceivePostedCount { get; protected set; }
        #endregion

        /// <summary>
        /// SmbdEndpoint constructor will load RDMA providers and initialize default connection values.
        /// </summary>
        /// <param name="inboundEntries">maximum number of outstanding Receive requests.</param>
        /// <param name="outboundEntries">maximum number of outstanding Send, SendAndInvalidate
        /// , Bind, Invalidate, Read, and Write requests.
        /// </param>
        /// <param name="inboundSegment">inbound segments limit</param>
        /// <param name="outboundSegment">outbound segments limit</param>
        /// <param name="inboundReadLimit">maximum inbound read limit for the local Network 
        /// Direct adapter. This value can be zero if you do not support
        /// </param>
        /// <param name="outboundReadLimit"></param>
        /// <param name="inboundDataSize">Max Size of RDMA inbound data</param>
        /// <param name="logEvent">Delegate to log SMBD event</param>
        public SmbdConnectionEndpoint(
            uint inboundEntries,
            uint outboundEntries,
            uint inboundSegment,
            uint outboundSegment,
            uint inboundReadLimit,
            uint outboundReadLimit,
            uint inboundDataSize,
            SmbdLogEvent logEvent = null
            )
        {
            this.logEndpointEvent = logEvent;

            LogEvent("Loading the providers of registered network drivers.");
            initializeStatus = (NtStatus)RdmaProvider.LoadRdmaProviders(out rdmaProvidersList);
            
            this.inboundEntries = inboundEntries;
            this.outboundEntries = outboundEntries;
            this.inboundSegment = inboundSegment;
            this.outboundSegment = outboundSegment;
            this.inboundReadLimit = inboundReadLimit;
            this.outboundReadLimit = outboundReadLimit;
            this.completionQueueDepth = (inboundEntries + outboundEntries);
            this.inboundDataSize = inboundDataSize;

            isConnected = false;
            memoryWindowList = new List<SmbdMemoryWindow>();
            receiveRequestList = new List<SmbdRequest>();

            locker = new Object();

            otherRequestResult = new SyncFilterQueue<SmbdRequestResult>();
            receiveRequestResult = new SyncFilterQueue<SmbdRequestResult>();

            disconnectSemaphore = new Semaphore(0, 1);
            //threadStopSemaphore = new Semaphore(0, THREAD_COUNT);

            // one more for Semaphore Release when disconnection
            rdmaNotificationSemaphore = new Semaphore(0, (int)completionQueueDepth + 1);
            requestCount = 0;

            ReceivePostedCount = 0;

            #region output provider information
            if (initializeStatus != NtStatus.STATUS_SUCCESS)
            {
                LogEvent(string.Format("Load provider with error code: {0}", (NtStatus)initializeStatus));
                return;
            }
            if (rdmaProvidersList == null)
            {
                LogEvent("The returned providers list is NULL");
                return;
            }

            LogEvent(string.Format("{0} providers of registered network drivers have been load,", rdmaProvidersList.Length));
            int providerIndex = 0;
            foreach (RdmaProviderInfo info in rdmaProvidersList)
            {
                if (info != null)
                {
                    LogEvent(string.Format("Load provider {1}: {0}", info.Path, ++providerIndex));
                }
            }
            LogEvent("Loading providers is completed");
            #endregion
        }

        #region Public method
        /// <summary>
        /// establish connection over RDMA
        /// </summary>
        /// <param name="localIpAddress">local IP address</param>
        /// <param name="remoteIpAddress">remote IP address</param>
        /// <param name="port">port</param>
        /// <param name="ipFamily">IP Family</param>
        /// <returns></returns>
        public NtStatus ConnectToServerOverRdma(
            String localIpAddress,
            String remoteIpAddress,
            UInt16 port,
            AddressFamily ipFamily)
        {
            if (initializeStatus != NtStatus.STATUS_SUCCESS)
            {
                return initializeStatus;
            }

            rdmaAdapter = OpenAdapter(rdmaProvidersList, localIpAddress, ipFamily, logEndpointEvent);
            if (rdmaAdapter == null)
            {
                // this is the return code of NDSPI
                return NtStatus.STATUS_NOT_SUPPORTED;
            }

            // create completion queue
            NtStatus status = (NtStatus)rdmaAdapter.CreateCompletionQueue(
                this.completionQueueDepth,
                out this.rdmaCompletionQueue
                );
            if (status != NtStatus.STATUS_SUCCESS)
            {
                return status;
            }

            // connector
            status = (NtStatus)rdmaAdapter.CreateConnector(out this.rdmaConnector);
            if (status != NtStatus.STATUS_SUCCESS)
            {
                return status;
            }

            // create endpoint
            status = (NtStatus)rdmaConnector.CreateEndpoint(
                rdmaCompletionQueue,
                inboundEntries,
                outboundEntries,
                1,
                1,
                inboundReadLimit,
                0,
                out rdmaMaxInlineData,
                out rdmaEndpoint);
            if (status != NtStatus.STATUS_SUCCESS)
            {
                return status;
            }

            // connect to server
            status = (NtStatus)rdmaConnector.Connect(rdmaEndpoint, remoteIpAddress, port, 6 /* for tcp */);
            if (status != NtStatus.STATUS_SUCCESS)
            {
                return status;
            }

            // complete connection and then can do RDMA operations
            status = (NtStatus)rdmaConnector.CompleteConnect();
            if (status != NtStatus.STATUS_SUCCESS)
            {
                return status;
            }

            CompleteConnect();
            return NtStatus.STATUS_SUCCESS;
        }

        /// <summary>
        /// listen and wait for peer connect
        /// </summary>
        /// <param name="localIpAddress"></param>
        /// <param name="port"></param>
        /// <param name="ipFamily">IP Family IPv4 or IPv6</param>
        /// <returns></returns>
        public NtStatus ListenConnection(String localIpAddress, ushort port, AddressFamily ipFamily)
        {
            if (initializeStatus != NtStatus.STATUS_SUCCESS)
            {
                return initializeStatus;
            }

            rdmaAdapter = OpenAdapter(rdmaProvidersList, localIpAddress, ipFamily, logEndpointEvent);
            if (rdmaAdapter == null)
            {
                // this is the return code of NDSPI
                return NtStatus.STATUS_NOT_SUPPORTED;
            }

            // listen and get incoming connection request
            RdmaListen listen;
            NtStatus status = (NtStatus)rdmaAdapter.Listen(6/* tcp */, port, out listen);
            if (status != NtStatus.STATUS_SUCCESS)
            {
                return status;
            }

            // create connector
            status = (NtStatus)rdmaAdapter.CreateConnector(out rdmaConnector);
            if (status != NtStatus.STATUS_SUCCESS)
            {
                return status;
            }

            // wait for connection
            status = (NtStatus)listen.GetConnectionRequest(rdmaConnector);
            if (status != NtStatus.STATUS_SUCCESS)
            {
                return status;
            }

            // create completion queue and endpoint
            status = (NtStatus)rdmaAdapter.CreateCompletionQueue(this.completionQueueDepth,
                out this.rdmaCompletionQueue);
            if (status != NtStatus.STATUS_SUCCESS)
            {
                return status;
            }
            status = (NtStatus)this.rdmaConnector.CreateEndpoint(
                rdmaCompletionQueue,
                inboundEntries,
                outboundEntries,
                1,
                1,
                inboundReadLimit,
                0,
                out rdmaMaxInlineData,
                out rdmaEndpoint);
            if (status != NtStatus.STATUS_SUCCESS)
            {
                return status;
            }

            // accept the arrival RDMA connection
            status = (NtStatus)this.rdmaConnector.Accept(rdmaEndpoint);
            if (status != NtStatus.STATUS_SUCCESS)
            {
                return status;
            }

            CompleteConnect();
            return NtStatus.STATUS_SUCCESS;
        }

        /// <summary>
        /// Disconnect
        /// </summary>
        /// <returns></returns>
        public void Disconnect()
        {
            lock (locker)
            {
                if (isConnected == false)
                {
                    return;
                }
                isConnected = false;
            }

            // disconnect
            rdmaConnector.Disconnect();

            // let notify thread stop
            rdmaNotificationSemaphore.Release();

            for (int i = 0; i < THREAD_COUNT; ++i)
            {
                //threadStopSemaphore.WaitOne(); // wait thread to stop
            }

            LogEvent("RDMA connection is disconnected by test suite.");
            this.logEndpointEvent = null;
        }

        /// <summary>
        /// Send data over RDMA
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public NtStatus SendData(byte[] data)
        {
            RdmaSegment sge = new RdmaSegment();
            sge.Length = (uint)data.LongLength;
            sge.MemoryHandler = 0;

            // register memory
            rdmaAdapter.RegisterMemory(sge.Length, out sge.MemoryHandler);
            // Write data to memory
            RdmaEndpoint.WriteToMemory(sge.MemoryHandler, data);

            // send
            UInt64 resultId;
            NtStatus status = (NtStatus)rdmaEndpoint.Send(new RdmaSegment[] { sge }, out resultId);
            if (status != NtStatus.STATUS_SUCCESS)
            {
                return status;
            }
            // send request has been submitted
            requestCount++;
            rdmaNotificationSemaphore.Release();

            // get the notification
            SmbdRequestResult item = GetRequestResult(new TimeSpan(0, 0, 5), RequestType.Send);
            this.LogEvent(string.Format("Send data with result id: 0x{0:X}. And get notification with id: 0x{1:X}, status: {2}",
                resultId,
                item.ResultId,
                item.ResultInfo.Status
                ));

            // deregister memory
            rdmaAdapter.DeregisterMemory(sge.MemoryHandler);

            return (NtStatus)item.ResultInfo.Status;
        }

        /// <summary>
        /// Receive data over RDMA
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public NtStatus ReceiveData(TimeSpan timeout, out byte[] data)
        {
            SmbdRequestResult item = GetRequestResult(timeout, RequestType.Receive);
            NtStatus status = (NtStatus)item.ResultInfo.Status;

            if (status != NtStatus.STATUS_SUCCESS)
            {
                data = null;
                return status;
            }

            this.LogEvent(string.Format("Receive {0} bytes from entry 0x{1:X}",
                item.ResultInfo.BytesTransferred,
                item.EntryIndex));
            
            data = new byte[item.ResultInfo.BytesTransferred];
            status = (NtStatus)RdmaEndpoint.ReadFromMemory(
                this.receiveEntries[item.EntryIndex].Segment.MemoryHandler,
                data);
            // reset
            this.receiveEntries[receiveIndex].IsOccupied = false;
            return status;
        }

        /// <summary>
        /// Post Receive request
        /// </summary>
        /// <param name="bufferSize"></param>
        /// <returns></returns>
        public NtStatus PostReceive(uint bufferSize)
        {
            if (receiveEntries[receiveIndex].IsOccupied)
            {
                this.LogEvent("No more entries");
                // return no more entries. The error code is the same with RDMA
                return NtStatus.STATUS_NO_MORE_ENTRIES;
            }

            SmbdRequest receiveRequest = new SmbdRequest();
            receiveRequest.EntryIndex = receiveIndex;
            receiveRequest.Type = RequestType.Receive;
            NtStatus ret = (NtStatus)rdmaEndpoint.Receive(
                new RdmaSegment[] { receiveEntries[receiveIndex].Segment },
                out receiveRequest.ResultId);
            if (ret != NtStatus.STATUS_SUCCESS)
            {
                this.LogEvent(string.Format("Raise receive request with error code {0}", ret));
                return ret;
            }
            // receive request has been submitted
            requestCount++;
            rdmaNotificationSemaphore.Release();

            lock (locker)
            {
                receiveRequestList.Add(receiveRequest);
                ReceivePostedCount++;
            }

            this.LogEvent(
                string.Format(
                    "Post receive successfully with entry 0x{0:X} with memory handler 0x{1:X} notification id: 0x{2:X}",
                    receiveIndex,
                    receiveEntries[receiveIndex].Segment.MemoryHandler,
                    receiveRequest.ResultId));
            receiveEntries[receiveIndex].IsOccupied = true;

            // calculate the index of next receive entry
            receiveIndex++;
            if ((UInt64)receiveIndex >= inboundEntries)
            {
                receiveIndex = 0;
            }


            return NtStatus.STATUS_SUCCESS;
        }

        /// <summary>
        /// Get result of request
        /// </summary>
        public SmbdRequestResult GetRequestResult(
            TimeSpan timeout,
            RequestType type)
        {
            if (type == RequestType.Receive)
            {
                return receiveRequestResult.Dequeue(timeout);
            }
            return otherRequestResult.Dequeue(timeout);
        }

        /// <summary>
        /// Register memory windows
        /// </summary>
        /// <param name="size">Size of memory to register</param>
        /// <param name="flag"></param>
        /// <param name="reversed">if it is true, little-endian and big-endian will be reversed in bufferDescriptor</param>
        /// <param name="bufferDescriptor">Buffer Descriptor point to memory windows</param>
        /// <returns></returns>
        public NtStatus RegisterMemoryWindow(
            uint size,
            RdmaOperationReadWriteFlag flag,
            bool reversed,
            out RdmaBufferDescriptorV1 bufferDescriptor)
        {
            bufferDescriptor = new RdmaBufferDescriptorV1();

            // regiser the buffer
            SmbdMemoryWindow memoryWindow = new SmbdMemoryWindow();
            memoryWindow.IsValid = true;
            NtStatus status = (NtStatus)rdmaAdapter.RegisterMemory(size, out memoryWindow.MemoryHandlerId);
            if (status != NtStatus.STATUS_SUCCESS)
            {
                return status;
            }

            // create memory window
            SmbdRequest invalidRequest = new SmbdRequest();
            invalidRequest.Type = RequestType.Invalid;
            status = (NtStatus)rdmaAdapter.CreateMemoryWindow(invalidRequest.ResultId,
                out memoryWindow.RdmaMW);
            memoryWindow.InvalidResultId = invalidRequest.ResultId;
            if (status != NtStatus.STATUS_SUCCESS)
            {
                return status;
            }
            // invalid notification request has been submitted
            requestCount++;
            rdmaNotificationSemaphore.Release();

            // bind
            UInt64 resultId;
            status = (NtStatus)rdmaEndpoint.Bind(
                memoryWindow.MemoryHandlerId,
                memoryWindow.RdmaMW,
                flag,
                reversed,
                out memoryWindow.BufferDescriptor, out resultId);
 
            if (status != NtStatus.STATUS_SUCCESS)
            {
                return status;
            }
            lock (locker)
            {
                this.receiveRequestList.Add(invalidRequest);
            }

            SmbdRequestResult requestResult = GetRequestResult(new TimeSpan(0, 0, 5), RequestType.Bind);
            this.LogEvent(string.Format("Bind memory window with result id: {0}. And get notification with id: {1}, status: {2}",
                resultId,
                requestResult.ResultId,
                requestResult.ResultInfo.Status
                ));

            status = (NtStatus)requestResult.ResultInfo.Status;
            if (status != NtStatus.STATUS_SUCCESS)
            {
                return status;
            }

            this.memoryWindowList.Add(memoryWindow);
            bufferDescriptor = memoryWindow.BufferDescriptor;
            return status;
        }

        /// <summary>
        /// Deregister memory window
        /// </summary>
        /// <param name="bufferDescriptor">Buffer Descriptor point to memory windows</param>
        /// <returns></returns>
        public void DeregisterMemoryWindow(RdmaBufferDescriptorV1 bufferDescriptor)
        {
            foreach (SmbdMemoryWindow mw in memoryWindowList)
            {
                if (mw.BufferDescriptor.Token != bufferDescriptor.Token)
                {
                    continue;
                }

                // get memory window
                if (mw.IsValid)
                {
                    UInt64 resultId;
                    NtStatus status = (NtStatus)rdmaEndpoint.Invalidate(mw.RdmaMW, out resultId);

                    if (status == NtStatus.STATUS_SUCCESS)
                    {
                        requestCount++;
                        rdmaNotificationSemaphore.Release();
                    }
                }

                rdmaAdapter.DeregisterMemory(mw.MemoryHandlerId);
                memoryWindowList.Remove(mw);

                return;
            }
        }

        /// <summary>
        /// Write data to memory window
        /// </summary>
        /// <param name="data"></param>
        /// <param name="bufferDescriptor">Buffer Descriptor point to memory windows</param>
        public NtStatus WriteMemoryWindow(byte[] data, RdmaBufferDescriptorV1 bufferDescriptor)
        {
            foreach (SmbdMemoryWindow mw in memoryWindowList)
            {
                if (mw.BufferDescriptor.Token != bufferDescriptor.Token)
                {
                    continue;
                }
                // get memory window
                if (mw.IsValid)
                {
                    NtStatus status = (NtStatus)RdmaEndpoint.WriteToMemory(mw.MemoryHandlerId, data);
                    return status;
                }
                return NtStatus.STATUS_INVALID_PARAMETER_2;
            }

            return NtStatus.STATUS_INVALID_PARAMETER_2;
        }

        /// <summary>
        /// Read data from memory window
        /// </summary>
        /// <param name="data"></param>
        /// <param name="bufferDescriptor">Buffer Descriptor point to memory windows</param>
        public NtStatus ReadMemoryWindow(byte[] data, RdmaBufferDescriptorV1 bufferDescriptor)
        {
            foreach (SmbdMemoryWindow mw in memoryWindowList)
            {
                if (mw.BufferDescriptor.Token != bufferDescriptor.Token)
                {
                    continue;
                }
                // get memory window
                if (mw.IsValid)
                {
                    NtStatus status = (NtStatus)RdmaEndpoint.ReadFromMemory(mw.MemoryHandlerId, data);
                    return status;
                }
                return NtStatus.STATUS_INVALID_PARAMETER_2;
            }

            return NtStatus.STATUS_INVALID_PARAMETER_2;
        }


        /// <summary>
        /// Wait until network is disconnected, using a TimeSpan to specify the time interval.
        /// </summary>
        /// <param name="timeout"></param>
        public void WaitDisconnect(TimeSpan timeout)
        {
            disconnectSemaphore.WaitOne(timeout);
        }


        #region IDisposable Members

        public void Dispose()
        {
            this.Disconnect();
        }

        #endregion

        #endregion

        /// <summary>
        /// Open adapter with local IP address
        /// </summary>
        /// <param name="providers"></param>
        /// <param name="localIpAddress"></param>
        /// <param name="ipFamily">IP Family, IPv4 or IPv6</param>
        /// <returns></returns>
        private static RdmaAdapter OpenAdapter(
            RdmaProviderInfo[] providers,
            string localIpAddress,
            AddressFamily ipFamily,
            SmbdLogEvent logEvent)
        {
            RdmaAdapter adapter;

            if (providers == null)
            {
                if (logEvent != null)
                {
                    logEvent("Providers list is null. Open adapter failed.");
                }
                return null;
            }
            foreach (RdmaProviderInfo providerInfo in providers)
            {
                if (providerInfo.Provider == null)
                {
                    continue;
                }

                if (logEvent != null)
                {
                    logEvent(string.Format("Try to open adapter from provider \"{0}\" with specific IP Address: \"{1}\"",
                        providerInfo.Path,
                        localIpAddress));
                }

                OutputAddressInfoSupportedByProvider(providerInfo, logEvent);

                NtStatus status = (NtStatus)providerInfo.Provider.OpenAdapter(localIpAddress, (short)ipFamily, out adapter);
                if (status != NtStatus.STATUS_SUCCESS)
                {
                    if (logEvent != null)
                    {
                        logEvent(string.Format("Provider '{0}' does not support IP address \"{1}\".",
                            providerInfo.Path,
                            localIpAddress));
                    }
                    continue; 
                }
                if (logEvent != null)
                {
                    logEvent(string.Format("Adapter on IP address \"{0}\" is open via provider '{1}'.",
                        localIpAddress,
                        providerInfo.Path));
                }
                return adapter;
            }

            if (logEvent != null)
            {
                logEvent(string.Format("IP address \"{0}\" is not supported by all providers. Open adapter failed.", localIpAddress));
            }
            return null;
        }

        /// <summary>
        /// output all addresses which are supported by the provider
        /// </summary>
        private static void OutputAddressInfoSupportedByProvider(RdmaProviderInfo providerInfo, SmbdLogEvent logEvent)
        {
            if (logEvent == null)
            {
                return;
            }

            if(providerInfo == null)
            {
                logEvent("ProviderInfo is null.");
                return;
            }

            RdmaAddress[] addressList;
            NtStatus status = (NtStatus)providerInfo.Provider.QueryAddressList(out addressList);

            if (status != NtStatus.STATUS_SUCCESS)
            {
                logEvent(string.Format("Return code of Provider.QueryAddressList is {0}", status));
                return;
            }

            if (addressList == null)
            {
                logEvent("The address list returned from Provider.QueryAddressList is null.");
                return;
            }

            if (addressList.Length == 0)
            {
                logEvent(string.Format("No address supported by provider \"{0}\".", providerInfo.Path));
                return;
            }

            logEvent(string.Format("Total {0} addresses supported by the provider \"{1}\":\n",
                addressList.Length,
                providerInfo.Path));
            int addressIndex = 0;
            foreach (RdmaAddress address in addressList)
            {
                if (address == null)
                {
                    continue;
                }

                if ((AddressFamily)address.Family == AddressFamily.InterNetwork)
                { // IPv4
                    logEvent(string.Format("{0}. {1}: {2}.{3}.{4}.{5}\n",
                        ++addressIndex,
                        AddressFamily.InterNetwork,
                        (byte)address.Data[0],
                        (byte)address.Data[1],
                        (byte)address.Data[2],
                        (byte)address.Data[3]));
                }
                else
                {
                    logEvent(string.Format("{0}. {1}", ++addressIndex, (AddressFamily)address.Family));
                }
            }
        }

        #region Callback
        /// <summary>
        /// Notify callback
        /// </summary>
        private void NotifyCallback(Object stateInfo)
        {
            // notify main thread, NotifyCallback is started.
            threadStartSemaphore.Release();
            LogEvent("NotifyCallback is started...");

            while (true)
            {
                // Wait for submitting request or disconnection
                rdmaNotificationSemaphore.WaitOne();
                rdmaNotificationSemaphore.Release();

                if (requestCount <= 0)
                { // no request in the list, the semaphore is released from disconnection
                    //threadStopSemaphore.Release();
                    LogEvent("NotifyCallback has stopped.");
                    return;
                }

                NtStatus status = (NtStatus)rdmaCompletionQueue.Notify();
                LogEvent(string.Format("NotifyCallback: get notification with status {0}.", status));
                if (status != NtStatus.STATUS_SUCCESS)
                {
                    continue;
                }

                while (true)
                {
                    UInt64 resultId;
                    RdmaNetworkDirectResult ndResult;
                    UInt64 size = rdmaCompletionQueue.GetResult(out resultId, out ndResult);

                    if (size == 0)
                    {
                        break;
                    }
                    // WaitOne for reduce the request count
                    rdmaNotificationSemaphore.WaitOne();
                    requestCount--;

                    RequestType type = RequestType.None;
                    int segmentIndex = -1;
                    #region Receive and invalid memory window
                    lock (locker)
                    {
                        foreach (SmbdRequest request in receiveRequestList)
                        {
                            if (request.ResultId == resultId)
                            {
                                type = request.Type;
                                segmentIndex = request.EntryIndex;

                                // get the result
                                SmbdRequestResult requestResultItem = new SmbdRequestResult();
                                requestResultItem.EntryIndex = request.EntryIndex;
                                requestResultItem.ResultInfo = ndResult;
                                requestResultItem.ResultId = request.ResultId;

                                switch (request.Type)
                                {
                                    case RequestType.Receive:
                                        receiveRequestResult.Enqueue(requestResultItem);
                                        ReceivePostedCount--;
                                        break;
                                    case RequestType.Invalid:
                                        for (int i = 0; i < memoryWindowList.Count; ++i)
                                        {
                                            if (memoryWindowList[i].InvalidResultId == request.ResultId)
                                            {
                                                memoryWindowList[i].IsValid = false;
                                            }
                                        }
                                        break;
                                }

                                receiveRequestList.Remove(request);
                                break;
                            }
                        }
                    }
                    #endregion
                    if (type == RequestType.None)
                    {
                        otherRequestResult.Enqueue(
                            new SmbdRequestResult()
                            {
                                ResultId = resultId,
                                ResultInfo = ndResult
                            });

                    }

                    // log
                    this.LogEvent(
                        string.Format(
                        "1 operation {0} has been finished with result {1} and result Id: {2:X};" +
                        " Bytes of data transferred is {3}; Segment Index is {4}; Count of work items is {5}",
                            type,
                            (NtStatus)ndResult.Status,
                            resultId,
                            ndResult.BytesTransferred,
                            segmentIndex,
                            this.receiveRequestList.Count));

                }
            }
        }

        /// <summary>
        /// notify disconnect
        /// </summary>
        /// <param name="stateInfo"></param>
        private void NotifyDisconnectCallBack(Object stateInfo)
        {
            // notify the main thread, Notify Disconnec Callback is started.
            threadStartSemaphore.Release();
            LogEvent("NotifyDisconnectCallBack is started...");

            while (true)
            {
                NtStatus status = (NtStatus)rdmaConnector.NotifyDisconnect();
                LogEvent(string.Format("Get the notification with status {0}", status));
                if (isConnected == false)
                {
                    disconnectSemaphore.Release();

                    LogEvent("NotifyDisconnectCallBack has stopped.");
                    return;
                }
                if (status == NtStatus.STATUS_SUCCESS)
                {
                    lock (locker)
                    {
                        if (isConnected == false)
                        {
                            // threadStopSemaphore.Release();
                            LogEvent("NotifyDisconnectCallBack has stopped.");
                            return;
                        }
                        isConnected = false;
                    }
                    disconnectSemaphore.Release();
                    LogEvent("RDMA connection is disconnected by server.");

                    // let notify thread stop
                    rdmaNotificationSemaphore.Release();

                    LogEvent("NotifyDisconnectCallBack has stopped.");
                    return;
                }
            }
            
        }
        #endregion
        /// <summary>
        /// Complete the work of connection
        /// </summary>
        private void CompleteConnect()
        {
            #region Require resource
            receiveEntries = new ReceiveEntry[inboundEntries];
            for (int i = 0; (UInt64)i < inboundEntries; ++i)
            {
                receiveEntries[i].Segment = new RdmaSegment();
                receiveEntries[i].Segment.Length = inboundDataSize;
                receiveEntries[i].IsOccupied = false;
                NtStatus ret = (NtStatus)rdmaAdapter.RegisterMemory(
                        receiveEntries[i].Segment.Length,
                        out receiveEntries[i].Segment.MemoryHandler);
                LogEvent(string.Format("Entry 0x{0:X} register with memory handler 0x{1:X}.",
                    i,
                    receiveEntries[i].Segment.MemoryHandler));

                if (ret != NtStatus.STATUS_SUCCESS)
                {
                    inboundEntries = (uint)i;
                    continue;
                }
            }
            LogEvent(string.Format("{0} entries resource has been required.", inboundEntries));

            #endregion
            LogEvent(string.Format("Register {0} blocks memory", inboundEntries));

            isConnected = true;
            threadStartSemaphore = new Semaphore(0, THREAD_COUNT);

            ThreadPool.QueueUserWorkItem(new WaitCallback(this.NotifyDisconnectCallBack));
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.NotifyCallback));

            for (int i = 0; i < THREAD_COUNT; ++i)
            {
                threadStartSemaphore.WaitOne();
            }
            LogEvent("All callback threads has launched.");
        }

        /// <summary>
        /// log event
        /// </summary>
        /// <param name="str"></param>
        private void LogEvent(string str)
        {
            // log
            if (logEndpointEvent != null)
            {
                this.logEndpointEvent(str);
            }
        }
    }
}
