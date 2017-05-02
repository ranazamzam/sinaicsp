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
using Sinaicsp_MVC.ViewModels;

namespace Sinaicsp_MVC.Controllers
{
    public class UserManagementController : Controller
    {
        public ActionResult Roles()
        {
            return View();
        }

        public ActionResult ApplicationRoles_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<ApplicationRole> applicationroles = ApplicationRole.GetAll().AsQueryable();
            DataSourceResult result = applicationroles.ToDataSourceResult(request, applicationRole => new
            {
                Id = applicationRole.Id,
                Name = applicationRole.Name,
                IsDeleted = applicationRole.IsDeleted,
                CreatedOn = applicationRole.CreatedOn,
            });

            return Json(result);
        }

        public ActionResult Login()
        {
            return View(new LoginViewModel());
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            bool isLoggedId = ApplicationHelper.UserLogin(model.UserName, model.Password);
            return View();
        }
    }
}
