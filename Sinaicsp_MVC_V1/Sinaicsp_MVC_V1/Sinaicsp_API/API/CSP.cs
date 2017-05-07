﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinaicsp_API
{
    public partial class CSP
    {
        [Display(Name = "Student")]
        public string StudentName
        {
            get
            {
                return Student.FullName;
            }
        }
        [Display(Name = "School")]
        public string SchoolName
        {
            get
            {
                return Student.School.Name;
            }
        }
        [Display(Name = "Subject")]
        public string SubjectName
        {
            get
            {
                return Subject.Name;
            }
        }
        [Display(Name = "School Year")]
        public string SchoolYearName
        {
            get
            {
                return SchoolYear.Name;
            }
        }
        [Display(Name = "Teacher")]
        public string TeacherNames
        {
            get
            {
                return string.Join(", ", TeacherCSPs.Select(a => a.Teacher.UserName).ToList());
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
        public static List<CSP> GetAll()
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.CSPs.ToList();
        }
        public static CSP GetById(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.CSPs.FirstOrDefault(a => a.Id == id);
        }
        public static bool AddNew(int studentId, int subjectId, int schoolYearId, string matrials, List<int> teacherIds, int LoggeduserId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            CSP _item = new Sinaicsp_API.CSP();
            _item.SubjectId = subjectId;
            _item.StudentId = studentId;
            _item.SchoolYearId= schoolYearId;
            _item.Materials = matrials;
            foreach (int item in teacherIds)
            {
                TeacherCSP _juTable = new TeacherCSP();
                _juTable.TeacherId = item;
                _juTable.CreationDate = DateTime.Now;
                _juTable.CreatedByUserId = LoggeduserId;
                _item.TeacherCSPs.Add(_juTable);
            }
            _item.CreationDate = DateTime.Now;
            _item.CreatedByUserId = LoggeduserId;
            _context.CSPs.Add(_item);
            _context.SaveChanges();
            return true;
        }

        public static void SoftDelete(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            CSP _item = _context.CSPs.FirstOrDefault(a => a.Id == id);
            _item.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}