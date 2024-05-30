using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApplicatationPollService.Models.ViewModels;

namespace WebApplicatationPollService.Models {
    public class AdminApplicationUserMenager : ApplicationUserManager{
        public AdminApplicationUserMenager(IUserStore<ApplicationUser> store ) : base(store) {

        }
        public TableModelView<ApplicationUser> GetListUser(FilterOptionModelView filterOptionModelView, IQueryable<ApplicationUser> applicationUsers) {
            var paginationHandler = new PaginationHandler<ApplicationUser>(GetProperSortExpression(filterOptionModelView.nameSort), GetFilterExpression(filterOptionModelView.phrase));
            return paginationHandler.GetEntityFromFilterOption(filterOptionModelView,applicationUsers);
        }
        private Expression<Func<ApplicationUser, object>> GetProperSortExpression(string nameSort) {
            if (nameSort == "UserName") return x => x.UserName;
            else if (nameSort == "Email") return x => x.Email;
            else  return x => x.Id.ToString();
        } 
        private Expression<Func<ApplicationUser, bool>> GetFilterExpression(string phrase) {
            return x => x.UserName.ToLower().Contains(phrase) ||
            x.Id.ToLower().Contains(phrase) ||
            x.Email.ToLower().Contains(phrase);          
        } 

    }
}