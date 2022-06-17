using Academy.Core.ViewModels;
using AcademyDataLayer.Entities.Course;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Academy.Core.Services.Interfaces
{
    public interface ICourseService
    {

        #region Group
        List<CourseGroup> GetAllGroups();
        List<SelectListItem> GetGroupForManageCourse();
        List<SelectListItem> GetSubGroupForManageCourse(int groupid);
        List<SelectListItem> GetTechearsForManageCourse();
        List<SelectListItem> GetStatusForManageCourse();
        List<SelectListItem> GetLevelForManageCourse();

        #endregion

        #region Course
        List<ShowCourseForAdminViewModel> GetShowCourseForAdmin();
        int AddCourse(Course course, IFormFile imgCourse, IFormFile demoUp);

        Course GetCourseById(int courdeId);
        void UpdateCourse(Course course, IFormFile imgCourse, IFormFile demoUp);
        Tuple<List<ShowCourseListItemViewModel>,int> GetCourse(int pageaid = 1, string filte = "", string getTipe = "All", string orderByType = "date", List<int> selectedGroups = null, int take = 0);
        Course GetCourseForShow(int courseId);
        #endregion

        #region Episode
        List<CourseEpisode> GetListEpispdeCourse(int courseid);
        bool CheckFile(string filename);
        int AddEpisode(CourseEpisode courseEpisode, IFormFile episodefile);
        CourseEpisode GetEpisodeBuID(int episodeId);
        void UpdateEpisode(CourseEpisode courseEpisode, IFormFile episodefile);
        #endregion
    }
}
