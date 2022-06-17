using Academy.Core.Security;
using Academy.Core.Services.Interfaces;
using Academy.Core.ViewModels;
using AcademyDataLayer.Entities.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineAcademy.Controllers
{
    public class AccountController : Controller
    {
        IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        #region ثبت نام 
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }
        [Route("Register")]
        [HttpPost]
        public IActionResult Register(RegisterViewModel register)
        {
            if (!ModelState.IsValid)
            {
                return View(register);
            }

            if (_userService.IsExistUserName(register.UserName))
            {
                ModelState.AddModelError("UserName", "نام کاربری معتبر نمی باشد");
                return View(register);
            }

            if (_userService.IsExistEmail(register.Email.Trim().ToLower()))
            {
                ModelState.AddModelError("Email", "با این ایمیل قبلا ثبت نام ;رده اید");
                return View(register);
            }
            User user = new User
            {
                UserName = register.UserName,
                Email = register.Email.Trim().ToLower(),
                Password = HashPassword.EncodePasswordMd5(register.Password),
                ActiveCode = Guid.NewGuid().ToString().Replace("-", ""),
                IsActive = false,
                RegisterDate = DateTime.Now,

            };
            _userService.AddUser(user);
            return View("SuccsesRegister", user);
        }
        #endregion

        #region ورود

        [Route("Login")]
        public IActionResult Login()
        {

            return View();
        }


        [HttpPost]
        [Route("Login")]
        public ActionResult Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            var user = _userService.LoginUser(login);
            if (user != null)
            {
                if (user.IsActive)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                        new Claim(ClaimTypes.Name,user.UserName)
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = login.Remmeber
                    };
                    HttpContext.SignInAsync(principal, properties);

                    return Redirect("/");
                }
                else
                {
                    ModelState.AddModelError("Email", "حساب کاربری شما فعال نمی باشد");
                }
            }
            ModelState.AddModelError("Email", "کاربری با مشخصات وارد شده یافت نشد");
            return View(login);
        }


        [Route("Logout")]
        public IActionResult Logout()
        {
            
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/Login");
        }

        #endregion

    }
}
