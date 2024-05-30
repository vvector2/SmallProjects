using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplicatationPollService.Models.ViewModels {
    public class PollModelView {
        [Required]
        [StringLength(50)]
        public string Question { get; set; }
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }        
        public bool UserChecking { get; set; }
        [ListAnswersValidationAtr]
        public List<string> Answers { get; set; }
    }
}

