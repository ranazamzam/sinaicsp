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
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Teachers_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<Teacher> items = Teacher.GetAll().AsQueryable();
            DataSourceResult result = items.ToDataSourceResult(request, _item => new
            {
                Id = _item.Id,
                UserName = _item.UserName,
                Email = _item.Email,
                Title = _item.Title,
                SchoolName = _item.School.Name,
                IsDeleted = _item.IsDeleted,
                CreatedOn = _item.CreatedOn,
                CreatedByUserId = _item.CreatedByUserId,
                CreatedByUserName = _item.CreatedByUserName
            });

            return Json(result);
        }
        public ActionResult AddNewTeacher()
        {
            ViewBag.AlreadyExists = false;
            Teacher _item = new Teacher();
            ViewBag.AllSchools = new SelectList(School.GetAll(), "Id", "Name");
            return View(_item);
        }
        [HttpPost]
        public ActionResult AddNewTeacher(Teacher model)
        {
            ViewBag.AlreadyExists = false;
            ViewBag.AllSchools = new SelectList(School.GetAll(), "Id", "Name");

            if (ModelState.IsValid)
            {
                bool isAdded = Teacher.AddNew(model.SchoolId, model.UserName, model.Email, model.Title, ApplicationHelper.LoggedUserId);
                if (isAdded)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.AllSchools = new SelectList(School.GetAll(), "Id", "Name");
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