using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinaicsp_API
{
    [MetadataType(typeof(SchoolMetadata))]
    public partial class School
    {

        [Display(Name ="Created By")]
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
        public static List<School> GetAll()
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.Schools.Where(a => a.IsDeleted == false).ToList();
        }
        public static List<School> GetAll(int userId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            string email = ApplicationUser.GetById(userId).Email;
            return new List<School>() { _context.Teachers.Where(a => a.IsDeleted == false).ToList().FirstOrDefault(a => a.Id == Teacher.GetByEmail(email).Id).School };
        }
        public static School GetById(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.Schools.FirstOrDefault(a => a.Id == id);
        }
        public static School GetByName(string schoolName)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.Schools.FirstOrDefault(a => a.Name== schoolName);
        }
        public static bool AddNew(string name,int LoggeduserId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            if (_context.Schools.ToList().Where(a => a.Name.ToLower() == name.ToLower()).Count() == 0)
            {
                School _item = new Sinaicsp_API.School();
                _item.Name = name;
                _item.CreationDate = DateTime.Now;
                _item.CreatedByUserId = LoggeduserId;
                _context.Schools.Add(_item);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public static bool Update(int id, string name)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            School itemById = _context.Schools.FirstOrDefault(a => a.Id == id);
            School itemByName = _context.Schools.ToList().FirstOrDefault(a => a.Name.ToLower() == name.ToLower());

            if (itemByName == null || itemByName.Id == itemById.Id)
            {
                School _item = itemById;
                _item.Name = name;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public static void SoftDelete(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            School _item = _context.Schools.FirstOrDefault(a => a.Id == id);
            _item.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}
