using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Sinaicsp_API
{
    public class SchoolMetadata
    {
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
    }
    public class CityMetadata
    {
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
    }
    public class StudentGradeMetadata
    {
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
    }
    public class StudentClassMetadata
    {
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
    }
    public class TeacherMetadata
    {
        [Display(Name ="Name")]
        [Required(ErrorMessage = "*")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "*")]
        public int SchoolId { get; set; }

    }
    public class SchoolYearMetadata
    {
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
    }
    
}