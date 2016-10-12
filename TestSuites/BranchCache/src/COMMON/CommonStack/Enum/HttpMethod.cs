// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.CommonStack
{
    /// <summary>
    /// The HTTP method.
    /// </summary>
    public enum HttpMethod
    {
        /// <summary>
        /// The GET method means retrieve whatever information (in the form of an
        /// entity) is identified by the Request-URI. If the Request-URI refers
        /// to a data-producing process, it is the produced data which shall be
        /// returned as the entity in the response and not the source text of the
        /// process, unless that text happens to be the output of the process.          
        /// </summary>
        GET,

        /// <summary>
        /// The POST method is used to request that the origin server accept the
        /// entity enclosed in the request as a new subordinate of the resource
        /// identified by the Request-URI in the Request-Line.
        /// </summary>
        POST,

        /// <summary>
        /// The PUT method requests that the enclosed entity be stored under the
        /// supplied Request-URI. If the Request-URI refers to an already
        /// existing resource, the enclosed entity SHOULD be considered as a
        /// modified version of the one residing on the origin server. If the
        /// Request-URI does not point to an existing resource, and that URI is
        /// capable of being defined as a new resource by the requesting user
        /// agent, the origin server can create the resource with that URI.
        /// </summary>
        PUT
    }
}
