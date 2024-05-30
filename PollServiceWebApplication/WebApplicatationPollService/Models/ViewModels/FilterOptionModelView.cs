using System.ComponentModel.DataAnnotations;

namespace WebApplicatationPollService.Models.ViewModels {
    public class FilterOptionModelView {
        [StringLength(50,ErrorMessage = "Phrase is too long!")]
        public string phrase { get; set; }//phrase which is used to filter elements       
        public bool orderMode { get; set; }//1- order by desceding , 0 - order by  ascending
        public string nameSort { get; set; }
        [Range(1,50)]
        public int elements { get; set; }//number of element in table 
        [Range(1, int.MaxValue)]//page start at 1
        public int page { get; set; }

        public FilterOptionModelView() {
            elements = 10;page = 1;
        }
    }
}