using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyDataLayer.Entities.Course
{
   public class CourseEpisode
    {
        [Key]
        public int EpisodeId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Display(Name ="عنوان قسمت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(400)]
        public string EpisodeTitle  { get; set; }
        [Display(Name = "زمان ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public TimeSpan EpisodeTime { get; set; }
        [Display(Name = "فایل ")]
        public string EpisodeFileName { get; set; }

        [Display(Name = "رایگان ")]
        public bool IsFree { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }

    }
}
