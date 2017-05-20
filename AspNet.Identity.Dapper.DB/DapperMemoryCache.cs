using System;
using System.Runtime.Caching;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.Identity.Dapper.DB
{
    public class DapperMemoryCache : SQLDB
    {
        ObjectCache cache = null;
        CacheItemPolicy policy = null;
        public DapperMemoryCache() : base()
        {
            cache = MemoryCache.Default;
            policy = new CacheItemPolicy();
        }
        public override void UserSave(User user)
        {
            cache.Remove("User" + user.UserID);
            cache.Remove("User_email" + user.Email);
            base.UserSave(user);
        }

        public override User User(string userID, string email)
        {
            var user = cache["User" + userID] as User;
            if (user == null)
            {
                user = cache["User_email" + email] as User;
                if (user == null)
                {
                    user = base.User(userID, email);
                    if (user != null)
                    {
                        cache.Add("User" + user.UserID, user, policy);
                        cache.Add("User_email" + user.Email, user, policy);
                    }
                }
            }

            return user;
        }
    }
}
