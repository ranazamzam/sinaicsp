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

    public class SubjectMetadata
    {
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
    }
    public class StudentMetadata
    {
        [Required(ErrorMessage = "*")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "*")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "*")]
        public DateTime DOB { get; set; }
        [Required(ErrorMessage = "*")]
        [Range(1, int.MaxValue, ErrorMessage = "*")]
        public int SchoolId { get; set; }
        [Required(ErrorMessage = "*")]
        [Range(1, int.MaxValue, ErrorMessage = "*")]
        public int StudentClassId { get; set; }
        [Required(ErrorMessage = "*")]
        [Range(1, int.MaxValue, ErrorMessage = "*")]
        public int StudentGradeId { get; set; }
        [Required(ErrorMessage = "*")]
        [Range(1, int.MaxValue,ErrorMessage = "*")]
        public int CityId { get; set; }


        [Required(ErrorMessage = "*")]
        public string PrimaryParent { get; set; }
        [Required(ErrorMessage = "*")]
        public string Address { get; set; }
        [Required(ErrorMessage = "*")]
        public string State { get; set; }
        [Required(ErrorMessage = "*")]
        public string MotherCell { get; set; }
        [Required(ErrorMessage = "*")]
        public string SecondaryParent { get; set; }
        [Required(ErrorMessage = "*")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "*")]
        public string FatherCell { get; set; }

    }
    public class ProviderMetadata
    {
        [Required(ErrorMessage = "*")]
        public string Name { get; set; }
    }

}