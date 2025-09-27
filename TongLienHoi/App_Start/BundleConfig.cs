using System.Web;
using System.Web.Optimization;

namespace HTTLVN.QLTLH.App_Start
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // jQuery
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-2.1.1.min.js"));

            // jQueryUI CSS
            bundles.Add(new ScriptBundle("~/Scripts/plugins/jquery-ui/jqueryuiStyles").Include(
                        "~/Scripts/plugins/jquery-ui/jquery-ui.css"));

            // jQueryUI 
            bundles.Add(new StyleBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/plugins/jquery-ui/jquery-ui.min.js"));

            // Bootstrap
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js"));
            // kendo css
            //2017.3.1018
            bundles.Add(new StyleBundle("~/plugins/kendocss").Include(
                      "~/Content/kendo/2014.2.716/kendo.common-bootstrap.min.css",
                      "~/Content/kendo/2014.2.716/kendo.mobile.all.min.css",
                      "~/Content/kendo/2014.2.716/kendo.dataviz.min.css",
                      "~/Content/kendo/2014.2.716/kendo.bootstrap.min.css",
                      "~/Content/kendo/2014.2.716/kendo.dataviz.bootstrap.min.css"));
            bundles.Add(new StyleBundle("~/Content/plugins/dataTables/dataTablesStyles").Include(
                    "~/Content/Content/plugins/dataTables/datatables.min.css"));

            // dataTables 
            bundles.Add(new ScriptBundle("~/plugins/dataTables").Include(
                      "~/Content/Content/plugins/dataTables/datatables.min.js"));

            // kendo js
            bundles.Add(new ScriptBundle("~/plugins/kendojs").Include(

                "~/Scripts/kendo/2014.2.716/jquery.min.js",
                "~/Scripts/kendo/2014.2.716/kendo.all.js",
                "~/Scripts/kendo/2014.2.716/kendo.web.min.js",
                "~/Scripts/kendo/2014.2.716/kendo.aspnetmvc.min.js",
               
                "~/Scripts/kendo.modernizr.custom.js"));
        }
    }
}