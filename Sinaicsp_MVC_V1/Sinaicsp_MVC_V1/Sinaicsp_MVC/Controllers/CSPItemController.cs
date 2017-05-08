using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Sinaicsp_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sinaicsp_MVC.Controllers
{
    public class CSPItemController : Controller
    {
        // GET: CSP
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddNewCSP()
        {
            return View();
        }
        public JsonResult Schools_Read()
        {
            IQueryable<School> items = new List<School>().AsQueryable();
            if (ApplicationHelper.IsLoggedIn)
            {
                if (ApplicationHelper.LoggedUser.CurrentRoles.Contains(E_Role.Admin))
                {
                    items = School.GetAll().AsQueryable();
                }
                if (ApplicationHelper.LoggedUser.CurrentRoles.Contains(E_Role.Teacher))
                {
                    items = School.GetAll(ApplicationHelper.LoggedUserId).AsQueryable();
                }
            }
            var result = from item in items
                         select new
                         {
                             Id = item.Id,
                             Name = item.Name
                         };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Students_Read(int schoolId)
        {
            IQueryable<Student> items = Student.GetAll(schoolId).AsQueryable();
            var result = from item in items
                         select new
                         {
                             Id = item.Id,
                             FullName = item.GradeName + "\\ " + item.FullName
                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Teachers_Read(int schoolId)
        {
            IQueryable<Teacher> items = Teacher.GetAll(schoolId).AsQueryable();
            var result = from item in items
                         select new
                         {
                             Id = item.Id,
                             UserName = item.UserName
                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Subjects_Read()
        {
            IQueryable<Subject> items = Subject.GetAll().AsQueryable();
            var result = from item in items
                         select new
                         {
                             Id = item.Id,
                             Name = item.Name
                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SchoolYears_Read()
        {
            IQueryable<SchoolYear> items = SchoolYear.GetCurrent().AsQueryable();
            var result = from item in items
                         select new
                         {
                             Id = item.Id,
                             Name = item.Name
                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CSPs_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<CSP> items = CSP.GetAll().AsQueryable();
            DataSourceResult result = items.ToDataSourceResult(request, _item => new
            {
                Id = _item.Id,
                StudentName= _item.StudentName,
                SchoolName = _item.SchoolName,
                SubjectName = _item.SubjectName,
                TeacherNames = _item.TeacherNames,
                Materials = _item.Materials,
                SchoolYearName = _item.SchoolYearName,
                CreatedOn = _item.CreatedOn,
                CreatedByUserName = _item.CreatedByUserName
            });

            return Json(result);
        }
        [HttpPost]
        public JsonResult AddNewCSPItem(CSP model, List<int> teachersIds)
        {
            if (model.StudentId > 0 && model.SubjectId > 0 && model.SchoolYearId > 0 && string.IsNullOrEmpty(model.Materials) == false && teachersIds.Count > 0)
            {
                CSP.AddNew(model.StudentId, model.SubjectId,model.SchoolYearId, model.Materials, teachersIds, ApplicationHelper.LoggedUserId);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}