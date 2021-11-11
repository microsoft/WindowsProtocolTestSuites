// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.IO.Packaging;

namespace Microsoft.Protocols.TestManager.PTMService.Common.Profile
{
    public class ProfileUtil : IDisposable
    {

        static Uri playlistPartUri = PackUriHelper.CreatePartUri(new Uri(@"config\playlist.xml", UriKind.Relative));

        static Uri profilePartUri = PackUriHelper.CreatePartUri(new Uri(@"config\profile.xml", UriKind.Relative));

        static Uri profileInfoPartUri = PackUriHelper.CreatePartUri(new Uri(@"config\info.xml", UriKind.Relative));

        private bool profileUtilClosed;
        private Package profilePackage;

        /// <summary>
        /// The information about the profile.
        /// </summary>
        public ProfileInfo Info { get; set; }

        /// <summary>
        /// The default constructor of ProfileUtil
        /// </summary>
        public ProfileUtil()
        {
            profileUtilClosed = false;
        }

        /// <summary>
        /// Loads the profile from file.
        /// </summary>
        /// <param name="filename">The file name</param>
        /// <param name="configurationId">Configuration id</param>
        /// <returns>An instance of ProfileUtil</returns>
        public static ProfileUtil LoadProfile(string filename)
        {
            ProfileUtil util = new ProfileUtil();
            util.profilePackage = Package.Open(filename, FileMode.Open);
            if (util.profilePackage.PartExists(playlistPartUri))
            {
                PackagePart playlist = util.profilePackage.GetPart(playlistPartUri);
                util.playlistStream = playlist.GetStream();
            }
            if (util.profilePackage.PartExists(profilePartUri))
            {
                PackagePart profilePart = util.profilePackage.GetPart(profilePartUri);
                util.profileStream = profilePart.GetStream();
            }
            if (util.profilePackage.PartExists(profileInfoPartUri))
            {
                PackagePart profileInfoPart = util.profilePackage.GetPart(profileInfoPartUri);
                util.Info = ProfileInfo.ReadFrom(profileInfoPart.GetStream());
            }
            return util;
        }

        /// <summary>
        /// Creates a new PTM profile
        /// </summary>
        /// <param name="filename">The file name to save the profile</param>
        /// <param name="testSuiteName">The test suite name</param>
        /// <param name="version">The test suite version</param>
        /// <param name="configurationId">Configuration id</param>
        /// <returns>An instance of ProfileUtil</returns>
        public static ProfileUtil CreateProfile(string filename, string testSuiteName, string version)
        {
            ProfileUtil profile = new ProfileUtil();
            profile.profilePackage = Package.Open(filename, FileMode.Create);

            PackagePart profilePart = profile.profilePackage.CreatePart(profilePartUri, System.Net.Mime.MediaTypeNames.Text.Xml);
            profile.profileStream = profilePart.GetStream();

            PackagePart playlist = profile.profilePackage.CreatePart(playlistPartUri, System.Net.Mime.MediaTypeNames.Text.Xml);
            profile.playlistStream = playlist.GetStream();

            PackagePart profileInfo = profile.profilePackage.CreatePart(profileInfoPartUri, System.Net.Mime.MediaTypeNames.Text.Xml);
            profile.Info = new ProfileInfo() { TestSuiteName = testSuiteName, Version = version };
            var stream = profileInfo.GetStream();
            profile.Info.WriteTo(stream);
            stream.Flush();
            stream.Close();

            return profile;
        }

        /// <summary>
        /// Verifies the test suite name and version of the profile.
        /// </summary>
        /// <param name="version">The version of the test suite</param>
        /// <returns>Returns true if the version and test suite name matches. If the package does not contains the version information returns false.</returns>
        public bool VerifyVersion(string version)
        {
            // TODO: Test suite name validation may be useful when test suite installation name and test suite name are separated.
            if (Info == null) return false;
            if (Info.Version == version) return true;
            return false;
        }

        /// <summary>
        /// Closes the ProfileUtil file.
        /// </summary>
        public void Close()
        {
            if (profileUtilClosed) throw new InvalidOperationException("Package closed.");

            profileUtilClosed = true;

            if (IsValid)
            {
                playlistStream.Flush();
                playlistStream.Close();
                profileStream.Flush();
                profileStream.Close();
                profilePackage.Flush();
                profilePackage.Close();
            }
        }

        /// <summary>
        /// Copy the content of a stream to another stream
        /// </summary>
        /// <param name="src">Source stream</param>
        /// <param name="des">Destination stream</param>
        public static void CopyStream(Stream src, Stream des)
        {
            const int bufSize = 4096;
            byte[] buf = new byte[bufSize];
            int bytesRead = 0;
            while ((bytesRead = src.Read(buf, 0, bufSize)) > 0) des.Write(buf, 0, bytesRead);
        }

        /// <summary>
        /// Adds a PTF Config file to the profile.
        /// </summary>
        /// <param name="filename">File name</param>
        public void AddPtfCfg(string filename)
        {
            if (profileUtilClosed) throw new InvalidOperationException("Package closed.");
            string name = Path.GetFileName(filename);
            Uri partUri = PackUriHelper.CreatePartUri(new Uri(string.Format(@"ptfconfig\{0}", name), UriKind.Relative));
            PackagePart ptfconfig = profilePackage.CreatePart(partUri, System.Net.Mime.MediaTypeNames.Text.Xml);
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                CopyStream(fs, ptfconfig.GetStream());
            }
        }
        /// <summary>
        /// Saves the PTFConfig files in the profile to the specified path.
        /// </summary>
        /// <param name="path">Path</param>
        public void SavePtfCfgTo(string path)
        {
            if (profileUtilClosed) throw new InvalidOperationException("Package closed.");
            Uri ptfcfgBase = PackUriHelper.CreatePartUri(new Uri("ptfconfig", UriKind.Relative));
            foreach (var part in profilePackage.GetParts())
            {
                if (part.Uri.ToString().IndexOf("/ptfconfig/") == 0)
                {
                    string uri = part.Uri.ToString();
                    string name = uri.Substring(uri.LastIndexOf("/") + 1);
                    using (var fs = new FileStream(Path.Combine(path, name), FileMode.Create))
                    {
                        CopyStream(part.GetStream(), fs);
                        fs.Flush();
                        fs.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Copies the content of a file to a stream
        /// <param name="sourceFile">The source file to read from.</param>
        /// <param name="destinationStream">Destination stream to copy content to.</param>
        /// </summary>
        public static void FileToStream(string sourceFile, Stream destinationStream)
        {
            var profileStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read);

            CopyStream(profileStream, destinationStream);

            profileStream.Flush();

            profileStream.Close();
        }

        private Stream playlistStream;
        /// <summary>
        /// The Stream to read and save the play list in the profile.
        /// </summary>
        public Stream PlaylistStream
        {
            get
            {
                if (profileUtilClosed) throw new InvalidOperationException("Package closed.");
                return playlistStream;
            }
        }

        public Stream profileStream;
        /// <summary>
        /// The stream to read and save the test selection profile.
        /// </summary>
        public Stream ProfileStream
        {
            get
            {
                if (profileUtilClosed) throw new InvalidOperationException("Package closed.");
                return profileStream;
            }
        }

        public bool IsValid
        {
            get
            {
                if (Info != null && profileStream != null && playlistStream != null) return true;

                return false;
            }
        }

        /// <summary>
        /// Closes the profile package.
        /// </summary>
        public void Dispose()
        {
            if (!profileUtilClosed)
            {
                Close();
            }
        }
    }
}
