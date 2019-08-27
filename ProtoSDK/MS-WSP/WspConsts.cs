// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.WSP
{
    /// <summary>
    /// WSP Constants.
    /// </summary>
    public class WspConsts
    {
        #region Magic numbers
        /// <summary>
        /// Used to test whether the client/server version is 32bit/64-bit (set/not set).
        /// </summary>
        public const UInt32 Is64bitVersion = 0x00010000;

        /// <summary>
        /// Used to calculate the bitwise XOR value when calculating checksum.
        /// </summary>
        public const UInt32 ChecksumMagicNumber = 0x59533959;
        #endregion

        #region GUID definitions
        /// <summary>
        /// Empty GUID.
        /// </summary>
        public static readonly Guid EmptyGuid = Guid.Empty;

        /// <summary>
        /// File system content index framework property set.
        /// </summary>
        public static readonly Guid DBPROPSET_FSCIFRMWRK_EXT = new Guid("A9BD1526-6A80-11D0-8C9D-0020AF1D740E");

        /// <summary>
        /// Content index framework core property set.
        /// </summary>
        public static readonly Guid DBPROPSET_CIFRMWRKCORE_EXT = new Guid("AFAFACA5-B5D1-11D0-8C62-00C04FC2DB8D");
        #endregion

        #region Properties
        /// <summary>
        /// System.ItemName
        /// This is the base-name of the System.ItemNameDisplay.
        /// If the item is a file this property includes the extension in all cases, and will be localized if a localized name is available.
        /// If the item is a message, then the value of this property does not include the forwarding or reply prefixes (see System.ItemNamePrefix).
        /// </summary>
        public static readonly CFullPropSpec System_ItemName = new CFullPropSpec(new Guid("6B8DA074-3B5C-43BC-886F-0A2CDCE00B6F"), 100);

        /// <summary>
        /// System.ItemFolderNameDisplay
        /// This is the user-friendly display name of the parent folder of an item. 
        /// </summary>
        public static readonly CFullPropSpec System_ItemFolderNameDisplay = new CFullPropSpec(new Guid("B725F130-47EF-101A-A5F1-02608C9EEBAC"), 2);

        /// <summary>
        /// System.Search.Scope
        /// Used to narrow the scope of a query to the specified directory and subdirectories.
        /// </summary>
        public static readonly CFullPropSpec System_Search_Scope = new CFullPropSpec(new Guid("B725F130-47EF-101A-A5F1-02608C9EEBAC"), 22);

        /// <summary>
        /// System.Search.Contents
        /// The contents of the item. This property is for query restrictions only; it cannot be retrieved in a query result. The WSS friendly name is 'contents'.
        /// </summary>
        public static readonly CFullPropSpec System_Search_Contents = new CFullPropSpec(new Guid("B725F130-47EF-101A-A5F1-02608C9EEBAC"), 19);

        /// <summary>
        /// All
        /// Allows a content restriction over all textual properties. Cannot be retrieved.
        /// </summary>
        public static readonly CFullPropSpec QueryAll = new CFullPropSpec(new Guid("49691C90-7E17-101A-A91C-08002B2ECDA9"), 6);


        #endregion
    }
}
