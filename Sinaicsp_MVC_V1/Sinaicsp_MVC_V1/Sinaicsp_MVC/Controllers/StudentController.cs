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
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Students_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<Student> items = Student.GetAll().AsQueryable();
            DataSourceResult result = items.ToDataSourceResult(request, _item => new
            {
                Id = _item.Id,
                SchoolName = _item.SchoolName,
                GradeName = _item.GradeName,
                ClassName = _item.ClassName,
                CityName = _item.CityName,
                Gender = _item.Gender,
                FirstName = _item.FirstName,
                LastName = _item.LastName,
                DOB = _item.DOB,
                IsDeleted = _item.IsDeleted,
                CreatedOn = _item.CreatedOn,
                CreatedByUserId = _item.CreatedByUserId,
                CreatedByUserName = _item.CreatedByUserName
            });

            return Json(result);
        }
        public ActionResult AddNewStudent()
        {
            ViewBag.AlreadyExists = false;
            ViewBag.AllSchools = new SelectList(School.GetAll(), "Id", "Name");
            ViewBag.AllClasses = new SelectList(StudentClass.GetAll(), "Id", "Name");
            ViewBag.AllGrades = new SelectList(StudentGrade.GetAll(), "Id", "Name");
            ViewBag.AllCities = new SelectList(City.GetAll(), "Id", "Name");
            Student _item = new Student();
            return View(_item);
        }
        [HttpPost]
        public ActionResult AddNewStudent(Student model)
        {
            ViewBag.AlreadyExists = false;
            ViewBag.AllSchools = new SelectList(School.GetAll(), "Id", "Name");
            ViewBag.AllClasses = new SelectList(StudentClass.GetAll(), "Id", "Name");
            ViewBag.AllGrades = new SelectList(StudentGrade.GetAll(), "Id", "Name");
            ViewBag.AllCities = new SelectList(City.GetAll(), "Id", "Name");
            if (ModelState.IsValid)
            {
                bool isAdded = Student.AddNew(model.SchoolId, model.StudentClassId, model.StudentGradeId,
                   model.CityId, model.FirstName, model.LastName, model.Gender, model.DOB, model.PrimaryParent,
                   model.Address, model.State, model.MotherCell, model.SecondaryParent, model.Phone,
                   model.FatherCell, ApplicationHelper.LoggedUserId);
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