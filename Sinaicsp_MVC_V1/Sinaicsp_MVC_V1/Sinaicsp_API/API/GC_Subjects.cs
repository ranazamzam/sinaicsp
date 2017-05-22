using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinaicsp_API
{
   public partial class GC_Subjects
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
        public static List<GC_Subjects> GetAll()
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.GC_Subjects.Where(a => a.IsDeleted == false).ToList();
        }
        public static GC_Subjects GetById(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.GC_Subjects.FirstOrDefault(a => a.Id == id);
        }
        public static GC_Subjects GetByName(string name)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.GC_Subjects.FirstOrDefault(a => a.Name == name);
        }
        public static bool AddNew(int gc_CategoryId, string name, int LoggeduserId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            GC_Subjects _item = new Sinaicsp_API.GC_Subjects();
            _item.Name = name;
            _item.GC_CategoryId = gc_CategoryId;
            _item.CreationDate = DateTime.Now;
            _item.CreatedByUserId = LoggeduserId;
            _context.GC_Subjects.Add(_item);
            _context.SaveChanges();
            return true;

        }
        public static bool Update(int id, string name)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            GC_Subjects itemById = _context.GC_Subjects.FirstOrDefault(a => a.Id == id);
            GC_Subjects _item = itemById;
            _item.Name = name;
            _context.SaveChanges();
            return true;
        }
        public static void SoftDelete(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            GC_Subjects _item = _context.GC_Subjects.FirstOrDefault(a => a.Id == id);
            _item.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}
