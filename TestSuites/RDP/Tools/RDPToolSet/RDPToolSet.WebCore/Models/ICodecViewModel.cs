using CodecToolSet.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RDPToolSet.WebCore.Models
{
    public interface ICodecViewModel
    {
        String Name { get; }

        IEnumerable<ICodecParam> Params { get; }

        IList<PanelViewModel> Panels { get; }

        IList<PanelViewModel> InPanels { get; }
    }
}
