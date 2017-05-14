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
    public class StudentClassController : Controller
    {
        // GET: StudentClass
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult StudentClasses_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<StudentClass> items = StudentClass.GetAll().AsQueryable();
            DataSourceResult result = items.ToDataSourceResult(request, _item => new
            {
                Id = _item.Id,
                Name = _item.Name,
                //IsDeleted = _item.IsDeleted,
                CreatedOn = _item.CreatedOn,
                CreatedByUserId = _item.CreatedByUserId,
                CreatedByUserName = _item.CreatedByUserName
            });

            return Json(result);
        }
        public ActionResult AddNewStudentClass(int? id)
        {
            if (id != null)
            {
                ViewBag.AlreadyExists = false;

                StudentClass _item = StudentClass.GetById(id.Value);
                return View(_item);
            }
            else
            {
                ViewBag.AlreadyExists = false;
                StudentClass _item = new StudentClass();
                return View(_item);
            }
        }
        [HttpPost]
        public ActionResult AddNewStudentClass(StudentClass model)
        {
            ViewBag.AlreadyExists = false;
            if (ModelState.IsValid)
            {
                if (model.Id != 0)
                {
                    bool IsUpdated = StudentClass.Update(model.Id, model.Name);
                    if (IsUpdated)
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    bool isAdded = StudentClass.AddNew(model.Name, ApplicationHelper.LoggedUserId);
                    if (isAdded)
                    {
                        return RedirectToAction("Index");
                    }
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
            StudentClass.SoftDelete(id);
            return RedirectToAction("Index");
        }
    }
}