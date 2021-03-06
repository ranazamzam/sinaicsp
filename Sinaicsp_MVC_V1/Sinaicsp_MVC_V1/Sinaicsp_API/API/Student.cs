﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinaicsp_API
{
    [MetadataType(typeof(StudentMetadata))]

    public partial class Student
    {
        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }
        [Display(Name = "School")]
        public string SchoolName
        {
            get
            {
                return School.Name;
            }
        }
        [Display(Name = "Grade")]
        public string GradeName
        {
            get
            {
                return StudentGrade.Name;
            }
        }
        [Display(Name = "Class")]
        public string ClassName
        {
            get
            {
                return StudentClass.Name;
            }
        }
        [Display(Name = "City")]
        public string CityName
        {
            get
            {
                return City.Name;
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
        public static List<Student> GetAll()
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.Students.Where(a => a.IsDeleted == false).ToList();
        }
        public static List<Student> GetAll(int schoolId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.Students.Where(a => a.SchoolId == schoolId && a.IsDeleted==false).ToList();
        }
        public static Student GetById(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.Students.FirstOrDefault(a => a.Id == id);
        }
        public static bool AddNew(int schoolId, int studentClassId, int StudentGradeId, int cityId, string firstName, string lastName, string gender, DateTime DOB,
            string primaryParent, string address, string state, string motherCell, string SecondaryParent, string phone, string fatherCell, int LoggeduserId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            Student _item = new Sinaicsp_API.Student()
            {
                SchoolId = schoolId,
                StudentClassId = studentClassId,
                StudentGradeId = StudentGradeId,
                CityId = cityId,
                FirstName = firstName,
                LastName = lastName,
                Gender = gender,
                DOB = DOB,
                PrimaryParent = primaryParent,
                Address = address,
                State = state,
                MotherCell = motherCell,
                SecondaryParent = SecondaryParent,
                Phone = phone,
                FatherCell = fatherCell,
                CreatedByUserId = LoggeduserId,
                CreationDate = DateTime.Now
            };
            _context.Students.Add(_item);
            _context.SaveChanges();
            return true;
        }
        public static bool Update(int id, string firstName, string lastName, string gender, DateTime DOB,
            string primaryParent, string address, string state, string motherCell, string SecondaryParent, string phone, string fatherCell)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            Student _item = _context.Students.FirstOrDefault(a => a.Id == id);
            _item.FirstName = firstName;
            _item.LastName = lastName;
            _item.Gender = gender;
            _item.DOB = DOB;
            _item.PrimaryParent = primaryParent;
            _item.Address = address;
            _item.State = state;
            _item.MotherCell = motherCell;
            _item.SecondaryParent = SecondaryParent;
            _item.Phone = phone;
            _item.FatherCell = fatherCell;
            _context.SaveChanges();
            return true;

        }
        public static void SoftDelete(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            Student _item = _context.Students.FirstOrDefault(a => a.Id == id);
            _item.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}
