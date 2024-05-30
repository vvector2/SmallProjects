using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplicatationPollService.Models.ViewModels {
    public class TableModelView<T> {
        public int NumberOfFilteredElm { get; set; }//number of elements after a filtration
        public IEnumerable<T> Elements { get; set; }
    }
}