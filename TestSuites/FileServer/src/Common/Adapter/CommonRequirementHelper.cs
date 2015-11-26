// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestSuites.FileSharing.Common.Adapter
{
    /// <summary>
    /// Present a requirement with an Id and Description
    /// </summary>
    public struct Requirement
    {
        /// <summary>
        /// Id of the requirement
        /// </summary>
        public int Id;

        /// <summary>
        /// Description of the requirement
        /// </summary>
        public string Description;
    }

    /// <summary>
    /// Requirement category for each situation that might be ignored
    /// </summary>
    public static class RequirementCategory
    {
        public static readonly Requirement MUST_BE_ZERO =
            new Requirement { Id = 0, Description = "This field MUST be set to ZERO" };

        public static readonly Requirement MUST_BE_SPECIFIED_VALUE =
            new Requirement { Id = 1, Description = "This field MUST be set to the specified value" };

        public static readonly Requirement STATUS_CANCELLED =
            new Requirement { Id = 2, Description = "Server should return error STATUS_CANCELLED" };

        public static readonly Requirement STATUS_FILE_LOCK_CONFLICT =
            new Requirement { Id = 3, Description = "Server should return error STATUS_FILE_LOCK_CONFLICT" };

        public static readonly Requirement STATUS_ACCESS_DENIED =
            new Requirement { Id = 4, Description = "Server should return error STATUS_ACCESS_DENIED" };

        public static readonly Requirement STATUS_SHARING_VIOLATION =
            new Requirement { Id = 5, Description = "Server should return error STATUS_SHARING_VIOLATION" };

        public static readonly Requirement STATUS_REQUEST_NOT_ACCEPTED =
            new Requirement { Id = 6, Description = "Server should return error STATUS_REQUEST_NOT_ACCEPTED" };

        public static readonly Requirement STATUS_OBJECT_NAME_NOT_FOUND =
            new Requirement { Id = 7, Description = "Server should return error STATUS_OBJECT_NAME_NOT_FOUND" };

        public static readonly Requirement STATUS_DIRECTORY_NOT_EMPTY =
            new Requirement { Id = 8, Description = "Server should return error STATUS_DIRECTORY_NOT_EMPTY" };

        public static readonly Requirement STATUS_FILE_CLOSED =
            new Requirement { Id = 9, Description = "Server should return error STATUS_FILE_CLOSED" };

        public static readonly Requirement STATUS_OFFLOAD_READ_FLT_NOT_SUPPORTED =
            new Requirement { Id = 10, Description = "Server should return error STATUS_OFFLOAD_READ_FLT_NOT_SUPPORTED." };

        public static readonly Requirement STATUS_NOT_SUPPORTED =
            new Requirement { Id = 11, Description = "Server should return error STATUS_NOT_SUPPORTED." };

        public static readonly Requirement STATUS_NO_RANGES_PROCESSED =
            new Requirement { Id = 12, Description = "Server should return error STATUS_NO_RANGES_PROCESSED." };

        public static readonly Requirement STATUS_INVALID_PARAMETER =
            new Requirement { Id = 13, Description = "Server should return error STATUS_INVALID_PARAMETER." };        
    }
}
