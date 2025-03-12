# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Write-Host  "Write signal file to system drive."
cmd /c ECHO "PostScript.Completed.signal" >$env:HOMEDRIVE\PostScript.Completed.signal
