using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApplicatationPollService.Models;

namespace WebApplicationPollServiceTest.UnitTest {
    [TestClass]
    public class ListAnswersValidationAtrTest {
        private ListAnswersValidationAtr validator;
        public ListAnswersValidationAtrTest() {
            validator = new ListAnswersValidationAtr();
        }
        [TestMethod]
        public void IsValid_objIsString() {
            Assert.AreEqual(false, validator.IsValid("A1"));
        }
        [TestMethod]
        public void IsValid_1Answer() {
            var list = new List<string>() { "A" };
            Assert.AreEqual(false, validator.IsValid(list));
        }
        [TestMethod]
        public void IsValid_TwoValidAnswer() {
            var list = new List<string>() { "A" , "B"};
            Assert.AreEqual(true, validator.IsValid(list));
        }
        [TestMethod]
        public void IsValid_TooLargeAnswer() {
            string str = "";
            for (int i = 0; i < 51; i++) str += "A";
            var list = new List<string>() { "A", "B",str };
            Assert.AreEqual(false, validator.IsValid(list));
        }
        [TestMethod]
        public void IsValid_OneValidOneNullEmpty() {
            var list = new List<string>() { "A", "", null};
            Assert.AreEqual(false, validator.IsValid(list));
        }
        [TestMethod]
        public void IsValid_TooManyAnswer() {
            var list = new List<string>() { "A", "A","A","A", "A", "A", "A", "A" };
            Assert.AreEqual(false, validator.IsValid(list));
        }
        [TestMethod]
        public void IsValid_8AnswerOneInEmpty() {
            var list = new List<string>() { "A", "A", "A", "A", "A", "A", "A", "" };
            Assert.AreEqual(true, validator.IsValid(list));
        }
    }
}
