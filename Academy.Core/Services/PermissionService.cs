using Academy.Core.Services.Interfaces;
using AcademyDataLayer.Context;
using AcademyDataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Core.Services
{
    public class PermissionService : IPermissionService
    {
        AcademyContext _context;
        public PermissionService(AcademyContext context)
        {
            _context = context;
        }


        public List<Role> GetRoles()
        {
            return _context.Roles.ToList();
        }
        public void AddRoleToUser(List<int> roleIds, int userId)
        {
            foreach (var roleId in roleIds)
            {
                _context.Add(new UserRole
                {
                    RoleId = roleId,
                     UserId=userId
                });
            }
            _context.SaveChanges();
        }

        public void EditRoleToUser( int userId,List<int> roleIds)
        {

            _context.UserRoles.Where(r => r.UserId == userId).ToList().ForEach(r => _context.UserRoles.Remove(r));
            AddRoleToUser(roleIds, userId);
        }

        public int AddRole(Role role)
        {
            _context.Roles.Add(role);
            _context.SaveChanges();
            return role.RoleId;
        }
    }
}
