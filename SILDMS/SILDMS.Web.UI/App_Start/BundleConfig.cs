
using System.Web;
using System.Web.Optimization;

namespace SILDMS.Web.UI
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

          
 
    //<script src="~/Scripts/jquery-2.1.4.min.js"></script>    
  

    //<script src="~/Scripts/bootstrap.min.js"></script>
    //<script src=""></script>

    //<script src=""></script>
    //<script src=""></script>
    //<script src=""></script>
    //<script src=""></script>

    //<script src=""></script>
    //<script src=""></script>

    //<script src=""></script>



            bundles.Add(new StyleBundle("~/bundles/css").Include(
                      "~/Content/bootstrap.min.css",
                      //"~/Content/css/font-awesome.min.css",
                      "~/Content/css/ionicons.min.css",
                      "~/Scripts/AdminLTE/plugins/morris/morris.css",
                      "~/Scripts/AdminLTE/plugins/jvectormap/jquery-jvectormap-1.2.2.css",
                      "~/Content/AdminLTE/AdminLTE.min.css",
                      "~/Content/AdminLTE/skins/_all-skins.min.css",
                      "~/Content/plugin/toaster/toastr.css",
                      "~/Content/css/animate.css",
                      "~/Scripts/AdminLTE/plugins/datepicker/datepicker/css/datepicker.css",
                     "~/Content/CustomSite.css"

                      ));

            bundles.Add(new ScriptBundle("~/bundles/preScript").Include(
                         "~/Scripts/jquery-{version}.js",
                         "~/Scripts/bootstrap.min.js",
                         "~/Scripts/respond.js",
                         "~/Scripts/angular.min.js",
                         "~/Scripts/angular-route.min.js",
                         "~/Scripts/jquery.validate.min.js",
                         "~/Content/plugin/smart-table/smart-table.min.js",
                         "~/Content/plugin/toaster/toastr.js",
                         "~/Scripts/angular-validation/angular-validation.min.js",
                         "~/Scripts/App/custom.js"
                         ));


            bundles.Add(new ScriptBundle("~/bundles/postScript").Include(
                      "~/Scripts/AdminLTE/plugins/datepicker/datepicker/js/bootstrap-datepicker.js",
                      "~/Scripts/AdminLTE/app.js",
                      "~/Scripts/AdminLTE/plugins/sparkline/jquery.sparkline.min.js",
                      "~/Scripts/AdminLTE/plugins/slimScroll/jquery.slimscroll.min.js"
                      ));


            BundleTable.EnableOptimizations = true;

        }
    }
}
