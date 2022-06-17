using Academy.Core.Services.Interfaces;
using AcademyDataLayer.Entities.Course;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineAcademy.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CourseController : Controller
    {
        ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        public IActionResult Index()
        {
            return View(_courseService.GetShowCourseForAdmin());
        }
        public IActionResult Create()
        {
            var group = _courseService.GetGroupForManageCourse();
            ViewData["Groups"] = new SelectList(group, "Value", "Text");

            var subGroup = _courseService.GetSubGroupForManageCourse(int.Parse(group.First().Value));
            ViewData["SubGroups"] = new SelectList(subGroup, "Value", "Text");

            var Teacher = _courseService.GetTechearsForManageCourse();
            ViewData["Teachers"] = new SelectList(Teacher, "Value", "Text");

            var Level = _courseService.GetLevelForManageCourse();
            ViewData["Levels"] = new SelectList(Level, "Value", "Text");

            var Status = _courseService.GetStatusForManageCourse();
            ViewData["Statues"] = new SelectList(Status, "Value", "Text");

            return View();
        }
        public IActionResult GetSubGroups(int id)
        {
            List<SelectListItem> List = new List<SelectListItem>()
            {
                new SelectListItem(){Text="انتخاب کنید",Value=""}
            };


            List.AddRange(_courseService.GetSubGroupForManageCourse(id));
            return Json(new SelectList(List, "Value", "Text"));
        }
        [HttpPost]
        public IActionResult Create([Bind("CourseId,GroupId,SubGroupId,TecherId,LevelId,StatusId,CourseTitle,Discription,CoursePrice,Tags,CourseImageName,DemoFileName,CreateDate,CreateUpdate")] Course course, IFormFile imgCourseUp, IFormFile demoUp)
        {
            if (!ModelState.IsValid)
            {
                return View(course);
            }

            _courseService.AddCourse(course, imgCourseUp, demoUp);

            return RedirectToAction("Index");

        }
        public IActionResult Edit(int id)
        {
            Course course = _courseService.GetCourseById(id);

            var group = _courseService.GetGroupForManageCourse();
            ViewData["Groups"] = new SelectList(group, "Value", "Text",course.GroupId);

            var subGroup = _courseService.GetSubGroupForManageCourse(int.Parse(group.First().Value));
            ViewData["SubGroups"] = new SelectList(subGroup, "Value", "Text",course.SubGroupId??0);

            var Teacher = _courseService.GetTechearsForManageCourse();
            ViewData["Teachers"] = new SelectList(Teacher, "Value", "Text",course.TecherId);

            var Level = _courseService.GetLevelForManageCourse();
            ViewData["Levels"] = new SelectList(Level, "Value", "Text",course.LevelId);

            var Status = _courseService.GetStatusForManageCourse();
            ViewData["Statues"] = new SelectList(Status, "Value", "Text",course.StatusId);

            return View(course);
        }
        [HttpPost]
        public IActionResult Edit(Course course, IFormFile imgCourseUp, IFormFile demoUp)
        {
            if (!ModelState.IsValid)
            {
                return View(course);
            }

            _courseService.UpdateCourse(course, imgCourseUp, demoUp);

            return RedirectToAction("Index");
        }
        public IActionResult IndexEpisode(int id)
        {
            ViewData["CourseId"] = id;
            
            return View(_courseService.GetListEpispdeCourse(id));
        }
        public IActionResult CreateEpisode(int id)
        {

            CourseEpisode courseEpisode = new CourseEpisode();
            courseEpisode.CourseId = id;
           
            return View(courseEpisode);
        }
        [HttpPost]
        public IActionResult CreateEpisode([Bind("EpisodeId,CourseId,EpisodeTitle,EpisodeTime,EpisodeFileName,IsFree")] CourseEpisode courseEpisode,IFormFile episodeFile)
        {
            if (!ModelState.IsValid || episodeFile==null)
            {
                return View(courseEpisode);
            }
            if (_courseService.CheckFile(episodeFile.FileName))
            {
                ViewData["IsExistFile"] = true;
                return View(courseEpisode);
            }
            _courseService.AddEpisode(courseEpisode, episodeFile);
            return Redirect("/Admin/Course/IndexEpisode/" + courseEpisode.CourseId);
        }
        public IActionResult EditEpisode(int id)
        {
            CourseEpisode courseEpisode = _courseService.GetEpisodeBuID(id);

            return View(courseEpisode);
        }
        [HttpPost]
        public IActionResult EditEpisode([Bind("EpisodeId,CourseId,EpisodeTitle,EpisodeTime,EpisodeFileName,IsFree")] CourseEpisode courseEpisode, IFormFile episodeFile)
        {
            if (!ModelState.IsValid)
            {
                return View(courseEpisode);
            }

            if (episodeFile != null)
            {
                if (_courseService.CheckFile(episodeFile.FileName))
                {
                    ViewData["IsExistFile"] = true;
                    return View(courseEpisode);
                }
            }

            _courseService.UpdateEpisode(courseEpisode, episodeFile);
            return Redirect("/Admin/Course/IndexEpisode/" + courseEpisode.CourseId);
        }

    }
}
