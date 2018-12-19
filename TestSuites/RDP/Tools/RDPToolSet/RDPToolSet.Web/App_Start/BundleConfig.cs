using System.Web;
using System.Web.Optimization;
// using RDPToolSet.Web.App_Start;

namespace RDPToolSet.Web
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/dropzone.js",
                "~/Scripts/popover.js",
                "~/Scripts/slider.js",
                "~/Scripts/ZeroClipboard.js",
                "~/Scripts/javascript.js"
                ));

            bundles.Add(new StyleBundle("~/content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-theme.css",
                "~/Content/bootstrap.vertical-tabs.css",
                "~/Content/dropzone.css",
                "~/Content/slider.css",
                "~/Content/site.css"
                ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));
        }
    }
}