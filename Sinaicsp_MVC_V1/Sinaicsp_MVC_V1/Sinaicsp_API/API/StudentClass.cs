using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinaicsp_API
{
    [MetadataType(typeof(StudentClassMetadata))]

    public partial class StudentClass
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
        public static List<StudentClass> GetAll()
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.StudentClasses.Where(a => a.IsDeleted == false).ToList();
        }
        public static StudentClass GetById(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.StudentClasses.FirstOrDefault(a => a.Id == id);
        }
        public static StudentClass GetByName(string className)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.StudentClasses.FirstOrDefault(a => a.Name == className);
        }
        public static bool AddNew(string name, int LoggeduserId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            if (_context.StudentClasses.ToList().Where(a => a.Name.ToLower() == name.ToLower()).Count() == 0)
            {
                StudentClass _item = new Sinaicsp_API.StudentClass();
                _item.Name = name;
                _item.CreationDate = DateTime.Now;
                _item.CreatedByUserId = LoggeduserId;
                _context.StudentClasses.Add(_item);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public static bool Update(int id, string name)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            StudentClass itemById = _context.StudentClasses.FirstOrDefault(a => a.Id == id);
            StudentClass itemByName = _context.StudentClasses.ToList().FirstOrDefault(a => a.Name.ToLower() == name.ToLower());

            if (itemByName == null || itemByName.Id == itemById.Id)
            {
                StudentClass _item = itemById;
                _item.Name = name;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public static void SoftDelete(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            StudentClass _item = _context.StudentClasses.FirstOrDefault(a => a.Id == id);
            _item.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}
