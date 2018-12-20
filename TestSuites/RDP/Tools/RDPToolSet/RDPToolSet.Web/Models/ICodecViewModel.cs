using System;
using System.Collections.Generic;
using CodecToolSet.Core;

namespace RDPToolSet.Web.Models
{
    public interface ICodecViewModel
    {
        String                   Name   { get; }

        IEnumerable<ICodecParam> Params { get; }

        IList<PanelViewModel>    Panels { get; }

        IList<PanelViewModel>    InPanels { get; }
    }
}
