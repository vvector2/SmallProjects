using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicatationPollService.Models;
using WebApplicatationPollService.Models.Entities;
using WebApplicatationPollService.Models.ViewModels;

namespace WebApplicatationPollService.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly AdminPollManager AdminPollManager;
        private readonly AdminApplicationUserMenager appUserManager;
        public AdminController() {
            db = new ApplicationDbContext();
            AdminPollManager = new AdminPollManager();
            appUserManager = new AdminApplicationUserMenager(new UserStore<ApplicationUser>(db));
        }

        //show list of all polls
        public ActionResult Polls(FilterOptionModelView filterOptionModelView){
            if (!ModelState.IsValid) return new HttpStatusCodeResult(400);
            var tableModelView = AdminPollManager.GetPollsFromFilterOption(filterOptionModelView, db.Set<PollEntity>().Include("UserCreator"));
            ViewBag.Elements = tableModelView.NumberOfFilteredElm;
            return View(tableModelView);
        }
        //show list of all user
        public ActionResult Users(FilterOptionModelView filterOptionModelView) {
            if (!ModelState.IsValid) return new HttpStatusCodeResult(400);
            var tableModelView = appUserManager.GetListUser(filterOptionModelView, db.Set<ApplicationUser>());
            ViewBag.Elements = tableModelView.NumberOfFilteredElm;
            return View(tableModelView);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePoll( int id) {
            var poll = db.Polls.Include("Answers").Where(x => x.Id == id).FirstOrDefault();
            if (poll == null) return new HttpStatusCodeResult(500);
            var sessions = db.SessionPrivatePoll.Where(x => x.Poll.Id == id).Select(x => x);
            db.SessionPrivatePoll.RemoveRange(sessions);
            db.Polls.Remove(poll);
            db.SaveChanges();
            return RedirectToAction("Polls");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteUser(string id) {
            var user = db.Users.Include(x => x.CreatedPoll).Include(x=>x.VotedPoll).Where(x=> x.Id==id).FirstOrDefault();
            if (user == null) return new HttpStatusCodeResult(500);
            appUserManager.Delete(user);
            return RedirectToAction("Users");
        }
    }
}