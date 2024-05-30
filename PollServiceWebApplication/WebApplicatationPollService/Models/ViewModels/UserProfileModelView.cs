using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicatationPollService.Models.ViewModels {
    public class UserProfileModelView {
        public ApplicationUser User { get; set; }
        public bool IsAdmin { get; set; }
    }
}