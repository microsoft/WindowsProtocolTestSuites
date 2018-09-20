using System.Collections.Generic;
using System.Linq;

namespace RDPToolSet.Web.Models
{
    public class PanelViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public IEnumerable<TabViewModel> Tabs { get; set; }

        public PanelViewModel(string id, string title)
        {
            Id = id;
            Title = title;
            Tabs = new List<TabViewModel>();
        }

        internal PanelViewModel(string id, string title, params string[] tabs)
            : this(id, title, false, tabs)
        { }

        internal PanelViewModel(string id, string title, bool tabEditable, params string[] tabs)
        {
            this.Title = title;
            this.Id = id.ToLowerInvariant();

            if (tabs != null) {
                this.Tabs = new List<TabViewModel>(
                    tabs.Select(tab => new TabViewModel {
                        Content = null,
                        Title = tab,
                        Id = string.Format("{0}-{1}", id, tab.Replace(' ', '-')).ToLowerInvariant(),
                        Editable = tabEditable,
                    }));
            }
        }

        internal PanelViewModel(string id, string title, bool tabEditable, string[] tabs, string[] contents)
        {
            this.Title = title;
            this.Id = id.ToLowerInvariant();

            Tabs = new List<TabViewModel>();
            if (tabs != null)
            {
                for (int i = 0; i < tabs.Length; i++)
                {
                    var content = contents == null ? null : (i < contents.Length ? contents[i] : null);
                    var tab = new TabViewModel
                        {
                            Content = content,
                            Title = tabs[i],
                            Id = string.Format("{0}-{1}", id, tabs[i].Replace(' ', '-')).ToLowerInvariant(),
                            Editable = tabEditable,
                        };
                    ((List<TabViewModel>)Tabs).Add(tab);
                }
            }
        }
    }

    public class TabViewModel 
    {
        public string Id       { get; set; }
        public string Title    { get; set; }
        public string Content  { get; set; }
        public bool   Editable { get; set; }
    }

    public class DialogViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}