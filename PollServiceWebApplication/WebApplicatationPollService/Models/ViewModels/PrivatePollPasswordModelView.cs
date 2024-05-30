using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicatationPollService.Models.ViewModels {
    
    public class PrivatePollPasswordModelView {
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public int Id { get; set; }

    }
}