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
    public class ProviderController : Controller
    {
        // GET: Provider
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Providers_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<Provider> providers = Provider.GetAll().AsQueryable();
            DataSourceResult result = providers.ToDataSourceResult(request, item => new
            {
                Id = item.Id,
                Name = item.Name,
                IsDeleted = item.IsDeleted,
                CreatedOn = item.CreatedOn,
                CreatedByUserId = item.CreatedByUserId,
                CreatedByUserName = item.CreatedByUserName
            });

            return Json(result);
        }
        public ActionResult AddNewProvider()
        {
            ViewBag.AlreadyExists = false;
            Provider _item = new Provider();
            return View(_item);
        }
        [HttpPost]
        public ActionResult AddNewProvider(Provider model)
        {
            ViewBag.AlreadyExists = false;
            if (ModelState.IsValid)
            {
                bool isAdded = Provider.AddNew(model.Name, ApplicationHelper.LoggedUserId);
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