// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;
#if !NETSTANDARD2_0
using System.Runtime.Versioning;
#endif

[assembly: AssemblyCompany("Microsoft")]
[assembly: AssemblyProduct("Windows Protocol Test Suites")]
[assembly: AssemblyVersion("4.23.3.0")]
#if !NETSTANDARD2_0
[assembly: RequiresPreviewFeatures]
#endif