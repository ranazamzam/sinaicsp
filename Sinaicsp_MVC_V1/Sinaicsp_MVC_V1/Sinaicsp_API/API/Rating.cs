using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinaicsp_API
{
    [MetadataType(typeof(RatingMetadata))]
    public partial class Rating
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
        public string SchoolName
        {
            get
            {
                return School != null ? School.Name : string.Empty;
            }
        }
        public static List<Rating> GetAll()
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.Ratings.Where(a => a.IsDeleted == false).ToList();
        }
        public static Rating GetById(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.Ratings.FirstOrDefault(a => a.Id == id);
        }
        public static bool AddNew(int schoolId,string rateValue, string description, int LoggeduserId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();

            Rating _item = new Sinaicsp_API.Rating();
            _item.SchoolId = schoolId;
            _item.RateValue = rateValue;
            _item.Description = description;
            _item.CreationDate = DateTime.Now;
            _item.CreatedByUserId = LoggeduserId;
            _context.Ratings.Add(_item);
            _context.SaveChanges();
            return true;


        }
        public static bool Update(int id,int schoolId, string rateValue, string description)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            Rating itemById = _context.Ratings.FirstOrDefault(a => a.Id == id);
            Rating _item = itemById;
            _item.SchoolId = schoolId;
            _item.RateValue = rateValue;
            _item.Description = description;
            _context.SaveChanges();
            return true;
        }
        public static void SoftDelete(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            Rating _item = _context.Ratings.FirstOrDefault(a => a.Id == id);
            _item.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}
