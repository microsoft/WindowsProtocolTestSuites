// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Protocols.TestTools;
using Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Wsp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Microsoft.Protocols.TestSuites.WspTS
{
    public partial class CPMCreateQueryTestCases : WspCommonTestBase
    {
        #region Test Cases

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.ItemFolderNameDisplay property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Storage_System_ItemFolderNameDisplay()
        {
            var queryResult = GetQueryResult(WspConsts.System_ItemFolderNameDisplay, "document");

            foreach (var row in queryResult.Rows)
            {
                var fileName = row.Columns[2].Data as string;
                Site.Assert.AreEqual("CreateQuery_CFullPropSpec", row.Columns[1].Data as string, $"The System.ItemFolderNameDisplay of {fileName} should be CreateQuery_CFullPropSpec");
            }
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if Path property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Storage_Path()
        {
            var queryPath = Site.Properties.Get("QueryPath") + "Data/CreateQuery_CFullPropSpec";
            var queryResult = GetQueryResult(WspConsts.Path, "document", queryPath: queryPath);

            var expectedResults = new string[]
            {
                $"{queryPath}/document1.doc",
                $"{queryPath}/document2.ppt",
                $"{queryPath}/document3.xls"
            };

            ValidateQueryResult(nameof(WspConsts.Path), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.ItemNameDisplay property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Storage_System_ItemNameDisplay()
        {
            var queryResult = GetQueryResult(WspConsts.System_ItemNameDisplay, "document");

            var expectedResults = new string[]
            {
                "document1.doc",
                "document2.ppt",
                "document3.xls"
            };

            ValidateQueryResult(nameof(WspConsts.System_ItemNameDisplay), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Size property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Storage_System_Size()
        {
            var queryResult = GetQueryResult(WspConsts.System_Size, "document");

            var expectedResults = new ulong[]
            {
                document1Size,
                document2Size,
                document3Size
            };

            ValidateQueryResult(nameof(WspConsts.System_Size), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.FileAttributes property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Storage_System_FileAttributes()
        {
            for (var i = 1; i <= 3; i++)
            {
                var fileName = $"..\\..\\Data\\CreateQuery_CFullPropSpec\\attr{i}.txt";
                wspSutAdapter.ModifyFileAttributes(fileName, i == 2, i == 3);
            }

            // Retry to wait for the index update.
            var retryTimes = 5;
            while (retryTimes-- > 0)
            {
                try
                {
                    var queryResult = GetQueryResult(WspConsts.System_FileAttributes, "attr");

                    var expectedResults = new uint[]
                    {
                        32, // Archive file
                        33, // Read-only file
                        34  // Hidden file
                    };

                    ValidateQueryResult(nameof(WspConsts.System_FileAttributes), expectedResults, queryResult);

                    break;
                }
                catch (AssertFailedException)
                {
                    if (retryTimes <= 0)
                    {
                        throw;
                    }
                    else
                    {
                        wspAdapter.CPMDisconnect();
                        Thread.Sleep(5000);
                    }
                }
            }
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.DateModified property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Storage_System_DateModified()
        {
            var queryResult = GetQueryResult(WspConsts.System_DateModified, "document");

            foreach (var row in queryResult.Rows)
            {
                Site.Assert.IsTrue(row.Columns[1].Data is DateTime, $"The System.DateModified column should be in DateTime form.");
            }
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.DateAccessed property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Storage_System_DateAccessed()
        {
            var queryResult = GetQueryResult(WspConsts.System_DateAccessed, "document");

            foreach (var row in queryResult.Rows)
            {
                Site.Assert.IsTrue(row.Columns[1].Data is DateTime, $"The System.DateAccessed column should be in DateTime form.");
            }
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Kind property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Storage_System_Kind()
        {
            var queryResult = GetQueryResult(WspConsts.System_Kind, "kind", totalRows: 6);

            var expectedResults = new string[][]
            {
                new string[] { "document" },
                new string[] { "picture" },
                new string[] { "music" },
                new string[] { "video" },
                new string[] { "contact", "communication" },
                new string[] { "email", "communication" }
            };

            ValidateQueryResult(nameof(WspConsts.System_Kind), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.KindText property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Storage_System_KindText()
        {
            var queryResult = GetQueryResult(WspConsts.System_KindText, "kind", totalRows: 6);

            var expectedResults = new string[]
            {
                "Document",
                "Picture",
                "Music",
                "Video",
                "Contact; Communication",
                "E-mail; Communication"
            };

            ValidateQueryResult(nameof(WspConsts.System_KindText), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Category property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Documents_System_Category()
        {
            var queryResult = GetQueryResult(WspConsts.System_Category, "document");

            var expectedResults = new string[][]
            {
                new string[] { "memo" },
                new string[] { "schedule" },
                new string[] { "white paper" }
            };

            ValidateQueryResult(nameof(WspConsts.System_Category), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Title property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Documents_System_Title()
        {
            var queryResult = GetQueryResult(WspConsts.System_Title, "document");

            var expectedResults = new string[]
            {
                "Very Important Text",
                "Very Important Slide",
                "Very Important Chart"
            };

            ValidateQueryResult(nameof(WspConsts.System_Title), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.ApplicationName property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Documents_System_ApplicationName()
        {
            var queryResult = GetQueryResult(WspConsts.System_ApplicationName, "document");

            var expectedResults = new string[]
            {
                "Microsoft Office Word",
                "Microsoft Office PowerPoint",
                "Microsoft Excel"
            };

            ValidateQueryResult(nameof(WspConsts.System_ApplicationName), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Subject property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Documents_System_Subject()
        {
            var queryResult = GetQueryResult(WspConsts.System_Subject, "document");

            var expectedResults = new string[]
            {
                "Important Text",
                "Important Slide",
                "Important Chart"
            };

            ValidateQueryResult(nameof(WspConsts.System_Subject), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Keywords property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Documents_System_Keywords()
        {
            var queryResult = GetQueryResult(WspConsts.System_Keywords, "document");

            var expectedResults = new string[][]
            {
                new string[] { "important text" },
                new string[] { "important slide" },
                new string[] { "important chart" }
            };

            ValidateQueryResult(nameof(WspConsts.System_Keywords), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Document.DateCreated property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Documents_System_Document_DateCreated()
        {
            var queryResult = GetQueryResult(WspConsts.System_Document_DateCreated, "document");

            foreach (var row in queryResult.Rows)
            {
                Site.Assert.IsTrue(row.Columns[1].Data is DateTime, $"The System.Document.DateCreated column should be in DateTime form.");
            }
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.ContentType property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Documents_System_ContentType()
        {
            var queryResult = GetQueryResult(WspConsts.System_ContentType, "img");

            var expectedResults = new string[]
            {
                "image/bmp",
                "image/jpeg",
                "image/bmp"
            };

            ValidateQueryResult(nameof(WspConsts.System_ContentType), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Music.Artist property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Music_System_Music_Artist()
        {
            var queryResult = GetQueryResult(WspConsts.System_Music_Artist, "music");

            var expectedResults = new string[][]
            {
                new string[] { "AAA" },
                new string[] { "BBB" },
                new string[] { "CCC" },
            };

            ValidateQueryResult(nameof(WspConsts.System_Music_Artist), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Music.Album property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Music_System_Music_Album()
        {
            var queryResult = GetQueryResult(WspConsts.System_Music_Album, "music");

            var expectedResults = new string[]
            {
                "WSPTest1",
                "WSPTest2",
                "WSPTest3",
            };

            ValidateQueryResult(nameof(WspConsts.System_Music_Album), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Music.Genre property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Music_System_Music_Genre()
        {
            var queryResult = GetQueryResult(WspConsts.System_Music_Genre, "music");

            var expectedResults = new string[][]
            {
                new string[] { "Pop" },
                new string[] { "Dance" },
                new string[] { "Rock" },
            };

            ValidateQueryResult(nameof(WspConsts.System_Music_Genre), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Media.Year property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Music_System_Media_Year()
        {
            var queryResult = GetQueryResult(WspConsts.System_Media_Year, "music");

            var expectedResults = new uint[]
            {
                1997,
                2007,
                2017,
            };

            ValidateQueryResult(nameof(WspConsts.System_Media_Year), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Music.TrackNumber property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Music_System_Music_TrackNumber()
        {
            var queryResult = GetQueryResult(WspConsts.System_Music_TrackNumber, "music");

            var expectedResults = new uint[]
            {
                15,
                4,
                11,
            };

            ValidateQueryResult(nameof(WspConsts.System_Music_TrackNumber), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.DRM.IsProtected property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_DRM_System_DRM_IsProtected()
        {
            var queryResult = GetQueryResult(WspConsts.System_DRM_IsProtected, "music");

            var expectedResults = new ushort[]
            {
                0,
                0,
                0,
            };

            ValidateQueryResult(nameof(WspConsts.System_DRM_IsProtected), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Image.HorizontalResolution property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Image_System_Image_HorizontalResolution()
        {
            var queryResult = GetQueryResult(WspConsts.System_Image_HorizontalResolution, "img");

            var expectedResults = new double[]
            {
                96,
                72,
                96,
            };

            ValidateQueryResult(nameof(WspConsts.System_Image_HorizontalResolution), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Image.Dimensions property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Image_System_Image_Dimensions()
        {
            var queryResult = GetQueryResult(WspConsts.System_Image_Dimensions, "img");

            var expectedResults = new string[]
            {
                "‪93 x 64‬",
                "‪930 x 636‬",
                "‪111 x 76‬",
            };

            ValidateQueryResult(nameof(WspConsts.System_Image_Dimensions), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Image.BitDepth property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Image_System_Image_BitDepth()
        {
            var queryResult = GetQueryResult(WspConsts.System_Image_BitDepth, "img");

            var expectedResults = new uint[]
            {
                24,
                24,
                1,
            };

            ValidateQueryResult(nameof(WspConsts.System_Image_BitDepth), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Image.VerticalSize property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Image_System_Image_VerticalSize()
        {
            var queryResult = GetQueryResult(WspConsts.System_Image_VerticalSize, "img");

            var expectedResults = new uint[]
            {
                64,
                636,
                76,
            };

            ValidateQueryResult(nameof(WspConsts.System_Image_VerticalSize), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Image.VerticalResolution property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Image_System_Image_VerticalResolution()
        {
            var queryResult = GetQueryResult(WspConsts.System_Image_VerticalResolution, "img");

            var expectedResults = new double[]
            {
                96,
                72,
                96,
            };

            ValidateQueryResult(nameof(WspConsts.System_Image_VerticalResolution), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if AudioFormat property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Audio_AudioFormat()
        {
            var queryResult = GetQueryResult(WspConsts.AudioFormat, "music");

            var expectedResults = new string[]
            {
                "{00001610-0000-0010-8000-00AA00389B71}",
                "{00000055-0000-0010-8000-00AA00389B71}",
                "{00000055-0000-0010-8000-00AA00389B71}",
            };

            ValidateQueryResult(nameof(WspConsts.AudioFormat), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Media.Duration property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Audio_System_Media_Duration()
        {
            var queryResult = GetQueryResult(WspConsts.System_Media_Duration, "music");

            var expectedResults = new ulong[]
            {
                57120861,
                57728750,
                57730500,
            };

            ValidateQueryResult(nameof(WspConsts.System_Media_Duration), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Audio.EncodingBitrate property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Audio_System_Audio_EncodingBitrate()
        {
            var queryResult = GetQueryResult(WspConsts.System_Audio_EncodingBitrate, "music");

            var expectedResults = new uint[]
            {
                163552,
                64000,
                160000,
            };

            ValidateQueryResult(nameof(WspConsts.System_Audio_EncodingBitrate), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Audio.SampleRate property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Audio_System_Audio_SampleRate()
        {
            var queryResult = GetQueryResult(WspConsts.System_Audio_SampleRate, "music");

            var expectedResults = new uint[]
            {
                44100,
                44100,
                44100,
            };

            ValidateQueryResult(nameof(WspConsts.System_Audio_SampleRate), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Audio.SampleSize property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Audio_System_Audio_SampleSize()
        {
            var queryResult = GetQueryResult(WspConsts.System_Audio_SampleSize, "music");

            var expectedResults = new uint[]
            {
                16,
                16,
                16,
            };

            ValidateQueryResult(nameof(WspConsts.System_Audio_SampleSize), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Audio.ChannelCount property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Audio_System_Audio_ChannelCount()
        {
            var queryResult = GetQueryResult(WspConsts.System_Audio_ChannelCount, "music");

            var expectedResults = new uint[]
            {
                2,
                1,
                2,
            };

            ValidateQueryResult(nameof(WspConsts.System_Audio_ChannelCount), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Video.EncodingBitrate property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Video_System_Video_EncodingBitrate()
        {
            var queryResult = GetQueryResult(WspConsts.System_Video_EncodingBitrate, "video");

            var expectedResults = new uint[]
            {
                58168,
                30584,
                20504,
            };

            ValidateQueryResult(nameof(WspConsts.System_Video_EncodingBitrate), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Video.FrameWidth property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Video_System_Video_FrameWidth()
        {
            var queryResult = GetQueryResult(WspConsts.System_Video_FrameWidth, "video");

            var expectedResults = new uint[]
            {
                1920,
                1280,
                960,
            };

            ValidateQueryResult(nameof(WspConsts.System_Video_FrameWidth), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Video.FrameHeight property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Video_System_Video_FrameHeight()
        {
            var queryResult = GetQueryResult(WspConsts.System_Video_FrameHeight, "video");

            var expectedResults = new uint[]
            {
                1080,
                720,
                540,
            };

            ValidateQueryResult(nameof(WspConsts.System_Video_FrameHeight), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Video.FrameRate property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Video_System_Video_FrameRate()
        {
            var queryResult = GetQueryResult(WspConsts.System_Video_FrameRate, "video");

            var expectedResults = new uint[]
            {
                30000,
                30000,
                30000,
            };

            ValidateQueryResult(nameof(WspConsts.System_Video_FrameRate), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Video.Compression property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Video_System_Video_Compression()
        {
            var queryResult = GetQueryResult(WspConsts.System_Video_Compression, "video");

            var expectedResults = new string[]
            {
                "{34363248-0000-0010-8000-00AA00389B71}",
                "{34363248-0000-0010-8000-00AA00389B71}",
                "{34363248-0000-0010-8000-00AA00389B71}",
            };

            ValidateQueryResult(nameof(WspConsts.System_Video_Compression), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Video.TotalBitrate property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Video_System_Video_TotalBitrate()
        {
            var queryResult = GetQueryResult(WspConsts.System_Video_TotalBitrate, "video");

            var expectedResults = new uint[]
            {
                139272,
                111688,
                101608,
            };

            ValidateQueryResult(nameof(WspConsts.System_Video_TotalBitrate), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Video.FourCC property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Video_System_Video_FourCC()
        {
            var queryResult = GetQueryResult(WspConsts.System_Video_FourCC, "video");

            var expectedResults = new uint[]
            {
                875967048,
                875967048,
                875967048,
            };

            ValidateQueryResult(nameof(WspConsts.System_Video_FourCC), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Video.Director property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Video_System_Video_Director()
        {
            var queryResult = GetQueryResult(WspConsts.System_Video_Director, "video");

            var expectedResults = new string[][]
            {
                new string[] { "AAA" },
                new string[] { "BBB" },
                new string[] { "CCC" },
            };

            ValidateQueryResult(nameof(WspConsts.System_Video_Director), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Contact.HomeTelephone property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Contact_System_Contact_HomeTelephone()
        {
            var queryResult = GetQueryResult(WspConsts.System_Contact_HomeTelephone, "person");

            var expectedResults = new string[]
            {
                "123456789",
                "456123789",
                "789456123",
            };

            ValidateQueryResult(nameof(WspConsts.System_Contact_HomeTelephone), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Contact.EmailAddress property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Contact_System_Contact_EmailAddress()
        {
            var queryResult = GetQueryResult(WspConsts.System_Contact_EmailAddress, "person");

            var expectedResults = new string[]
            {
                "AAA.BBB@example.com",
                "CCC.DDD@example.com",
                "EEE.FFF@example.com",
            };

            ValidateQueryResult(nameof(WspConsts.System_Contact_EmailAddress), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Contact.FullName property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Contact_System_Contact_FullName()
        {
            var queryResult = GetQueryResult(WspConsts.System_Contact_FullName, "person");

            var expectedResults = new string[]
            {
                "AAA BBB",
                "CCC DDD",
                "EEE FFF",
            };

            ValidateQueryResult(nameof(WspConsts.System_Contact_FullName), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Contact.NickName property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Contact_System_Contact_NickName()
        {
            var queryResult = GetQueryResult(WspConsts.System_Contact_NickName, "person");

            var expectedResults = new string[]
            {
                "aaa",
                "ccc",
                "eee",
            };

            ValidateQueryResult(nameof(WspConsts.System_Contact_NickName), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Contact.HomeAddressStreet property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Contact_System_Contact_HomeAddressStreet()
        {
            var queryResult = GetQueryResult(WspConsts.System_Contact_HomeAddressStreet, "person");

            var expectedResults = new string[]
            {
                "AAA",
                "BBB",
                "CCC",
            };

            ValidateQueryResult(nameof(WspConsts.System_Contact_HomeAddressStreet), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Search.Store property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Search_System_Search_Store()
        {
            var queryResult = GetQueryResult(WspConsts.System_Search_Store, "document");

            var expectedResults = new string[]
            {
                "file",
                "file",
                "file",
            };

            ValidateQueryResult(nameof(WspConsts.System_Search_Store), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Search.EntryID property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Search_System_Search_EntryID()
        {
            var queryResult = GetQueryResult(WspConsts.System_Search_EntryID, "document");

            var uniqueIds = new HashSet<int>();
            foreach (var row in queryResult.Rows)
            {
                Site.Assert.IsTrue(row.Columns[1].Data is int, $"The System.Search.EntryID column should be an int value.");
                uniqueIds.Add((int)row.Columns[1].Data);
            }

            var areAllIdsUnique = uniqueIds.Count == 3;
            Site.Assert.IsTrue(areAllIdsUnique, "The values of System.Search.EntryID associated with each file in the query results should be unique.");
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Search.HitCount property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Search_System_Search_HitCount()
        {
            var queryResult = GetQueryResult(WspConsts.System_Search_HitCount, "document");

            var expectedResults = new int[]
            {
                1,
                1,
                1,
            };

            ValidateQueryResult(nameof(WspConsts.System_Search_HitCount), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Search.GatherTime property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Search_System_Search_GatherTime()
        {
            var queryResult = GetQueryResult(WspConsts.System_Search_GatherTime, "document");

            foreach (var row in queryResult.Rows)
            {
                Site.Assert.IsTrue(row.Columns[1].Data is DateTime, $"The System.Search.GatherTime column should be in DateTime form.");
            }
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Search.LastIndexedTotalTime property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Search_System_Search_LastIndexedTotalTime()
        {
            var queryResult = GetQueryResult(WspConsts.System_Search_LastIndexedTotalTime, "document");

            foreach (var row in queryResult.Rows)
            {
                Site.Assert.IsTrue(row.Columns[1].Data is double, $"The System.Search.LastIndexedTotalTime column should be a double value.");
            }
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.MIMEType property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Search_System_MIMEType()
        {
            var queryResult = GetQueryResult(WspConsts.System_MIMEType, "img");

            var expectedResults = new string[]
            {
                "image/bmp",
                "image/jpeg",
                "image/bmp"
            };

            ValidateQueryResult(nameof(WspConsts.System_MIMEType), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Rating property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Media_System_Rating()
        {
            var queryResult = GetQueryResult(WspConsts.System_Rating, "music");

            var expectedResults = new uint[]
            {
                1,
                50,
                99,
            };

            ValidateQueryResult(nameof(WspConsts.System_Rating), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.ParentalRating property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Media_System_ParentalRating()
        {
            var queryResult = GetQueryResult(WspConsts.System_ParentalRating, "video");

            var expectedResults = new string[]
            {
                "AAA",
                "BBB",
                "CCC",
            };

            ValidateQueryResult(nameof(WspConsts.System_ParentalRating), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Media.Writer property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Media_System_Media_Writer()
        {
            var queryResult = GetQueryResult(WspConsts.System_Media_Writer, "video");

            var expectedResults = new string[][]
            {
                new string[] { "AAA" },
                new string[] { "BBB" },
                new string[] { "CCC" },
            };

            ValidateQueryResult(nameof(WspConsts.System_Media_Writer), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.Media.DateEncoded property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Media_System_Media_DateEncoded()
        {
            var queryResult = GetQueryResult(WspConsts.System_Media_DateEncoded, "video");

            var expectedResults = new DateTime[]
            {
                DateTime.Parse("2020-06-23T04:03:23Z").ToUniversalTime(),
                DateTime.Parse("2020-07-23T04:03:23Z").ToUniversalTime(),
                DateTime.Parse("2020-05-23T04:03:23Z").ToUniversalTime(),
            };

            ValidateQueryResult(nameof(WspConsts.System_Media_DateEncoded), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.ComputerName property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Shell_System_ComputerName()
        {
            var sutComputerName = Site.Properties.Get("ServerComputerName");
            if (IPAddress.TryParse(sutComputerName, out _))
            {
                Site.Assume.Inconclusive($"The ServerComputerName is an IP address, which is not supported in this case.");
            }
            sutComputerName = sutComputerName.Split('.')[0]; // Handle FQDN.

            var queryResult = GetQueryResult(WspConsts.System_ComputerName, "document");

            foreach (var row in queryResult.Rows)
            {
                var propValue = row.Columns[1].Data as string;
                Site.Assert.AreEqual(sutComputerName.ToUpper(), propValue.ToUpper(), $"The System.ComputerName column should be {sutComputerName}.");
            }
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.ItemPathDisplayNarrow property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Shell_System_ItemPathDisplayNarrow()
        {
            var queryPath = Site.Properties.Get("QueryPath") + "Data/CreateQuery_CFullPropSpec";
            var queryFolderPath = queryPath.Replace("file://", @"\\").Replace("/", @"\");
            var queryResult = GetQueryResult(WspConsts.System_ItemPathDisplayNarrow, "document", queryPath: queryPath);

            var expectedResults = new string[]
            {
                $"document1.doc ({queryFolderPath})",
                $"document2.ppt ({queryFolderPath})",
                $"document3.xls ({queryFolderPath})",
            };

            ValidateQueryResult(nameof(WspConsts.System_ItemPathDisplayNarrow), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.ItemType property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Shell_System_ItemType()
        {
            var queryResult = GetQueryResult(WspConsts.System_ItemType, "document");

            var expectedResults = new string[]
            {
                ".doc",
                ".ppt",
                ".xls",
            };

            ValidateQueryResult(nameof(WspConsts.System_ItemType), expectedResults, queryResult);
        }

        [TestMethod]
        [TestCategory("CPMCreateQuery")]
        [Description("This test case is designed to test if System.ParsingName property can be retrieved by CPMCreateQuery.")]
        public void CPMCreateQuery_CFullPropSpec_Shell_System_ParsingName()
        {
            var queryResult = GetQueryResult(WspConsts.System_ParsingName, "document");

            var expectedResults = new string[]
            {
                "document1.doc",
                "document2.ppt",
                "document3.xls",
            };

            ValidateQueryResult(nameof(WspConsts.System_ParsingName), expectedResults, queryResult);
        }

        #endregion

        private CPMGetRowsOut GetQueryResult(CFullPropSpec property, string queryText, string queryPath = null, uint totalRows = 3)
        {
            queryPath = queryPath ?? Site.Properties.Get("QueryPath") + "Data/CreateQuery_CFullPropSpec";

            argumentType = ArgumentType.AllValid;
            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMConnectIn and expects success.");
            wspAdapter.CPMConnectIn();

            var columnSet = new CColumnSet();
            columnSet.count = 3;
            columnSet.indexes = new uint[] { 0, 1, 3 };

            CBaseStorageVariant queryPathVaraint = wspAdapter.Builder.GetBaseStorageVariant(CBaseStorageVariant_vType_Values.VT_LPWSTR, new VT_LPWSTR(queryPath));
            var restrictionArray = wspAdapter.Builder.GetRestrictionArray(
                wspAdapter.Builder.GetPropertyRestriction(CPropertyRestriction_relop_Values.PREQ, WspConsts.System_Search_Scope, queryPathVaraint),
                wspAdapter.Builder.GetContentRestriction(queryText, WspConsts.System_FileName, CContentRestriction_ulGenerateMethod_Values.GENERATE_METHOD_PREFIX));

            var pidMapper = new CPidMapper();
            pidMapper.aPropSpec = new CFullPropSpec[]
            {
                WspConsts.System_ItemName,
                property,
                WspConsts.System_Search_Scope,
                WspConsts.System_FileName,
            };
            pidMapper.count = (uint)pidMapper.aPropSpec.Length;

            var inGroupSortAggregSets = new CInGroupSortAggregSets();
            inGroupSortAggregSets.cCount = 1;
            inGroupSortAggregSets.SortSets = new CSortSet[1];
            inGroupSortAggregSets.SortSets[0].count = 1;
            inGroupSortAggregSets.SortSets[0].sortArray = new CSort[1];
            inGroupSortAggregSets.SortSets[0].sortArray[0].dwOrder = CSort_dwOrder_Values.QUERY_SORTASCEND;
            inGroupSortAggregSets.SortSets[0].sortArray[0].pidColumn = 3; // Sort by file name.
            inGroupSortAggregSets.SortSets[0].sortArray[0].locale = wspAdapter.Builder.Parameter.LcidValue;

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMCreateQueryIn and expects success.");
            wspAdapter.CPMCreateQueryIn(columnSet, restrictionArray, inGroupSortAggregSets, null, new CRowsetProperties(), pidMapper, new CColumnGroupArray(), wspAdapter.Builder.Parameter.LcidValue);

            var columns = new CTableColumn[]
            {
                wspAdapter.Builder.GetTableColumn(WspConsts.System_ItemName, CBaseStorageVariant_vType_Values.VT_VARIANT),
                wspAdapter.Builder.GetTableColumn(property, CBaseStorageVariant_vType_Values.VT_VARIANT),
                wspAdapter.Builder.GetTableColumn(WspConsts.System_FileName, CBaseStorageVariant_vType_Values.VT_VARIANT)
            };

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMSetBindingsIn and expects success.");
            wspAdapter.CPMSetBindingsIn(columns);

            Site.Log.Add(LogEntryKind.TestStep, "Client sends CPMGetRowsIn and expects success.");
            argumentType = ArgumentType.AllValid;
            wspAdapter.CPMGetRowsIn(out CPMGetRowsOut getRowsOut);

            Site.Assert.AreEqual(totalRows, getRowsOut._cRowsReturned, $"Number of rows returned should be {totalRows}.");

            for (var i = 0; i < totalRows; i++)
            {
                var fileNamePrefix = $"{queryText}{i + 1}";
                Site.Assert.IsTrue((getRowsOut.Rows[i].Columns[2].Data as string).StartsWith(fileNamePrefix), $"The file name of Row {i} in Ascend order should start with \"{fileNamePrefix}\".");
            }

            return getRowsOut;
        }

        private void ValidateQueryResult<T>(string propertyConstantName, T[] expectedResults, CPMGetRowsOut queryResult)
        {
            var propertyName = propertyConstantName.Replace("_", ".");
            for (var i = 0; i < expectedResults.Length; i++)
            {
                var fileName = queryResult.Rows[i].Columns[2].Data as string;
                Site.Assert.IsNotNull(queryResult.Rows[i].Columns[1].Data, $"The {propertyName} of {fileName} should not be null if the server supports querying the property.");

                Site.Assert.AreEqual(expectedResults[i], (T)queryResult.Rows[i].Columns[1].Data, $"The \"{propertyName}\" of \"{fileName}\" should be \"{expectedResults[i]}\".");
            }
        }

        private void ValidateQueryResult<T>(string propertyConstantName, T[][] expectedResults, CPMGetRowsOut queryResult)
        {
            var propertyName = propertyConstantName.Replace("_", ".");
            for (var i = 0; i < expectedResults.Length; i++)
            {
                var fileName = queryResult.Rows[i].Columns[2].Data as string;
                Site.Assert.IsNotNull(queryResult.Rows[i].Columns[1].Data, $"The \"{propertyName}\" of \"{fileName}\" should not be null if the server supports querying the property.");

                var dataArray = queryResult.Rows[i].Columns[1].Data as T[];
                var succeed = expectedResults[i].SequenceEqual(dataArray);
                Site.Assert.IsTrue(succeed, $"The \"{propertyName}\" of \"{fileName}\" should be \"{GetPrintableVector(expectedResults[i])}\".");
            }
        }

        private string GetPrintableVector<T>(T[] vector)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append('[');
            foreach (var element in vector)
            {
                stringBuilder.Append($"{element};");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            stringBuilder.Append(']');

            return stringBuilder.ToString();
        }
    }
}