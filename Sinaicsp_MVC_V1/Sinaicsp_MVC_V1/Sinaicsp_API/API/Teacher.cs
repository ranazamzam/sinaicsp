using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinaicsp_API
{
    [MetadataType(typeof(TeacherMetadata))]
    public partial class Teacher
    {
        [Display(Name = "School")]
        public string SchoolName
        {
            get
            {
                return this.School.Name;
            }
        }
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
        public static List<Teacher> GetAll()
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.Teachers.ToList();
        }
        public static Teacher GetById(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.Teachers.FirstOrDefault(a => a.Id == id);
        }
        public static bool AddNew(int schoolId, string userName, string email, string title, int LoggeduserId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            if (_context.Teachers.ToList().Where(a => a.Email.ToLower() == email.ToLower()).Count() == 0)
            {
                Teacher _item = new Sinaicsp_API.Teacher();
                _item.SchoolId = schoolId;
                _item.Email = email;
                _item.UserName = userName;
                _item.Title = title;
                _item.CreationDate = DateTime.Now;
                _item.CreatedByUserId = LoggeduserId;
                _context.Teachers.Add(_item);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public static bool Update(int id, string userName, string email, string title)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            Teacher itemById = _context.Teachers.FirstOrDefault(a => a.Id == id);
            Teacher itemByName = _context.Teachers.ToList().FirstOrDefault(a => a.Email.ToLower() == email.ToLower());

            if (itemByName == null || itemByName.Id == itemById.Id)
            {
                Teacher _item = itemById;
                _item.Email = email;
                _item.UserName = userName;
                _item.Title = title;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public static void SoftDelete(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            Teacher _item = _context.Teachers.FirstOrDefault(a => a.Id == id);
            _item.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}
