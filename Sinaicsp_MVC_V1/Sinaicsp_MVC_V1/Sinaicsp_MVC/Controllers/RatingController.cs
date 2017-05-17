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
    public class RatingController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Ratings_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<Rating> items = Rating.GetAll().AsQueryable();
            DataSourceResult result = items.ToDataSourceResult(request, _currItem => new
            {
                Id = _currItem.Id,
                Description = _currItem.Description,
                RateValue = _currItem.RateValue,
                CreatedOn = _currItem.CreatedOn,
                CreatedByUserId = _currItem.CreatedByUserId,
                CreatedByUserName = _currItem.CreatedByUserName
            });

            return Json(result);
        }
        public ActionResult AddNewRating(int? id)
        {
            if (id != null)
            {
                ViewBag.AlreadyExists = false;

                Rating _item = Rating.GetById(id.Value);
                return View(_item);
            }
            else
            {
                ViewBag.AlreadyExists = false;
                Rating _item = new Rating();
                return View(_item);

            }

        }
        [HttpPost]
        public ActionResult AddNewRating(Rating model)
        {
            ViewBag.AlreadyExists = false;
            if (ModelState.IsValid)
            {
                if (model.Id != 0)
                {
                    bool IsUpdated = Rating.Update(model.Id,model.RateValue,model.Description);
                    if (IsUpdated)
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    bool isAdded = Rating.AddNew( model.RateValue, model.Description, ApplicationHelper.LoggedUserId);
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
            Rating.SoftDelete(id);
            return RedirectToAction("Index");
        }
    }
}