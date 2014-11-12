using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AspNet.Identity.Dapper.DB;
using System.Security.Claims;

namespace AspNet.Identity.Dapper
{
    public class UserStore<TUser> : IUserStore<TUser>, IUserStore<TUser, string>, IQueryableUserStore<TUser>, IUserPasswordStore<TUser>, IUserRoleStore<TUser>, IUserLockoutStore<TUser, string>,
      IUserTwoFactorStore<TUser, string>, IUserEmailStore<TUser>, IUserLoginStore<TUser>, IUserClaimStore<TUser>, IDisposable where TUser : ApplicationUser
    {
        IDB db;
        public UserStore(IDB db)
        {
            this.db = db;


        }
        public Task CreateAsync(TUser user)
        {
            db.UserCreate(ApplicationUser.Convert(user));
            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(TUser user)
        {
            var dbUser = ApplicationUser.Convert(user as ApplicationUser);
            dbUser.IsDeleted = true;
            dbUser.DeletedDateUtc = DateTime.UtcNow;
            db.UserSave(dbUser);
            return Task.FromResult<object>(null);
        }

        public Task<TUser> FindByIdAsync(string userId)
        {
            var user = db.User(userId, "");
            return Task.FromResult<TUser>(ApplicationUser.Convert(user) as TUser);
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            var user = db.User("", userName);
            return Task.FromResult<TUser>(ApplicationUser.Convert(user) as TUser);
        }

        public Task UpdateAsync(TUser user)
        {
            db.UserSave(ApplicationUser.Convert(user as ApplicationUser));
            return Task.FromResult<object>(null);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IQueryable<TUser> Users
        {
            get { return db.Users().Select(u => ApplicationUser.Convert(u) as TUser).AsQueryable(); }
        }

        public Task<string> GetPasswordHashAsync(TUser user)
        {
            var u = user as ApplicationUser;
            var current = db.Users().Where(us => us.PasswordHash == u.PasswordHash).FirstOrDefault();
            return Task.FromResult<string>(current.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user)
        {
            var u = user as ApplicationUser;
            var current = db.Users().Where(us => us.PasswordHash == u.PasswordHash).FirstOrDefault();
            return Task.FromResult<bool>(!string.IsNullOrEmpty(current.PasswordHash));
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult<Object>(null);
        }

        public Task AddToRoleAsync(TUser user, string roleName)
        {
            db.UserRoleAdd(ApplicationUser.Convert(user).UserID, roleName);
            return Task.FromResult<object>(null);
        }

        public Task<IList<string>> GetRolesAsync(TUser user)
        {
            List<string> roles = db.UserRoles(ApplicationUser.Convert(user).UserID).Select(r => r.Name).ToList();
            return Task.FromResult<IList<string>>(roles);
        }

        public Task<bool> IsInRoleAsync(TUser user, string roleName)
        {
            var roles = db.UserRoles(ApplicationUser.Convert(user).UserID);
            return Task.FromResult<bool>(roles.Count(r => r.Name == roleName) > 0);
        }

        public Task RemoveFromRoleAsync(TUser user, string roleName)
        {
            db.UserRoleRemove(ApplicationUser.Convert(user).UserID, roleName);
            return Task.FromResult<object>(null);
        }

        public Task<int> GetAccessFailedCountAsync(TUser user)
        {
            var current = db.User("", user.UserName);
            return Task.FromResult<int>(current.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(TUser user)
        {
            var current = db.User("", user.UserName);
            return Task.FromResult<bool>(current.LockoutEnabled);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(TUser user)
        {
            return
             Task.FromResult(user.LockoutEndDateUtc.HasValue
                 ? new DateTimeOffset(DateTime.SpecifyKind(user.LockoutEndDateUtc.Value, DateTimeKind.Utc))
                 : new DateTimeOffset());
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(TUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            db.UserSave(ApplicationUser.Convert(user));
            return Task.FromResult(0);

        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset lockoutEnd)
        {
            var current = db.User("", user.UserName);
            current.LockoutEndDateUtc = lockoutEnd.UtcDateTime;
            db.UserSave(current);
            return Task.FromResult<object>(null);
        }

        public Task<bool> GetTwoFactorEnabledAsync(TUser user)
        {
            var current = db.User("", user.UserName);
            return Task.FromResult<bool>(current.TwoFactorEnabled);

        }

        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled)
        {
            var current = db.User("", user.UserName);
            current.TwoFactorEnabled = enabled;
            db.UserSave(current);
            return Task.FromResult<object>(null);
        }
        public Task<string> GetEmailAsync(TUser user)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user)
        {
            var current = db.User(user.UserID, "");
            if (current != null)
            {
                return Task.FromResult<bool>(false);
            }
            return Task.FromResult<bool>(current.EmailConfirmed);
        }

        public Task SetEmailAsync(TUser user, string email)
        {
            var current = db.User(user.UserID, "");
            current.Email = email;
            return Task.FromResult<object>(null);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed)
        {
            var current = db.User(user.UserID, "");
            current.EmailConfirmed = confirmed;
            return Task.FromResult<object>(null);
        }

        public Task<TUser> FindByEmailAsync(string email)
        {
            var user = db.Users().SingleOrDefault(u => u.Email == email);

            return Task.FromResult<TUser>(ApplicationUser.Convert(user) as TUser);
        }


        /// <summary>
        /// Inserts a Login in the UserLoginsTable for a given User
        /// </summary>
        /// <param name="user">User to have login added</param>
        /// <param name="login">Login to be added</param>
        /// <returns></returns>
        public Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            db.UserLoginCreate(new UserLogin { UserID = user.UserID, LoginProvider = login.LoginProvider, ProviderKey = login.ProviderKey });

            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Returns an TUser based on the Login info
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public Task<TUser> FindAsync(UserLoginInfo login)
        {
            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            var user = db.UserByLogin(login.LoginProvider, login.ProviderKey);
            if (user != null)
            {
                var tUser = ApplicationUser.Convert(user) as TUser;
                if (user != null)
                {
                    return Task.FromResult<TUser>(tUser);
                }
            }

            return Task.FromResult<TUser>(null);
        }

        /// <summary>
        /// Returns list of UserLoginInfo for a given TUser
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            List<UserLoginInfo> userLogins = new List<UserLoginInfo>();
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var logins = db.UserLogins(user.Id).Select(ul => new UserLoginInfo(ul.LoginProvider, ul.ProviderKey)).ToList();
            if (logins != null)
            {
                return Task.FromResult<IList<UserLoginInfo>>(logins);
            }

            return Task.FromResult<IList<UserLoginInfo>>(null);
        }

        /// <summary>
        /// Deletes a login from UserLoginsTable for a given TUser
        /// </summary>
        /// <param name="user">User to have login removed</param>
        /// <param name="login">Login to be removed</param>
        /// <returns></returns>
        public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            if (login == null)
            {
                throw new ArgumentNullException("login");
            }

            db.UserLoginsDelete(user.UserID, login.LoginProvider, login.ProviderKey);

            return Task.FromResult<Object>(null);
        }

        public Task AddClaimAsync(TUser user, Claim claim)
        {
            db.ClaimAdd(new UserClaim{ UserID=user.UserID, ClaimType=claim.Type, ClaimValue=claim.Value});
            return Task.FromResult<Object>(null);
        }

        public Task<IList<Claim>> GetClaimsAsync(TUser user)
        {
            var claims = db.Claims(user.UserID).Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToList();
            return Task.FromResult<IList<Claim>>(claims);
        }

        public Task RemoveClaimAsync(TUser user, Claim claim)
        {
            db.ClaimAdd(new UserClaim{ UserID=user.UserID, ClaimType=claim.Type, ClaimValue=claim.Value});
            return Task.FromResult<Object>(null);
        }
    }
}
