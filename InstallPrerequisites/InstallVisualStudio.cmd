:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

:: First argument is the vs_community.exe full path
:: Update vs installer
%1 update --installPath %3 --passive --norestart

:: Install vs_community with the following 
%1 %2 --installPath %3 ^
--add Microsoft.VisualStudio.Workload.NativeDesktop ^
--add Microsoft.VisualStudio.Workload.ManagedDesktop ^
--add Microsoft.VisualStudio.Component.VC.Tools.x86.x64 ^
--add Microsoft.Net.Component.4.7.1.TargetingPack ^
--add Microsoft.Net.ComponentGroup.4.7.1.DeveloperTools ^
--add Microsoft.VisualStudio.Component.Windows10SDK.16299.Desktop ^
--add Microsoft.VisualStudio.Component.TestTools.Core ^
--add Microsoft.Component.MSBuild ^
--passive --norestart

