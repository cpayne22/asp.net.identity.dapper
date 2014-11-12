using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AspNet.Identity.Dapper;
using Microsoft.AspNet.Identity;

namespace AspNet.Identity.Dapper.Tests
{
    [TestClass]
    public class EmailStoreTest
    {
        [TestMethod]
        public void FindByEmailAsync_Should_Return_The_Correct_User_If_Available()
        {
            const string userName = "TestUserName";
            string userId = Guid.NewGuid().ToString();
            const string email = "test@test.com";

            var db = new TestDB();
            var user = new ApplicationUser { UserID = userId, UserName = userName };
            db.UserCreate(ApplicationUser.Convert(user));

            IUserEmailStore<ApplicationUser> userEmailStore = new UserStore<ApplicationUser>(db);
            var currentUser = userEmailStore.FindByEmailAsync(email);

            Assert.IsNotNull(currentUser);
            Assert.AreEqual(userId, user.Id);
            Assert.AreEqual(userName, user.UserName);
        }
    }
}
