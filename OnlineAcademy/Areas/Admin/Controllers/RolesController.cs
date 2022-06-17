using Academy.Core.Services.Interfaces;
using AcademyDataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAcademy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        IPermissionService _permissionService;
        public RolesController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }
        public IActionResult Index()
        {
            return View(_permissionService.GetRoles());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Role role)
        {
            if (!ModelState.IsValid)
            {
                return View(role);
            }

            int RoleId= _permissionService.AddRole(role);

            return RedirectToAction("Index");
        }
    }
}
