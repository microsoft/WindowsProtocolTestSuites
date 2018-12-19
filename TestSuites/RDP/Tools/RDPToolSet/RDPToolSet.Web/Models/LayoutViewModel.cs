using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RDPToolSet.Web.Models
{
    public class LayoutViewModel
    {
        public string Title { get; private set; }
        public string LinkName { get; private set; }
        public string LinkController { get; private set; }
        public string Project { get; private set; }
        public string Author { get; private set; }
        public string Version { get; private set; }
        public List<Menu> Menu { get; private set; }

        public LayoutViewModel(string title, string linkname, string linkcontroller, string project, string author, string version, List<Menu> menu)
        {
            Title = title;
            LinkName = linkname;
            LinkController = linkcontroller;
            Project = project;
            Author = author;
            Version = version;
            Menu = menu;
        }

        public LayoutViewModel(string title, string linkname, string linkcontroller, string project, string author, string version, params Menu[] menus)
        {
            Title = title;
            LinkName = linkname;
            LinkController = linkcontroller;
            Project = project;
            Author = author;
            Version = version;
            Menu = new List<Menu>(menus);
        }

    }

    public class Menu
    {
        public string Name { get; private set; }
        public List<SubMenu> SubMenu { get; private set; }

        public Menu(string name, List<SubMenu> subMenu)
        {
            Name = name;
            SubMenu = subMenu;
        }

        public Menu(string name, params string[] submenu)
        {
            var submenuList = new List<SubMenu>();
            for (int i = 0; i < submenu.Length; i += 2)
            {
                if (i + 1 < submenu.Length)
                {
                    submenuList.Add(new SubMenu(submenu[i], submenu[i + 1]));
                }
            }
            Name = name;
            SubMenu = submenuList;
        }
    }

    public class SubMenu
    {
        public string SubName { get; private set; }
        public string @Controller { get; private set; }

        public SubMenu(string subName, string controller)
        {
            SubName = subName;
            @Controller = controller;
        }
    }
}