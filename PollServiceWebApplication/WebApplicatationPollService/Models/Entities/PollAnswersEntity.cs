using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicatationPollService.Models.Entities {
    public class PollAnswersEntity {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Answers { get; set; }
        public int Votes { get; set; }
        public virtual PollEntity Poll { get; set; }
        public virtual ICollection<ApplicationUser> ListUserWhoVoted { get; set; }
    }
}