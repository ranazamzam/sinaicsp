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
    public class CityController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Cities_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<City> cities = City.GetAll().AsQueryable();
            DataSourceResult result = cities.ToDataSourceResult(request, city => new
            {
                Id = city.Id,
                Name = city.Name,
                IsDeleted = city.IsDeleted,
                CreatedOn = city.CreatedOn,
                CreatedByUserId = city.CreatedByUserId,
                CreatedByUserName = city.CreatedByUserName
            });

            return Json(result);
        }
        public ActionResult AddNewCity()
        {
            ViewBag.AlreadyExists = false;
            City _item = new City();
            return View(_item);
        }
        [HttpPost]
        public ActionResult AddNewCity(City model)
        {
            ViewBag.AlreadyExists = false;
            if (ModelState.IsValid)
            {
                bool isAdded = City.AddNew(model.Name, ApplicationHelper.LoggedUserId);
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