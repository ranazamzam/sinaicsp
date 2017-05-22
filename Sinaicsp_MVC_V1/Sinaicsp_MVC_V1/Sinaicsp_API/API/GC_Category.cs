using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinaicsp_API
{
    public partial class GC_Category
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
        public static List<GC_Category> GetAll()
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.GC_Category.Where(a => a.IsDeleted == false).ToList();
        }
        public static GC_Category GetById(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.GC_Category.FirstOrDefault(a => a.Id == id);
        }
        public static GC_Category GetByName(string name)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.GC_Category.FirstOrDefault(a => a.Name == name);
        }
        public static bool AddNew(string name, int LoggeduserId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            GC_Category _item = new Sinaicsp_API.GC_Category();
            _item.Name = name;
            _item.CreationDate = DateTime.Now;
            _item.CreatedByUserId = LoggeduserId;
            _context.GC_Category.Add(_item);
            _context.SaveChanges();
            return true;

        }
        public static bool Update(int id, string name)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            GC_Category itemById = _context.GC_Category.FirstOrDefault(a => a.Id == id);
            GC_Category _item = itemById;
            _item.Name = name;
            _context.SaveChanges();
            return true;
        }
        public static void SoftDelete(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            GC_Category _item = _context.GC_Category.FirstOrDefault(a => a.Id == id);
            _item.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}
