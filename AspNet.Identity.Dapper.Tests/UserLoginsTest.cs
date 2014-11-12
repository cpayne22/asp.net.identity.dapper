using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNet.Identity;
using AspNet.Identity.Dapper;

namespace AspNet.Identity.Dapper.Tests
{
    [TestClass]
    public class UserLoginsTest
    {
        [TestMethod]
        public void Add_Should_Add_New_Login_If_User_Exists()
        {
            var db = new TestDB();
            const string loginProvider = "Twitter";
            const string providerKey = "12345678";

            IUserLoginStore<ApplicationUser, string> userLoginStore = new UserStore<ApplicationUser>(db);
            var user = new ApplicationUser { UserName = "test@test.com" };
            db.UserCreate(ApplicationUser.Convert(user));


            userLoginStore = new UserStore<ApplicationUser>(db);
            var dbUser = db.User("", "test@test.com");
            user = ApplicationUser.Convert(dbUser);

            // Act
            UserLoginInfo loginToAdd = new UserLoginInfo(loginProvider, providerKey);
            userLoginStore.AddLoginAsync(user, loginToAdd);

            // Assert
            var foundLogins = db.UserLogins(user.UserID);
            Assert.AreEqual(1, foundLogins.Count());


        }
    }
}
