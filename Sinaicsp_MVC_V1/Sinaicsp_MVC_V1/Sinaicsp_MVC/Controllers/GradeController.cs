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
    public class GradeController : Controller
    {
        // GET: Grade
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StudentGrades_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<StudentGrade> items = StudentGrade.GetAll().AsQueryable();
            DataSourceResult result = items.ToDataSourceResult(request, _item => new
            {
                Id = _item.Id,
                Name = _item.Name,
                IsDeleted = _item.IsDeleted,
                CreatedOn = _item.CreatedOn,
                CreatedByUserId = _item.CreatedByUserId,
                CreatedByUserName = _item.CreatedByUserName
            });

            return Json(result);
        }
        public ActionResult AddNewStudentGrade()
        {
            ViewBag.AlreadyExists = false;
            StudentGrade _item = new StudentGrade();
            return View(_item);
        }
        [HttpPost]
        public ActionResult AddNewStudentGrade(StudentGrade model)
        {
            ViewBag.AlreadyExists = false;
            if (ModelState.IsValid)
            {
                bool isAdded = StudentGrade.AddNew(model.Name, ApplicationHelper.LoggedUserId);
                if (isAdded)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.AlreadyExists = true;
                return View(model);
            }
            else
            {
                return View(model);
            }
        }
    }
}