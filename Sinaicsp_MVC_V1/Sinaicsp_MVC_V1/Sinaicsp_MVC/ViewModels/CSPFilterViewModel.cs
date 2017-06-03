using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sinaicsp_MVC.ViewModels
{
    [Serializable]
    public class CSPFilterViewModel
    {
        public int StudentId { get; set; }
        public int TeacherId { get; set; }
        public int SchoolId { get; set; }
        public int SchoolYearId { get; set; }
        public int SubjectId { get; set; }
        public int ClassId { get; set; }
    }
}