using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplicatationPollService.Models;
using Microsoft.AspNet.Identity;
using WebApplicatationPollService.Models.Entities;
using WebApplicatationPollService.Models.ViewModels;
using Microsoft.AspNet.Identity.Owin;

namespace WebApplicatationPollService.Controllers {
    public class HomeController : Controller {

        private readonly ApplicationDbContext db;
        private readonly PollManager pollManager;

        private readonly ApplicationUserManager appUserManager;
        public HomeController() {
            db = new ApplicationDbContext();
            pollManager = new PollManager();
            appUserManager= new ApplicationUserManager(new UserStore<ApplicationUser>(db));
        }
        //home page
        public ActionResult Index() {
            return View();
        }

        //list of all polls 
        public ActionResult Explore(FilterOptionModelView filterOptionModelView) {
            if (ModelState.IsValid) {
                var tableModelView = pollManager.GetPollsFromFilterOption(filterOptionModelView, db.Set<PollEntity>());
                ViewBag.Elements = tableModelView.NumberOfFilteredElm;
                return View(tableModelView);
            } else return new HttpStatusCodeResult(500);
        }

        [Authorize]
        public ActionResult CreatePoll() {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreatePoll(PollModelView pollModelView) {
            if (!ModelState.IsValid) return View(pollModelView);
            var user = appUserManager.FindById(User.Identity.GetUserId());
            if (user.CreatedPoll.Count > 10) {
                ModelState.AddModelError("LimitPollUser", "You can't have more than 10 poll on your account. You must delete some old one.");
                return View(pollModelView);
            }          
            var poll = pollManager.AddNewPollAndReturnPoll(pollModelView, user, db);
            return RedirectToAction("PollVote", new { id = poll.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PollVote(VotePollModelView votePollModelView) {
            var poll = db.Polls.Find(votePollModelView.IdPoll);
            var pollAnswer = db.PollAnswers.Find(votePollModelView.IdAnswer);
            if (poll == null || pollAnswer == null) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            if (!IsPrivPollAuthorised(poll)) return RedirectToAction("PrivatePollAuth", "PrivatePoll", new { id = votePollModelView.IdPoll });
            
            if (poll.UserChecking ) {//checking if user is logged in
                if (!User.Identity.IsAuthenticated) return new HttpStatusCodeResult(System.Net.HttpStatusCode.Unauthorized);
                else {
                    var user =appUserManager.FindById(User.Identity.GetUserId());
                    if (user.VotedPoll.Any(x => x.Poll == poll)) {
                        ModelState.AddModelError("UserAlreadyVote", "You have already voted in this poll.");
                        return View(poll);//if user have already voted on this poll return error
                    } else user.VotedPoll.Add(pollAnswer);//adding pollAnswer entity to list of Votedpoll of user
                }
            }
            //to do ip or cookie
            pollManager.Vote(votePollModelView, db);
            return RedirectToAction("PollResult", new { id=poll.Id});
        }
        public ActionResult PollResult(int id) {
            return GetPollAndReturnView("PollResult", id);
        }
        public ActionResult PollVote(int id) {
            return GetPollAndReturnView("PollVote", id);
        }

        //Get access to the poll and put as a model in View 
        [NonAction]
        private ActionResult GetPollAndReturnView(string nameOfView, int id) {
            PollEntity poll = db.Polls.Find(id);            
            if (poll == null) return new HttpStatusCodeResult(404);//check if the poll exists
            if (!IsPrivPollAuthorised(poll)) return RedirectToAction("PrivatePollAuth", "PrivatePoll" ,new { id });
            poll.View++;
            db.SaveChanges();
            return View(nameOfView, poll);
        }
        
        [NonAction]
        private bool IsPrivPollAuthorised(PollEntity poll) {
            if (!string.IsNullOrEmpty(poll.Password)) {
                var controller = DependencyResolver.Current.GetService<PrivatePollController>();
                controller.ControllerContext = new ControllerContext(Request.RequestContext, this);
                return controller.IsRequestAuthorised(poll);
            } else return true;
        }


    }
}