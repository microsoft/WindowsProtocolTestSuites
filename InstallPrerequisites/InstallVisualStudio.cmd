:: Copyright (c) Microsoft. All rights reserved.
:: Licensed under the MIT license. See LICENSE file in the project root for full license information.

:: First argument is the vs_community.exe full path
:: Update vs installer
%1 update --installPath %3 --passive --norestart

:: Install vs_community with the following 
%1 %2 --installPath %3 ^
--add Microsoft.NetCore.Component.SDK ^
--add Microsoft.VisualStudio.Component.Roslyn.Compiler ^
--add Microsoft.VisualStudio.Component.VC.Tools.x86.x64	 ^
--add Microsoft.VisualStudio.Component.VC.CLI.Support ^
--add Microsoft.VisualStudio.Component.VC.Redist.14.Latest ^
--add Microsoft.VisualStudio.Component.VC.CoreIde ^
--add Microsoft.VisualStudio.Component.Windows10SDK.19041 ^
--passive --norestart

