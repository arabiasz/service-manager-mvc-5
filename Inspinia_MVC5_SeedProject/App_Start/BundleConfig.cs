using System.Web;
using System.Web.Optimization;

namespace Inspinia_MVC5_SeedProject
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {
            // Vendor scripts
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.1.1.min.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"
                        ));

            // jQuery Validation
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        //"~/Scripts/jquery.unobtrusive-ajax.js",
                        //"~/Scripts/jquery.unobtrusive-ajax.min.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js"));

            // Inspinia script
            bundles.Add(new ScriptBundle("~/bundles/inspinia").Include(
                      "~/Scripts/app/inspinia.js"));

            // SlimScroll
            bundles.Add(new ScriptBundle("~/plugins/slimScroll").Include(
                      "~/Scripts/plugins/slimScroll/jquery.slimscroll.min.js"));

            // jQuery plugins
            bundles.Add(new ScriptBundle("~/plugins/metsiMenu").Include(
                      "~/Scripts/plugins/metisMenu/metisMenu.min.js"));

            bundles.Add(new ScriptBundle("~/plugins/pace").Include(
                      "~/Scripts/plugins/pace/pace.min.js"));

            // CSS style (bootstrap/inspinia)
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/animate.css",
                      "~/Content/style.css"));

            // Font Awesome icons
            bundles.Add(new StyleBundle("~/font-awesome/css").Include(
                      "~/fonts/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform()));

            // dataTables css styles
            bundles.Add(new StyleBundle("~/Content/plugins/dataTables/dataTablesStyles").Include(
                      "~/Content/plugins/dataTables/datatables.min.css",
                      "~/Content/plugins/dataTables/responsive.dataTables.min.css"));

            // dataTables 
            bundles.Add(new ScriptBundle("~/plugins/dataTables").Include(
                      "~/Scripts/plugins/dataTables/datatables.min.js",
                      "~/Scripts/plugins/dataTables/buttons.colVis.min.js"));

            bundles.Add(new ScriptBundle("~/plugins/dataTables/responsive").Include(
              "~/Scripts/plugins/dataTables/dataTables.responsive.min.js"));

            // dataPicker styles
            bundles.Add(new StyleBundle("~/plugins/dataPickerStyles").Include(
                      "~/Content/plugins/datapicker/datepicker3.css"));

            // dataPicker 
            bundles.Add(new ScriptBundle("~/plugins/dataPicker").Include(
                      "~/Scripts/plugins/datapicker/bootstrap-datepicker.js"));

            // datetimepicker 
            bundles.Add(new ScriptBundle("~/plugins/dateTimePicker").Include(
                      "~/Scripts/plugins/datetimepicker/jquery.datetimepicker.full.min.js"));
            // datetimepicker styles
            bundles.Add(new StyleBundle("~/plugins/dateTimePickerStyles").Include(
                      "~/Content/plugins/datetimepicker/jquery.datetimepicker.min.css"));

            // mask 
            bundles.Add(new ScriptBundle("~/plugins/mask").Include(
                      "~/Scripts/plugins/mask/jasny-bootstrap.min.js"));
            // mask styles
            bundles.Add(new StyleBundle("~/plugins/maskStyles").Include(
                      "~/Content/plugins/mask/jasny-bootstrap.min.css"));



            bundles.IgnoreList.Ignore("*.unobtrusive-ajax.min.js", OptimizationMode.WhenDisabled);

        }
    }
}
