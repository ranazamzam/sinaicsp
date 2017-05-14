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
                //IsDeleted = _item.IsDeleted,
                CreatedOn = _item.CreatedOn,
                CreatedByUserId = _item.CreatedByUserId,
                CreatedByUserName = _item.CreatedByUserName
            });

            return Json(result);
        }
        public ActionResult AddNewSubject(int? id)
        {
            if (id != null)
            {
                ViewBag.AlreadyExists = false;
                Subject _item = Subject.GetById(id.Value);
                ViewBag.AllSubjects = new SelectList(Subject.GetAll(), "Id", "Name");
                return View(_item);
            }
            else
            {


                ViewBag.AlreadyExists = false;
                Subject _item = new Subject();
                ViewBag.AllSubjects = new SelectList(Subject.GetAll(), "Id", "Name");
                return View(_item);
            }
        }
        [HttpPost]
        public ActionResult AddNewSubject(Subject model)
        {
            bool isAdded = false;
            bool IsUpdated = false;
            ViewBag.AllSubjects = new SelectList(Subject.GetAll(), "Id", "Name");
            ViewBag.AlreadyExists = false;
            if (ModelState.IsValid)
            {
                if (model.ParentId == null)
                {
                    if (model.Id != 0)
                    {
                         IsUpdated = Subject.Update(model.Id,null,model.Name);
                      
                    }
                    else
                    {
                        isAdded = Subject.AddNew(model.Name, ApplicationHelper.LoggedUserId);
                    }
                }
                else
                {
                    if (model.Id != 0)
                    {
                         IsUpdated = Subject.Update(model.Id,model.ParentId, model.Name);

                    }
                    else
                    {
                        isAdded = Subject.AddNew(model.ParentId.Value, model.Name, ApplicationHelper.LoggedUserId);
                    }
                }
                if (isAdded||IsUpdated)
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
        public ActionResult Delete(int id)
        {
            Subject.SoftDelete(id);
            return RedirectToAction("Index");
        }
    }
}