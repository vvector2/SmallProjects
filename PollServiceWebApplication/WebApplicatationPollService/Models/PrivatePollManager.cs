using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using WebApplicatationPollService.Models.Entities;

namespace WebApplicatationPollService.Models {
    public class PrivatePollManager {
        private SessionUserPrivatePollEntity session;
        private const int sessionTimeoutMinute = 10;
        //check if session still last 
        public bool IsAuthorisedByCookie(string cookieCode, ApplicationDbContext db) {
            var session = db.SessionPrivatePoll.Where(x => x.SessionID.ToString() == cookieCode).FirstOrDefault();
            this.session = session;
            if (session == null || session.DateTime.AddMinutes(10) < DateTime.Now) return false;
            else {
                session.DateTime = DateTime.Now;
                db.SaveChanges();
                return true;
            }
        }
        //Create new session in datebase and return cookie
        public HttpCookie GetSessionCookie(ApplicationDbContext db , PollEntity poll) {
            var idSession = Guid.NewGuid();
            db.SessionPrivatePoll.Add(new SessionUserPrivatePollEntity() { SessionID = idSession, DateTime = DateTime.Now, Poll = poll });
            var cookie = new HttpCookie("privPoll");
            cookie.Expires = DateTime.Now.AddMinutes(sessionTimeoutMinute);
            cookie.Value = idSession.ToString();
            db.SaveChanges();
            return cookie;
        }
        
        public string HashPassword(string password) {
            var sha1 = new SHA1CryptoServiceProvider();
            return Convert.ToBase64String(sha1.ComputeHash(Encoding.ASCII.GetBytes(password)));
        } 
        public bool VerifyPassword(string password , string notHashedPassword) {
            if (password == HashPassword(notHashedPassword)) return true;
            else return false;
        }

    }
}