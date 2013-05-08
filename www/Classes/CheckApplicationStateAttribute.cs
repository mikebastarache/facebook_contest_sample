using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace www.Classes
{
    public class CheckApplicationStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            DateTime dateStart = Properties.Settings.Default.ContestStartDate;
            DateTime dateEnd = Properties.Settings.Default.ContestCloseDate;
            DateTime todayDate = DateTime.Now;

            // get the area, controller and action
            var controllerContext = new ControllerContext(filterContext.RequestContext, filterContext.Controller);
            var controller = (string)filterContext.RouteData.Values["controller"];
            var action = (string)filterContext.RouteData.Values["action"];

            Dictionary<string, string> publicPageList = new Dictionary<string, string>();
            publicPageList.Add("ContestClosed", "Message");
            publicPageList.Add("ContestNotOpen", "Message");
            publicPageList.Add("PrizesPopUp", "Home");
            publicPageList.Add("RulesPopUp","Home");
            publicPageList.Add("PrivacyPopUp","Home");

            if (!publicPageList.ContainsKey(action))
            {
                //CONTEST NOT OPENED YET!
                if (todayDate < dateStart)
                    filterContext.HttpContext.Response.Redirect("~/Message/ContestNotOpen?signed_request=" + filterContext.HttpContext.Request.Params["signed_request"], true);

                //CONTEST CLOSED!
                if (todayDate >= dateEnd)
                    filterContext.HttpContext.Response.Redirect("~/Message/ContestClosed?signed_request=" + filterContext.HttpContext.Request.Params["signed_request"], true);
               
            }
        }
    }
}
