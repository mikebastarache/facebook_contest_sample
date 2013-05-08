using System.Web.Mvc;
using System.Globalization;
using MMContest;
using MMContest.Models;
using MMContest.Dal;
using System.Text;
using System;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using Facebook;
using www.Models;


namespace www.Controllers
{
    public class HomeController : Controller
    {
        private readonly CrmContext _db = new CrmContext();
        private readonly int _contestId = Properties.Settings.Default.ContestId;

        public string AppId = Properties.Settings.Default.AppID;
        public string AppSecret = Properties.Settings.Default.AppSecret;
        public string local_url = Properties.Settings.Default.ContestUrl_En + Properties.Settings.Default.ApplicationPath_En;
        public string redirect_uri = Properties.Settings.Default.facebook_app_url + "app_" + Properties.Settings.Default.AppID;
        public string ExtendedPermissions = Properties.Settings.Default.ExtendedPermissions;
        public int ContestID = Properties.Settings.Default.ContestId;
        private readonly string _ballotSource = Properties.Settings.Default.BallotSource;


        //
        // GET: /Home/

        public ActionResult Index()
        {
            //Set current Protocol to HTTP or HTTPS
            var currentProtocol = HttpContext.Request.Url.Scheme;

            //var currentProtocol = HttpContext.Current.Request.Url.Scheme;
            string redirect_uri = currentProtocol + Properties.Settings.Default.facebook_app_url + "app_" + Properties.Settings.Default.AppID;

            //NO SIGNED REQUEST AVAILABLE BECAUSE FACEBOOK AUTHENTICATED REDIRECTED USE TO WEB HOST
            //THIS WILL HANDLE THE RE-DIRECT FROM THE WEB HOST BACK TO FACEBOOK AFTER THE USER AUTHENTICATES THE APPLICATION.
            if (Request.Params["code"] != null)
            {
                Session["NowEntered"] = false;
                Session["app_data"] = "register";
                Response.Redirect(redirect_uri, true);
            }

            if (Request.Params["error"] != null)
            {
                Session["NowEntered"] = false;
                Response.Redirect(redirect_uri, true);
            }


            //IF SIGNED REQUEST HAS NO ACCESS TOKEN, THEN APPLICATION IS NOT AUTHENTICATED
            if ((string)Session["oauth_token"] != "")
            { 
                //IF SIGNED REQUEST HAS AN AUTHENTICATED TOKEN, THEN PROCEED WITH THE APPLICATION
                //CHECK TO SEE IF THE USER HAS ENTERED THE CONTEST
                var contest = new FacebookLogic();
                switch (contest.EnterContest((string)Session["fbuid"], ContestID))
                {
                    case "userHasAlreadyEnteredToday":
                        //user has already entered contest today
                        Response.Redirect("~/Message/AlreadyEntered?signed_request=" + Request.Params["signed_request"]);
                        break;

                    case "userIsReadyToEnterTheContest":
                        // the user is back for a second time, send to upload form
                        Response.Redirect("~/Upload/?signed_request=" + Request.Params["signed_request"]);
                        break;

                    default:
                        //1st time to enter this contest
                        if ((string)Session["app_data"] == "register")
                            Response.Redirect("~/Register/?signed_request=" + Request.Params["signed_request"]);

                        return View();
                }
            }
            return View();
        }




        //
        // GET: /fblogin/
        public ActionResult fblogin()
        {
            //Set current Protocol to HTTP or HTTPS
            var currentProtocol = HttpContext.Request.Url.Scheme;

            //IF SIGNED REQUEST HAS NO ACCESS TOKEN, THEN APPLICATION IS NOT AUTHENTICATED
            if ((string)Session["oauth_token"] == "")
            {
                dynamic parameters = new ExpandoObject();
                parameters.client_id = AppId;
                parameters.client_secret = AppSecret;
                parameters.redirect_uri = currentProtocol + local_url + "Home/fblogin";

                // The requested response: an access token (token), an authorization code (code), or both (code token).
                parameters.response_type = "token";

                // add the 'scope' parameter only if we have extendedPermissions.
                if (!string.IsNullOrWhiteSpace(ExtendedPermissions))
                    parameters.scope = ExtendedPermissions;

                // generate the login url
                var fb = new FacebookClient();
                var loginUrl = fb.GetLoginUrl(parameters);

                string oauth_url = currentProtocol + "://www.facebook.com/dialog/oauth/?client_id=" + AppId + "&redirect_uri=" + currentProtocol + local_url + "&scope=" + ExtendedPermissions;

                Response.Write("<script>");
                Response.Write("var oauth_url = '" + oauth_url + "';");
                Response.Write("window.top.location = oauth_url;");
                Response.Write("</script>");

            }
            else
            {
                //IF SIGNED REQUEST HAS AN AUTHENTICATED TOKEN, THEN PROCEED WITH THE APPLICATION
                //CHECK TO SEE IF THE USER HAS ENTERED THE CONTEST
                var contest = new FacebookLogic();
                switch (contest.EnterContest((string)Session["fbuid"], ContestID))
                {
                    case "userHasAlreadyEnteredToday":
                        //user has already entered contest today
                        Response.Redirect("~/Message/AlreadyEntered?signed_request=" + Request.Params["signed_request"]);
                        break;

                    case "userIsReadyToEnterTheContest":
                        // the user is back for a second time, send to upload form
                        Response.Redirect("~/Upload/?signed_request=" + Request.Params["signed_request"]);
                        break;

                    default:
                        //1st time to enter this contest
                        Response.Redirect("~/Register/?signed_request=" + Request.Params["signed_request"]);
                        
                        break;
                }
            }
            return View();
        }

        


        //
        // GET: /Rules/
        public ActionResult Rules()
        {
            return View();
        }

        //
        // GET: /RulesPopUp/
        public ActionResult RulesPopUp()
        {
            return View();
        }

        //
        // GET: /PrivacyPolicy
        public ActionResult PrivacyPolicy()
        {
            return View();
        }

        //
        // GET: /PrivacyPopUp/
        public ActionResult PrivacyPopUp()
        {
            return View();
        }


        

        private string GetIpAddress()
        {
            // GET users IP address
            var ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(ipAddress))
            {
                var ipRange = ipAddress.Split(',');
                ipAddress = ipRange[0].Trim();
            }
            else
            {
                ipAddress = Request.ServerVariables["REMOTE_ADDR"];
            }

            return ipAddress;
        }
    }
}
