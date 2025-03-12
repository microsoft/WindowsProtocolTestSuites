# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Write-Host  "Write signal file to system drive."
cmd /c ECHO "Install.Completed.signal" >$env:HOMEDRIVE\Install.Completed.signal
