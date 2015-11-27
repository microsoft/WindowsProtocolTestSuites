// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.FileSharing.SMB2Model.Adapter.AppInstanceId
{
    public enum AppInstanceIdType
    {
        /// <summary>
        /// No AppInstanceId context in create request
        /// </summary>
        NoAppInstanceId,  

        /// <summary>
        /// AppInstanceId is included, value is 0
        /// </summary>
        AppInstanceIdIsZero,

        /// <summary>
        /// AppInstanceId is included, value is not zero or it is the same with the AppInstanceId in the first create request
        /// </summary>
        ValidAppInstanceId,

        /// <summary>
        /// AppInstanceId is included, value is not the same with the AppInstanceId in the first create request
        /// </summary>
        InvalidAppInstanceId
    }

    public enum CreateType
    {
        /// <summary>
        /// No context is included in create request besides AppInstanceId context
        /// </summary>
        NoContext,

        /// <summary>
        /// SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 is included in create request besides AppInstanceId context
        /// </summary>
        CreateDurable,

        /// <summary>
        /// SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 is included in create request besides AppInstanceId context
        /// </summary>
        ReconnectDurable,

        /// <summary>
        /// SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 is included in create request besides AppInstanceId context
        /// Then send disconnect to server
        /// </summary>
        CreateDurableThenDisconnect,

        /// <summary>
        /// Other context other than SMB2_CREATE_DURABLE_HANDLE_REQUEST_V2 or SMB2_CREATE_DURABLE_HANDLE_RECONNECT_V2 is included
        /// in create request besides AppInstanceId context
        /// </summary>
        OtherContext,
    }


    public enum ShareType
    {
        SameShare,
        DifferentShareSameLocal,
        DifferentShareDifferentLocal
    }

    // Use enum instead of boolean to make log more readable.
    public enum GrantedAccessType
    {
        IncludeFileGenericRead,
        NotIncludeFileGenericRead
    }

    public enum PathNameType
    {
        SamePathName,
        DifferentPathName
    }

    public enum ClientGuidType
    {
        SameClientGuid,
        DifferentClientGuid
    }

    public enum OpenStatus
    {
        OpenClosed,
        OpenNotClosed
    }

    public class AppInstanceIdModelOpen
    {
        public AppInstanceIdType AppInstanceId { get; set; }

        public CreateType CreateTypeWhenPrepare { get; set; }

        public override string ToString()
        {
            StringBuilder outputInfo = new StringBuilder();
            outputInfo.AppendFormat("{0}: \r\n", "AppInstanceIdModelOpen State");
            outputInfo.AppendFormat("{0}: {1} \r\n", "AppInstanceId", this.AppInstanceId.ToString());
            outputInfo.AppendFormat("{0}: {1} \r\n", "CreateTypeWhenPrepare", this.CreateTypeWhenPrepare.ToString());

            return outputInfo.ToString();
        }
    }
}