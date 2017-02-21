// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Protocols.TestSuites.Identity.DeviceRegistration
{
    public class BaseModifiyOperation
    {
        public string Path;
        public ModifyOperationType Operation;
        public string Value;

        public virtual string Modify(string original)
        {
            return null;
        }
    }

    public class XmlModifyOperation : BaseModifiyOperation
    {
        /// <summary>
        /// used when it's to add/replace/remove an attribute instead of value in the xml
        /// </summary>
        public string AttributeName;

        /// <summary>
        /// NOTICE! when modify xml, for each child level, need specify the index of child element, 
        /// because xml supports multiple child elements with same name, 
        /// proper xml path string is like "envelop\body\container\child:1\leafnode", 
        /// means there are multiple nodes named "child" and we go to the one with index 1
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        public override string Modify(string original)
        {
            return base.Modify(original);
        }
    }

    public class JsonModifyOperation : BaseModifiyOperation
    {
        public override string Modify(string original)
        {
            return base.Modify(original);
        }
    }

    public enum ModifyOperationType
    {
        Add,
        Replace,
        Remove
    }
}
