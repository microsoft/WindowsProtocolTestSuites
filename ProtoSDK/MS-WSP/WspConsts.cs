// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp
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
        public const uint Is64bitVersion = 0x00010000;

        /// <summary>
        /// Used to calculate the bitwise XOR value when calculating checksum.
        /// </summary>
        public const uint ChecksumMagicNumber = 0x59533959;
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
        /// Query Property Set
        /// </summary>
        private static readonly Guid QueryGuid = new Guid("49691C90-7E17-101A-A91C-08002B2ECDA9");
      
        /// <summary>
        /// Storage Property Set
        /// </summary>
        private static readonly Guid StorageGuid = new Guid("B725F130-47EF-101A-A5F1-02608C9EEBAC");

        /// <summary>
        /// Property Sets for Documents
        /// </summary>
        private static readonly Guid DocPropSetGuid = new Guid("F29F85E0-4FF9-1068-AB91-08002B27B3D9");

        /// <summary>
        /// Property Sets for Documents
        /// </summary>
        private static readonly Guid DocPropSetGuid2 = new Guid("D5CDD502-2E9C-101B-9397-08002B2CF9AE");

        /// <summary>
        /// Document characterization
        /// </summary>
        private static readonly Guid DocCharacterGuid = new Guid("560C36C0-503A-11CF-BAA1-00004C752A9A");

        /// <summary>
        /// Music Property Set
        /// </summary>
        private static readonly Guid PSGUID_MUSIC = new Guid("56A3372E-CE9C-11d2-9F0E-006097C686F6");

        /// <summary>
        /// Digital Rights Management
        /// </summary>
        private static readonly Guid PSGUID_DRM = new Guid("AEAC19E4-89AE-4508-B9B7-BB867ABEE2ED");

        /// <summary>
        /// Image Property Set
        /// </summary>
        private static readonly Guid PSGUID_IMAGESUMMARYINFORMATION = new Guid("6444048F-4C8B-11D1-8B70-080036B11A03");

        /// <summary>
        /// Audio Property Set
        /// </summary>
        private static readonly Guid PSGUID_AUDIO = new Guid("64440490-4C8B-11D1-8B70-080036B11A03");

        /// <summary>
        /// Video Property Set
        /// </summary>
        private static readonly Guid PSGUID_VIDEO = new Guid("64440491-4C8B-11D1-8B70-080036B11A03");

        /// <summary>
        /// Media Property Set
        /// </summary>
        private static readonly Guid PSGUID_MEDIAFILESUMMARYINFORMATION = new Guid("64440492-4C8B-11D1-8B70-080036B11A03");

        /// <summary>
        /// Mime Properties
        /// </summary>
        private static readonly Guid NNTPGuid = new Guid("AA568EEC-E0E5-11CF-8FDA-00AA00A14F93");

        /// <summary>
        /// Shell Details
        /// </summary>
        private static readonly Guid PSGUID_SHELLDETAILS = new Guid("28636AA6-953D-11D2-B5D6-00C04FD918D0");

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
        public static readonly CFullPropSpec System_ItemFolderNameDisplay = new CFullPropSpec(StorageGuid, 2);

        /// <summary>
        /// System.Search.Scope
        /// Used to narrow the scope of a query to the specified directory and subdirectories.
        /// </summary>
        public static readonly CFullPropSpec System_Search_Scope = new CFullPropSpec(StorageGuid, 22);

        /// <summary>
        /// System.Search.Contents
        /// The contents of the item. This property is for query restrictions only; it cannot be retrieved in a query result. The WSS friendly name is 'contents'.
        /// </summary>
        public static readonly CFullPropSpec System_Search_Contents = new CFullPropSpec(StorageGuid, 19);

        /// <summary>
        /// All
        /// Allows a content restriction over all textual properties. Cannot be retrieved.
        /// </summary>
        public static readonly CFullPropSpec QueryAll = new CFullPropSpec(QueryGuid, 6);

        /// <summary>
        /// System.Search.Store
        /// The identifier for the protocol handler that produced this item. (E.g. MAPI, CSC, FILE etc.)
        /// </summary>
        public static readonly CFullPropSpec System_Search_Store = new CFullPropSpec(new Guid("A06992B3-8CAF-4ED7-A547-B259E32AC9FC"), 100);

        /// <summary>
        /// System.Search.EntryID
        /// The entry ID for an item within a given catalog in the Windows Search Index. This value can be recycled, and therefore is not considered unique over time.
        /// </summary>
        public static readonly CFullPropSpec System_Search_EntryID = new CFullPropSpec(QueryGuid, 5);

        /// <summary>
        /// System.Search.HitCount
        /// When using CONTAINS over the Windows Search Index, this is the number of matches of the term. 
        /// If there are multiple CONTAINS, an AND computes the min number of hits and an OR the max number of hits.
        /// </summary>
        public static readonly CFullPropSpec System_Search_HitCount = new CFullPropSpec(QueryGuid, 4);

        /// <summary>
        /// System.Search.GatherTime
        /// The Datetime that the Windows Search Gatherer process last pushed properties of this document to the Windows Search Gatherer Plugins.
        /// </summary>
        public static readonly CFullPropSpec System_Search_GatherTime = new CFullPropSpec(new Guid("0B63E350-9CCC-11D0-BCDB-00805FCCCE04"), 8);

        /// <summary>
        /// System.Search.LastIndexedTotalTime
        /// The total time in seconds taken to index this document the last time it was indexed.
        /// </summary>
        public static readonly CFullPropSpec System_Search_LastIndexedTotalTime = new CFullPropSpec(new Guid("0B63E350-9CCC-11D0-BCDB-00805FCCCE04"), 11);

        /// <summary>
        /// System.MIMEType
        /// The MIME type. Eg, for EML files: 'message/rfc822'.
        /// </summary>
        public static readonly CFullPropSpec System_MIMEType = new CFullPropSpec(new Guid("0B63E350-9CCC-11D0-BCDB-00805FCCCE04"), 5);

        /// <summary>
        /// System.DateCreated
        /// The date and time the item was created. 
        /// </summary>
        public static readonly CFullPropSpec System_DateCreated = new CFullPropSpec(StorageGuid, 15);

        /// <summary>
        /// System.Size
        /// The size, in bytes, of the file.
        /// </summary>
        public static readonly CFullPropSpec System_Size = new CFullPropSpec(StorageGuid, 12);

        /// <summary>
        /// System.FileName
        /// This is the file name (including extension) of the file. 
        /// </summary>
        public static readonly CFullPropSpec System_FileName = new CFullPropSpec(new Guid("41CF5AE0-F75A-4806-BD87-59C7D9248EB9"), 100);

        /// <summary>
        /// System.FileExtension
        /// This is the file extension of the file-based item, including the leading period
        /// </summary>
        public static readonly CFullPropSpec System_FileExtension = new CFullPropSpec(new Guid("E4F10A3C-49E6-405D-8288-A23BD4EEAA6C"), 100);

        /// <summary>
        /// System.Path
        /// The full physical path to the file, including the file name.
        /// </summary>
        public static readonly CFullPropSpec Path = new CFullPropSpec(StorageGuid, 11);

        /// <summary>
        /// System.Author
        /// Represents the author or authors of the document
        /// </summary>
        public static readonly CFullPropSpec System_Author = new CFullPropSpec(DocPropSetGuid, 4);

        /// <summary>
        /// ClassId
        /// The class ID of the object, for example, WordPerfect or Microsoft® Office Word.
        /// </summary>
        public static readonly CFullPropSpec ClassId = new CFullPropSpec(StorageGuid, 3);

        /// <summary>
        /// FileIndex
        /// The unique ID of the file.
        /// </summary>
        public static readonly CFullPropSpec FileIndex = new CFullPropSpec(StorageGuid, 8);

        /// <summary>
        /// System.ItemNameDisplay
        /// The name of the file.
        /// </summary>
        public static readonly CFullPropSpec System_ItemNameDisplay = new CFullPropSpec(StorageGuid, 10);

        /// <summary>
        /// System.FileAttributes
        /// The file attributes. Documented in Win32 SDK.
        /// </summary>
        public static readonly CFullPropSpec System_FileAttributes = new CFullPropSpec(StorageGuid, 13);

        /// <summary>
        /// System.DateModified
        /// The date and time of the last modification to the item. 
        /// The Indexing Service friendly name is 'write'.
        /// </summary>
        public static readonly CFullPropSpec System_DateModified = new CFullPropSpec(StorageGuid, 14);

        /// <summary>
        /// System.DateAccessed
        /// The last time the file was accessed.
        /// </summary>
        public static readonly CFullPropSpec System_DateAccessed = new CFullPropSpec(StorageGuid, 16);

        /// <summary>
        /// System.Kind
        /// System.Kind is used to map extensions to various .Search folders.
        /// </summary>
        public static readonly CFullPropSpec System_Kind = new CFullPropSpec(new Guid("1E3EE840-BC2B-476C-8237-2ACD1A839B22"), 3);

        /// <summary>
        /// System.KindText
        /// This is the user-friendly form of System.Kind.
        /// </summary>
        public static readonly CFullPropSpec System_KindText = new CFullPropSpec(new Guid("F04BEF95-C585-4197-A2B7-DF46FDC9EE6D"), 100);

        /// <summary>
        /// System.Document.CharacterCount
        /// The number of characters in the document.
        /// </summary>
        public static readonly CFullPropSpec System_Document_CharacterCount = new CFullPropSpec(DocPropSetGuid, 16);

        /// <summary>
        /// System.Title
        /// The title of the document.
        /// </summary>
        public static readonly CFullPropSpec System_Title = new CFullPropSpec(DocPropSetGuid, 2);

        /// <summary>
        /// System.Subject
        /// The subject of the document.
        /// </summary>
        public static readonly CFullPropSpec System_Subject = new CFullPropSpec(DocPropSetGuid, 3);

        /// <summary>
        /// System.Keywords
        /// The document keywords.
        /// </summary>
        public static readonly CFullPropSpec System_Keywords = new CFullPropSpec(DocPropSetGuid, 5);

        /// <summary>
        /// System.Document.RevisionNumber
        /// The current version number of the document.
        /// </summary>
        public static readonly CFullPropSpec System_Document_RevisionNumber = new CFullPropSpec(DocPropSetGuid, 9);

        /// <summary>
        /// System.Document.DateCreated
        /// The date the document was created.
        /// </summary>
        public static readonly CFullPropSpec System_Document_DateCreated = new CFullPropSpec(DocPropSetGuid, 12);

        /// <summary>
        /// System.ApplicationName
        /// The name of the application that created the file.
        /// </summary>
        public static readonly CFullPropSpec System_ApplicationName = new CFullPropSpec(DocPropSetGuid, 18);

        /// <summary>
        /// System.Category
        /// The type of document, such as a memo, schedule, or white paper.
        /// </summary>
        public static readonly CFullPropSpec System_Category = new CFullPropSpec(DocPropSetGuid2, 2);

        /// <summary>
        /// System.Documnet.ByteCount
        /// The number of bytes in a document.
        /// </summary>
        public static readonly CFullPropSpec System_Document_ByteCount = new CFullPropSpec(DocPropSetGuid2, 4);

        /// <summary>
        /// DocPartTitles
        /// The names of document parts.
        /// </summary>
        public static readonly CFullPropSpec DocPartTitles = new CFullPropSpec(DocPropSetGuid2, 13);

        /// <summary>
        /// System.ContentType
        /// </summary>
        public static readonly CFullPropSpec System_ContentType = new CFullPropSpec(DocPropSetGuid2, 26);

        /// <summary>
        /// System.Search.Autosummary
        /// A characterization or abstract of the document.
        /// </summary>
        public static readonly CFullPropSpec System_Search_Autosummary = new CFullPropSpec(DocCharacterGuid, 2);

        /// <summary>
        /// System.Music.Artist
        /// The artist who recorded the song.
        /// </summary>
        public static readonly CFullPropSpec System_Music_Artist = new CFullPropSpec(PSGUID_MUSIC, 2);

        /// <summary>
        /// System.Music.Album
        /// The album on which the song was released.
        /// </summary>
        public static readonly CFullPropSpec System_Music_Album = new CFullPropSpec(PSGUID_MUSIC, 4);

        /// <summary>
        /// System.Music.Genre
        /// The song's genre.
        /// </summary>
        public static readonly CFullPropSpec System_Music_Genre = new CFullPropSpec(PSGUID_MUSIC, 11);

        /// <summary>
        /// System.Media.Year
        /// The year that the song was published.
        /// </summary>
        public static readonly CFullPropSpec System_Media_Year = new CFullPropSpec(PSGUID_MUSIC, 5);

        /// <summary>
        /// System.Music.TrackNumber
        /// The track number for the song.
        /// </summary>
        public static readonly CFullPropSpec System_Music_TrackNumber = new CFullPropSpec(PSGUID_MUSIC, 7);

        /// <summary>
        /// System.DRM.IsProtected
        /// TRUE if there is a license.
        /// </summary>
        public static readonly CFullPropSpec System_DRM_IsProtected = new CFullPropSpec(PSGUID_DRM, 2);

        /// <summary>
        /// ImageFileType
        /// The type of image file.
        /// </summary>
        public static readonly CFullPropSpec ImageFileType = new CFullPropSpec(PSGUID_IMAGESUMMARYINFORMATION, 2);

        /// <summary>
        /// ImageColorSpace
        /// The description of the image color space.
        /// </summary>
        public static readonly CFullPropSpec ImageColorSpace = new CFullPropSpec(PSGUID_IMAGESUMMARYINFORMATION, 8);

        /// <summary>
        /// System.Image.HorizontalSize
        /// The horizontal size, in pixels.
        /// </summary>
        public static readonly CFullPropSpec System_Image_HorizontalSize = new CFullPropSpec(PSGUID_IMAGESUMMARYINFORMATION, 3);

        /// <summary>
        /// System.Image.HorizontalResolution
        /// The horizontal resolution, in pixels per inch.
        /// </summary>
        public static readonly CFullPropSpec System_Image_HorizontalResolution = new CFullPropSpec(PSGUID_IMAGESUMMARYINFORMATION, 5);

        /// <summary>
        /// System.Image.BitDepth
        /// The number of bits per pixel.
        /// </summary>
        public static readonly CFullPropSpec System_Image_BitDepth = new CFullPropSpec(PSGUID_IMAGESUMMARYINFORMATION, 7);

        /// <summary>
        /// System.Image.Dimensions
        /// The description of the image dimensions.
        /// </summary>
        public static readonly CFullPropSpec System_Image_Dimensions = new CFullPropSpec(PSGUID_IMAGESUMMARYINFORMATION, 13);

        /// <summary>
        /// System.Image.VerticalSize
        /// The vertical size, in pixels.
        /// </summary>
        public static readonly CFullPropSpec System_Image_VerticalSize = new CFullPropSpec(PSGUID_IMAGESUMMARYINFORMATION, 4);

        /// <summary>
        /// System.Image.VerticalResolution
        /// The vertical resolution, in pixels per inch.
        /// </summary>
        public static readonly CFullPropSpec System_Image_VerticalResolution = new CFullPropSpec(PSGUID_IMAGESUMMARYINFORMATION, 6);

        /// <summary>
        /// System.Media.FrameCount
        /// The frame count for the image.
        /// </summary>
        public static readonly CFullPropSpec System_Media_FrameCount = new CFullPropSpec(PSGUID_IMAGESUMMARYINFORMATION, 12);

        /// <summary>
        /// ImageCompression
        /// The description of the image compression.
        /// </summary>
        public static readonly CFullPropSpec ImageCompression = new CFullPropSpec(PSGUID_IMAGESUMMARYINFORMATION, 9);

        /// <summary>
        /// AudioFormat
        /// The audio format.
        /// </summary>
        public static readonly CFullPropSpec AudioFormat = new CFullPropSpec(PSGUID_AUDIO, 2);

        /// <summary>
        /// System.Media.Duration
        /// The duration, in 100-ns units.
        /// </summary>
        public static readonly CFullPropSpec System_Media_Duration = new CFullPropSpec(PSGUID_AUDIO, 3);

        /// <summary>
        /// System.Audio.EncodingBitrate
        /// The average encoding rate, in bits per second.
        /// </summary>
        public static readonly CFullPropSpec System_Audio_EncodingBitrate = new CFullPropSpec(PSGUID_AUDIO, 4);

        /// <summary>
        /// System.Audio.SampleRate
        /// The sample rate, in samples per second.
        /// </summary>
        public static readonly CFullPropSpec System_Audio_SampleRate = new CFullPropSpec(PSGUID_AUDIO, 5);

        /// <summary>
        /// System.Audio.SampleSize
        /// The sample size, in bits per sample.
        /// </summary>
        public static readonly CFullPropSpec System_Audio_SampleSize = new CFullPropSpec(PSGUID_AUDIO, 6);

        /// <summary>
        /// System.Audio.ChannelCount
        /// The number of channels of audio.
        /// </summary>
        public static readonly CFullPropSpec System_Audio_ChannelCount = new CFullPropSpec(PSGUID_AUDIO, 7);

        /// <summary>
        /// System.Video.StreamName
        /// The name of the stream.
        /// </summary>
        public static readonly CFullPropSpec System_Video_StreamName = new CFullPropSpec(PSGUID_VIDEO, 2);

        /// <summary>
        /// System.Video.FrameWidth
        /// The width, in pixels, of a frame.
        /// </summary>
        public static readonly CFullPropSpec System_Video_FrameWidth = new CFullPropSpec(PSGUID_VIDEO, 3);

        /// <summary>
        /// System.Video.FrameHeight
        /// The height, in pixels, of a frame.
        /// </summary>
        public static readonly CFullPropSpec System_Video_FrameHeight = new CFullPropSpec(PSGUID_VIDEO, 4);

        /// <summary>
        /// System.Video.EncodingBitrate
        /// The bits per second.
        /// </summary>
        public static readonly CFullPropSpec System_Video_EncodingBitrate = new CFullPropSpec(PSGUID_VIDEO, 8);

        /// <summary>
        /// VideoTimeLength
        /// The duration, in 100-ns units.
        /// </summary>
        public static readonly CFullPropSpec VideoTimeLength = new CFullPropSpec(PSGUID_VIDEO, 5);

        /// <summary>
        /// System.Video.FrameRate
        /// Indicates the frame rate in "frames per millisecond" for the video stream.
        /// </summary>
        public static readonly CFullPropSpec System_Video_FrameRate = new CFullPropSpec(PSGUID_VIDEO, 6);

        /// <summary>
        /// System.Video.SampleSize
        /// The bits per sample.
        /// </summary>
        public static readonly CFullPropSpec System_Video_SampleSize = new CFullPropSpec(PSGUID_VIDEO, 9);

        /// <summary>
        /// System.Video.Compression
        /// The description of video compression.
        /// </summary>
        public static readonly CFullPropSpec System_Video_Compression = new CFullPropSpec(PSGUID_VIDEO, 10);

        /// <summary>
        /// System.Video.TotalBitrate
        /// Indicates the total data rate in "bits per second" for all video and audio streams.
        /// </summary>
        public static readonly CFullPropSpec System_Video_TotalBitrate = new CFullPropSpec(PSGUID_VIDEO, 43);

        /// <summary>
        /// System.Video.FourCC
        /// Indicates the 4CC for the video stream.
        /// </summary>
        public static readonly CFullPropSpec System_Video_FourCC = new CFullPropSpec(PSGUID_VIDEO, 44);

        /// <summary>
        /// System.Video.Director
        /// Indicates the person who directed the video.
        /// </summary>
        public static readonly CFullPropSpec System_Video_Director = new CFullPropSpec(PSGUID_MEDIAFILESUMMARYINFORMATION, 20);

        /// <summary>
        /// MsgNewsgroup
        /// The newsgroup for the message.
        /// </summary>
        public static readonly CFullPropSpec MsgNewsgroup = new CFullPropSpec(NNTPGuid, 2);

        /// <summary>
        /// MsgSubject
        /// The subject of the message.
        /// </summary>
        public static readonly CFullPropSpec MsgSubject = new CFullPropSpec(NNTPGuid, 5);

        /// <summary>
        /// MsgFrom
        /// Who sent the message.
        /// </summary>
        public static readonly CFullPropSpec MsgFrom = new CFullPropSpec(NNTPGuid, 6);

        /// <summary>
        /// MsgDate
        /// When the message was sent.
        /// </summary>
        public static readonly CFullPropSpec MsgDate = new CFullPropSpec(NNTPGuid, 12);

        /// <summary>
        /// System.Contact.HomeTelephone
        /// </summary>
        public static readonly CFullPropSpec System_Contact_HomeTelephone = new CFullPropSpec(new Guid("176DC63C-2688-4E89-8143-A347800F25E9"), 20);

        /// <summary>
        /// System.Contact.EmailAddress
        /// </summary>
        public static readonly CFullPropSpec System_Contact_EmailAddress = new CFullPropSpec(new Guid("F8FA7FA3-D12B-4785-8A4E-691A94F7A3E7"), 100);

        /// <summary>
        /// System.Contact.FullName
        /// </summary>
        public static readonly CFullPropSpec System_Contact_FullName = new CFullPropSpec(new Guid("635E9051-50A5-4BA2-B9DB-4ED056C77296"), 100);

        /// <summary>
        /// System.Contact.NickName
        /// </summary>
        public static readonly CFullPropSpec System_Contact_NickName = new CFullPropSpec(new Guid("176DC63C-2688-4E89-8143-A347800F25E9"), 74);

        /// <summary>
        /// System.Contact.HomeAddressStreet
        /// </summary>
        public static readonly CFullPropSpec System_Contact_HomeAddressStreet = new CFullPropSpec(new Guid("0ADEF160-DB3F-4308-9A21-06237B16FA2A"), 100);

        /// <summary>
        /// System.Rating
        /// Indicates the users preference rating of an item on a scale of 1-99.
        /// </summary>
        public static readonly CFullPropSpec System_Rating = new CFullPropSpec(PSGUID_MEDIAFILESUMMARYINFORMATION, 9);

        /// <summary>
        /// System.ParentalRating
        /// The parental rating stored in a format typically determined by the organization named in System.ParentalRatingsOrganization.
        /// </summary>
        public static readonly CFullPropSpec System_ParentalRating = new CFullPropSpec(PSGUID_MEDIAFILESUMMARYINFORMATION, 21);

        /// <summary>
        /// System.Media.Writer
        /// </summary>
        public static readonly CFullPropSpec System_Media_Writer = new CFullPropSpec(PSGUID_MEDIAFILESUMMARYINFORMATION, 23);

        /// <summary>
        /// System.Media.DateEncoded
        /// DateTime is in UTC (in the doc, not file system).
        /// </summary>
        public static readonly CFullPropSpec System_Media_DateEncoded = new CFullPropSpec(new Guid("2E4B640D-5019-46D8-8881-55414CC5CAA0"), 100);

        /// <summary>
        /// System.ComputerName
        /// </summary>
        public static readonly CFullPropSpec System_ComputerName = new CFullPropSpec(PSGUID_SHELLDETAILS, 5);

        /// <summary>
        /// System.ItemPathDisplayNarrow
        /// This is the user-friendly display path to the item. 
        /// </summary>
        public static readonly CFullPropSpec System_ItemPathDisplayNarrow = new CFullPropSpec(PSGUID_SHELLDETAILS, 8);

        /// <summary>
        /// System.ItemType
        /// This is the canonical type of the item.
        /// </summary>
        public static readonly CFullPropSpec System_ItemType = new CFullPropSpec(PSGUID_SHELLDETAILS, 11);

        /// <summary>
        /// System.ParsingName
        /// The shell namespace name of an item relative to a parent folder. 
        /// </summary>
        public static readonly CFullPropSpec System_ParsingName = new CFullPropSpec(PSGUID_SHELLDETAILS, 24);
        #endregion
    }
}
