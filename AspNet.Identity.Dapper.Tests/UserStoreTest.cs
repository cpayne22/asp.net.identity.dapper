using System;
using System.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNet.Identity;
using AspNet.Identity.Dapper;

namespace AspNet.Identity.Dapper.Tests
{
    [TestClass]
    public class UserStoreTest
    {
        [TestMethod]
        public void Should_Create_User()
        {
            string username = "test@test.com";

            var db = new TestDB();
            IUserStore<ApplicationUser> userStore = new UserStore<ApplicationUser>(db);
            userStore.CreateAsync(new ApplicationUser { UserName = username });

            var dbUser = db.User("", username);
            IUser user = ApplicationUser.Convert(dbUser);

            Assert.IsNotNull(user);
        }

        [TestMethod]
        public void Should_Be_Null_with_noUser()
        {
            var login = new UserLoginInfo("ProviderTest", "ProviderKey");
            var db = new TestDB();
            var store = new UserStore<ApplicationUser>(db);
            var user = store.FindAsync(login).Result;

            Assert.IsNull(user);
        }

        [TestMethod]
        public void Should_Login_With_LoginInfo()
        {
            var user = new ApplicationUser();
            var login = new UserLoginInfo("ProviderTest02", "ProviderKey02");
            user.UserName = "test@test.com";
            var db = new TestDB();
            var store = new UserStore<ApplicationUser>(db);
            var result = store.AddLoginAsync(user, login);

            var actual = db.User("", user.UserName);
            var userStored = store.FindAsync(login).Result;

            Assert.IsNull(result.Exception);
            Assert.IsNotNull(actual);
            Assert.AreEqual(user.UserName, actual.UserName);
          
        }

        [TestMethod]
        public void WhenRemoveLoginAsync()
        {
            var user = new ApplicationUser();
            user.UserName = "test@test.com";
            var login = new UserLoginInfo("ProviderTest03", "ProviderKey03");
            var db = new TestDB();
            var store = new UserStore<ApplicationUser>(db);
            store.AddLoginAsync(user, login);
            
            //Assert.IsTrue(user.Logins.Any());

            var result = store.RemoveLoginAsync(user, login);

            var actual = db.User("", user.UserName);
            Assert.IsNull(result.Exception);
          //  Assert.IsFalse(actual.Logins.Any());
        }

        [TestMethod]
        public void WhenCeateUserAsync()
        {
            var db = new TestDB();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var user = new ApplicationUser() { UserName = "RealUserName" };

            using (var transaction = new TransactionScope())
            {
                var result = userManager.CreateAsync(user, "RealPassword");
                transaction.Complete();
                Assert.IsNull(result.Exception);
            }

            var actual = db.User("", user.UserName);

            Assert.IsNotNull(actual);
            Assert.AreEqual(user.UserName, actual.UserName);
        }

        [TestMethod]
        public void GivenHaveRoles_WhenDeleteUser_ThenDeletingCausesNoCascade()
        {
            var user = new ApplicationUser();
            user.UserName="test@test.com";

            var role = new IdentityRole("ADM");

            var db = new TestDB();
            var store = new UserStore<ApplicationUser>(db);
            var roleStore = new RoleStore<IdentityRole>(db);

            roleStore.CreateAsync(role);
            store.CreateAsync(user);
            store.AddToRoleAsync(user, "ADM");

            //Assert.IsTrue(_session.Query<IdentityRole>().Any(x => x.Name == "ADM"));
            //Assert.IsTrue(_session.Query<ApplicationUser>().Any(x => x.UserName == "Lukz 04"));

            var result = store.DeleteAsync(user);

            Assert.IsNull(result.Exception);
           // Assert.IsFalse(_session.Query<ApplicationUser>().Any(x => x.UserName == "Lukz 04"));
           // Assert.IsTrue(_session.Query<IdentityRole>().Any(x => x.Name == "ADM"));
        }
    }
}
