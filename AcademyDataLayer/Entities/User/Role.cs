using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyDataLayer.Entities.User
{
  public class Role
    {
        public Role()
        {

        }

        [Key]
        public int RoleId { get; set; }
        [Display(Name ="عنوان نقش")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200)]
        public string RoleTitle { get; set; }


        public virtual List<UserRole> userRoles { get; set; }
        
    }
}
