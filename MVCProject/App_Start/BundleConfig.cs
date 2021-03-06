﻿using System.Web;
using System.Web.Optimization;

namespace MVCProject
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/frontend").Include(
                      "~/Scripts/app.js",
                      "~/Scripts/main.js",
                      "~/Scripts/notify.min.js",
                      "~/Scripts/accounting.min.js",
                      "~/Scripts/alertify.min.js",
                      "~/Scripts/jquery.dataTables.min.js",
                      "~/Scripts/dataTables.bootstrap.min.js", 
                      "~/Scripts/select2.full.min.js",
                      "~/Scripts/libs/fancybox/jquery.fancybox.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/modalStyles.css",
                      "~/Content/AdminLTE.min.css",
                      "~/Content/skin-blue.min.css",
                      "~/Content/jquery.datetimepicker.css",
                      "~/Content/alertify.css",
                      "~/Content/semantic.min.css",
                      "~/Content/dataTables.bootstrap.css",
                      "~/Content/select2.min.css", 
                      "~/Scripts/libs/fancybox/jquery.fancybox.css"));
        }
    }
}
