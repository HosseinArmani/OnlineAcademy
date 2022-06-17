using AcademyDataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Core.Services.Interfaces
{
  public  interface IPermissionService
    {
        List<Role> GetRoles();
        int AddRole(Role role);
        void AddRoleToUser(List<int> roleIds, int userId);
        void EditRoleToUser(int userId, List<int> roleIds);
    }
}
