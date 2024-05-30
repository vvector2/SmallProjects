using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicatationPollService.Models.Entities {
    public class SessionUserPrivatePollEntity {
        [Key]
        public Guid SessionID { get; set; }
        public DateTime DateTime { get; set; }

        public virtual PollEntity Poll { get; set; }
    }
}