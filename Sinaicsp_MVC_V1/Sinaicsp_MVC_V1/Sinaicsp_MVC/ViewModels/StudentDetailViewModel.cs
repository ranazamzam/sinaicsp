using Sinaicsp_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sinaicsp_MVC.ViewModels
{
    public class StudentDetailViewModel
    {
        public static StudentDetailViewModel GetViewData(int studentId)
        {
            StudentDetailViewModel _retVal = new ViewModels.StudentDetailViewModel();
            Student _student = Student.GetById(studentId);
            _retVal.StudentId = studentId;
            _retVal.CurrentStudent = _student;
            return _retVal;
        }
        public int StudentId { get; set; }
        public Student CurrentStudent { get; set; }
        public string SelectedSubject { get; set; }
        public string SelectedTeacher { get; set; }
        public string SelectedClass { get; set; }
        public string SessionStart { get; set; }
        public string SessionEnd { get; set; }
        public string Accommodation { get; set; }
    }
}