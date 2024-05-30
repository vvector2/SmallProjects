using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebApplicatationPollService.Models;
using WebApplicatationPollService.Models.Entities;

namespace WebApplicationPollServiceTest.UnitTest {
    [TestClass]
    public class PrivatePollManagerTets {
        private PrivatePollManager manager;
        public PrivatePollManagerTets() {
            manager = new PrivatePollManager();
        }
        private  Mock<DbSet<T>> CreateDbSetMock<T>(IEnumerable<T> elements) where T : class {
            var elementsAsQueryable = elements.AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();

            dbSetMock.As<IQueryable<T>>().Setup(m => m.Provider).Returns(elementsAsQueryable.Provider);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.Expression).Returns(elementsAsQueryable.Expression);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(elementsAsQueryable.ElementType);
            dbSetMock.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(elementsAsQueryable.GetEnumerator());

            return dbSetMock;
        }

        [TestMethod]
        public void IsAuthorisedByCookie_SessionDoNoExistInDatabase() {
            var dbMock = new Mock<ApplicationDbContext>();
            var list = new List<SessionUserPrivatePollEntity>() { new SessionUserPrivatePollEntity() { SessionID = Guid.NewGuid(), DateTime = DateTime.Now } };
            dbMock.Setup(x => x.SessionPrivatePoll).Returns( CreateDbSetMock<SessionUserPrivatePollEntity>(list).Object );
            dbMock.Setup(x => x.SaveChanges());
            Assert.AreEqual(false, manager.IsAuthorisedByCookie("idDonotExists", dbMock.Object));
        }
        [TestMethod]
        public void IsAuthorisedByCookie_SessionIsOld() {
            var dbMock = new Mock<ApplicationDbContext>();
            var guid = Guid.NewGuid();
            var list = new List<SessionUserPrivatePollEntity>() { new SessionUserPrivatePollEntity() { SessionID = guid, DateTime = DateTime.Now.Subtract(new TimeSpan(0, 11, 0)) } };
            dbMock.Setup(x => x.SessionPrivatePoll).Returns(CreateDbSetMock<SessionUserPrivatePollEntity>(list).Object);
            dbMock.Setup(x => x.SaveChanges());
            Assert.AreEqual(false, manager.IsAuthorisedByCookie(guid.ToString(), dbMock.Object));
        }
        [TestMethod]
        public void IsAuthorisedByCookie_Success() {
            var dbMock = new Mock<ApplicationDbContext>();
            var guid = Guid.NewGuid();
            var list = new List<SessionUserPrivatePollEntity>() { new SessionUserPrivatePollEntity() { SessionID = guid, DateTime = DateTime.Now.Subtract(new TimeSpan(0, 9, 0)) } };
            dbMock.Setup(x => x.SessionPrivatePoll).Returns(CreateDbSetMock<SessionUserPrivatePollEntity>(list).Object);
            dbMock.Setup(x => x.SaveChanges());
            Assert.AreEqual(true, manager.IsAuthorisedByCookie(guid.ToString(), dbMock.Object));
        }
        [TestMethod]
        public void HashingPassword() {
            string passwd = "ABCDE";
            string hashed = manager.HashPassword(passwd);

            Assert.AreEqual(true, manager.VerifyPassword(hashed, passwd));
            Assert.AreEqual(false, manager.VerifyPassword(hashed, "ABCDEF"));
        }

    }
}
