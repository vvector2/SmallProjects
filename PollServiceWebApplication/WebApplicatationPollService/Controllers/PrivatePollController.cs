using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicatationPollService.Models;
using WebApplicatationPollService.Models.Entities;
using WebApplicatationPollService.Models.ViewModels;

namespace WebApplicatationPollService.Controllers
{
    public class PrivatePollController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly ApplicationUserManager appUserManager;
        public PrivatePollController() {
            db = new ApplicationDbContext();
            appUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }
        public ActionResult PrivatePollAuth(int id) {
            return View( new PrivatePollPasswordModelView() {Id=id } );
        }
        [HttpPost]
        public ActionResult PrivatePollAuth(PrivatePollPasswordModelView modelView) {
            //check if poll exist
            var poll = db.Polls.Find(modelView.Id);
            if (poll == null) return new HttpNotFoundResult();
            var privatePollManager = new PrivatePollManager();
            if (privatePollManager.VerifyPassword(poll.Password, modelView.Password)) {
                Response.Cookies.Add(privatePollManager.GetSessionCookie(db, poll));//give user session that last 10 minutes
                return RedirectToAction("PollVote", "Home", new {@id=modelView.Id});
            } else {
                ModelState.AddModelError("passwdNotValid", "Password is not correct.");
                return View(modelView);
            }
        }

        [NonAction]
        public bool IsRequestAuthorised(PollEntity poll) {
            var user = appUserManager.FindById(User.Identity.GetUserId());
            if (user!=null && poll.UserCreator.Id == user.Id) return true;//creator of poll always has access 
            var privatePollManager = new PrivatePollManager();
            if (Request.Cookies["privPoll"] != null && privatePollManager.IsAuthorisedByCookie(Request.Cookies["privPoll"].Value, db)) {
                Request.Cookies["privPoll"].Expires = DateTime.Now.AddMinutes(10);//updating cookie
                return true;
            } else  return false;
            
        }
    }
}