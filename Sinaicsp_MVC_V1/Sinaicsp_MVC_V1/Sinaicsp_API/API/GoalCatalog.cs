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
                return GC_Subjects.Name;
            }
        }
        public string CategoryName
        {
            get
            {
                return GC_Subjects.GC_Category.Name;
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
        public static List<GoalCatalog> GetAllParents()
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.GoalCatalogs.Where(a => a.IsDeleted == false && a.ParentGoalCatalogId == null).ToList();
        }
        public static GoalCatalog GetById(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.GoalCatalogs.FirstOrDefault(a => a.Id == id);
        }
        public static bool AddNew(int? parentId, int GCsubjectId, string textGoal, int LoggeduserId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();

            GoalCatalog _item = new Sinaicsp_API.GoalCatalog();
            _item.TextGoal = textGoal;
            _item.GC_SubjectsId = GCsubjectId;
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
        public static GC_Subjects SoftDelete(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            GoalCatalog _item = _context.GoalCatalogs.FirstOrDefault(a => a.Id == id);
            _item.IsDeleted = true;
            _context.SaveChanges();
            return _item.GC_Subjects;
        }

        public static GC_Subjects MoveUp(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            GoalCatalog _goalItem = _context.GoalCatalogs.Where(a => a.Id == id).FirstOrDefault();
            GC_Subjects _item = _goalItem.GC_Subjects;
            if (_goalItem.ParentGoalCatalogId == null)
            {
                List<GoalCatalog> targetParentList = _item.GoalCatalogs.Where(a => a.ParentGoalCatalogId == null).OrderBy(a => a.TextOrder).ToList();
                int index = targetParentList.IndexOf(_goalItem);
                if (index > 0)
                {
                    GoalCatalog _goalAbove = targetParentList[index - 1];
                    int NeworderSwap = _goalAbove.TextOrder;
                    _goalAbove.TextOrder = _goalItem.TextOrder;
                    _goalItem.TextOrder = NeworderSwap;
                    foreach (GoalCatalog item in _goalItem.GoalCatalogs)
                    {
                        item.TextOrder = _goalItem.TextOrder;
                    }
                    foreach (GoalCatalog item in _goalAbove.GoalCatalogs)
                    {
                        item.TextOrder = _goalAbove.TextOrder;
                    }
                }
            }
            else
            {
                GoalCatalog _parent = _goalItem.ParentGoalCatalog;
                List<GoalCatalog> targetchildList = _parent.GoalCatalogs.OrderBy(a => a.SubTextOrder).ToList();

                int index = targetchildList.IndexOf(_goalItem);
                if (index > 0)
                {
                    GoalCatalog _goalAbove = targetchildList[index - 1];
                    int NeworderSwap = _goalAbove.SubTextOrder;
                    _goalAbove.SubTextOrder = _goalItem.SubTextOrder;
                    _goalItem.SubTextOrder = NeworderSwap;
                }
            }
            _context.SaveChanges();
            return _item;
        }
        public static GC_Subjects MoveDown(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            GoalCatalog _goalItem = _context.GoalCatalogs.Where(a => a.Id == id).FirstOrDefault();
            GC_Subjects _item = _goalItem.GC_Subjects;
            if (_goalItem.ParentGoalCatalogId == null)
            {
                List<GoalCatalog> targetParentList = _item.GoalCatalogs.Where(a => a.ParentGoalCatalogId == null).OrderByDescending(a => a.TextOrder).ToList();
                int index = targetParentList.IndexOf(_goalItem);
                if (index > 0)
                {
                    GoalCatalog _goalAbove = targetParentList[index - 1];
                    int NeworderSwap = _goalAbove.TextOrder;
                    _goalAbove.TextOrder = _goalItem.TextOrder;
                    _goalItem.TextOrder = NeworderSwap;
                    foreach (GoalCatalog item in _goalItem.GoalCatalogs)
                    {
                        item.TextOrder = _goalItem.TextOrder;
                    }
                    foreach (GoalCatalog item in _goalAbove.GoalCatalogs)
                    {
                        item.TextOrder = _goalAbove.TextOrder;
                    }
                }
            }
            else
            {
                GoalCatalog _parent = _goalItem.ParentGoalCatalog;
                List<GoalCatalog> targetchildList = _parent.GoalCatalogs.OrderByDescending(a => a.SubTextOrder).ToList();

                int index = targetchildList.IndexOf(_goalItem);
                if (index > 0)
                {
                    GoalCatalog _goalAbove = targetchildList[index - 1];
                    int NeworderSwap = _goalAbove.SubTextOrder;
                    _goalAbove.SubTextOrder = _goalItem.SubTextOrder;
                    _goalItem.SubTextOrder = NeworderSwap;
                }
            }
            _context.SaveChanges();
            return _item;
        }
    }
}
