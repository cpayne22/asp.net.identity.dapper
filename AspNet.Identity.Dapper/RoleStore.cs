using AspNet.Identity.Dapper.DB;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.Identity.Dapper
{
    public class RoleStore<TRole> : IRoleStore<TRole>, IRoleStore<TRole, string>, IQueryableRoleStore<TRole>, IDisposable where TRole : IdentityRole
    {
        IDB db;
        public RoleStore(IDB db)
        {
            this.db = db;
        }
        public Task CreateAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            db.RoleCreate(IdentityRole.Convert(role));

            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(TRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            db.RoleDelete(IdentityRole.Convert(role));

            return Task.FromResult<object>(null);
        }

        public Task<TRole> FindByIdAsync(string roleId)
        {
            var role = db.Role(roleId, "");
            if (role != null)
            {
                return Task.FromResult<TRole>(IdentityRole.Convert(role) as TRole);
            }
            return Task.FromResult<TRole>(null);
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            TRole result = db.Role("", roleName) as TRole;
            return Task.FromResult<TRole>(result);
        }

        public Task UpdateAsync(TRole role)
        {
            db.RoleSave(IdentityRole.Convert(role));
            return Task.FromResult<TRole>(null);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IQueryable<TRole> Roles
        {
            get
            {
                var roles = db.Roles();
                return roles.Select(r => (TRole)IdentityRole.Convert(r)).AsQueryable();
            }
        }
    }
}
