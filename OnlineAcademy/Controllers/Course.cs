using Academy.Core.Services;
using Academy.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAcademy.Controllers
{
    public class Course : Controller
    {
        ICourseService _courseService;
        IOrderServise _orderServise;
        public Course(ICourseService courseService,IOrderServise orderServise)
        {
            _courseService = courseService;
            _orderServise = orderServise;
        }

        public IActionResult Index(int pageaid = 1, string filte = "", string getTipe = "all", string orderByType = "date", List<int> selectedGroups = null)
        {
            ViewBag.pageId = pageaid;
            ViewBag.CourseGroups = _courseService.GetAllGroups();
            ViewBag.selectedGroups = selectedGroups;
            return View(_courseService.GetCourse(pageaid,filte,getTipe,orderByType,selectedGroups,12));
        }
        [Route("ShowCourse/{id}")]
        public  IActionResult ShowCourse(int id)
        {
            var course = _courseService.GetCourseForShow(id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }
        [Authorize]
        public IActionResult BuyCourse(int id)
        {
           int orderId=_orderServise.AddOrder(User.Identity.Name, id);

            return Redirect("/UserPanel/Invoice/ShowInvoice/" + orderId);
        }
    }
}
