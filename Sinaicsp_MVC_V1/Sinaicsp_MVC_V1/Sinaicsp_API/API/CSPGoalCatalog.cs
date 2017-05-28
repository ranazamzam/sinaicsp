using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinaicsp_API
{
    public partial class CSPGoalCatalog
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
        public string ParentName
        {
            get
            {
                return ParentCSPGoalCatalog != null ? ParentCSPGoalCatalog.TextGoal : "NONE";
            }
        }
        public string CreatedOn
        {
            get
            {
                return CreationDate.ToShortDateString();
            }
        }
        public static List<CSPGoalCatalog> GetAll()
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.CSPGoalCatalogs.Where(a => a.IsDeleted == false).ToList();
        }
        public static List<CSPGoalCatalog> GetAll(int CSPId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.CSPGoalCatalogs.Where(a => a.IsDeleted == false && a.CSPId == CSPId).ToList();
        }
        public static CSPGoalCatalog GetById(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.CSPGoalCatalogs.FirstOrDefault(a => a.Id == id);
        }
        public static bool AddNew(int? parentId, int CSPId, string dateInitiated, string rate1, string rate2, string rate3, string textGoal, int LoggeduserId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            CSPGoalCatalog _item = new Sinaicsp_API.CSPGoalCatalog();

            CSP _cspitem = _context.CSPs.FirstOrDefault(a => a.Id == CSPId);
            if (parentId == null)
            {
                _item.TextOrder = _cspitem.CSPGoalCatalogs.Count > 0 ? _cspitem.CSPGoalCatalogs.Select(a => a.TextOrder).Max() + 1 : 0;
                _item.SubTextOrder = 0;
            }
            else
            {
                _item.TextOrder = _cspitem.CSPGoalCatalogs.FirstOrDefault(a => a.Id == parentId.Value).TextOrder;
                _item.SubTextOrder = _cspitem.CSPGoalCatalogs.Where(a => a.ParentCSPGoalCatalogId == parentId).Count() > 0 ? _cspitem.CSPGoalCatalogs.Where(a => a.ParentCSPGoalCatalogId == parentId).Select(a => a.SubTextOrder).Max() + 1 : 0;
            }
            _item.ParentCSPGoalCatalogId = parentId;
            _item.TextGoal = textGoal;
            _item.CSPId = CSPId;
            _item.DateInitiated = dateInitiated;
            _item.Rate1 = rate1;
            _item.Rate2 = rate2;
            _item.Rate3 = rate3;
            _item.CreationDate = DateTime.Now;
            _item.CreatedByUserId = LoggeduserId;
            _context.CSPGoalCatalogs.Add(_item);
            _context.SaveChanges();
            return true;

        }
        public static bool Update(int id, string dateInitiated, string rate1, string rate2, string rate3, string textGoal)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            CSPGoalCatalog itemById = _context.CSPGoalCatalogs.FirstOrDefault(a => a.Id == id);
            CSPGoalCatalog _item = itemById;
            _item.DateInitiated = dateInitiated;
            _item.Rate1 = rate1;
            _item.Rate2 = rate2;
            _item.Rate3 = rate3;
            _item.TextGoal = textGoal;
            _context.SaveChanges();
            return true;

        }
        public static void SoftDelete(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            CSPGoalCatalog _item = _context.CSPGoalCatalogs.FirstOrDefault(a => a.Id == id);
            if (_item.CSPGoalCatalogs.Count() > 0)
            {
                foreach (CSPGoalCatalog Chitem in _item.CSPGoalCatalogs)
                {
                    Chitem.IsDeleted = true;
                }
            }
            _item.IsDeleted = true;
            _context.SaveChanges();
        }

        public static CSP MoveUp(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            CSPGoalCatalog _goalItem = _context.CSPGoalCatalogs.Where(a => a.Id == id).FirstOrDefault();
            CSP _item = _goalItem.CSP;
            if (_goalItem.ParentCSPGoalCatalogId == null)
            {
                List<CSPGoalCatalog> targetParentList = _item.CSPGoalCatalogs.Where(a => a.ParentCSPGoalCatalogId == null).OrderBy(a => a.TextOrder).ToList();
                int index = targetParentList.IndexOf(_goalItem);
                if (index > 0)
                {
                    CSPGoalCatalog _goalAbove = targetParentList[index - 1];
                    int NeworderSwap = _goalAbove.TextOrder;
                    _goalAbove.TextOrder = _goalItem.TextOrder;
                    _goalItem.TextOrder = NeworderSwap;
                    foreach (CSPGoalCatalog item in _goalItem.CSPGoalCatalogs)
                    {
                        item.TextOrder = _goalItem.TextOrder;
                    }
                    foreach (CSPGoalCatalog item in _goalAbove.CSPGoalCatalogs)
                    {
                        item.TextOrder = _goalAbove.TextOrder;
                    }
                }
            }
            else
            {
                CSPGoalCatalog _parent = _goalItem.ParentCSPGoalCatalog;
                List<CSPGoalCatalog> targetchildList = _parent.CSPGoalCatalogs.OrderBy(a => a.SubTextOrder).ToList();

                int index = targetchildList.IndexOf(_goalItem);
                if (index > 0)
                {
                    CSPGoalCatalog _goalAbove = targetchildList[index - 1];
                    int NeworderSwap = _goalAbove.SubTextOrder;
                    _goalAbove.SubTextOrder = _goalItem.SubTextOrder;
                    _goalItem.SubTextOrder = NeworderSwap;
                }
            }
            _context.SaveChanges();
            return _item;
        }
        public static CSP MoveDown(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            CSPGoalCatalog _goalItem = _context.CSPGoalCatalogs.Where(a => a.Id == id).FirstOrDefault();
            CSP _item = _goalItem.CSP;
            if (_goalItem.ParentCSPGoalCatalogId == null)
            {
                List<CSPGoalCatalog> targetParentList = _item.CSPGoalCatalogs.Where(a => a.ParentCSPGoalCatalogId == null).OrderByDescending(a => a.TextOrder).ToList();
                int index = targetParentList.IndexOf(_goalItem);
                if (index > 0)
                {
                    CSPGoalCatalog _goalAbove = targetParentList[index - 1];
                    int NeworderSwap = _goalAbove.TextOrder;
                    _goalAbove.TextOrder = _goalItem.TextOrder;
                    _goalItem.TextOrder = NeworderSwap;
                    foreach (CSPGoalCatalog item in _goalItem.CSPGoalCatalogs)
                    {
                        item.TextOrder = _goalItem.TextOrder;
                    }
                    foreach (CSPGoalCatalog item in _goalAbove.CSPGoalCatalogs)
                    {
                        item.TextOrder = _goalAbove.TextOrder;
                    }
                }
            }
            else
            {
                CSPGoalCatalog _parent = _goalItem.ParentCSPGoalCatalog;
                List<CSPGoalCatalog> targetchildList = _parent.CSPGoalCatalogs.OrderByDescending(a => a.SubTextOrder).ToList();

                int index = targetchildList.IndexOf(_goalItem);
                if (index > 0)
                {
                    CSPGoalCatalog _goalAbove = targetchildList[index - 1];
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
