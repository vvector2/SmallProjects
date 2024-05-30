using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Web;
using WebApplicatationPollService.Models.ViewModels;

namespace WebApplicatationPollService.Models {

    public class PaginationHandler<T> where T : class  {
        Expression<Func<T, object>> SortFunction;
        Expression<Func<T, bool>> FilterFunction;
        public PaginationHandler(Expression<Func<T, object>> _SortFunction, Expression<Func<T, bool>> _FilterFunction) {
            SortFunction = _SortFunction;
            FilterFunction = _FilterFunction;
        }
        public TableModelView<T> GetEntityFromFilterOption(FilterOptionModelView filterModelOption, 
            IQueryable<T> db) {
            IQueryable<T> query;
            if (filterModelOption.phrase != null) //filter collection using phrase
                query = db.Where(FilterFunction);
            else query = db;
            
            if (!filterModelOption.orderMode)//sort element 
                query = query.OrderBy(SortFunction);
            else query = query.OrderByDescending(SortFunction);

            int numberOfFilteredElm = query.Count();//The number will be used to calculate last page.

            IEnumerable<T> elements= query.Skip((filterModelOption.page - 1) * filterModelOption.elements).Take(filterModelOption.elements);
            return new TableModelView<T>() { NumberOfFilteredElm = numberOfFilteredElm, Elements = elements };
        }
    }
}