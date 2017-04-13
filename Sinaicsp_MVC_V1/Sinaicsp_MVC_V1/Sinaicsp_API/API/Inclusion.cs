using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinaicsp_API
{
    public partial class Inclusion
    {
        public static List<Inclusion> GetAll(int studentId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.Inclusions.Where(a => a.StudentId == studentId).ToList();
        }
        public static Inclusion GetById(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.Inclusions.FirstOrDefault(a => a.Id == id);
        }
        public static bool AddNew(int studentId, string subject, string classes, string teacher, string sessionStart, string sessionEnd, int LoggeduserId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            Inclusion _item = new Sinaicsp_API.Inclusion();
            _item.StudentId = studentId;
            _item.Subject = subject;
            _item.Classes = classes;
            _item.Teacher = teacher;
            _item.SessionStart = sessionStart;
            _item.SessionEnd = sessionEnd;
            _item.CreationDate = DateTime.Now;
            _item.CreatedByUserId = LoggeduserId;
            _context.Inclusions.Add(_item);
            _context.SaveChanges();
            return true;
        }
        public static bool Update(int id, string subject, string classes, string teacher, string sessionStart, string sessionEnd, int LoggeduserId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            Inclusion _item = _context.Inclusions.FirstOrDefault(a => a.Id == id);
            _item.Subject = subject;
            _item.Classes = classes;
            _item.Teacher = teacher;
            _item.SessionStart = sessionStart;
            _item.SessionEnd = sessionEnd;
            _context.SaveChanges();
            return true;
        }
        public static void SoftDelete(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            Inclusion _item = _context.Inclusions.FirstOrDefault(a => a.Id == id);
            _item.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}
