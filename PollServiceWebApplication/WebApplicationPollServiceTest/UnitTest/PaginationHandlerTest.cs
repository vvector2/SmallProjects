using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplicatationPollService.Models;
using WebApplicatationPollService.Models.Entities;
using WebApplicatationPollService.Models.ViewModels;

namespace WebApplicationPollServiceTest {
    //this tests check correctness of function GetEntityFromFilterOption
    [TestClass]
    public class PaginationHandlerTest {
        private List<PollEntity> listPolls; 
        public PaginationHandlerTest() {
            listPolls = new List<PollEntity>() {//seeding objects
                new PollEntity(){Id=0, Question="Q1", DateTime=DateTime.Now.AddDays(5) , View=12 },
                new PollEntity(){Id=1, Question="filter,filter", DateTime=DateTime.Now.AddHours(1) , View=10 },
                new PollEntity(){Id=2, Question="HELLO", DateTime=DateTime.Now.AddSeconds(123) , View=0 },
                new PollEntity(){Id=3, Question="Hello", DateTime=DateTime.Now.AddSeconds(1) , View=111 },
                new PollEntity(){Id=4, Question="FILTER", DateTime=DateTime.Now.AddHours(2) , View=56 },
                new PollEntity(){Id=5, Question="filTER", DateTime=DateTime.Now , View=55 },
                new PollEntity(){Id=6, Question="What is your favourite pizza?", DateTime=DateTime.Now.AddSeconds(12) , View=13 },
                new PollEntity(){Id=7, Question="The best pizza", DateTime=DateTime.Now.AddHours(10) , View=14 },
                new PollEntity(){Id=8, Question="HI", DateTime=DateTime.Now.AddDays(12) , View=119 },
                new PollEntity(){Id=9, Question="The best game?", DateTime=DateTime.Now.AddMonths(1) , View=90 },
                new PollEntity(){Id=10, Question="Q123", DateTime=DateTime.Now.AddDays(1) , View=1 }
            };
        }
        //checkif the object properties are the same
        private void AreTableModelViewEqual(TableModelView<PollEntity> exceptedTable, TableModelView<PollEntity> resultTable) {
            var listResutPolls = resultTable.Elements.ToList();
            var listExpectedPolls = exceptedTable.Elements.ToList();

            Assert.AreEqual(exceptedTable.NumberOfFilteredElm, resultTable.NumberOfFilteredElm);
            Assert.AreEqual(listExpectedPolls.Count, listResutPolls.Count);
            for (int i = 0; i < listExpectedPolls.Count; i++) {
                Assert.AreEqual(listExpectedPolls[i].Id, listResutPolls[i].Id);
            }
        }
        [TestMethod]
        public void GetPollsFromFilterOption_Option_Element10_page1() {
            var options = new FilterOptionModelView() { elements = 10, page = 1 };
            var paginationHandler = new PaginationHandler<PollEntity>(x => x.DateTime, x => true);
            var resultTable = paginationHandler.GetEntityFromFilterOption(options, listPolls.AsQueryable());
            var expectedTable = new TableModelView<PollEntity>() { Elements = new List<PollEntity>() {listPolls[5], listPolls[3], listPolls[6],listPolls[2], listPolls[1], listPolls[4],
                listPolls[7], listPolls[10],listPolls[0], listPolls[8]}, NumberOfFilteredElm=11 };
            AreTableModelViewEqual(expectedTable, resultTable);
        }
        [TestMethod]
        public void GetPollsFromFilterOption_Option_Element5_page3() {
            var options = new FilterOptionModelView() { elements = 5, page = 3 };
            var paginationHandler = new PaginationHandler<PollEntity>(x => x.DateTime, x => true);
            var resultTable = paginationHandler.GetEntityFromFilterOption(options, listPolls.AsQueryable());
            var expectedTable = new TableModelView<PollEntity>() { Elements = new List<PollEntity>() {listPolls[9] }, NumberOfFilteredElm=11 };
            AreTableModelViewEqual(expectedTable, resultTable);
        }
        [TestMethod]
        public void GetPollsFromFilterOption_Option_OrderMode1_Element2_page1() {
            var options = new FilterOptionModelView() { elements = 2, page = 1, orderMode=true };

            var paginationHandler = new PaginationHandler<PollEntity>(x => x.DateTime, x => true);
            var resultTable = paginationHandler.GetEntityFromFilterOption(options, listPolls.AsQueryable());
            var expectedTable = new TableModelView<PollEntity>() { Elements = new List<PollEntity>() { listPolls[9], listPolls[8] }, NumberOfFilteredElm=11 };
            AreTableModelViewEqual(expectedTable, resultTable);
        }
        [TestMethod]
        public void GetPollsFromFilterOption_Option_nameSortQuestion_Element3_page4() {
            var options = new FilterOptionModelView() { elements = 3, page = 4, nameSort="Question"};
            var paginationHandler = new PaginationHandler<PollEntity>(x => x.Question, x => true);
            var resultTable = paginationHandler.GetEntityFromFilterOption(options, listPolls.AsQueryable());
            var expectedTable = new TableModelView<PollEntity>() { Elements = new List<PollEntity>() { listPolls[7],listPolls[6]}, NumberOfFilteredElm = 11 };
            AreTableModelViewEqual(expectedTable, resultTable);
        }
        [TestMethod]
        public void GetPollsFromFilterOption_Option_Phrasefil_nameSortView_OrderMode1_Element2_page1() {
            var options = new FilterOptionModelView() { elements = 2, page = 1, nameSort = "View", orderMode = true , phrase="fil" };
            var paginationHandler = new PaginationHandler<PollEntity>(x => x.View, x => x.Question.ToLower().Contains(options.phrase));
            var resultTable = paginationHandler.GetEntityFromFilterOption(options, listPolls.AsQueryable());
            var expectedTable = new TableModelView<PollEntity>() {
                Elements = new List<PollEntity>() { listPolls[4],listPolls[5]}, NumberOfFilteredElm = 3 };
            AreTableModelViewEqual(expectedTable, resultTable);
        }
        [TestMethod]
        public void GetPollsFromFilterOption_Option_Phrasepizza_nameSortQuestion_Element2_page1() {
            var options = new FilterOptionModelView() { elements = 2, page = 1, nameSort = "View",phrase = "pizza" };
            var paginationHandler = new PaginationHandler<PollEntity>(x => x.View, x => x.Question.ToLower().Contains(options.phrase));
            var resultTable = paginationHandler.GetEntityFromFilterOption(options, listPolls.AsQueryable());
            var expectedTable = new TableModelView<PollEntity>() {
                Elements = new List<PollEntity>() {listPolls[6] , listPolls[7] }, NumberOfFilteredElm = 2};
            AreTableModelViewEqual(expectedTable, resultTable);
        }
    }
}
