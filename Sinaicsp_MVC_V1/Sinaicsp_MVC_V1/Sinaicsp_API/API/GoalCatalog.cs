using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinaicsp_API
{
    [MetadataType(typeof(GoalCatalogMetadata))]
    public partial class GoalCatalog
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
        [Display(Name = "Subject Name")]
        public string SubjectName
        {
            get
            {
                return Subject.Name;
            }
        }
        [Display(Name = "Parent")]
        public string ParentName
        {
            get
            {
                return ParentGoalCatalog != null ? ParentGoalCatalog.TextGoal : "None";
            }
        }
        public string CreatedOn
        {
            get
            {
                return CreationDate.ToShortDateString();
            }
        }
        public static List<GoalCatalog> GetAll()
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.GoalCatalogs.Where(a => a.IsDeleted == false).ToList();
        }
        public static GoalCatalog GetById(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.GoalCatalogs.FirstOrDefault(a => a.Id == id);
        }
        public static bool AddNew(int? parentId, int subjectId, string textGoal, int LoggeduserId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();

            GoalCatalog _item = new Sinaicsp_API.GoalCatalog();
            _item.TextGoal = textGoal;
            _item.SubjectId = subjectId;
            _item.ParentGoalCatalogId = parentId;
            _item.CreationDate = DateTime.Now;
            _item.CreatedByUserId = LoggeduserId;
            _context.GoalCatalogs.Add(_item);
            _context.SaveChanges();
            return true;


        }
        public static bool Update(int id, string textGoal)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            GoalCatalog itemById = _context.GoalCatalogs.FirstOrDefault(a => a.Id == id);
            GoalCatalog _item = itemById;
            _item.TextGoal = textGoal;
            _context.SaveChanges();
            return true;
        }
        public static void SoftDelete(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            GoalCatalog _item = _context.GoalCatalogs.FirstOrDefault(a => a.Id == id);
            _item.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}
