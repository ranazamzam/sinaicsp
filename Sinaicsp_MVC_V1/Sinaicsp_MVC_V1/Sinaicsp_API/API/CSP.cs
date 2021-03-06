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
        public string StudentClass
        {
            get
            {
                return Student.ClassName;
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
            return _context.CSPs.Where(a => a.IsDeleted == false).ToList();
        }
        public static List<CSP> GetAll(int studentId, int teacherId, int schooldId, int schoolYearId, int subjectId, int classId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            List<CSP> allItems = _context.CSPs.Where(a => a.IsDeleted == false).ToList();
            allItems = studentId != -1 ? allItems.Where(a => a.StudentId == studentId).ToList() : allItems;
            allItems = schoolYearId != -1 ? allItems.Where(a => a.SchoolYearId == schoolYearId).ToList() : allItems;
            allItems = subjectId != -1 ? allItems.Where(a => a.SubjectId == subjectId).ToList() : allItems;
            allItems = schooldId != -1 ? allItems.Where(a => a.Student.SchoolId == schooldId).ToList() : allItems;
            allItems = classId != -1 ? allItems.Where(a => a.Student.StudentClassId == classId).ToList() : allItems;
            allItems = teacherId != -1 ? allItems.Where(a => a.TeacherCSPs.Where(x => x.TeacherId == teacherId).Count() > 0).ToList() : allItems;

            return allItems;
        }
        public static CSP GetById(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.CSPs.FirstOrDefault(a => a.Id == id);
        }
        public static int GetNextCSP(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            List<CSP> allStudentCSPs = _context.CSPs.Where(a => a.Id == id).FirstOrDefault().Student.CSPs.ToList();
            List<int> allStudentIds = allStudentCSPs.OrderBy(a => a.Id).Select(a => a.Id).ToList();
            int currentIndex = allStudentIds.IndexOf(id);
            if (currentIndex < allStudentCSPs.Count - 1)
            {
                currentIndex++;
                return allStudentIds[currentIndex];
            }
            else
            {
                return id;
            }
        }
        public static int GetPreviousCSP(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            List<CSP> allStudentCSPs = _context.CSPs.Where(a => a.Id == id).FirstOrDefault().Student.CSPs.ToList();
            List<int> allStudentIds = allStudentCSPs.OrderBy(a => a.Id).Select(a => a.Id).ToList();
            int currentIndex = allStudentIds.IndexOf(id);

            if (currentIndex > 0)
            {
                currentIndex--;
                return allStudentIds[currentIndex];
            }
            else
            {
                return id;
            }
        }
        public static bool AddNew(int studentId, int subjectId, int schoolYearId, string matrials, List<int> teacherIds, int LoggeduserId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            CSP _item = new Sinaicsp_API.CSP();
            _item.SubjectId = subjectId;
            _item.StudentId = studentId;
            _item.SchoolYearId = schoolYearId;
            _item.Materials = matrials;
            foreach (int item in teacherIds)
            {
                TeacherCSP _juTable = new TeacherCSP();
                _juTable.TeacherId = item;
                _juTable.CreationDate = DateTime.Now;
                _juTable.CreatedByUserId = LoggeduserId;
                _item.TeacherCSPs.Add(_juTable);
            }
            _item.Comments = string.Empty;
            _item.FebruaryNotes = string.Empty;
            _item.JuneNotes = string.Empty;
            _item.CreationDate = DateTime.Now;
            _item.CreatedByUserId = LoggeduserId;
            _context.CSPs.Add(_item);
            _context.SaveChanges();
            return true;
        }

        public static bool Update(int cspId, int subjectId, int schoolYearId, string matrials, List<int> teacherIds, int LoggeduserId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            CSP _item = _context.CSPs.FirstOrDefault(a => a.Id == cspId);
            _item.SubjectId = subjectId;
            _item.SchoolYearId = schoolYearId;
            _item.Materials = matrials;
            List<TeacherCSP> allteachers = _item.TeacherCSPs.ToList();
            for (int i = 0; i < allteachers.Count; i++)
            {
                _context.TeacherCSPs.Remove(allteachers[i]);
            }
            foreach (int item in teacherIds)
            {
                TeacherCSP _juTable = new TeacherCSP();
                _juTable.TeacherId = item;
                _juTable.CreationDate = DateTime.Now;
                _juTable.CreatedByUserId = LoggeduserId;
                _item.TeacherCSPs.Add(_juTable);
            }
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
        public static void SaveCSPNotes(int cspId, string comments, string februaryNotes, string juneNotes)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            CSP _item = _context.CSPs.FirstOrDefault(a => a.Id == cspId);
            _item.Comments = comments;
            _item.FebruaryNotes = februaryNotes;
            _item.JuneNotes = juneNotes;
            _context.SaveChanges();
        }
        public static void AdjustOrder(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            CSP _item = _context.CSPs.Where(a => a.Id == id).FirstOrDefault();
            if (_item.CSPGoalCatalogs.Count() > 0 && _item.CSPGoalCatalogs.Select(a => a.TextOrder).Max() == 0)
            {
                List<CSPGoalCatalog> parentGoals = _item.CSPGoalCatalogs.Where(a => a.ParentCSPGoalCatalogId == null).ToList();
                for (int i = 0; i < parentGoals.Count; i++)
                {
                    parentGoals.ElementAt(i).TextOrder = i;
                    if (parentGoals.ElementAt(i).CSPGoalCatalogs.Count() > 0)
                    {
                        for (int x = 0; x < _item.CSPGoalCatalogs.ElementAt(i).CSPGoalCatalogs.Count(); x++)
                        {
                            _item.CSPGoalCatalogs.ElementAt(i).CSPGoalCatalogs.ElementAt(x).TextOrder = i;
                            _item.CSPGoalCatalogs.ElementAt(i).CSPGoalCatalogs.ElementAt(x).SubTextOrder = x;
                        }
                    }
                }
                _context.SaveChanges();
            }
        }

    }
}
