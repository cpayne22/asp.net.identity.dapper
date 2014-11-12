using AspNet.Identity.Dapper.DB;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.Identity.Dapper
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IUser
    {
        public ApplicationUser()
        {
            UserID = Guid.NewGuid().ToString();
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string Id { get { return UserID; } set { UserID = value; } }

        public string UserID { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }

        public IEnumerable<IdentityRole> Roles { get; set; }
        public static User Convert(ApplicationUser user)
        {
            var u = new User { AccessFailedCount = user.AccessFailedCount, Email = user.UserName, EmailConfirmed = user.EmailConfirmed, LockoutEnabled = user.LockoutEnabled, LockoutEndDateUtc = user.LockoutEndDateUtc, PasswordHash = user.PasswordHash, PhoneNumber = user.PhoneNumber, PhoneNumberConfirmed = user.PhoneNumberConfirmed, SecurityStamp = user.SecurityStamp, TwoFactorEnabled = user.TwoFactorEnabled, UserID = user.UserID, UserName = user.UserName };
            return u;
        }
        public static ApplicationUser Convert(User user)
        {
            if (user == null)
            {
                return null;
            }

            var u = new ApplicationUser { AccessFailedCount = user.AccessFailedCount, Email = user.Email, EmailConfirmed = user.EmailConfirmed, LockoutEnabled = user.LockoutEnabled, LockoutEndDateUtc = user.LockoutEndDateUtc, PasswordHash = user.PasswordHash, PhoneNumber = user.PhoneNumber, PhoneNumberConfirmed = user.PhoneNumberConfirmed, SecurityStamp = user.SecurityStamp, TwoFactorEnabled = user.TwoFactorEnabled, UserID = user.UserID, UserName = user.UserName };
            return u;
        }
    }

}
