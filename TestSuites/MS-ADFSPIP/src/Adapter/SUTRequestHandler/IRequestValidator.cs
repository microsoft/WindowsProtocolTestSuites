// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.Identity.ADFSPIP
{
    /// <summary>
    /// Derived classes from IRequestHandler might also 
    /// implement this interface to verify the HTTP request.
    /// </summary>
    public interface IRequestValidator
    {
        /// <summary>
        /// Verify the client request.
        /// </summary>
        /// <param name="message">
        /// The message returned from the verification process.
        /// The message might be error happened, or something else.
        /// </param>
        /// <returns>
        /// True is the client request is valid; False, otherwise.
        /// </returns>
        bool VerifyRequest(out string message);
    }
}
