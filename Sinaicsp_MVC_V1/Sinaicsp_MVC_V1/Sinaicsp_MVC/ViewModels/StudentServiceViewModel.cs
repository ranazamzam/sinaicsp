using Sinaicsp_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sinaicsp_MVC.ViewModels
{
    public class StudentServiceViewModel
    {
        public StudentServiceViewModel()
        {
            students = new List<int>();
        }
        public Service CurrentService { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<int> students { get; set; }
        public List<Provider> AllProviders { get; set; }
        public int SelectedProvider { get; set; }
        public List<string> AllEModels { get; set; }

        public static StudentServiceViewModel GetViewData()
        {
            StudentServiceViewModel _retVal = new StudentServiceViewModel();
            _retVal.CurrentService = new Service();

            return _retVal;
        }
    }
}