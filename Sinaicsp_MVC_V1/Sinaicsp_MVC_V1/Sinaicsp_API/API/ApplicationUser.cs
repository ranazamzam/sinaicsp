using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinaicsp_API
{
    public partial class ApplicationUser
    {
        public List< E_Role> CurrentRoles
        {
            get
            {
                List<E_Role> RetVal = new List<E_Role>();
                foreach (UserRole item in UserRoles)
                {
                    RetVal.Add((E_Role)Enum.Parse(typeof(E_Role), item.ApplicationRole.Name));
                }
                return RetVal;
            }
        }
        public string Roles
        {
            get
            {
                return string.Join(",", UserRoles.Select(a => a.ApplicationRole.Name));
            }
        }
        public string CreatedOn
        {
            get
            {
                return CreationDate.ToShortDateString();
            }
        }
        public static void Init()
        {
            SinaicspDataModelContainer _context = new SinaicspDataModelContainer();
            if (_context.ApplicationUsers.ToList().Count == 0)
            {
                AddNew("admin", "admin", "admin", E_Role.Admin);
            }
        }
        public static List<ApplicationUser> GetAll()
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.ApplicationUsers.ToList();
        }
        public static ApplicationUser GetById(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.ApplicationUsers.FirstOrDefault(a => a.Id == id);
        }
        public static ApplicationUser GetByEmail(string email)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.ApplicationUsers.FirstOrDefault(a => a.Email == email);
        }
        public static ApplicationUser Login(string email, string password)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            ApplicationUser _usr = _context.ApplicationUsers.FirstOrDefault(a => a.Email == email && a.Password == password);
            if (_usr != null)
            {
                _usr.LastLogin = DateTime.Now;
                _context.SaveChanges();
                return _usr;
            }
            return null;
        }
        public static bool AddNew(string email, string userName, string password, E_Role role)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            if (_context.ApplicationUsers.ToList().Where(a => a.Email.ToLower() == email.ToLower()).Count() == 0 && _context.ApplicationUsers.ToList().Where(a => a.UserName.ToLower() == userName.ToLower()).Count() == 0)
            {
                ApplicationUser _item = new Sinaicsp_API.ApplicationUser();
                _item.Email = email;
                _item.UserName = userName;
                _item.Password = password;
                _item.CreationDate = DateTime.Now;
                ApplicationRole _currentRole = ApplicationRole.GetByRole(role);
                UserRole _userRole = new Sinaicsp_API.UserRole()
                {
                    ApplicationUser = _item,
                    ApplicationRoleId = _currentRole.Id
                };
                _item.UserRoles.Add(_userRole);
                _context.ApplicationUsers.Add(_item);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public static void SoftDelete(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            ApplicationUser _usr = _context.ApplicationUsers.FirstOrDefault(a => a.Id == id);
            _usr.IsDeleted = true;
            _context.SaveChanges();
        }
        public static ApplicationUser UserLogin(string userName, string password)
        {
            SinaicspDataModelContainer _context = new SinaicspDataModelContainer();
            ApplicationUser _item = _context.ApplicationUsers.ToList().FirstOrDefault(a => a.UserName == userName && a.Password == password && a.IsDeleted == false);
            return _item;
        }
    }
}
