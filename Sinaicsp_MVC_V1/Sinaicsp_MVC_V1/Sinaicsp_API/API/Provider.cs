using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinaicsp_API
{
    [MetadataType(typeof(ProviderMetadata))]
    public partial class Provider
    {

        [Display(Name = "Created By")]
        public string CreatedByUserName
        {
            get
            {
                if (CreatedByUserId > 0)
                {
                    return ApplicationUser.GetById(CreatedByUserId).UserName;
                }
                else
                {
                    return "T-Administartor";
                }
            }
        }
        public string CreatedOn
        {
            get
            {
                return CreationDate.ToShortDateString();
            }
        }
        public static List<Provider> GetAll()
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.Providers.Where(a => a.IsDeleted == false).ToList();
        }
        public static Provider GetById(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.Providers.FirstOrDefault(a => a.Id == id);
        }
        public static Provider GetByName(string name)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.Providers.FirstOrDefault(a => a.Name == name);
        }
        public static bool AddNew(string name, int LoggeduserId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            if (_context.Providers.ToList().Where(a => a.Name.ToLower() == name.ToLower()).Count() == 0)
            {
                Provider _item = new Sinaicsp_API.Provider();
                _item.Name = name;
                _item.CreationDate = DateTime.Now;
                _item.CreatedByUserId = LoggeduserId;
                _context.Providers.Add(_item);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public static bool Update(int id, string name)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            Provider itemById = _context.Providers.FirstOrDefault(a => a.Id == id);
            Provider itemByName = _context.Providers.ToList().FirstOrDefault(a => a.Name.ToLower() == name.ToLower());

            if (itemByName == null || itemByName.Id == itemById.Id)
            {
                Provider _item = itemById;
                _item.Name = name;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public static void SoftDelete(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            Provider _item = _context.Providers.FirstOrDefault(a => a.Id == id);
            _item.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}
