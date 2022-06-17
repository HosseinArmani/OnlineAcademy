using Academy.Core.Services.Interfaces;
using Academy.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAcademy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        IUserService _userService;
        IPermissionService _permissionService;
        public UserController(IUserService userService, IPermissionService permissionService)
        {
            _userService = userService;
            _permissionService = permissionService;
        }

        public IActionResult Index(int Pageid = 1, string filterUserName = "", string filterEmail = "")
        {
            return View(_userService.GetAllUser(Pageid,filterUserName,filterEmail));
        }
        public IActionResult Create()
        {
            ViewData["Roles"] = _permissionService.GetRoles();
            return View();
        }
        [HttpPost]
        public IActionResult Create(CreateUserViewModel user, List<int> SelectedRoles)
        {
            if(!ModelState.IsValid)
            {
                return View(user);
            }
            int UserId = _userService.AddUserForAdmin(user);

            _permissionService.AddRoleToUser(SelectedRoles, UserId);

            return Redirect("Index");
        }
        public IActionResult Edit(int id)
        {
            EditUserViewModel editUserViewModel = _userService.GetEditUserForShow(id);
            

            return View(editUserViewModel);
        }

        [HttpPost]
        public IActionResult Edit([Bind("UseBrId,UserName,Email,Password,ActiveCode,IsActive,RegisterDate")] EditUserViewModel editUser, List<int> SelectedRoles)
        {
            if (!ModelState.IsValid)
            {
                return View(editUser);
            }

            _userService.EditUserForAdmin(editUser);

            _permissionService.EditRoleToUser(editUser.UaerId, SelectedRoles);

            return RedirectToAction("Index");

        }
        public IActionResult Delete(int id )
        {
            InformationUserViewModel informationUserViewModel;
            ViewData["UserId"] = id;
            informationUserViewModel = _userService.GetUserInformation(id);

            return View(informationUserViewModel);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteUser(int UserId)
        {
            _userService.DeleteUser(UserId);
            return RedirectToAction("Index");
        }

    }
}
