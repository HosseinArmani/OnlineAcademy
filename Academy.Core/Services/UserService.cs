using Academy.Core.Security;
using Academy.Core.Services.Interfaces;
using Academy.Core.ViewModels;
using AcademyDataLayer.Context;
using AcademyDataLayer.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Core.Services
{
    public class UserService : IUserService
    {
        AcademyContext _context;
        public UserService(AcademyContext context)
        {
            _context = context;
        }
        public bool IsExistUserName(string userName)
        {
            return _context.Users.Any(u => u.UserName == userName);
        }

        public bool IsExistEmail(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public int AddUser(User user)
        {
            _context.Add(user);
            _context.SaveChanges();
            return user.UserId;
        }
        public void UpdateUser(User user)
        {
            _context.Update(user);
            _context.SaveChanges();
        }
        public User LoginUser(LoginViewModel login)
        {
            string hash = HashPassword.EncodePasswordMd5(login.Password);
            string email = login.Email.Trim().ToLower();
            return _context.Users.SingleOrDefault(u => u.Email == email && u.Password == hash);
        }
        public User GetUserById(int userId)
        {
            return _context.Users.Find(userId);
        }
        public void DeleteUser(int userId)
        {
            User user = GetUserById(userId);
            user.IsDelete = true;
            UpdateUser(user);
        }

        public UserForAdminViewModel GetAllUser(int Pageid = 1, string filterUserName = "", string filterEmail = "")
        {
            IQueryable<User> List = _context.Users;

            if (!string.IsNullOrEmpty(filterUserName))
            {
                List = List.Where(u => u.UserName.Contains(filterUserName));
            }
            if (!string.IsNullOrEmpty(filterEmail))
            {
                List = List.Where(u => u.Email.Contains(filterEmail));
            }
            int take = 20;
            int skip = (Pageid - 1) * take;

            UserForAdminViewModel result = new UserForAdminViewModel()
            {
                CurrentPage = Pageid,
                CountPage = List.Count() / take,
                Users = List.OrderBy(u => u.UserName).Skip(skip).Take(take).ToList()
            };

            return result;
        }

        public int AddUserForAdmin(CreateUserViewModel user)
        {
            User Adduser = new User();
            Adduser.UserName = user.UserName;
            Adduser.Email = user.Email.Trim().ToLower();
            Adduser.Password = HashPassword.EncodePasswordMd5(user.Password);
            Adduser.IsActive = true;
            Adduser.ActiveCode = Guid.NewGuid().ToString().Replace("-", "");
            Adduser.RegisterDate = DateTime.Now;

            return AddUser(Adduser);

        }

        public EditUserViewModel GetEditUserForShow(int userId)
        {
            return _context.Users.Where(u => u.UserId == userId).Select(u => new EditUserViewModel()
            {
                UaerId=u.UserId,
                UserName = u.UserName,
                Email = u.Email,
                UserRoles = u.userRoles.Select(r=>r.RoleId).ToList()

            }).Single();
        }

        public void EditUserForAdmin(EditUserViewModel editUser)
        {

            User Updateuser = GetUserById(editUser.UaerId);
           Updateuser.UserName = editUser.UserName;
           Updateuser.Email = editUser.Email.Trim().ToLower();
            if (!string.IsNullOrEmpty(editUser.Password))
            {
                Updateuser.Password = HashPassword.EncodePasswordMd5(editUser.Password);
            }

            UpdateUser(Updateuser);
            
        }

        public InformationUserViewModel GetUserInformation(int userId)
        {
            var user = GetUserById(userId);
            InformationUserViewModel information = new InformationUserViewModel();
            information.UserName = user.UserName;
            information.Email = user.Email;
            information.RegisterDate = user.RegisterDate;

            return information;
        }
    }
}
