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
            IQueryable<School> items = School.GetAll().AsQueryable();
            var result = from item in items
                         select new
                         {
                             Id = item.Id,
                             Name = item.Name
                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Students_Read()
        {
            IQueryable<Student> items = Student.GetAll().AsQueryable();
            var result = from item in items
                         select new
                         {
                             Id = item.Id,
                             FullName = item.GradeName + "\\ " + item.FullName
                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Teachers_Read()
        {
            IQueryable<Teacher> items = Teacher.GetAll().AsQueryable();
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
        public ActionResult CSPs_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<CSP> items = CSP.GetAll().AsQueryable();
            DataSourceResult result = items.ToDataSourceResult(request, _item => new
            {
                Id = _item.Id,
                CreatedByUserId = _item.CreatedByUserId,
                CreatedByUserName = _item.CreatedByUserName
            });

            return Json(result);
        }
    }
}