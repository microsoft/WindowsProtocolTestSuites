// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using System;
using System.IO;
using System.IO.Compression;

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    internal static class Utility
    {
        public static void ExtractArchive(string format, Stream archive, string targetDirectory)
        {
            switch (format)
            {
                case ".zip":
                    {
                        using var zip = new ZipArchive(archive, ZipArchiveMode.Read);

                        zip.ExtractToDirectory(targetDirectory);
                    }
                    break;

                case ".tar.gz":
                    {
                        using var gzip = new GZipInputStream(archive);

                        using var tar = TarArchive.CreateInputTarArchive(gzip);

                        tar.ExtractContents(targetDirectory);
                    }
                    break;

                default:
                    {
                        throw new InvalidOperationException("The package type is not supported.");
                    }
            }
        }
    }
}
