using MMContest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using www.Models;

namespace www.Controllers
{
    public class MessageController : Controller
    {
        private readonly www.Dal.UnitOfWork _unitOfWork = new www.Dal.UnitOfWork();
        

        //
        // GET: /Message/
        public ActionResult Index()
        {
            return View();
        }


        //
        // GET: /Message/AccountCreationFailed
        public ActionResult AccountCreationFailed()
        {
            return View();
        }


        //
        // GET: /Message/AccountCreationFailed
        public ActionResult AccountUpdateFailed()
        {
            return View();
        }


        //
        // GET: /Message/AlreadyEntered
        public ActionResult AlreadyEntered()
        {
            FacebookLogic contest = new FacebookLogic();
            int userId = contest.GetUserId((string)Session["fbuid"]);
            var uploadedFiles = _unitOfWork.SubmissionRepository.GetMany(f => f.userID == userId);
            return View(uploadedFiles);
        }


        //
        // GET: /Message/BallotError
        public ActionResult BallotError()
        {
            return View();
        }


        //
        // GET: /Message/ContestClosed
        public ActionResult ContestClosed()
        {
            return View();
        }


        //
        // GET: /Message/ContestNotOpen
        public ActionResult ContestNotOpen()
        {
            return View();
        }


        //
        // GET: /Message/FacebookAuthenticationFailed
        public ActionResult FacebookAuthenticationFailed()
        {
            return View();
        }


        //
        // GET: /Message/NotAgeOfMajority
        public ActionResult NotAgeOfMajority()
        {
            return View();
        }


        //
        // GET: /Message/SessionLost
        public ActionResult SessionLost()
        {
            return View();
        }


        //
        // GET: /Message/UserDoesNotExist
        public ActionResult UserDoesNotExist()
        {
            return View();
        }

        //
        // GET: /Message/Thanks
        public ActionResult Thanks()
        {
            FacebookLogic contest = new FacebookLogic();
            int userId = contest.GetUserId((string)Session["fbuid"]);
            var uploadedFiles = _unitOfWork.SubmissionRepository.GetMany(f => f.userID == userId);
            return View(uploadedFiles);
        }

    }
}
