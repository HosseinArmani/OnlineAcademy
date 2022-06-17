using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyDataLayer.Entities.Course
{
   public class Course
    {
        [Key]
        public int CourseId { get; set; }
        [Required]
        public int GroupId { get; set; }
        public int? SubGroupId { get; set; }
        [Required]
        public int TecherId { get; set; }
        [Required]
        public int LevelId { get; set; }
        [Required]
        public int StatusId { get; set; }
        [Display(Name ="عنوان دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(400)]
        public string CourseTitle { get; set; }

        [Display(Name = "توضیحات دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Discription { get; set; }

        [Display(Name = "قیمت دوره")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int CoursePrice { get; set; }

        [MaxLength(600)] 
        public string Tags { get; set; }
        public string CourseImageName { get; set; }

        [MaxLength(100)]
        public string DemoFileName { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        public DateTime? CreateUpdate { get; set; }

        #region REL

        [ForeignKey("TecherId ")]
        public User.User User { get; set; }

        [ForeignKey("GroupId  ")]
        public CourseGroup CourseGroup { get; set; }

        [ForeignKey("SubGroupId")]
        public CourseGroup SubGroup { get; set; }

        [ForeignKey("StatusId")]
        public CourseStatus CourseStatus { get; set; }

        [ForeignKey("LevelId")]
        public CourseLevel CourseLevel { get; set; }

        public List<CourseEpisode> CourseEpisodes { get; set; }
        public List<Order.OrderDetail> OrderDetails { get; set; }


        #endregion

    }
}
