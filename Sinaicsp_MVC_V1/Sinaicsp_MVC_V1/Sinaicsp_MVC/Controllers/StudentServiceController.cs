using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Sinaicsp_API;
using Sinaicsp_MVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Sinaicsp_MVC.Controllers
{
    public class StudentServiceController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Services_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<Service> services = Service.GetAll().AsQueryable();
            DataSourceResult result = services.ToDataSourceResult(request, item => new
            {
                Id = item.Id,
                Name = item.Name,
                Model = item.Model,
                ProviderName = item.ProviderName,
                WeeklySession = item.WeeklySession,
                SessionLength = item.SessionLength,
                NumberOfStudents = item.NumberOfStudents,
                SessionStart = item.SessionStart,
                SessionEnd = item.SessionEnd,
                IsDeleted = item.IsDeleted,
                CreatedOn = item.CreatedOn,
                CreatedByUserId = item.CreatedByUserId,
                CreatedByUserName = item.CreatedByUserName
            });

            return Json(result);
        }
        public JsonResult Students_Read()
        {
            IQueryable<Student> students = Student.GetAll().AsQueryable();
            var result = from item in students
                         select new
                         {
                             Id = item.Id,
                             FullName = item.FullName
                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddNewStudentService()
        {
            ViewBag.AlreadyExists = false;
            StudentServiceViewModel _item = StudentServiceViewModel.GetViewData();
            _item.AllProviders = Provider.GetAll();
            _item.AllEModels = Enum.GetNames(typeof(E_ServiceModel)).ToList().Select(a => a.ToString().Replace("_", " ")).ToList();
            return View(_item);
        }

        [HttpPost]
        public JsonResult AddStudentService(StudentServiceViewModel model, List<int> studentIds)
        {
            model.AllProviders = Provider.GetAll();
            model.AllEModels = Enum.GetNames(typeof(E_ServiceModel)).ToList().Select(a => a.ToString().Replace("_", " ")).ToList();
            if (ModelState.IsValid)
            {
                Service.AddNew(model.CurrentService.ProviderId, model.CurrentService.Name, model.CurrentService.Model,
                    model.CurrentService.NumberOfStudents, model.CurrentService.SessionLength, model.CurrentService.WeeklySession,
                    model.CurrentService.SessionStart, model.CurrentService.SessionEnd, studentIds, ApplicationHelper.LoggedUserId);

                return Json(true, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ViewDetails(int id)
        {
            Service _item = Service.GetById(id);
            return View(_item);
        }
    }
}