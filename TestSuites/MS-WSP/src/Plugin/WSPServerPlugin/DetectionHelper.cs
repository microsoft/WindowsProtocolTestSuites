// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Microsoft.Protocols.TestManager.WSPServerPlugin
{
    public static class DetectionHelper
    {
        public static MessageBuilderParameter BuildParameter()
        {
            char[] delimiter = new char[] { ',' };

            Configs configs = new Configs();
            var parameter = new MessageBuilderParameter();

            parameter.PropertySet_One_DBProperties = configs.PropertySet_One_DBProperties.Split(delimiter);

            parameter.PropertySet_Two_DBProperties = configs.PropertySet_Two_DBProperties.Split(delimiter);

            parameter.Array_PropertySet_One_Guid = new Guid(configs.Array_PropertySet_One_Guid);

            parameter.Array_PropertySet_One_DBProperties = configs.Array_PropertySet_One_DBProperties.Split(delimiter);

            parameter.Array_PropertySet_Two_Guid = new Guid(configs.Array_PropertySet_Two_Guid);

            parameter.Array_PropertySet_Two_DBProperties = configs.Array_PropertySet_Two_DBProperties.Split(delimiter);

            parameter.Array_PropertySet_Three_Guid = new Guid(configs.Array_PropertySet_Three_Guid);

            parameter.Array_PropertySet_Three_DBProperties = configs.Array_PropertySet_Three_DBProperties.Split(delimiter);

            parameter.Array_PropertySet_Four_Guid = new Guid(configs.Array_PropertySet_Four_Guid);

            parameter.Array_PropertySet_Four_DBProperties = configs.Array_PropertySet_Four_DBProperties.Split(delimiter);

            parameter.EachRowSize = MessageBuilder.RowWidth;

            parameter.EType = uint.Parse(configs.EType);

            parameter.BufferSize = uint.Parse(configs.BufferSize);

            parameter.LcidValue = uint.Parse(configs.LcidValue);

            parameter.ClientBase = uint.Parse(configs.ClientBase);

            parameter.RowsToTransfer = uint.Parse(configs.RowsToTransfer);

            return parameter;
        }
    }
}
