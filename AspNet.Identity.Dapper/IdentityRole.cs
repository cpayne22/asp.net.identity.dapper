using AspNet.Identity.Dapper.DB;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.Identity.Dapper
{
    public class IdentityRole : IRole
    {
        public IdentityRole() { }
        public IdentityRole(string roleName)
        {
            Name = roleName;
            Id = Guid.NewGuid().ToString();
        }
        public string Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public static IdentityRole Convert(Role r)
        {
            var ir = new IdentityRole(r.Name);
            ir.Id = r.RoleID;
            return ir;
        }

        public static Role Convert(IdentityRole r)
        {
            var role = new Role { RoleID = r.Id, Name = r.Name };
            return role;
        }
    }

  
}
