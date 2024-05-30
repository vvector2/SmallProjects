using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebApplicatationPollService.Models;
using WebApplicatationPollService.Models.ViewModels;

namespace WebApplicatationPollService.Models.Entities {
    public class PollEntity {
        [Key]
        public int Id { get; set; }
        [StringLength(maximumLength:50)]
        public string Question { get; set; }
        public int View { get; set; }
        public DateTime DateTime { get; set; }
        public string Password { get; set; }
        public bool UserChecking { get; set; }
        public virtual ICollection<PollAnswersEntity> Answers { get; set; }
        public virtual ApplicationUser UserCreator { get; set; }
        

        public PollEntity() {

        }
        public PollEntity(PollModelView pollModelView, ApplicationUser user) {
            Question = pollModelView.Question;
            DateTime = DateTime.Now;
            UserChecking = pollModelView.UserChecking;
            View = 0;
            UserCreator = user;
            Answers = new List<PollAnswersEntity>();
            foreach (var item in pollModelView.Answers) Answers.Add(new PollAnswersEntity() { Answers = item, Votes = 0, Poll = this });          
            if (!string.IsNullOrEmpty(pollModelView.Password)) {
                var privPollManager = new PrivatePollManager();
                Password = privPollManager.HashPassword(pollModelView.Password);
            }
        }
    }
}