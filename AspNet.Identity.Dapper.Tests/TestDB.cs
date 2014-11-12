using AspNet.Identity.Dapper.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AspNet.Identity.Dapper.Tests
{
    public class TestDB : IDB
    {
        List<User> users;
        List<UserLogin> userLogins;
        List<UserRole> userRoles;
        List<Role> roles;
        public TestDB()
        {
            users = new List<User>();
            userLogins = new List<UserLogin>();
            roles = new List<Role>();
            userRoles = new List<UserRole>();

            users.Add(new User { UserName = "test@test.com", Email = "test@test.com", UserID = Guid.NewGuid().ToString() });
        }
        public Role[] Roles()
        {
            throw new NotImplementedException();
        }

        public Role Role(string roleID, string name)
        {
            throw new NotImplementedException();
        }

        public void RoleCreate(Role r)
        {
            roles.Add(r);
        }

        public void RoleSave(Role role)
        {
            RoleDelete(role);
            roles.Add(role);
        }

        public void RoleDelete(Role role)
        {
            roles.Remove(role);
        }

        public User[] Users()
        {
            return users.Where(u => !u.IsDeleted).ToArray();
        }

        public void UserSave(User user)
        {
            users.Remove(users.First(u => u.UserName == u.UserName));
            users.Add(user);
        }

        public void UserCreate(User user)
        {
            users.Add(user);
        }

        public User User(string userID, string email)
        {
            return users.FirstOrDefault(u => u.UserID == userID || u.Email == email);
        }

        public void UserRoleAdd(string userID, string roleName)
        {
            var role = roles.First(r => r.Name == roleName);
            userRoles.Add(new UserRole { UserID = userID, RoleID = role.RoleID });
        }

        public void UserRoleRemove(string userID, string roleName)
        {
            userRoles.Remove(userRoles.First(ur => ur.UserID == userID));
        }

        public Role[] UserRoles(string userID)
        {
            return (from r in roles
                    join ur in userRoles on r.RoleID equals ur.RoleID
                    select r).ToArray();
        }

        public ToDo[] ToDos()
        {
            throw new NotImplementedException();
        }

        public ToDo ToDo(int todoID)
        {
            throw new NotImplementedException();
        }

        public void ToDoCreate(ToDo t)
        {
            throw new NotImplementedException();
        }

        public void ToDoSave(ToDo t)
        {
            throw new NotImplementedException();
        }

        public void ToDoRemove(int todoID)
        {
            throw new NotImplementedException();
        }

        public User UserByLogin(string loginProvider, string providerKey)
        {
            var user = (from u in users
                        join ul in userLogins on u.UserID equals ul.UserID
                        select u).FirstOrDefault();
            return user;
        }

        public UserLogin[] UserLogins(string userID)
        {
            return userLogins.Where(u => u.UserID == userID).ToArray();
        }

        public void UserLoginCreate(UserLogin ul)
        {
            userLogins.Add(ul);
        }

        public void UserLoginsDelete(string UserID, string loginProvider, string providerKey)
        {
            var current = userLogins.First(ul => ul.UserID == UserID && ul.LoginProvider == loginProvider && ul.ProviderKey == providerKey);
            userLogins.Remove(current);
        }


        public void ClaimAdd(UserClaim c)
        {
            throw new NotImplementedException();
        }

        public UserClaim[] Claims(string userID)
        {
            throw new NotImplementedException();
        }

        public void ClaimRemove(UserClaim c)
        {
            throw new NotImplementedException();
        }
    }
}
