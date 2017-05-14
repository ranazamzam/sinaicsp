using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Sinaicsp_API;

namespace Sinaicsp_MVC.Controllers
{
    public class SchoolController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Schools_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<School> schools = School.GetAll().AsQueryable();
            DataSourceResult result = schools.ToDataSourceResult(request, school => new
            {
                Id = school.Id,
                Name = school.Name,
                // IsDeleted = school.IsDeleted,
                CreatedOn = school.CreatedOn,
                CreatedByUserId = school.CreatedByUserId,
                CreatedByUserName = school.CreatedByUserName
            });

            return Json(result);
        }
        public ActionResult AddNewSchool(int? id)
        {
            if (id != null)
            {
                ViewBag.AlreadyExists = false;

                School _item = School.GetById(id.Value);
                return View(_item);
            }
            else
            {
                ViewBag.AlreadyExists = false;
                School _item = new School();
                return View(_item);
            }
        }
        [HttpPost]
        public ActionResult AddNewSchool(School model)
        {
            ViewBag.AlreadyExists = false;
            if (ModelState.IsValid)
            {
                if (model.Id != 0)
                {
                    bool IsUpdated = School.Update(model.Id, model.Name);
                    if (IsUpdated)
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    bool isAdded = School.AddNew(model.Name, ApplicationHelper.LoggedUserId);
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
            School.SoftDelete(id);
            return RedirectToAction("Index");
        }
    }
}
