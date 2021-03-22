// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestManager.PTMService.Abstractions.Kernel
{
    /// <summary>
    /// Interface of test run.
    /// </summary>
    public interface ITestRun
    {
        /// <summary>
        /// The Id of test run.
        /// </summary>
        int Id { get; }

        /// <summary>
        /// The configuration of test run.
        /// </summary>
        IConfiguration Configuration { get; }

        /// <summary>
        /// The storage root of test run.
        /// </summary>
        IStorageNode StorageRoot { get; }

        /// <summary>
        /// Abort the test run.
        /// </summary>
        void Abort();
    }
}
