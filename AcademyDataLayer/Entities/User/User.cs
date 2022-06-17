using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyDataLayer.Entities.User
{
   public class User
    {
        public User()
        {

        }

        [Key]
        public int UserId { get; set; }
        [Display(Name ="نام کاربری")]
        [Required(ErrorMessage ="لطفا {0} را وارد کنید")]
        [MaxLength(200)]
        public string UserName { get; set; }
        [Display(Name ="ایمیل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200)]
        [EmailAddress(ErrorMessage ="ایمیل وارد شده نامعتبر می باشد")]
        public string Email { get; set; }
        [Display(Name = "رمز عبور")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200)]
        public string Password { get; set; }
        [Display(Name ="کدفعال سازی")]
        [MaxLength(200)]
        public string ActiveCode { get; set; }
        [Display(Name = "وضعیت")]
        public bool IsActive { get; set; }
        [Display(Name = "تاریخ ثبت نام")]

        public DateTime RegisterDate { get; set; }
        public bool IsDelete { get; set; }


        public virtual List<UserRole> userRoles { get; set; }
        public virtual List<Course.Course> Courses { get; set; }
        public virtual List<Order.Order> Orders{ get; set; }


    }
}
