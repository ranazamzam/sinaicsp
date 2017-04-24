﻿using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Sinaicsp_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sinaicsp_MVC.Controllers
{
    public class SchoolYearController : Controller
    {
        // GET: SchoolYear
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SchoolYears_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<SchoolYear> items = SchoolYear.GetAll().AsQueryable();
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
        public ActionResult AddNewSchoolYear()
        {
            ViewBag.AlreadyExists = false;
            SchoolYear _item = new SchoolYear();
            return View(_item);
        }
        [HttpPost]
        public ActionResult AddNewSchoolYear(SchoolYear model)
        {
            ViewBag.AlreadyExists = false;
            if (ModelState.IsValid)
            {
                bool isAdded = SchoolYear.AddNew(model.Name, ApplicationHelper.LoggedUserId);
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