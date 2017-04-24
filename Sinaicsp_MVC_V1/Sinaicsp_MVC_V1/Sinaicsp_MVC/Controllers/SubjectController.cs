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
    public class SubjectController : Controller
    {
        // GET: Subject
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Subjects_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<Subject> items = Subject.GetAll().AsQueryable();
            DataSourceResult result = items.ToDataSourceResult(request, _item => new
            {
                Id = _item.Id,
                Name = _item.Name,
                ParentName = _item.ParentName,
                IsDeleted = _item.IsDeleted,
                CreatedOn = _item.CreatedOn,
                CreatedByUserId = _item.CreatedByUserId,
                CreatedByUserName = _item.CreatedByUserName
            });

            return Json(result);
        }
        public ActionResult AddNewSubject()
        {
            ViewBag.AlreadyExists = false;
            Subject _item = new Subject();
            ViewBag.AllSubjects = new SelectList(Subject.GetAll(), "Id", "Name");
            return View(_item);
        }
        [HttpPost]
        public ActionResult AddNewSubject(Subject model)
        {
            bool isAdded = false;
            ViewBag.AllSubjects = new SelectList(Subject.GetAll(), "Id", "Name");
            ViewBag.AlreadyExists = false;
            if (ModelState.IsValid)
            {
                if (model.ParentId == null)
                {
                    isAdded = Subject.AddNew(model.Name, ApplicationHelper.LoggedUserId);
                }
                else
                {
                    isAdded = Subject.AddNew(model.ParentId.Value, model.Name, ApplicationHelper.LoggedUserId);
                }
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