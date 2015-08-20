# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

Please add your script to config signing policy for Non-Windows SUT.

/// <summary>
/// Config the SUT signing policy.
/// when signState is DISABLED, set enablesecuritysignature to 0,and requiresecuritysignature to 0.
/// when signState is ENABLED, set enablesecuritysignature to 1,and requiresecuritysignature to 0.
/// when signState is REQUIRED, set enablesecuritysignature to 0,and requiresecuritysignature to 1.
/// when signState is DISABLEDUNLESSREQUIRED, set enablesecuritysignature to 0,and requiresecuritysignature to 0.
/// </summary>
/// <param name="serverName">The SUT name.</param>
/// <param name="signState">A state that determines whether this node signs messages.</param>

/// void ConfigSignState(string sutName, string signState);  

exit 
        
