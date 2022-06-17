using Academy.Core.Convertors;
using Academy.Core.Services.Interfaces;
using Academy.Core.ViewModels;
using AcademyDataLayer.Context;
using AcademyDataLayer.Entities.Course;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Core.Services
{
    public class CourseService : ICourseService
    {
        AcademyContext _context;
        public CourseService(AcademyContext context)
        {
            _context = context;
        }
        public List<ShowCourseForAdminViewModel> GetShowCourseForAdmin()
        {
            return _context.Courses.Select(c => new ShowCourseForAdminViewModel()
            {
                CourseId = c.CourseId,
                CourseTitle = c.CourseTitle,
                CourseImageName = c.CourseImageName,
                EpisodeCount = c.CourseEpisodes.Count

            }).ToList();

        }
        public int AddCourse(Course course, IFormFile imgCourse, IFormFile demoUp)
        {
            course.CreateDate = DateTime.Now;
            course.CourseImageName = "default.jpg";
            if (imgCourse != null)
            {
                course.CourseImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(imgCourse.FileName);
                string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageCourse/Image", course.CourseImageName);

                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    imgCourse.CopyTo(stream);
                }
                string ImagePathThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageCourse/Thumb", course.CourseImageName);

                ImageConvertor imagResize = new ImageConvertor();

                imagResize.Image_resize(ImagePath, ImagePathThumb, 150);
            }

            if (demoUp != null)
            {
                course.DemoFileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(demoUp.FileName);
                string DemoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/DemoCourse", course.DemoFileName);

                using (var stream = new FileStream(DemoPath, FileMode.Create))
                {
                    demoUp.CopyTo(stream);
                }
            }
            _context.Courses.Add(course);
            _context.SaveChanges();
            return course.CourseId;
        }

        public List<CourseGroup> GetAllGroups()
        {
            return _context.CourseGroups.ToList();
        }

        public List<SelectListItem> GetGroupForManageCourse()
        {
            return _context.CourseGroups.Where(g => g.ParentID == null).Select(g => new SelectListItem()
            {
                Text = g.GroupTitle,
                Value = g.GroupId.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetLevelForManageCourse()
        {
            return _context.CourseLevels.Select(l => new SelectListItem()
            {
                Text = l.LevelTitle,
                Value = l.LevelId.ToString()

            }).ToList();
        }



        public List<SelectListItem> GetStatusForManageCourse()
        {
            return _context.CourseStatuses.Select(s => new SelectListItem()
            {
                Text = s.StatusTitle,
                Value = s.StatusId.ToString()

            }).ToList();
        }

        public List<SelectListItem> GetSubGroupForManageCourse(int groupid)
        {
            return _context.CourseGroups.Where(g => g.ParentID == groupid).Select(g => new SelectListItem()
            {
                Text = g.GroupTitle,
                Value = g.GroupId.ToString()
            }).ToList();
        }

        public List<SelectListItem> GetTechearsForManageCourse()
        {
            return _context.UserRoles.Where(r => r.RoleId == 3).Include(r => r.User).Select(u => new SelectListItem()
            {
                Text = u.User.UserName,
                Value = u.UserId.ToString()

            }).ToList();

        }

        public Course GetCourseById(int courdeId)
        {
            return _context.Courses.Find(courdeId);
        }

        public void UpdateCourse(Course course, IFormFile imgCourse, IFormFile demoUp)
        {
            course.CreateUpdate = DateTime.Now;

            if (imgCourse != null)
            {
                if (course.CourseImageName != "default.jpg")
                {
                    string RemoveImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageCourse/Image", course.CourseImageName);
                    if (File.Exists(RemoveImagePath))
                    {
                        File.Delete(RemoveImagePath);
                    }
                    string RemoveThombPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageCourse/Thumb", course.CourseImageName);
                    if (File.Exists(RemoveThombPath))
                    {
                        File.Delete(RemoveThombPath);
                    }
                }
                course.CourseImageName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(imgCourse.FileName);
                string ImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageCourse/Image", course.CourseImageName);

                using (var stream = new FileStream(ImagePath, FileMode.Create))
                {
                    imgCourse.CopyTo(stream);
                }
                string ImagePathThumb = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/ImageCourse/Thumb", course.CourseImageName);

                ImageConvertor imagResize = new ImageConvertor();

                imagResize.Image_resize(ImagePath, ImagePathThumb, 150);
            }

            if (demoUp != null)
            {
                if (course.DemoFileName != null)
                {
                    string RemoveDemoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/DemoCourse", course.DemoFileName);
                    if (File.Exists(RemoveDemoPath))
                    {
                        File.Delete(RemoveDemoPath);
                    }
                }
                course.DemoFileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(demoUp.FileName);
                string DemoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/DemoCourse", course.DemoFileName);

                using (var stream = new FileStream(DemoPath, FileMode.Create))
                {
                    demoUp.CopyTo(stream);
                }
            }
            _context.Courses.Update(course);
            _context.SaveChanges();


        }
        public Tuple<List<ShowCourseListItemViewModel>,int> GetCourse(int pageaid = 1, string filte = "", string getTipe = "all", string orderByType = "date", List<int> selectedGroups = null, int take = 0)
        {
            if (take == 0)
                take = 8;

            IQueryable<Course> result = _context.Courses;

            if (!string.IsNullOrEmpty(filte))
            {
                result = result.Where(c => c.CourseTitle.Contains(filte));
            }

            switch (getTipe)
            {
                case "all":
                    break;
                case "buy":
                    {
                        result = result.Where(c => c.CoursePrice != 0);
                        break;
                    }
                case "free":
                    {
                        result = result.Where(c => c.CoursePrice == 0);
                        break;
                    }
            }

            switch (orderByType)
            {
                case "date":
                    {
                        result = result.OrderByDescending(c => c.CreateDate);
                        break;
                    }
                case "Updatedate":
                    {
                        result = result.OrderByDescending(c => c.CreateUpdate);
                        break;
                    }
            }
            if (selectedGroups != null && selectedGroups.Any())
            {
                foreach (var groupId in selectedGroups)
                {
                    result = result.Where(g => g.GroupId == groupId || g.SubGroupId == groupId);
                }
            }

            int skip = (pageaid - 1) * take;

            int PageCount = result.Include(e => e.CourseEpisodes).AsEnumerable().Select(c => new ShowCourseListItemViewModel()
            {
                CourseId = c.CourseId,
                Title = c.CourseTitle,
                ImageName = c.CourseImageName,
                Price = c.CoursePrice,
                TotalTime = new TimeSpan(c.CourseEpisodes.Sum(e => e.EpisodeTime.Ticks))
            }).Count() / take;

            var query = result.Include(e => e.CourseEpisodes).AsEnumerable().Select(c => new ShowCourseListItemViewModel()
            {
                CourseId = c.CourseId,
                Title = c.CourseTitle,
                ImageName = c.CourseImageName,
                Price = c.CoursePrice,
                TotalTime = new TimeSpan(c.CourseEpisodes.Sum(e => e.EpisodeTime.Ticks))
            }).Skip(skip).Take(take).ToList();

            return Tuple.Create(query, PageCount);
        }
        public List<CourseEpisode> GetListEpispdeCourse(int courseid)
        {
            return _context.CourseEpisodes.Where(e => e.CourseId == courseid).ToList();
        }
        public int AddEpisode(CourseEpisode courseEpisode, IFormFile episodefile)
        {
            courseEpisode.EpisodeFileName = episodefile.FileName;

            string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/FileEpisod", courseEpisode.EpisodeFileName);

            using (var stream = new FileStream(FilePath, FileMode.Create))
            {
                episodefile.CopyTo(stream);
            }
            _context.CourseEpisodes.Add(courseEpisode);
            _context.SaveChanges();

            return courseEpisode.EpisodeId;
        }

        public bool CheckFile(string filename)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/FileEpisod", filename);

            return File.Exists(path);
        }

        public CourseEpisode GetEpisodeBuID(int episodeId)
        {
            return _context.CourseEpisodes.Find(episodeId);
        }

        public void UpdateEpisode(CourseEpisode courseEpisode, IFormFile episodefile)
        {
            if (episodefile != null)
            {

                string RemoveFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/FileEpisod", courseEpisode.EpisodeFileName);

                File.Delete(RemoveFilePath);

                courseEpisode.EpisodeFileName = episodefile.FileName;

                string FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/FileEpisod", courseEpisode.EpisodeFileName);

                using (var stream = new FileStream(FilePath, FileMode.Create))
                {
                    episodefile.CopyTo(stream);
                }
            }

            _context.CourseEpisodes.Update(courseEpisode);
            _context.SaveChanges();


        }

        public Course GetCourseForShow(int courseId)
        {
            return _context.Courses
                .Include(c => c.CourseEpisodes)
                .Include(c => c.User)
                .Include(c => c.CourseLevel)
                .Include(c => c.CourseStatus)
                .FirstOrDefault(c => c.CourseId == courseId);
        }
    }
}
