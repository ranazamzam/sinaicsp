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
            _item.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}
