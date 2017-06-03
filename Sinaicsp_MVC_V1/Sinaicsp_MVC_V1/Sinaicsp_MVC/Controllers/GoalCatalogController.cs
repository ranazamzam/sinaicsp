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
    public class GoalCatalogController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">GC_Subject ID</param>
        /// <returns></returns>
        public ActionResult Index(int? id)
        {
            if (id != null)
            {
                ViewBag.SubjectId = id.Value;
                GC_Subjects.AdjustOrder(id.Value);
                return View(GoalCatalog.GetAllBySubjectId(id.Value));
            }
            else
            {
                GC_Subjects _item = GC_Subjects.GetAll().FirstOrDefault();
                if (_item != null)
                {
                    ViewBag.SubjectId = _item.Id;
                    GC_Subjects.AdjustOrder(_item.Id);
                    return View(GoalCatalog.GetAllBySubjectId(_item.Id));
                }
                ViewBag.SubjectId = -1;
                return View(new List<GoalCatalog>());
            }
        }

        public ActionResult GoalCatalogs_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<GoalCatalog> items = GoalCatalog.GetAll().AsQueryable();
            DataSourceResult result = items.ToDataSourceResult(request, _currItem => new
            {
                Id = _currItem.Id,
                TextGoal = _currItem.TextGoal,
                CategoryName = _currItem.CategoryName,
                SubjectName = _currItem.SubjectName,
                ParentName = _currItem.ParentName,
                CreatedOn = _currItem.CreatedOn,
                CreatedByUserId = _currItem.CreatedByUserId,
                CreatedByUserName = _currItem.CreatedByUserName
            });

            return Json(result);
        }
        public ActionResult AddNewGoalCatalog(int subjectId)
        {
            ViewBag.SubjectId = subjectId;
            List<GC_Subjects> _subjects = GC_Subjects.GetAll().Where(a => a.Id == subjectId).ToList();
            ViewBag.AllSubjects = new SelectList(_subjects, "Id", "Name");
            List<GoalCatalog> allGoalCatalog = GoalCatalog.GetAllParents(subjectId);
            allGoalCatalog.Insert(0, new GoalCatalog() { Id = -1, TextGoal = "NONE" });
            ViewBag.AllGoals = new SelectList(allGoalCatalog, "Id", "TextGoal");
            GoalCatalog _item = new GoalCatalog();
            return View(_item);

        }
        [HttpPost]
        public ActionResult AddNewGoalCatalog(GoalCatalog model)
        {
            ViewBag.SubjectId = model.GC_SubjectsId ;
            List<GoalCatalog> allGoalCatalog = GoalCatalog.GetAllParents(model.GC_SubjectsId);
            allGoalCatalog.Insert(0, new GoalCatalog() { Id = -1, TextGoal = "NONE" });
            ViewBag.AllGoals = new SelectList(allGoalCatalog, "Id", "TextGoal");
            ViewBag.AllSubjects = new SelectList(GC_Subjects.GetAll(), "Id", "Name");
            ViewBag.AlreadyExists = false;
            if (ModelState.IsValid)
            {

                bool isAdded = GoalCatalog.AddNew(model.ParentGoalCatalogId != -1 ? model.ParentGoalCatalogId : null, model.GC_SubjectsId, model.TextGoal, ApplicationHelper.LoggedUserId);
                if (isAdded)
                {
                    return RedirectToAction("Index", new { id = model.GC_SubjectsId });
                }

                return View(model);
            }
            else
            {
                return View(model);
            }
        }

        public ActionResult DeleteGoalCatalog(int id)
        {
            GC_Subjects _item = GoalCatalog.SoftDelete(id);
            return RedirectToAction("Index", new { id = _item.Id });
        }

        /// <summary>
        /// Move  Goal Catalog Above
        /// </summary>
        /// <param name="id"> Goal Catalog</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult MoveOrderUp(int id)
        {
            GC_Subjects _item = GoalCatalog.MoveUp(id);
            return RedirectToAction("Index", new { id = _item.Id });
        }

        /// <summary>
        /// Move  Goal Catalog Down
        /// </summary>
        /// <param name="id"> Goal Catalog</param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult MoveOrderDown(int id)
        {
            GC_Subjects _item = GoalCatalog.MoveDown(id);
            return RedirectToAction("Index", new { id = _item.Id });
        }


        public JsonResult GetAllSubjects_Read()
        {
            IQueryable<GC_Subjects> items = GC_Subjects.GetAll().AsQueryable();
            var result = from item in items
                         select new
                         {
                             Id = item.Id,
                             Name = item.Name
                         };

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}