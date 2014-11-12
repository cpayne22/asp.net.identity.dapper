using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DapperExtensions;

namespace AspNet.Identity.Dapper.DB
{
    public interface IDB
    {
        Role[] Roles();
        Role Role(string roleID, string name);
        void RoleCreate(Role r);
        void RoleSave(Role role);
        void RoleDelete(Role role);

        User[] Users();

        void UserSave(User user);
        void UserCreate(User user);

        User User(string userID, string email);

        void UserRoleAdd(string userID, string roleName);
        void UserRoleRemove(string userID, string roleName);
        Role[] UserRoles(string userID);
        ToDo[] ToDos();
        ToDo ToDo(int todoID);
        void ToDoCreate(ToDo t);
        void ToDoSave(ToDo t);
        void ToDoRemove(int todoID);

        User UserByLogin(string loginProvider, string providerKey);

        UserLogin[] UserLogins(string userID);
        void UserLoginCreate(UserLogin ul);

        void UserLoginsDelete(string UserID, string loginProvider, string providerKey);

        void ClaimAdd(UserClaim c);
        UserClaim[] Claims(string userID);
        void ClaimRemove(UserClaim c);

    }

    public class SQLDB : IDB
    {
        private IDbConnection cn;
        private string cnStr;
        public SQLDB()
        {
            cnStr = System.Configuration.ConfigurationManager.ConnectionStrings["db"].ConnectionString;
        }
        private DbConnection getCN()
        {
            var cn = new SqlConnection(cnStr);
            cn.Open();

            return new StackExchange.Profiling.Data.ProfiledDbConnection(cn, MiniProfiler.Current);
        }


        public Role[] Roles()
        {
            using (var cn = getCN())
            {
                return cn.Query<Role>("select * from Role").ToArray();
            }
        }
        public Role Role(string roleID, string name)
        {
            using (var cn = getCN())
            {
                if (!string.IsNullOrEmpty(roleID))
                {
                    var r = cn.Query<Role>("select * from Role where RoleID = @RoleID", new { RoleID = roleID }).FirstOrDefault();
                    return r;
                }
                return cn.Query<Role>("select * from Role where Name = @name", new { name = name }).FirstOrDefault();
            }
        }
        public void RoleCreate(Role r)
        {
            using (var cn = getCN())
            {
                cn.Insert(r);
            }
        }
        public void RoleSave(Role r)
        {
            using (var cn = getCN())
            {
                cn.Update(r);
            }
        }
        public void RoleDelete(Role r)
        {
            using (var cn = getCN())
            {
                cn.Delete(r);
            }
        }


        public User[] Users()
        {
            using (var cn = getCN())
            {
                return cn.Query<User>("select * from [User] where IsDeleted = 0").ToArray();
            }
        }

        public void UserSave(User user)
        {
            using (var cn = getCN())
            {
                cn.Update(user);
            }
        }

        public void UserCreate(User user)
        {
            using (var cn = getCN())
            {
                cn.Insert(user);
            }
        }

        public User User(string userID, string email)
        {
            using (var cn = getCN())
            {
                return cn.Query<User>("select * from [User] where UserID = @UserID or Username = @Email", new { UserID = userID, Email = email }).FirstOrDefault();
            }
        }


        public void UserRoleAdd(string userID, string roleName)
        {
            using (var cn = getCN())
            {
                var role = Roles().Single(r => r.Name == roleName);
                var userRole = new UserRole { RoleID = role.RoleID, UserID = userID };
                cn.Insert<UserRole>(userRole);
            }
        }

        public void UserRoleRemove(string userID, string roleName)
        {
            using (var cn = getCN())
            {
                var role = Roles().Single(r => r.Name == roleName);
                var userRole = new UserRole { RoleID = role.RoleID, UserID = userID };
                cn.Delete<UserRole>(userRole);
            }
        }

        public Role[] UserRoles(string userID)
        {
            using (var cn = getCN())
            {
                var roles = cn.Query<Role>("select Role.* from dbo.Role INNER JOIN  dbo.UserRole ON dbo.Role.RoleID = dbo.UserRole.RoleID where UserID = @userID", new { userID = userID }).ToArray();
                return roles;
            }
        }


        public ToDo[] ToDos()
        {
            using (var cn = getCN())
            {
                return cn.Query<ToDo>("select * from ToDo").ToArray();
            }
        }

        public ToDo ToDo(int todoID)
        {
            using (var cn = getCN())
            {
                return cn.Query<ToDo>("select * from ToDo where ToDoID = @ToDoID", new { ToDoID = todoID }).FirstOrDefault();
            }
        }

        public void ToDoCreate(ToDo t)
        {
            using (var cn = getCN())
            {
                cn.Insert(t);
            }
        }

        public void ToDoSave(ToDo t)
        {
            using (var cn = getCN())
            {
                cn.Update(t);
            }
        }

        public void ToDoRemove(int todoID)
        {
            using (var cn = getCN())
            {
                cn.Execute("delete form ToDo where ToDoID = @todoID", new { todoID = todoID });
            }
        }


        public User UserByLogin(string loginProvider, string providerKey)
        {
            using (var cn = getCN())
            {
                return cn.Query<User>("SELECT dbo.[User].*, dbo.UserLogin.LoginProvider, dbo.UserLogin.ProviderKey FROM dbo.[User] INNER JOIN dbo.UserLogin ON dbo.[User].UserID = dbo.UserLogin.UserID WHERE (dbo.UserLogin.LoginProvider = @loginProvider) AND (dbo.UserLogin.ProviderKey = @providerKey) ", new { loginProvider = loginProvider, providerKey = providerKey }).FirstOrDefault();
            }
        }

        public UserLogin[] UserLogins(string userID)
        {
            using (var cn = getCN())
            {
                return cn.Query<UserLogin>("select * from UserLogin where UserID = @UserID ", new { UserID = userID }).ToArray();
            }
        }

        public void UserLoginsDelete(string UserID, string loginProvider, string providerKey)
        {
            using (var cn = getCN())
            {
                cn.Execute("delete form UserLogin where UserID = @UserID and loginProvider= @loginProvider and providerKey = @providerKey", new { UserID = UserID, loginProvider = loginProvider, providerKey = providerKey });
            }
        }

        public void UserLoginCreate(UserLogin ul)
        {
            using (var cn = getCN())
            {
                cn.Insert(ul);
            }
        }


        public void ClaimAdd(UserClaim c)
        {
            using (var cn = getCN())
            {
                cn.Insert(c);
            }
        }

        public UserClaim[] Claims(string userID)
        {
            using (var cn = getCN())
            {
                return cn.Query<UserClaim>("select * from UserClaim where UserID = @userID", new { userID = userID }).ToArray();
            }
        }

        public void ClaimRemove(UserClaim c)
        {
            using (var cn = getCN())
            {
                cn.Delete(c);
            }
        }
    }
}
