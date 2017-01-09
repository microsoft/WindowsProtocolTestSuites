// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Modeling;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{  
    /// <summary>
    /// The kind of a NC (name context) replica.
    /// </summary>
    public enum ReplicaKind
    {
        /// <summary>
        /// The domain NC.
        /// </summary>
        Domain,

        /// <summary>
        /// The application NC.
        /// </summary>
        Application,

        /// <summary>
        /// The configuration NC.
        /// </summary>
        Configuration,

        /// <summary>
        /// The schema NC.
        /// </summary>
        Schema
    }


    /// <summary>
    /// The replica of a naming context NC.
    /// </summary>
    public class Replica
    {
        /// <summary>
        /// The DC this replica belongs to.
        /// </summary>
        public ModelDomainController dc;

        /// <summary>
        /// The kind of the replica.
        /// </summary>
        public ReplicaKind kind;

        /// <summary>
        /// The DN of the NC.
        /// </summary>
        public string dn;

        /// <summary>
        /// Whether this is a partial replica.
        /// </summary>
        public bool partial;

        /// <summary>
        /// The root object in this replica.
        /// </summary>
        public ModelObject root;


        /// <summary>
        /// Constructs a new replica.
        /// </summary>
        /// <param name="dc">Domain controller</param>
        /// <param name="kind">Replica kind.</param>
        /// <param name="dn">Distinguished name.</param>
        /// <param name="partial">Is it partial replica</param>
        public Replica(ModelDomainController dc, ReplicaKind kind, string dn, bool partial)
        {
            this.dc = dc;
            this.kind = kind;
            this.dn = dn;
            this.partial = partial;
            this.root = new ModelObject();
            root.dc = dc;
            AttributeContext objectClassContext =
                new AttributeContext(
                    null, 
                    StandardNames.objectClass,                        
                    Syntax.StringObjectIdentifier,                         
                    false, 
                    null);
            AttributeContext cnContext =
                new AttributeContext(
                    null, 
                    StandardNames.cn,                      
                    Syntax.StringUnicode);

            switch (kind)
            {
                case ReplicaKind.Domain:
                    root[StandardNames.objectClass] =
                        objectClassContext.Parse(StandardNames.top + ";" + StandardNames.domainDNS);
                    break;
                case ReplicaKind.Application:
                    root[StandardNames.objectClass] =
                        objectClassContext.Parse(StandardNames.top + ";" + StandardNames.domainDNS);
                    break;
                case ReplicaKind.Configuration:
                    root[StandardNames.cn] = cnContext.Parse("Configuration");
                    root[StandardNames.objectClass] =
                        objectClassContext.Parse(StandardNames.top + ";" + StandardNames.configuration);
                    break;
                case ReplicaKind.Schema:
                    root[StandardNames.cn] = cnContext.Parse("Schema");
                    root[StandardNames.objectClass] =
                        objectClassContext.Parse(StandardNames.top + ";" + StandardNames.dMD);
                    break;
            }
        }


        /// <summary>
        /// This method is used to print the object sequence.
        /// </summary>
        /// <param name="writer">Stream to write.</param>
        public void Print(IndentedTextWriter writer)
        {
            writer.WriteLine("Replica {0}; {1}, partial={2} {{", dn, kind.ToString(), partial);
            writer.Indent += 2;

            if (root != null)
            {
                root.Print(writer);
            }
            writer.Indent -= 2;
            writer.WriteLine("}");
        }       
    }
}
