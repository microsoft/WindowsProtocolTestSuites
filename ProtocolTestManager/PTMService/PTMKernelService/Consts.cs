// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestManager.PTMService.PTMKernelService
{
    public static class KnownStorageNodeNames
    {
        public const string TestSuite = "testsuite";

        public const string Configuration = "configuration";
    }

    public static class TestSuiteConsts
    {
        public const string Bin = "Bin";

        public const string VersionFile = ".version";
    }

    public static class ConfigurationConsts
    {
        public const string PtfConfig = "ptfconfig";

        public const string DefaultGroup = "Default Group";

        public const string AdapterKindAttributeName = "xsi:type";

        public const string AdapterKindManaged = "managed";

        public const string AdapterKindPowerShell = "powershell";

        public const string AdapterKindShell = "shell";

        public const string AdapterKindInteractive = "interactive";

        public const string AdapterTypeAttributeName = "adaptertype";

        public const string AdapterScriptDirectoryAttributeName = "scriptdir";
    }
}
