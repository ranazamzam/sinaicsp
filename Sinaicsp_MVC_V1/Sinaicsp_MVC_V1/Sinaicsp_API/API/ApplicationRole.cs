using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinaicsp_API
{
    public partial class ApplicationRole
    {
        public string CreatedOn
        {
            get
            {
                return CreationDate.ToShortDateString();
            }
        }
        public static void Init()
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            if (_context.ApplicationRoles.ToList().Count == 0)
            {
                List<string> _roles = Enum.GetNames(typeof(E_Role)).ToList();
                foreach (var item in _roles)
                {
                    ApplicationRole _role = new ApplicationRole();
                    _role.Name = item;
                    _role.CreationDate = DateTime.Now;
                    _context.ApplicationRoles.Add(_role);
                }
                _context.SaveChanges();
            }
        }
        public static List<ApplicationRole> GetAll()
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.ApplicationRoles.ToList();
        }
        public static ApplicationRole GetById(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.ApplicationRoles.FirstOrDefault(a => a.Id == id);
        }
        public static ApplicationRole GetByRole(E_Role role)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.ApplicationRoles.ToList().FirstOrDefault(a => a.Name == role.ToString());
        }
    }
}
