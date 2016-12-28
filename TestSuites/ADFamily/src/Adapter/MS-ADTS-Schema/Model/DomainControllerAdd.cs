// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Modeling;

namespace Microsoft.Protocol.TestSuites.ActiveDirectory.Adts.Schema
{
    public partial class ModelDomainController
    {
        /// <summary>
        /// Adds an object to DC at parent location. 
        /// The objects content is specified by a sequence of strings where each string
        /// consists of a pair of attribute name and value.
        /// </summary>
        /// <param name="parentDN">The DN of parent.</param>
        /// <param name="attributes">All attributes of model object.</param>
        public ModelResult Add(string parentDN, params string[] attributes)
        {
            ModelObject parent = TryFindObject(parentDN);

            if (parent == null)
            {
                return new ModelResult(ResultCode.NoSuchObject, "cannot find '{0}'", parentDN);
            }

            ModelObject obj = new ModelObject();
            obj.dc = this;

            foreach (string line in attributes)
            {
                string attr, valueString;
                string[] splits = line.Split(new char[] { ':' }, 2);
                if (splits == null || splits.Length != 2)
                {
                    return new ModelResult(ResultCode.InvalidAttributeSyntax);
                }
                attr = splits[0].Trim();
                valueString = splits[1].Trim();
                ModelObject attrObj;

                if (!TryGetAttribute(splits[0], out attrObj))
                {
                    return new ModelResult(ResultCode.NoSuchAttribute, attr);
                }

                AttributeContext parsingContext = GetAttributeContext(attr);
                obj[attr] = parsingContext.Parse(valueString);

                if (Checks.HasDiagnostics)
                {
                    return new ModelResult(ResultCode.InvalidAttributeSyntax, Checks.GetAndClearDiagnostics());
                }
            }
            string rdnName = GetRDNAttributeName(obj);

            if (rdnName != null)
            {
                string rdn = (string)obj[rdnName];
                if (parent.childs.ContainsKey(rdn))
                {
                    return new ModelResult(ResultCode.EntryAlreadyExists);
                }
                AddChild(parent, obj);
                Consistency.Check(new Sequence<ModelObject>(obj));
            }
            if (Checks.HasDiagnostics)
            {
                return new ModelResult(ResultCode.ConstraintViolation, Checks.GetAndClearDiagnostics());
            }
            else
            {
                return ModelResult.Success;
            }
        }

    }
}
