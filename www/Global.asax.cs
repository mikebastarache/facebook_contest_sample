using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Globalization;
using System.Threading;
using System.Web.WebPages;
using www.Classes;

namespace www
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public string AppId = Properties.Settings.Default.AppID;
        public string AppSecret = Properties.Settings.Default.AppSecret;
        public string local_url = Properties.Settings.Default.ContestUrl_En;
        public string redirect_uri = Properties.Settings.Default.facebook_app_url + "app_" + Properties.Settings.Default.AppID;
        public string ExtendedPermissions = Properties.Settings.Default.ExtendedPermissions;
        public int ContestID = Properties.Settings.Default.ContestId;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalFilters.Filters.Add(new SignedRequestAttribute());
            GlobalFilters.Filters.Add(new CheckApplicationStateAttribute());
            GlobalFilters.Filters.Add(new P3PHeaderAttribute());

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //Set default values for all session variables
            Session["PageID"] = 0;
            Session["oauth_token"] = null;
            Session["algorithm"] = null;
            Session["PageLiked"] = false;
            Session["fbuid"] = null;
            Session["Country"] = null;
            Session["NowEntered"] = false;
            Session["Email"] = null;
            Session["app_data"] = null;
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            //It's important to check whether session object is ready
            if (HttpContext.Current.Session == null)
                return;

            var ci = (CultureInfo)Session["Culture"];

            //Checking first if there is no value in session 
            //and set default language 
            //this can happen for first user's request
            if (ci == null)
            {
                //Sets default culture to English invariant
                string langName = "en-CA";

                //Try to get values from Accept lang HTTP header
                if (HttpContext.Current.Request.UserLanguages != null && HttpContext.Current.Request.UserLanguages.Length != 0)
                {
                    try
                    {
                        //Gets accepted list 
                        langName = HttpContext.Current.Request.UserLanguages[0].Substring(0, 5);
                    }
                    catch
                    {
                    }
                }

                ci = new CultureInfo(langName);
                Session["Culture"] = ci;
            }

            //Finally setting culture for each request
            Thread.CurrentThread.CurrentUICulture = ci;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
        }

    }
}