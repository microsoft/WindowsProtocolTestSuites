// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CodecToolSet.Core;
using System;
using System.Collections.Generic;

namespace RDPToolSet.Web.Models
{
    public interface ICodecViewModel
    {
        String Name { get; }

        IEnumerable<ICodecParam> Params { get; }

        IList<PanelViewModel> Panels { get; }

        IList<PanelViewModel> InPanels { get; }
    }
}
