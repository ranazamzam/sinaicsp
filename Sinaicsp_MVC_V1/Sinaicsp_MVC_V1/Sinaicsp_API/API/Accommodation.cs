using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinaicsp_API
{
    public partial class Accommodation
    {
        public static List<Accommodation> GetAll(int studentId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.Accommodations.Where(a => a.StudentId == studentId).ToList();
        }

        public static Accommodation GetById(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.Accommodations.FirstOrDefault(a => a.Id == id);
        }
        public static bool AddNew(int studentId, string name, int LoggeduserId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();

            Accommodation _item = new Sinaicsp_API.Accommodation();
            _item.Name = name;
            _item.CreationDate = DateTime.Now;
            _item.StudentId = studentId;
            _item.CreatedByUserId = LoggeduserId;
            _context.Accommodations.Add(_item);
            _context.SaveChanges();
            return true;

        }
        public static bool Update(int id, string name)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            Accommodation itemById = _context.Accommodations.FirstOrDefault(a => a.Id == id);
            Accommodation _item = itemById;
            _item.Name = name;
            _context.SaveChanges();
            return true;

        }
        public static void SoftDelete(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            Accommodation _item = _context.Accommodations.FirstOrDefault(a => a.Id == id);
            _item.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}
