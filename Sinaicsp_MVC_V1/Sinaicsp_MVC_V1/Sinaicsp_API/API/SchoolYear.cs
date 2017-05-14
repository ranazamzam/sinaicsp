using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinaicsp_API
{
    [MetadataType(typeof(SchoolYearMetadata))]
    public partial class SchoolYear
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
        public static List<SchoolYear> GetAll()
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.SchoolYears.Where(a => a.IsDeleted == false).ToList();
        }
        public static List<SchoolYear> GetCurrent()
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.SchoolYears.Where(a=>a.IsCurrent).ToList();
        }
        public static SchoolYear GetById(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.SchoolYears.FirstOrDefault(a => a.Id == id);
        }
        public static bool AddNew(string name, int LoggeduserId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            if (_context.SchoolYears.ToList().Where(a => a.Name.ToLower() == name.ToLower()).Count() == 0)
            {
                SchoolYear _item = new Sinaicsp_API.SchoolYear();
                _item.Name = name;
                _item.CreationDate = DateTime.Now;
                _item.CreatedByUserId = LoggeduserId;
                _context.SchoolYears.Add(_item);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public static bool Update(int id, string name)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            SchoolYear itemById = _context.SchoolYears.FirstOrDefault(a => a.Id == id);
            SchoolYear itemByName = _context.SchoolYears.ToList().FirstOrDefault(a => a.Name.ToLower() == name.ToLower());
            if (itemByName == null || itemByName.Id == itemById.Id)
            {
                SchoolYear _item = itemById;
                _item.Name = name;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public static void SoftDelete(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            SchoolYear _item = _context.SchoolYears.FirstOrDefault(a => a.Id == id);
            _item.IsDeleted = true;
            _context.SaveChanges();
        }
        public static void SetCurrent(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            List<SchoolYear> allItems = _context.SchoolYears.ToList();
            foreach (SchoolYear item in allItems)
            {
                if(item.Id==id)
                {
                    item.IsCurrent = true;
                }else
                {
                    item.IsCurrent = false;
                }
            }
            _context.SaveChanges();
        }
    }
}
