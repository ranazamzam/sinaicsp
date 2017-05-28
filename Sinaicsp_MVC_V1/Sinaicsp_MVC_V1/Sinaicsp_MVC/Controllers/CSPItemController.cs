using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Sinaicsp_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Reporting.Processing;

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
            IQueryable<School> items = new List<School>().AsQueryable();
            if (ApplicationHelper.IsLoggedIn)
            {
                if (ApplicationHelper.LoggedUser.CurrentRoles.Contains(E_Role.Admin))
                {
                    items = School.GetAll().AsQueryable();
                }
                if (ApplicationHelper.LoggedUser.CurrentRoles.Contains(E_Role.Teacher))
                {
                    items = School.GetAll(ApplicationHelper.LoggedUserId).AsQueryable();
                }
            }
            var result = from item in items
                         select new
                         {
                             Id = item.Id,
                             Name = item.Name
                         };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Students_Read(int schoolId)
        {
            IQueryable<Student> items = Student.GetAll(schoolId).AsQueryable();
            var result = from item in items
                         select new
                         {
                             Id = item.Id,
                             FullName = item.GradeName + "\\ " + item.FullName
                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Teachers_Read(int schoolId)
        {
            IQueryable<Teacher> items = Teacher.GetAll(schoolId).AsQueryable();
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
        public JsonResult SchoolYears_Read()
        {
            IQueryable<SchoolYear> items = SchoolYear.GetCurrent().AsQueryable();
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
                StudentName = _item.StudentName,
                SchoolName = _item.SchoolName,
                SubjectName = _item.SubjectName,
                TeacherNames = _item.TeacherNames,
                Materials = _item.Materials,
                SchoolYearName = _item.SchoolYearName,
                CreatedOn = _item.CreatedOn,
                CreatedByUserName = _item.CreatedByUserName
            });

            return Json(result);
        }
        [HttpPost]
        public JsonResult AddNewCSPItem(CSP model, List<int> teachersIds)
        {
            if (model.StudentId > 0 && model.SubjectId > 0 && model.SchoolYearId > 0 && string.IsNullOrEmpty(model.Materials) == false && teachersIds.Count > 0)
            {
                CSP.AddNew(model.StudentId, model.SubjectId, model.SchoolYearId, model.Materials, teachersIds, ApplicationHelper.LoggedUserId);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Notes(int id)
        {
            string rate1 = string.Empty;
            string rate2 = string.Empty;
            string rate3 = string.Empty;
            CSP _item = CSP.GetById(id);
            CSP.AdjustOrder(id);
            List<string> dateinitVal = new List<string>();
            int year = int.Parse(DateTime.Now.Year.ToString().Replace("20", ""));
            int orgYear = year;
            int month = 7;
            for (int i = 0; i < 12; i++)
            {

                dateinitVal.Add(month + "/" + year);
                if (month == 11 && year == orgYear)
                {
                    rate1 = month + "/" + year;
                }
                if (month == 2)
                {
                    rate2 = month + "/" + year;
                }
                if (month == 6)
                {
                    rate3 = month + "/" + year;
                }
                month++;
                if (month > 12)
                {
                    month = 1;
                    year++;
                }
            }
            ViewBag.rate1 = rate1;
            ViewBag.rate2 = rate2;
            ViewBag.rate3 = rate3;
            ViewBag.AllDateInitiated = new SelectList(dateinitVal);
            List<Rating> allrates = Rating.GetAll(_item.Student.SchoolId);
            allrates.Insert(0, new Rating() { Description = "" });
            ViewBag.AllRates = new SelectList(allrates, "RateValue", "Description");
            List<CSPGoalCatalog> allGoals = CSPGoalCatalog.GetAll(id).Where(a => a.ParentCSPGoalCatalogId == null).ToList();
            allGoals.Insert(0, new CSPGoalCatalog() { Id = -1, TextGoal = "NONE" });
            ViewBag.allGoals = new SelectList(allGoals, "Id", "TextGoal");
            return View(_item);
        }

        [HttpGet]
        public JsonResult SaveCSPNotes(int itemId, string comment, string febNotes, string juneNotes)
        {
            CSP.SaveCSPNotes(itemId, comment, febNotes, juneNotes);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NextCSP(int id)
        {
            int nextId = CSP.GetNextCSP(id);
            return RedirectToAction("Notes", new { id = nextId });
        }
        public ActionResult PreviousCSP(int id)
        {
            int nextId = CSP.GetPreviousCSP(id);
            return RedirectToAction("Notes", new { id = nextId });
        }
        [HttpGet]
        public JsonResult AddNewCSPGoalCatalog(int cspGoalCatalogId, int? parentId, int CSPId, string dateInitiated, string rate1, string rate2, string rate3, string textGoal)
        {
            if (cspGoalCatalogId == 0)
            {
                CSPGoalCatalog.AddNew(parentId > -1 ? parentId : null, CSPId, dateInitiated, rate1, rate2, rate3, textGoal, ApplicationHelper.LoggedUserId);
            }else
            {
                CSPGoalCatalog.Update(cspGoalCatalogId, dateInitiated, rate1, rate2, rate3, textGoal);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CSPGoaCatalog_Read([DataSourceRequest]DataSourceRequest request, int cspId)
        {

            IQueryable<CSPGoalCatalog> items = CSPGoalCatalog.GetAll(cspId).AsQueryable();
            DataSourceResult result = items.ToDataSourceResult(request, _item => new
            {
                Id = _item.Id,
                ParentName = _item.ParentName,
                TextGoal = _item.TextGoal,
                DateInitiated = _item.DateInitiated,
                Rate1 = _item.Rate1,
                Rate2 = _item.Rate2,
                Rate3 = _item.Rate3,
                CreatedOn = _item.CreatedOn,
                CreatedByUserName = _item.CreatedByUserName
            });

            return Json(result);
        }

        [HttpGet]
        public JsonResult RemoveCSPGoalCatalog(int id)
        {
            CSPGoalCatalog.SoftDelete(id);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Move CSP Goal Catalog Above
        /// </summary>
        /// <param name="id">CSP Goal Catalog</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult MoveOrderUp(int id)
        {
            CSP _item = CSPGoalCatalog.MoveUp(id);
            return RedirectToAction("Notes", new { id = _item.Id });
        }

        /// <summary>
        /// Move CSP Goal Catalog Down
        /// </summary>
        /// <param name="id">CSP Goal Catalog</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult MoveOrderDown(int id)
        {
            CSP _item = CSPGoalCatalog.MoveDown(id);
            return RedirectToAction("Notes", new { id = _item.Id });
        }

        public JsonResult GetCspGoalCatalog(int id)
        {
            CSPGoalCatalog _item = CSPGoalCatalog.GetById(id);
            CSPGoalCatalog _retVal = new CSPGoalCatalog()
            {
                Id = _item.Id,
                ParentCSPGoalCatalogId = _item.ParentCSPGoalCatalogId,
                TextGoal = _item.TextGoal,
                DateInitiated = _item.DateInitiated,
                Rate1 = _item.Rate1,
                Rate2 = _item.Rate2,
                Rate3 = _item.Rate3
            };
            return Json(_retVal, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DownloadCSPReport(int id)
        {
            ExportToPDF(new Report.CSPItemReport());
            return Json(true, JsonRequestBehavior.AllowGet);

        }

        void ExportToPDF(Telerik.Reporting.Report reportToExport)
        {
            ReportProcessor reportProcessor = new ReportProcessor();
            Telerik.Reporting.InstanceReportSource instanceReportSource = new Telerik.Reporting.InstanceReportSource();
            instanceReportSource.ReportDocument = reportToExport;
            RenderingResult result = reportProcessor.RenderReport("PDF", instanceReportSource, null);

            string fileName = result.DocumentName + "." + result.Extension;

            Response.Clear();
            Response.ContentType = result.MimeType;
            Response.Cache.SetCacheability(HttpCacheability.Private);
            Response.Expires = -1;
            Response.Buffer = true;

            Response.AddHeader("Content-Disposition",
                               string.Format("{0};FileName=\"{1}\"",
                                             "attachment",
                                             fileName));

            Response.BinaryWrite(result.DocumentBytes);
            Response.End();
        }

    }
}