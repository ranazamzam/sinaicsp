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
            ViewBag.InValid = false;
            return View(new LoginViewModel());
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            ViewBag.InValid = false;
            bool isLoggedId = ApplicationHelper.UserLogin(model.UserName, model.Password);
            if (!isLoggedId)
            {
                ViewBag.InValid = true;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Users()
        {
            return View();
        }
        public ActionResult ApplicationUsers_Read([DataSourceRequest]DataSourceRequest request)
        {
            IQueryable<ApplicationUser> _items = ApplicationUser.GetAll().AsQueryable();
            DataSourceResult result = _items.ToDataSourceResult(request, CurrentItem => new
            {
                Id = CurrentItem.Id,
                UserName = CurrentItem.UserName,
                Email = CurrentItem.Email,
                Roles = CurrentItem.Roles,
                CreatedOn = CurrentItem.CreatedOn
            });

            return Json(result);
        }
        public ActionResult AddNewUser()
        {
            ViewBag.AlreadyExists = false;
            ViewBag.AllRoles = new SelectList(ApplicationRole.GetAll().Where(a => a.Name == E_Role.Admin.ToString()), "Id", "Name");
            ApplicationUser _item = new ApplicationUser();
            return View(_item);
        }
        [HttpPost]
        public JsonResult SaveNewUser(ApplicationUser model, int roleId)
        {
            E_Role currentRole = (E_Role)Enum.Parse(typeof(E_Role), ApplicationRole.GetById(roleId).Name);
            bool isAdded = ApplicationUser.AddNew(model.Email, model.UserName, model.Password, currentRole);
            if (isAdded)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);

        }

        public ActionResult LogOut()
        {
            ApplicationHelper.LogOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
