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
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GoalCatalogs_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<GoalCatalog> items = GoalCatalog.GetAll().AsQueryable();
            DataSourceResult result = items.ToDataSourceResult(request, _currItem => new
            {
                Id = _currItem.Id,
                TextGoal = _currItem.TextGoal,
                SubjectName = _currItem.SubjectName,
                ParentName = _currItem.ParentName,
                CreatedOn = _currItem.CreatedOn,
                CreatedByUserId = _currItem.CreatedByUserId,
                CreatedByUserName = _currItem.CreatedByUserName
            });

            return Json(result);
        }
        public ActionResult AddNewGoalCatalog(int? id)
        {
            ViewBag.AllSubjects = new SelectList(GC_Subjects.GetAll(), "Id", "Name");
            List<GoalCatalog> allGoalCatalog = GoalCatalog.GetAll();
            allGoalCatalog.Insert(0, new GoalCatalog() { Id = -1, TextGoal = "NONE" });
            ViewBag.AllGoals = new SelectList(allGoalCatalog, "Id", "TextGoal");
            if (id != null)
            {
                ViewBag.AlreadyExists = false;

                GoalCatalog _item = GoalCatalog.GetById(id.Value);
                return View(_item);
            }
            else
            {
                ViewBag.AlreadyExists = false;
                GoalCatalog _item = new GoalCatalog();
                if (Request.QueryString.AllKeys.Contains("ParentId"))
                {
                    int _parentId = int.Parse(Request.QueryString["ParentId"]);
                    _item.ParentGoalCatalogId = _parentId;
                }
                return View(_item);

            }

        }
        [HttpPost]
        public ActionResult AddNewGoalCatalog(GoalCatalog model)
        {
            List<GoalCatalog> allGoalCatalog = GoalCatalog.GetAll();
            allGoalCatalog.Insert(0, new GoalCatalog() { Id = -1, TextGoal = "NONE" });
            ViewBag.AllGoals = new SelectList(allGoalCatalog, "Id", "TextGoal");
            ViewBag.AllSubjects = new SelectList(GC_Subjects.GetAll(), "Id", "Name");
            ViewBag.AlreadyExists = false;
            if (ModelState.IsValid)
            {
                if (model.Id != 0)
                {
                    bool IsUpdated = GoalCatalog.Update(model.Id, model.TextGoal);
                    if (IsUpdated)
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    bool isAdded = GoalCatalog.AddNew(model.ParentGoalCatalogId != -1 ? model.ParentGoalCatalogId : null, model.GC_SubjectsId, model.TextGoal, ApplicationHelper.LoggedUserId);
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
            GoalCatalog.SoftDelete(id);
            return RedirectToAction("Index");
        }
    }
}