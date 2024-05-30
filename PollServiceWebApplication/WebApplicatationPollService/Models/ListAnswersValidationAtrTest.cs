using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicatationPollService.Models {
    public class ListAnswersValidationAtr : ValidationAttribute {
        public override bool IsValid(object obj) {
            if (!(obj is List<string>)) return false;
            var list = (List<string>)obj;
            for (int i = 0; i < list.Count; i++) {
                if (string.IsNullOrEmpty(list[i])) {
                    list.RemoveAt(i);
                    i--;
                } else if (list[i].Length > 50) return false;//maximum number of letter is 50 
            }
            if (list.Count >= 2 && list.Count <= 7) return true;
            else return false;
        }
    }
}