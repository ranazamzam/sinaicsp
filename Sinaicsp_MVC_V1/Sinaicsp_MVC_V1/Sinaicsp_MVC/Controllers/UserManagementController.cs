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
                CreationDate = applicationRole.CreationDate,
            });

            return Json(result);
        }
    }
}
