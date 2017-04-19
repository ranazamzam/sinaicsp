using Sinaicsp_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sinaicsp_MVC
{
    public class ApplicationHelper
    {
        public static bool IsLoggedIn
        {
            get
            {
                return LoggedUser != null;
            }
        }
        public static string LoggedUserName
        {
            get
            {
                if(IsLoggedIn)
                {
                    return LoggedUser.Email;
                }else
                {
                    return "T-Administartor";
                }
            }
        }
        public static int LoggedUserId
        {
            get
            {
                if (IsLoggedIn)
                {
                    return LoggedUser.Id;
                }
                else
                {
                    return -1;
                }
            }
        }
        public static ApplicationUser LoggedUser
        {
            get
            {
                if (HttpContext.Current.Session["LoggedUser"] != null)
                {
                    return HttpContext.Current.Session["LoggedUser"] as ApplicationUser;
                }
                return null;
            }
        }
        public static bool UserLogin(string userName, string password)
        {
            ApplicationUser _user = ApplicationUser.UserLogin(userName, password);
            if (_user != null)
            {
                HttpContext.Current.Session["LoggedUser"] = _user;
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool Login(int id)
        {
            ApplicationUser _user = ApplicationUser.GetById(id);
            if (_user != null)
            {
                HttpContext.Current.Session["LoggedUser"] = _user;
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool Login(ApplicationUser applicationUser)
        {

            if (applicationUser != null)
            {
                HttpContext.Current.Session["LoggedUser"] = applicationUser;
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool ReloadCurrentUser(int id)
        {
            ApplicationUser _user = ApplicationUser.GetById(id);
            if (_user != null)
            {
                HttpContext.Current.Session["LoggedUser"] = _user;
                return true;
            }
            else
            {
                return false;
            }
        }
        public static void LogOut()
        {

            HttpContext.Current.Session.Clear();
        }

    }
}