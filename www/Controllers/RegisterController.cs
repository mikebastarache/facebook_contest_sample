using System.Web.Mvc;
using MMContest;
using MMContest.Models;
using System.Collections.Generic;
using System;
using www.Models;

namespace www.Controllers
{
    public class RegisterController : Controller
    {
        private readonly CrmContext _db = new CrmContext();
        private readonly int _contestId = Properties.Settings.Default.ContestId;
        private readonly string _ballotSource = Properties.Settings.Default.BallotSource;

        //
        // GET: /Register/
        public ActionResult Index()
        {
            if (Request.Params["signed_request"] == null)
                return RedirectToAction("SessionLost", "Message");

            List<SelectListItem> yearsOfBirth = new List<SelectListItem>();
            for (int i = 1995; i > 1900; i--)
                yearsOfBirth.Add(new SelectListItem
                {
                    Selected = false,
                    Text = i.ToString(),
                    Value = i.ToString()
                });

            ViewBag.Salutation = new SelectList(_db.Salutations, "Id", "SalutationEn");
            ViewBag.Province = Session["Culture"].ToString() == "fr-CA" ? new SelectList(_db.Provinces, "Abbreviation", "NameFR") : new SelectList(_db.Provinces, "Abbreviation", "NameEN");
            ViewBag.YearsOfBirth = new SelectList(yearsOfBirth, "Text", "Value");
            ViewBag.Fbuid = (string)Session["fbuid"];
            ViewBag.OptIn = false;
            ViewBag.sessionvalue = "Session Value : " + Session["NowEntered"];

            // CREATE User prefilled data
            UserViewModels userModel = new UserViewModels();
            return View(userModel);
        }

        //
        // POST: /Registration/
        [HttpPost]
        public ActionResult Index(UserViewModels model)
        {
            if (Request.Params["signed_request"] == null)
                return RedirectToAction("SessionLost", "Message");


            ViewBag.Fbuid = model.Fbuid;
            ViewBag.Email = model.Email;
            ViewBag.Language = model.Language;
            ViewBag.Salutation = model.Salutation;
            ViewBag.FirstName = model.FirstName;
            ViewBag.LastName = model.LastName;
            ViewBag.Address1 = model.Address1;
            ViewBag.City = model.City;
            ViewBag.PostalCode = model.PostalCode;
            ViewBag.Province = model.Province;
            ViewBag.PostalCode = model.PostalCode;
            ViewBag.Telephone = model.Telephone;
            ViewBag.OptIn = false;

            if (!ModelState.IsValid)
            {
                List<SelectListItem> yearsOfBirth = new List<SelectListItem>();
                for (int i = 1995; i > 1900; i--)
                    yearsOfBirth.Add(new SelectListItem
                    {
                        Selected = false,
                        Text = i.ToString(),
                        Value = i.ToString()
                    });

                //SET Form to default values
                ViewBag.Salutation = new SelectList(_db.Salutations, "Id", "SalutationEn");
                ViewBag.Province = Session["Culture"].ToString() == "fr-CA" ? new SelectList(_db.Provinces, "Abbreviation", "NameFR") : new SelectList(_db.Provinces, "Abbreviation", "NameEN");
                ViewBag.state = ModelState.IsValid.ToString();

                return View();
            }

            //BIND DATA TO USER VIEW MODEL
            User userObj = new User();
            userObj.Fbuid = model.Fbuid;
            userObj.Email = model.Email;
            userObj.Language = model.Language;
            userObj.Salutation = model.Salutation;
            userObj.FirstName = model.FirstName;
            userObj.LastName = model.LastName;
            userObj.Address1 = model.Address1;
            userObj.City = model.City;
            userObj.PostalCode = model.PostalCode;
            userObj.Province = model.Province;
            userObj.PostalCode = model.PostalCode;
            userObj.Telephone = model.Telephone;
            userObj.YearOfBirth = "1900";
            userObj.ContestSignupId = _contestId;
            userObj.OptIn = false;

            FacebookLogic contest = new FacebookLogic();
            string result = contest.Register(userObj, Properties.Settings.Default.WebApiUri, "moncton");

            int userId = 0;
            if (result == "AccountUpdateSuccessful" || result == "AccountCreationSuccessful")
            {
                userId = contest.GetUserId(model.Fbuid);
                if (userId == -1)
                    result = "UserDoesNotExist";
            }

            string ballotSource = _ballotSource;
            string ipAddress = GetIpAddress();

            switch (result)
            {
                case "AccountUpdateSuccessful":
                    contest.CreateUserContestRegistration(userId, _contestId);
                    
                    // INSERT Ballot for the user into the contest
                    // No ballot required for registration
                    //contest.CreateNewBallot(userId, _contestId, ballotSource, ipAddress);

                    Response.Redirect("~/Upload/?signed_request=" + Request.Params["signed_request"]);
                    break;

                case "AccountCreationSuccessful":
                    contest.CreateUserContestRegistration(userId, _contestId);

                    // INSERT Ballot for the user into the contest
                    // No ballot required for registration
                    //contest.CreateNewBallot(userId, _contestId, ballotSource, ipAddress);

                    Response.Redirect("~/Upload/?signed_request=" + Request.Params["signed_request"]);
                    break;

                case "NotAgeOfMajority":
                    Response.Redirect("~/Message/NotAgeOfMajority?signed_request=" + Request.Params["signed_request"]);
                    break;

                case "UserDoesNotExist":
                    Response.Redirect("~/Message/UserDoesNotExist?signed_request=" + Request.Params["signed_request"]);
                    break;

                case "AccountUpdateFailed":
                    Response.Redirect("~/Message/AccountUpdateFailed?signed_request=" + Request.Params["signed_request"]);
                    break;

                case "AccountCreationFailed":
                    Response.Redirect("~/Message/AccountCreationFailed?signed_request=" + Request.Params["signed_request"]);
                    break;

                default:
                    Response.Redirect("~/Message/Index?signed_request=" + Request.Params["signed_request"]);
                    break;
            }
            return View();
        }


        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);
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
