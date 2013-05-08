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
using www.Dal;

namespace www.Controllers
{
    public class UploadController : Controller
    {
        private readonly SubCrmContext _db = new SubCrmContext();
        private readonly www.Dal.UnitOfWork _unitOfWork = new www.Dal.UnitOfWork();
        private readonly int _contestId = Properties.Settings.Default.ContestId;
        private readonly string _ballotSource = Properties.Settings.Default.BallotSource;
        public int ContestID = Properties.Settings.Default.ContestId;


        //
        // GET: /Upload/

        public ActionResult Index()
        {
            if (Request.Params["signed_request"] == null)
                return RedirectToAction("SessionLost", "Message");

            ViewBag.Fbuid = (string)Session["fbuid"];
            ViewBag.UserId = 0;

            var model = new UploadViewModels();
            model.location = "1";
            model.alertMsg = null;

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(UploadViewModels model)
        {
            model.alertMsg = null;

            //CHECK TO SEE IF THE USER HAS ENTERED THE CONTEST
            var fbcontest = new FacebookLogic();
            var fbBallotCount = fbcontest.HasUserEnteredContestToday((string)Session["fbuid"], ContestID);
            if(fbBallotCount == true)
            {
                //user has already uploaded today
                Response.Redirect("~/Message/AlreadyEntered?signed_request=" + Request.Params["signed_request"]);
            }

            var tFileSize = model.filename.ContentLength;
            var tFileTypeExtension = System.IO.Path.GetExtension(model.filename.FileName);
            var tFileName = System.IO.Path.GetFileName(model.filename.FileName);
            tFileName = MakeValidFileName(tFileName);
            tFileName = DateTime.Now.ToString("yyMMddHHmmssffff") + "_" + tFileName; //add timestamp to ensure unique name
            var tPath = System.IO.Path.Combine(Server.MapPath("~/uploads"), tFileName);


            string listOfImages = ".jpg, .bmp, .png, .gif, .tiff";
            int maxImageSize = 15728640;//1000000=1MB; //15728640=15MB
            string listOfVideos = ".3g2, .3gp, .3gpp, .asf, .avi, .dat, .divx, .dv, .f4v, .flv, .m2ts, .m4v, .mkv, .mod, .mov, .mp4, .mpe, .mpeg, .mpeg4, .mpg, .mts, .nsv, .ogm, .ogv, .qt, .tod, .ts, .vob, .wmv";
            int maxVideoSize = 262144000; //MM Standard 262144000=250MB | FB=1073741824=1024MB

            bool tFileStatus = false;
            string tErrorMessage = "Incorrect file type.  Please refer to How to Upload in instructions.";
                        
            if (tFileSize > 0) //file exists
            {

                if (listOfImages.Contains(tFileTypeExtension.ToLower())) //file is image
                {
                    if (tFileSize <= maxImageSize) //image is not too large
                    {
                        tFileStatus = true;
                    }
                    else
                    {
                        tErrorMessage = "The size of the image was too large.";
                        tFileStatus = false;
                    }
                }
                if (listOfVideos.Contains(tFileTypeExtension.ToLower())) //file is video
                {

                    if (tFileSize <= maxVideoSize) //video is not too large
                    {
                        tFileStatus = true;
                    }
                    else
                    {
                        tErrorMessage = "The size of the video was too large.";
                        tFileStatus = false;
                    }
                }
            }
            else //file is blank
            {
                tErrorMessage = "You must select a file to upload.";
                tFileStatus = false;
            }


            if (tFileStatus)
            {
                model.filename.SaveAs(tPath);

                FacebookLogic contest = new FacebookLogic();
                string ballotSource = _ballotSource;
                string ipAddress = GetIpAddress();
                int userId = contest.GetUserId((string)Session["fbuid"]);

                // INSERT Ballot for the user into the contest
                contest.CreateNewBallot(userId, _contestId, ballotSource, ipAddress);

                //INSERT FILE INTO DB
                var newSubmission = new Submission
                {
                    filename = tFileName,
                    caption = model.caption,
                    description = model.description,
                    location = model.location,
                    dateCreated = DateTime.Now,
                    userID = userId,
                    contestID = model.contestID
                };

                _unitOfWork.SubmissionRepository.Insert(newSubmission);
                _unitOfWork.Save();


                //Send to thank you page
                Response.Redirect("~/Message/Thanks?signed_request=" + Request.Params["signed_request"]);

            }
            else
            {
                model.alertMsg = tErrorMessage;
            }
            return View(model);

        }


        private static string MakeValidFileName(string name)
        {
            string invalidChars = System.Text.RegularExpressions.Regex.Escape(new string(System.IO.Path.GetInvalidFileNameChars()));
            string invalidReStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);
            return System.Text.RegularExpressions.Regex.Replace(name, invalidReStr, "_");
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
