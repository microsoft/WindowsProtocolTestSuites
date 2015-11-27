// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Protocols.TestTools.StackSdk.Security.KerberosLib
{
    /*
    TicketFlags     ::= KerberosFlags
        -- reserved(0),
        -- forwardable(1),
        -- forwarded(2),
        -- proxiable(3),
        -- proxy(4),
        -- may-postdate(5),
        -- postdated(6),
        -- invalid(7),
        -- renewable(8),
        -- initial(9),
        -- pre-authent(10),
        -- hw-authent(11),
        -- the following are new since 1510
        -- transited-policy-checked(12),
        -- ok-as-delegate(13)
    */
    public class TicketFlags : KerberosFlags
    {
        
        public TicketFlags()
            : base()
        {
        }

        public TicketFlags(string s)
            : base(s)
        {

        }
    }
}

