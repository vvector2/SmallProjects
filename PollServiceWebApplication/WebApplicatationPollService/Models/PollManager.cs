using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApplicatationPollService.Controllers;
using WebApplicatationPollService.Models.Entities;
using WebApplicatationPollService.Models.ViewModels;
using System.Data.Entity.SqlServer;

namespace WebApplicatationPollService.Models {
    public class PollManager {
        
        public TableModelView<PollEntity> GetPollsFromFilterOption(FilterOptionModelView filterModelOption,IQueryable<PollEntity> polls) {
            PaginationHandler<PollEntity> paginationHandler = new PaginationHandler<PollEntity>(GetProperSortExpression(filterModelOption.nameSort),
                GetFilterExpression(filterModelOption.phrase));
            return paginationHandler.GetEntityFromFilterOption(filterModelOption,polls );
        }
        protected virtual Expression<Func<PollEntity,object>> GetProperSortExpression (string propertyName) {
            if (propertyName == "Question") return (x => x.Question);
            else if (propertyName == "View") return (x => SqlFunctions.StringConvert( (decimal) x.View , 10) );
            else return x => x.DateTime.ToString();
        }
        protected virtual Expression<Func<PollEntity, bool>> GetFilterExpression(string phrase) {
            return x => x.Question.ToLower().Contains(phrase);
        }

        //add vote to one answer in poll
        public void Vote(VotePollModelView votePollModelView, ApplicationDbContext db) {
            var pollAnswer= db.PollAnswers.First(x => x.Id == votePollModelView.IdAnswer);
            pollAnswer.Votes++;
            db.SaveChanges();
        }
        public PollEntity AddNewPollAndReturnPoll(PollModelView pollModelView, ApplicationUser applicationUser, ApplicationDbContext db) {
            var pollEntity = new PollEntity(pollModelView, applicationUser);
            db.Polls.Add(pollEntity);
            db.SaveChanges();
            return pollEntity;
        }
    }
    public class AdminPollManager : PollManager {
        protected override Expression<Func<PollEntity, object>> GetProperSortExpression(string propertyName) {
            if (propertyName == "Question") return (x => x.Question);
            else if (propertyName == "View") return (x => SqlFunctions.StringConvert((decimal)x.View, 10));
            else if (propertyName == "Id") return (x => x.Id.ToString());
            else if (propertyName == "UserName") return (x => x.UserCreator.UserName);
            else if (propertyName == "UserChecking") return x => x.UserChecking.ToString();
            else return (x => x.DateTime.ToString());
        }
        protected override Expression<Func<PollEntity, bool>> GetFilterExpression(string phrase) {
            return x => x.Question.ToLower().Contains(phrase) || x.Id.ToString().ToLower().Contains(phrase) || x.UserCreator.UserName.ToString().ToLower().Contains(phrase);
        }
    }



}