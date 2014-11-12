using AspNet.Identity.Dapper.DB;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.Identity.Dapper
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        IDB db;
        public ApplicationSignInManager(IDB db, ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
            this.db = db;
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            var db = new SQLDB();
            return new ApplicationSignInManager(db, context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }

    }
}
