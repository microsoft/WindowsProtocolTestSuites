#############################################################################
## Copyright (c) Microsoft. All rights reserved.
## Licensed under the MIT license. See LICENSE file in the project root for full license information.
#############################################################################

# End the domain rename operation. Removes the restrictions placed on the Directory Service 
# during the rename operation.

#This script can only be run on the system which installed AD DS and AD LDS Tools 

rendom /end

return $true