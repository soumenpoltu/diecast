using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace DeecastZoneCC.App_Start
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                                                  "~/Content/main-assets/css/bootstrap.min.css",
                                                  "~/Content/main-assets/css/style.css",
                                                  "~/Content/main-assets/css/responsive.css",
                                                  "~/Content/main-assets/css/slimNav_sk78.css"

                                              ));
        }


    }
}