﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinaicsp_API
{
    [MetadataType(typeof(ServiceMetadata))]
    public partial class Service
    {
        [Display(Name = "Provider")]
        public string ProviderName
        {
            get
            {
                return this.Provider.Name;
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
        public static List<Service> GetAll()
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.Services.Where(a => a.IsDeleted == false).ToList();
        }
        public static Service GetById(int id)
        {
            SinaicspDataModelContainer _context = new SinaicspDataModelContainer();
            return _context.Services.FirstOrDefault(a => a.Id == id);
        }
        public static void AddNew(int providerId, string name, string model, int numberOfStudent, string sessionLength, string weeklySession, string sessionStart, string sessionEnd, List<int> studentIds, int createdByUserId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            Service _item = new Sinaicsp_API.Service()
            {
                ProviderId = providerId,
                Name = name,
                Model = model,
                NumberOfStudents = numberOfStudent,
                SessionEnd = sessionEnd,
                WeeklySession = weeklySession,
                SessionStart = sessionStart,
                SessionLength = sessionLength,
                CreationDate = DateTime.Now,
                CreatedByUserId = createdByUserId,
                IsDeleted = false,
            };
            foreach (int item in studentIds)
            {
                StudentService _sService = new Sinaicsp_API.StudentService();
                _sService.StudentId = item;
                _sService.CreationDate = DateTime.Now;
                _sService.CreatedByUserId = createdByUserId;
                _item.StudentServices.Add(_sService);
            }
            _context.Services.Add(_item);
            _context.SaveChanges();
        }

        public static void Delete(int id)
        {
            SinaicspDataModelContainer _context = new SinaicspDataModelContainer();
            Service _service = _context.Services.FirstOrDefault(a => a.Id == id);
            List<StudentService> _studentServices = _service.StudentServices.ToList();
            foreach (StudentService item in _studentServices)
            {
                item.IsDeleted = true;
            }
            _service.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}
