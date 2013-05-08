using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Web.Mvc;

namespace www.Classes
{
    public class SignedRequestAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //GETS SIGNED REQUEST FROM FACEBOOK WHEN APPLICATION IS LOADED
            if (!String.IsNullOrEmpty(filterContext.HttpContext.Request.Params["signed_request"]))
            {
                string payload = filterContext.HttpContext.Request.Params["signed_request"].Split('.')[1];
                var encoding = new UTF8Encoding();
                var decodedJson = payload.Replace("=", string.Empty).Replace('-', '+').Replace('_', '/');
                var base64JsonArray = Convert.FromBase64String(decodedJson.PadRight(decodedJson.Length + (4 - decodedJson.Length % 4) % 4, '='));
                var json = encoding.GetString(base64JsonArray);
                var o = JObject.Parse(json);

                string PageID = Convert.ToString(o.SelectToken("page.id")).Replace("\"", "");
                string oauth_token = Convert.ToString(o.SelectToken("oauth_token")).Replace("\"", "");
                string algorithm = Convert.ToString(o.SelectToken("algorithm")).Replace("\"", "");
                string PageLiked = Convert.ToString(o.SelectToken("page.liked")).Replace("\"", "");
                string fbuid = Convert.ToString(o.SelectToken("user_id")).Replace("\"", "");
                string Country = Convert.ToString(o.SelectToken("user.country")).Replace("\"", "");
                string locale = Convert.ToString(o.SelectToken("user.locale")).Replace("\"", "");

                //SET SESSION VALUES
                filterContext.HttpContext.Session["PageID"] = PageID;
                filterContext.HttpContext.Session["oauth_token"] = oauth_token;
                filterContext.HttpContext.Session["algorithm"] = algorithm;
                filterContext.HttpContext.Session["PageLiked"] = PageLiked;
                filterContext.HttpContext.Session["fbuid"] = fbuid;
                filterContext.HttpContext.Session["Country"] = Country;

                //Sets culture based on Facebook
                string langName = "en-CA";

                if (locale.Substring(0, 2) == "fr")
                {
                    langName = "fr-CA";
                }
                var ci = new CultureInfo(langName);
                filterContext.HttpContext.Session["locale"] = langName;
                filterContext.HttpContext.Session["Culture"] = ci;

                System.Threading.Thread.CurrentThread.CurrentCulture = ci;
                System.Threading.Thread.CurrentThread.CurrentUICulture = ci;
            }
        }
    }
}