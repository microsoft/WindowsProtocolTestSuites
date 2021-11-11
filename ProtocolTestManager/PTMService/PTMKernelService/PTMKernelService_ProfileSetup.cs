// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Protocols.TestManager.Kernel;
using ProfileUtil = Microsoft.Protocols.TestManager.PTMService.Common.Profile.ProfileUtil;
using Microsoft.Protocols.TestManager.PTMService.Common.Profile;

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    internal partial class PTMKernelService
    {
        /// <summary>
        /// Saves the configurations as a profile.
        /// </summary>
        /// <param name="request">Profile request.</param>
        /// <returns>profile location</returns>
        public string SaveProfileSettings(ProfileExportRequest request)
        {
            CreateProfile(request);

            string filePublicDirectory = MoveProfileToTempDirectory(request.FileName);

            return filePublicDirectory;
        }

        /// <summary>
        /// Saves the configurations as a profile.
        /// </summary>
        /// <param name="testResultId">Test result id.</param>
        /// <returns>profile location</returns>
        public string SaveProfileSettingsByTestResult(int testResultId)
        {
            var testRun = GetTestRun(testResultId);

            var profileRequest = new ProfileExportRequest()
            {
                FileName = EnsureProfileName(null),
                TestSuiteId = testRun.Configuration.TestSuite.Id,
                ConfigurationId = testRun.Configuration.Id,
                TestResultId = testResultId,
            };

            return SaveProfileSettings(profileRequest);
        }

        /// <summary>
        /// Loads the configurations from a saved profile.
        /// </summary>
        /// <param name="request">Profile request.</param>
        public void LoadProfileSettings(ProfileRequest request)
        {
            string filenameWithoutExtension = Path.GetFileNameWithoutExtension(request.FileName);

            var baseNode = StoragePool.GetKnownNode(KnownStorageNodeNames.TestSuite);

            if (baseNode.NodeExists(filenameWithoutExtension))
            {
                baseNode.RemoveNode(filenameWithoutExtension);
            }

            var profileNode = baseNode.CreateNode(filenameWithoutExtension);

            profileNode.CreateFile(request.FileName, request.Stream);

            string fullName = Path.Combine(profileNode.AbsolutePath, request.FileName);

            var configurationNode = StoragePool.GetKnownNode(KnownStorageNodeNames.Configuration);

            var testSuite = GetTestSuite(request.TestSuiteId);

            using (ProfileUtil profile = ProfileUtil.LoadProfile(fullName))
            {
                if (!profile.VerifyVersion(testSuite.Version))
                {
                    if (profile.Info != null)
                    {
                        throw new InvalidDataException(string.Format
                            (
                                StringMessages.ProfileNotMatchError,
                                profile.Info.TestSuiteName,
                                profile.Info.Version,
                                testSuite.Name,
                                testSuite.Version
                            )
                        );
                    }
                    else
                    {
                        throw new InvalidDataException(StringMessages.InvalidProfile);
                    }
                }

                string configBaseNode = GetConfigurationsPath(request.ConfigurationId, configurationNode.AbsolutePath);

                if (!configurationNode.NodeExists(configBaseNode))
                {
                    configurationNode.CreateNode(configBaseNode);
                }

                string profileDestinationPath = Path.Combine(configBaseNode, ConfigurationConsts.Profile);
                string playlistDestinationPath = Path.Combine(configBaseNode, ConfigurationConsts.PlayList);

                configurationNode.RemoveFile(profileDestinationPath);
                configurationNode.RemoveFile(playlistDestinationPath);

                configurationNode.CreateFile(profileDestinationPath, profile.ProfileStream);
                configurationNode.CreateFile(playlistDestinationPath, profile.PlaylistStream);

                string ptfConfigsDestinationDir = GetPtfConfigBasePath(request.ConfigurationId, configurationNode.AbsolutePath);

                profile.SavePtfCfgTo(ptfConfigsDestinationDir);
            }
        }

        /// <summary>
        /// Export test cases to a Visual Studio playlist.
        /// </summary>
        /// <param name="stream">A Stream</param>
        /// <param name="checkedOnly">True if only export checked cases. False export all test cases.</param>
        public void ExportPlaylist(Stream stream, bool checkedOnly, int testResultId, List<TestCase> testCases)
        {
            List<TestCase> selectedTestCases = testCases;

            if (selectedTestCases == null || selectedTestCases.Count == 0)
            {
                selectedTestCases = GetSelectedTestCases(testResultId);
            }

            XmlWriter writer = XmlWriter.Create(stream);
            writer.WriteStartElement("Playlist");
            writer.WriteStartAttribute("Version");
            writer.WriteValue("1.0");
            writer.WriteEndAttribute();
            foreach (var testcase in selectedTestCases)
            {
                if (checkedOnly & !testcase.IsChecked) continue;
                writer.WriteStartElement("Add");
                writer.WriteStartAttribute("Test");
                writer.WriteValue(testcase.FullName);
                writer.WriteEndAttribute();
                writer.WriteEndElement();
            }
            writer.WriteEndElement();
            writer.Close();
        }

        public List<TestCase> GetSelectedTestCases(int testResultId)
        {
            List<TestCase> testCases = new List<TestCase>();

            var testRun = GetTestRun(testResultId);

            var status = testRun.GetRunningStatus();

            var cases = status.Values.ToArray();

            foreach (var testCase in cases)
            {
                testCases.Add(new TestCase()
                {
                    FullName = testCase.FullName,
                    IsChecked = true
                });
            }

            return testCases;
        }

        public string EnsureProfileName(string profileName)
        {
            if (string.IsNullOrWhiteSpace(profileName))
            {
                profileName = $"{Guid.NewGuid().ToString()}{TestSuiteConsts.ProfileExtension}";
            }
            else
            {
                string extension = Path.GetExtension(profileName);

                if (string.Compare(extension, TestSuiteConsts.ProfileExtension, true) != 0)
                {
                    string filenameWithoutExtension = Path.GetFileNameWithoutExtension(profileName);

                    profileName = $"{filenameWithoutExtension}{TestSuiteConsts.ProfileExtension}";
                }
            }

            return profileName;
        }

        private string CreateProfile(ProfileExportRequest request)
        {
            List<TestCase> testCases = new List<TestCase>();

            var configurationNode = StoragePool.GetKnownNode(KnownStorageNodeNames.Configuration);

            var testSuite = GetTestSuite(request.TestSuiteId);

            using (ProfileUtil profile = ProfileUtil.CreateProfile(
                request.FileName, testSuite.TestSuiteName, testSuite.Version))
            {
                string configBasePath = GetConfigurationsPath(request.ConfigurationId, configurationNode.AbsolutePath);
                string profileSourceFile = Path.Combine(configBasePath, ConfigurationConsts.Profile);

                ProfileUtil.FileToStream(profileSourceFile, profile.ProfileStream);

                string ptfConfigSourceDir = GetPtfConfigBasePath(request.ConfigurationId, configurationNode.AbsolutePath);

                string[] ptfConfigFiles = Directory.GetFiles(ptfConfigSourceDir, "*.ptfconfig", SearchOption.TopDirectoryOnly);

                foreach (string settingsConfigFile in ptfConfigFiles)
                {
                    profile.AddPtfCfg(settingsConfigFile);
                }

                if (request.SelectedTestCases != null)
                {
                    foreach (var testCaseName in request.SelectedTestCases)
                    {
                        var testCase = new TestCase()
                        {
                            FullName = testCaseName,
                            IsChecked = true
                        };

                        testCases.Add(testCase);
                    }
                }

                ExportPlaylist(profile.PlaylistStream, true, request.TestResultId, testCases);
            }

            return request.FileName;
        }

        private string MoveProfileToTempDirectory(string fileName)
        {
            var tempFolderLocation = Path.GetTempPath();

            string fileLocation = Path.Combine(tempFolderLocation, fileName);

            File.Move(fileName, fileLocation);

            return fileLocation;
        }

        private string GetConfigurationsPath(int configurationId, string path)
        {
            string configBasePath = Path.Combine(path, configurationId.ToString(), ConfigurationConsts.ProfileConfig);

            return configBasePath;
        }

        private string GetPtfConfigBasePath(int configurationId, string path)
        {
            string ptfConfigBasePath = Path.Combine(path, configurationId.ToString(), ConfigurationConsts.PtfConfig);

            return ptfConfigBasePath;
        }
    }
}
