using Academy.Core.ViewModels;
using AcademyDataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Core.Services.Interfaces
{
  public  interface IUserService
    {
        #region User
        bool IsExistUserName(string userName);
        bool IsExistEmail(string email);
        int AddUser(User user);
        void UpdateUser(User user);
        User LoginUser(LoginViewModel login);
        User GetUserById(int userId);
        public void DeleteUser(int userId);
        InformationUserViewModel GetUserInformation(int userId);
        #endregion

        #region User Admin
        UserForAdminViewModel GetAllUser(int Pageid = 1, string filterUserName="",string filterEmail="");
        int AddUserForAdmin(CreateUserViewModel user);
        EditUserViewModel GetEditUserForShow(int userId);
       void EditUserForAdmin(EditUserViewModel editUser);

        
        #endregion

    }
}
