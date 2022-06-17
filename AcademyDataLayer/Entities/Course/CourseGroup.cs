using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyDataLayer.Entities.Course
{
  public  class CourseGroup
    {
        [Key]
        public int GroupId { get; set; }
        [Display(Name ="عنوان گروه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200)]
        public string GroupTitle { get; set; }
        [Display(Name = "حذف شده؟")]
        public bool IsDelete { get; set; }
        [Display(Name = " گروه اصلی")]
        public int? ParentID { get; set; }

        [ForeignKey("ParentID")]
        public List<CourseGroup> CourseGroups { get; set; }
        [InverseProperty("CourseGroup")]
        public List<Course> Courses { get; set; }

        [InverseProperty("SubGroup")]
        public List<Course> SubGroups { get; set; }


    }
}
