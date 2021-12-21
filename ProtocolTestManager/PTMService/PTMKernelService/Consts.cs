// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    public static class KnownStorageNodeNames
    {
        public const string TestSuite = "testsuite";

        public const string Configuration = "configuration";

        public const string TestResult = "testresult";
    }

    public static class TestSuiteConsts
    {
        public const string Bin = "Bin";

        public const string VersionFile = ".version";

        public const string PluginFolderName = "Plugin";

        public const string PluginConfigXml = "config.xml";

        public const string TestSuiteName = "TestSuiteName";

        public const string DllFileNames = "DllFileNames";

        public const string DllFileName = "DllFileName";

        public const string ConfigCaseRule = "ConfigCaseRule";

        public const string FeatureMapping = "FeatureMapping";

        public const string ProfileExtension = ".ptm";

        public const string AutoDetectionDllName = "AutoDetectionDllName";

        public const string Adapters = "Adapters";

        public const string UserGuideFolderName = "doc";

        public const string SupportedMinVersion = "4.21.9.0";
    }

    public static class ConfigurationConsts
    {
        public const string PtfConfig = "ptfconfig";

        public const string ProfileConfig = "config";

        public const string Profile = "profile.xml";

        public const string PlayList = "playlist.xml";

        public const string DefaultGroup = "Default Group";

        public const string AdapterKindAttributeName = "xsi:type";

        public const string AdapterKindManaged = "managed";

        public const string AdapterKindPowerShell = "powershell";

        public const string AdapterKindShell = "shell";

        public const string AdapterKindInteractive = "interactive";

        public const string AdapterTypeAttributeName = "adaptertype";

        public const string AdapterScriptDirectoryAttributeName = "scriptdir";

        public const string RuleSelectAllDisplayName = "(Select All)";

        public const string RuleSelectAll = "All";
    }

    public static class TestRunConsts
    {
        public const string TestCaseListFile = "list.json";
    }

    public static class StringMessages
    {
        public const string InvalidProfile = "It is not a valid PTM test profile";

        public const string ProfileNotMatchError = "The profile does not match the installed test suite. The profile is for {0} {1}. The test suite is {2} {3}.";
    }

    public static class AutoDetectionConsts
    {
        public const string LoadProfileError = "Error in loading profile: {0}";

        public const string LoadingAutoDetectorFailed = "Loading auto-detector assembly failed.";

        public const string LoadPtfconfigError = "Failed to load the PTFConfig files: {0}";

        public readonly static string[] ignoredAssemblies = new string[] { "PropertyValueDetector" };

        public readonly static string[] mixedAssemblies = new string[] { "Microsoft.Protocols.TestTools.StackSdk.FileAccessService.Rdma" };
    }
}
