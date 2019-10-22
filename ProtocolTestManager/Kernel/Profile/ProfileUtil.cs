// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Packaging;

namespace Microsoft.Protocols.TestManager.Kernel
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
        public ProfileInfo Info {get;set;}

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
                util.Info = ProfileInfo.Readfrom(profileInfoPart.GetStream());
            }
            return util;
        }

        /// <summary>
        /// Creates a new PTM profile
        /// </summary>
        /// <param name="filename">The file name to save the profile</param>
        /// <param name="testSuiteName">The test suite name</param>
        /// <param name="version">The test suite version</param>
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
        /// <param name="testSuiteName">The name of the test suite</param>
        /// <param name="version">The version of the test suite</param>
        /// <returns>Returns true if the version and test suite name matches. If the package does not contains the version information returns false.</returns>
        public bool VerifyVersion(string testSuiteName, string version)
        {
            if (Info == null) return false;
            if (Info.Version == version && Info.TestSuiteName == testSuiteName) return true;
            return false;
        }

        /// <summary>
        /// Closes the ProfileUtil file.
        /// </summary>
        public void Close()
        {
            if (profileUtilClosed) throw new InvalidOperationException("Package closed.");
            profileUtilClosed = true;
            playlistStream.Flush();
            playlistStream.Close();
            profileStream.Flush();
            profileStream.Close();
            profilePackage.Flush();
            profilePackage.Close();
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
        
        public static Dictionary<string, string> DefaultPtfConfigContent = new Dictionary<string, string>
        {
            ["AD_ServerTestSuite.ptfconfig"]                        = StringResource.AD_ServerTestSuite_ptfconfig,
            ["AD_ServerTestSuite.deployment.ptfconfig"]             = StringResource.AD_ServerTestSuite_deployment_ptfconfig,
            ["MS-ADFSPIP_ClientTestSuite.ptfconfig"]                = StringResource.MS_ADFSPIP_ClientTestSuite_ptfconfig,
            ["MS-ADFSPIP_ClientTestSuite.deployment.ptfconfig"]     = StringResource.MS_ADFSPIP_ClientTestSuite_deployment_ptfconfig,
            ["MS-ADOD_ODTestSuite.ptfconfig"]                       = StringResource.MS_ADOD_ODTestSuite_ptfconfig,
            ["MS-ADOD_ODTestSuite.deployment.ptfconfig"]            = StringResource.MS_ADOD_ODTestSuite_deployment_ptfconfig,
            ["MS-AZOD_ODTestSuite.ptfconfig"]                       = StringResource.MS_AZOD_ODTestSuite_ptfconfig,
            ["MS-AZOD_ODTestSuite.deployment.ptfconfig"]            = StringResource.MS_AZOD_ODTestSuite_deployment_ptfconfig,
            ["BranchCache_TestSuite.ptfconfig"]                     = StringResource.BranchCache_TestSuite_ptfconfig,
            ["BranchCache_TestSuite.deployment.ptfconfig"]          = StringResource.BranchCache_TestSuite_deployment_ptfconfig,
            ["Auth_ServerTestSuite.ptfconfig"]                      = StringResource.Auth_ServerTestSuite_ptfconfig,
            ["Auth_ServerTestSuite.deployment.ptfconfig"]           = StringResource.Auth_ServerTestSuite_deployment_ptfconfig,
            ["CommonTestSuite.ptfconfig"]                           = StringResource.CommonTestSuite_ptfconfig,
            ["CommonTestSuite.deployment.ptfconfig"]                = StringResource.CommonTestSuite_deployment_ptfconfig,
            ["MS-DFSC_ServerTestSuite.ptfconfig"]                   = StringResource.MS_DFSC_ServerTestSuite_ptfconfig,
            ["MS-DFSC_ServerTestSuite.deployment.ptfconfig"]        = StringResource.MS_DFSC_ServerTestSuite_deployment_ptfconfig,
            ["MS-FSA_ServerTestSuite.ptfconfig"]                    = StringResource.MS_FSA_ServerTestSuite_ptfconfig,
            ["MS-FSA_ServerTestSuite.deployment.ptfconfig"]         = StringResource.MS_FSA_ServerTestSuite_deployment_ptfconfig,
            ["MS-FSRVP_ServerTestSuite.ptfconfig"]                  = StringResource.MS_FSRVP_ServerTestSuite_ptfconfig,
            ["MS-FSRVP_ServerTestSuite.deployment.ptfconfig"]       = StringResource.MS_FSRVP_ServerTestSuite_deployment_ptfconfig,
            ["MS-RSVD_ServerTestSuite.ptfconfig"]                   = StringResource.MS_RSVD_ServerTestSuite_ptfconfig,
            ["MS-RSVD_ServerTestSuite.deployment.ptfconfig"]        = StringResource.MS_RSVD_ServerTestSuite_deployment_ptfconfig,
            ["MS-SMB2_ServerTestSuite.ptfconfig"]                   = StringResource.MS_SMB2_ServerTestSuite_ptfconfig,
            ["MS-SMB2_ServerTestSuite.deployment.ptfconfig"]        = StringResource.MS_SMB2_ServerTestSuite_deployment_ptfconfig,
            ["MS-SMB2Model_ServerTestSuite.ptfconfig"]              = StringResource.MS_SMB2Model_ServerTestSuite_ptfconfig,
            ["MS-SMB2Model_ServerTestSuite.deployment.ptfconfig"]   = StringResource.MS_SMB2Model_ServerTestSuite_deployment_ptfconfig,
            ["MS-SQOS_ServerTestSuite.ptfconfig"]                   = StringResource.MS_SQOS_ServerTestSuite_ptfconfig,
            ["MS-SQOS_ServerTestSuite.deployment.ptfconfig"]        = StringResource.MS_SQOS_ServerTestSuite_deployment_ptfconfig,
            ["ServerFailoverTestSuite.ptfconfig"]                   = StringResource.ServerFailoverTestSuite_ptfconfig,
            ["ServerFailoverTestSuite.deployment.ptfconfig"]        = StringResource.ServerFailoverTestSuite_deployment_ptfconfig,
            ["Kerberos_ServerTestSuite.ptfconfig"]                  = StringResource.Kerberos_ServerTestSuite_ptfconfig,
            ["Kerberos_ServerTestSuite.deployment.ptfconfig"]       = StringResource.Kerberos_ServerTestSuite_deployment_ptfconfig,
            ["RDP_ClientTestSuite.ptfconfig"]                       = StringResource.RDP_ClientTestSuite_ptfconfig,
            ["RDP_ClientTestSuite.deployment.ptfconfig"]            = StringResource.RDP_ClientTestSuite_deployment_ptfconfig,
            ["RDP_ServerTestSuite.ptfconfig"]                       = StringResource.RDP_ServerTestSuite_ptfconfig,
            ["RDP_ServerTestSuite.deployment.ptfconfig"]            = StringResource.RDP_ServerTestSuite_deployment_ptfconfig,
            ["MS-SMBD_ServerTestSuite.ptfconfig"]                   = StringResource.MS_SMBD_ServerTestSuite_ptfconfig,
            ["MS-SMBD_ServerTestSuite.deployment.ptfconfig"]        = StringResource.MS_SMBD_ServerTestSuite_deployment_ptfconfig,
        };

        public static Dictionary<string, string[]> PtfConfigFilesByTestSuite = new Dictionary<string, string[]>
        {
            ["ADFamily"] = new string[]
            {
                "AD_ServerTestSuite.ptfconfig",
                "AD_ServerTestSuite.deployment.ptfconfig",
            },
            ["MS-ADFSPIP"] = new string[]
            {
                "MS-ADFSPIP_ClientTestSuite.ptfconfig",
                "MS-ADFSPIP_ClientTestSuite.deployment.ptfconfig",
            },
            ["MS-ADOD"] = new string[]
            {
                "MS-ADOD_ODTestSuite.ptfconfig",
                "MS-ADOD_ODTestSuite.deployment.ptfconfig",
            },
            ["MS-AZOD"] = new string[]
            {
                "MS-AZOD_ODTestSuite.ptfconfig",
                "MS-AZOD_ODTestSuite.deployment.ptfconfig",
            },
            ["BranchCache"] = new string[]
            {
                "BranchCache_TestSuite.ptfconfig",
                "BranchCache_TestSuite.deployment.ptfconfig",
            },
            ["FileServer"] = new string[]
            {
                "Auth_ServerTestSuite.ptfconfig",
                "Auth_ServerTestSuite.deployment.ptfconfig",
                "CommonTestSuite.ptfconfig",
                "CommonTestSuite.deployment.ptfconfig",
                "MS-DFSC_ServerTestSuite.ptfconfig",
                "MS-DFSC_ServerTestSuite.deployment.ptfconfig",
                "MS-FSA_ServerTestSuite.ptfconfig",
                "MS-FSA_ServerTestSuite.deployment.ptfconfig",
                "MS-FSRVP_ServerTestSuite.ptfconfig",
                "MS-FSRVP_ServerTestSuite.deployment.ptfconfig",
                "MS-RSVD_ServerTestSuite.ptfconfig",
                "MS-RSVD_ServerTestSuite.deployment.ptfconfig",
                "MS-SMB2_ServerTestSuite.ptfconfig",
                "MS-SMB2_ServerTestSuite.deployment.ptfconfig",
                "MS-SMB2Model_ServerTestSuite.ptfconfig",
                "MS-SMB2Model_ServerTestSuite.deployment.ptfconfig",
                "MS-SQOS_ServerTestSuite.ptfconfig",
                "MS-SQOS_ServerTestSuite.deployment.ptfconfig",
                "ServerFailoverTestSuite.ptfconfig",
                "ServerFailoverTestSuite.deployment.ptfconfig",
            },
            ["Kerberos"] = new string[]
            {
                "Kerberos_ServerTestSuite.ptfconfig",
                "Kerberos_ServerTestSuite.deployment.ptfconfig",
            },
            ["RDP-Client"] = new string[]
            {
                "RDP_ClientTestSuite.ptfconfig",
                "RDP_ClientTestSuite.deployment.ptfconfig",
            },
            ["RDP-Server"] = new string[]
            {
                "RDP_ServerTestSuite.ptfconfig",
                "RDP_ServerTestSuite.deployment.ptfconfig",
            },
            ["MS-SMBD"] = new string[]
            {
                "MS-SMBD_ServerTestSuite.ptfconfig",
                "MS-SMBD_ServerTestSuite.deployment.ptfconfig",
            },
        };
    }
}
