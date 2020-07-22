#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

[string]$uncSharePath
[string]$fileName

Copy-Item "$fileName" -Destination "$uncSharePath"
