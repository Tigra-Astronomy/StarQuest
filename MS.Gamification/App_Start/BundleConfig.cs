// This file is part of the MS.Gamification project
// 
// File: BundleConfig.cs  Created: 2016-05-10@22:28
// Last modified: 2016-07-30@19:23

using System.Web.Optimization;

namespace MS.Gamification
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

            // Scripts that go in the <head> section of the HTML document
            bundles.Add(new ScriptBundle("~/bundles/header").Include(
                "~/Scripts/applicationInsights.js",
                "~/Scripts/modernizr-*"
                ));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/jira").Include("~/Scripts/jira-web-collector.js"));

            bundles.Add(new ScriptBundle("~/bundles/typescript").Include(
                "~/Scripts/password.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                "~/Scripts/DataTables/jquery.dataTables.js",
                "~/Scripts/DataTables/dataTables.bootstrap.js"));


            bundles.Add(new StyleBundle("~/Content/dataTables").Include(
                "~/Content/DataTables/jquery.dataTables.css",
                "~/Content/DataTables/dataTables.bootstrap.css",
                "~/Content/dataTables.fontawesome.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/font-awesome.css",
                "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/c3Charts").Include(
                "~/Scripts/d3.js",
                "~/Scripts/c3.js"));

            bundles.Add(new StyleBundle("~/Content/c3Charts").Include(
                "~/Content/c3.css"));
            }
        }
    }