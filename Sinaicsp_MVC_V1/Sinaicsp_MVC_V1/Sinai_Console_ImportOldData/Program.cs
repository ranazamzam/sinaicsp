using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinai_Console_ImportOldData
{
    class Program
    {
        static void Main(string[] args)
        {
            // School
            eiepEntities _context = new Sinai_Console_ImportOldData.eiepEntities();
            List<school> allSchool = _context.schools.ToList();
            Sinaicsp_API.School.AddNew("-", -1);
            foreach (school item in allSchool)
            {
                Sinaicsp_API.School.AddNew(item.school_name, -1);
                int schoolId = Sinaicsp_API.School.GetByName(item.school_name).Id;
                foreach (grade gradeItem in item.grades)
                {
                    Sinaicsp_API.Rating.AddNew(schoolId, gradeItem.grade_text, gradeItem.grade_desc, -1);
                }

            }

            // Students
            List<student> allStudents = _context.students.ToList();
            List<string> allCities = allStudents.Select(a => a.home_city).Distinct().ToList();
            foreach (string cityItem in allCities)
            {
                Sinaicsp_API.City.AddNew("-", -1);
                if (string.IsNullOrEmpty(cityItem) == false)
                {
                    Sinaicsp_API.City.AddNew(cityItem, -1);
                }
            }
            List<string> allclasses = allStudents.Select(a => a.current_placement).Distinct().ToList();
            foreach (string classItem in allclasses)
            {
                Sinaicsp_API.StudentClass.AddNew("-", -1);
                if (string.IsNullOrEmpty(classItem) == false)
                {
                    Sinaicsp_API.StudentClass.AddNew(classItem, -1);
                }
            }

            List<Byte?> allgrades = allStudents.Select(a => a.student_grade).Distinct().ToList();
            foreach (Byte? grade in allgrades)
            {
                Sinaicsp_API.StudentGrade.AddNew("-", -1);
                if (grade != null)
                {
                    Sinaicsp_API.StudentGrade.AddNew(grade.Value.ToString(), -1);
                }
            }

            foreach (student item in allStudents)
            {

                string schoolName = item.school_id != null ? _context.schools.FirstOrDefault(a => a.school_id == item.school_id).school_name : "-";
                int schoolId = Sinaicsp_API.School.GetByName(schoolName).Id;
                int classId = string.IsNullOrEmpty(item.current_placement) ? Sinaicsp_API.StudentClass.GetByName("-").Id : Sinaicsp_API.StudentClass.GetByName(item.current_placement).Id;
                int gradeId = item.student_grade == null ? Sinaicsp_API.StudentGrade.GetByName("-").Id : Sinaicsp_API.StudentGrade.GetByName(item.student_grade.ToString()).Id;
                int cityId = string.IsNullOrEmpty(item.home_city) ? Sinaicsp_API.City.GetByName("-").Id : Sinaicsp_API.City.GetByName(item.home_city).Id;
                DateTime DOB = item.date_of_birth != null ? item.date_of_birth.Value : DateTime.Now.AddYears(-50);
                string gender = string.IsNullOrEmpty(item.student_sex) ? "M" : item.student_sex;
                string motherCell = string.IsNullOrEmpty(item.mother_cell) ? "-" : item.mother_cell;
                motherCell = motherCell.Trim().Length > 0 ? motherCell : "-";
                string parent_primary = string.IsNullOrEmpty(item.parent_primary) ? "-" : item.parent_primary;
                string home_address = string.IsNullOrEmpty(item.home_address) ? "-" : item.home_address;
                string home_state = string.IsNullOrEmpty(item.home_state) ? "-" : item.home_state;

                string parent_secondary = string.IsNullOrEmpty(item.parent_secondary) ? "-" : item.parent_secondary;
                string home_phone = string.IsNullOrEmpty(item.home_phone) ? "-" : item.home_phone;
                home_phone = home_phone.Trim().Length > 0 ? home_phone : "-";
                string father_cell = string.IsNullOrEmpty(item.father_cell) ? "-" : item.father_cell;
                father_cell = father_cell.Trim().Length > 0 ? father_cell : "-";


                Sinaicsp_API.Student.AddNew(schoolId, classId, gradeId, cityId, item.student_first_name, item.student_last_name, gender, DOB, parent_primary, home_address, home_state, motherCell, parent_secondary, home_phone, father_cell, -1);

            }


            // Teacher
            List<usr> allTeacher = _context.usrs.ToList();
            foreach (usr item in allTeacher)
            {
                string schoolName = item.school_id != null ? _context.schools.FirstOrDefault(a => a.school_id == item.school_id).school_name : "-";
                int schoolId = Sinaicsp_API.School.GetByName(schoolName).Id;
                string usr_title = string.IsNullOrEmpty(item.usr_title) ? "-" : item.usr_title;
                Sinaicsp_API.Teacher.AddNew(schoolId, item.usr_display_name, item.usr_login, usr_title, -1);
            }


            // Subjects
            List<subject> allSubjects = _context.subjects.ToList();
            Sinaicsp_API.Subject.AddNew("-", -1);
            foreach (subject item in allSubjects)
            {
                Sinaicsp_API.Subject.AddNew(item.subject_name, -1);
            }
            foreach (subject item in allSubjects)
            {
                if (item.subject_category != null)
                {
                    subject parentSubject = _context.subjects.Where(a => a.subject_uuid == item.subject_category.Value).FirstOrDefault();
                    int N_parentId = Sinaicsp_API.Subject.GetByName(parentSubject.subject_name).Id;
                    int N_subjectId = Sinaicsp_API.Subject.GetByName(item.subject_name).Id;
                    Sinaicsp_API.Subject.Update(N_subjectId, N_parentId, item.subject_name);

                }
            }
            // Providers
            List<service_providers> allProviders = _context.service_providers.ToList();
            Sinaicsp_API.Provider.AddNew("-", -1);
            foreach (service_providers item in allProviders)
            {
                Sinaicsp_API.Provider.AddNew(item.provider_name, -1);
            }

            // Serviecs
            List<service> allServices = _context.services.ToList();
            foreach (service item in allServices)
            {
                service_providers _prov = _context.service_providers.FirstOrDefault(a => a.providers_uuid == item.provider_uuid);
                string providerName = _prov != null ? _prov.provider_name : "-";

                int providerId = Sinaicsp_API.Provider.GetByName(providerName).Id;
                List<int> _allStudentsService = new List<int>();
                List<services_students> allStudentsServices = _context.services_students.Where(a => a.services_uuid == item.services_uuid).ToList();
                foreach (services_students srStudent in allStudentsServices)
                {
                    student _oldStudent = _context.students.Where(a => a.student_uuid == srStudent.student_uuid).FirstOrDefault();
                    Sinaicsp_API.SinaicspDataModelContainer _newcontext = new Sinaicsp_API.SinaicspDataModelContainer();
                    Sinaicsp_API.Student _NewStudents = _newcontext.Students.Where(a => a.FirstName == _oldStudent.student_first_name && a.LastName == _oldStudent.student_last_name).FirstOrDefault();
                    _allStudentsService.Add(_NewStudents.Id);
                }
                string serviceName = string.IsNullOrEmpty(item.services_name) ? "-" : item.services_name;
                string session_end_date = string.IsNullOrEmpty(item.session_end_date) ? "-" : item.session_end_date;

                string service_model = string.IsNullOrEmpty(item.service_model) ? "-" : item.service_model;
                string session_length = string.IsNullOrEmpty(item.session_length) ? "-" : item.session_length;
                string weekly_sessions = string.IsNullOrEmpty(item.weekly_sessions) ? "-" : item.weekly_sessions;
                string session_start_date = string.IsNullOrEmpty(item.session_start_date) ? "-" : item.session_start_date;

                Sinaicsp_API.Service.AddNew(providerId, serviceName, service_model, item.num_students, session_length, weekly_sessions, session_start_date, session_end_date, _allStudentsService, -1);
                _allStudentsService.Clear();
            }

            // Student Inclusion
            List<student_inclusions> allInclusions = _context.student_inclusions.ToList();
            foreach (student_inclusions item in allInclusions)
            {
                student _oldStudent = _context.students.Where(a => a.student_uuid == item.student_uuid).FirstOrDefault();
                Sinaicsp_API.SinaicspDataModelContainer _newcontext = new Sinaicsp_API.SinaicspDataModelContainer();
                Sinaicsp_API.Student _NewStudents = _newcontext.Students.Where(a => a.FirstName == _oldStudent.student_first_name && a.LastName == _oldStudent.student_last_name).FirstOrDefault();
                string session_end_date = string.IsNullOrEmpty(item.session_end_date) ? "-" : item.session_end_date;
                Sinaicsp_API.Inclusion.AddNew(_NewStudents.Id, item.subject_name, item.num_classes, item.teacher_name, item.session_start_date, session_end_date, -1);
            }

            // Student Accomdation
            List<student_accommodations> allaccommodations = _context.student_accommodations.ToList();
            foreach (student_accommodations item in allaccommodations)
            {
                student _oldStudent = _context.students.Where(a => a.student_uuid == item.student_uuid).FirstOrDefault();
                Sinaicsp_API.SinaicspDataModelContainer _newcontext = new Sinaicsp_API.SinaicspDataModelContainer();
                Sinaicsp_API.Student _NewStudents = _newcontext.Students.Where(a => a.FirstName == _oldStudent.student_first_name && a.LastName == _oldStudent.student_last_name).FirstOrDefault();
                Sinaicsp_API.Accommodation.AddNew(_NewStudents.Id, item.accommodation_details, -1);
            }

            // Goal Catalog

            List<gc_categories> allCategories = _context.gc_categories.ToList();
            Sinaicsp_API.SinaicspDataModelContainer _Ncontext = new Sinaicsp_API.SinaicspDataModelContainer();

            foreach (gc_categories item in allCategories)
            {
                Sinaicsp_API.GC_Category _NewCategory = new Sinaicsp_API.GC_Category();
                _NewCategory.Name = item.gc_category_name;
                _NewCategory.CreationDate = DateTime.Now;
                _NewCategory.CreatedByUserId = -1;

                List<gc_subjects> allCategoiesSubjects = _context.gc_subjects.Where(a => a.gc_category_uuid == item.gc_category_uuid).ToList();
                foreach (gc_subjects subjItem in allCategoiesSubjects)
                {
                    Sinaicsp_API.GC_Subjects _NewSubject = new Sinaicsp_API.GC_Subjects();
                    _NewSubject.Name = subjItem.gc_subject_name;
                    _NewSubject.CreationDate = DateTime.Now;
                    _NewSubject.CreatedByUserId = -1;
                    _NewCategory.GC_Subjects.Add(_NewSubject);

                    List<gc_goals> allSubjectGoals = _context.gc_goals.Where(a => a.gc_subject_uuid == subjItem.gc_subject_uuid).ToList();
                    foreach (gc_goals goalItem in allSubjectGoals)
                    {
                        Sinaicsp_API.GoalCatalog _NewGoal = new Sinaicsp_API.GoalCatalog();
                        _NewGoal.TextGoal = goalItem.gc_goal_text;
                        _NewGoal.CreationDate = DateTime.Now;
                        _NewGoal.CreatedByUserId = -1;
                        _NewSubject.GoalCatalogs.Add(_NewGoal);
                    }
                    List<gc_goals> Child_allSubjectGoals = allSubjectGoals.Where(a => a.gc_parent_uuid != null).ToList();
                    foreach (gc_goals Childitem in Child_allSubjectGoals)
                    {
                        string goalParent = _context.gc_goals.Where(a => a.gc_goal_uuid == Childitem.gc_parent_uuid).FirstOrDefault().gc_goal_text;
                        Sinaicsp_API.GoalCatalog _ChildGoal = _NewSubject.GoalCatalogs.Where(a => a.TextGoal == Childitem.gc_goal_text).FirstOrDefault();
                        Sinaicsp_API.GoalCatalog _ParentGoal = _NewSubject.GoalCatalogs.Where(a => a.TextGoal == goalParent).FirstOrDefault();
                        _ChildGoal.ParentGoalCatalog = _ParentGoal;
                    }
                }
                _Ncontext.GC_Category.Add(_NewCategory);
                _Ncontext.SaveChanges();
            }
            // School Year
            // CSP
            List<iep> allCsps = _context.ieps.ToList();
            List<string> schoolYears = allCsps.Select(a => a.iep_year).ToList().Distinct().ToList();
            Sinaicsp_API.SchoolYear.AddNew("-", -1);
            foreach (string item in schoolYears)
            {
                if (string.IsNullOrEmpty(item.Trim()) == false)
                {
                    Sinaicsp_API.SchoolYear.AddNew(item, -1);
                }
            }

            foreach (iep item in allCsps)
            {

                student old_student = _context.students.Where(a => a.student_uuid == item.student_uuid).FirstOrDefault();
                int newStudentId = _Ncontext.Students.Where(a => a.FirstName == old_student.student_first_name && a.LastName == old_student.student_last_name).FirstOrDefault().Id;
                subject old_subject = _context.subjects.Where(a => a.subject_uuid == item.iep_subject_uuid).FirstOrDefault();
                int newSubjectId = 0;
                if (old_subject == null)
                {
                    newSubjectId = Sinaicsp_API.Subject.GetByName("-").Id;
                }
                else
                {
                    newSubjectId = _Ncontext.Subjects.Where(a => a.Name == old_subject.subject_name).FirstOrDefault().Id;
                }
                int newschoolYearId = 0;
                if (_Ncontext.SchoolYears.Where(a => a.Name == item.iep_year).FirstOrDefault() != null)
                {
                    newschoolYearId = _Ncontext.SchoolYears.Where(a => a.Name == item.iep_year).FirstOrDefault().Id;
                }
                else
                {
                    newschoolYearId = _Ncontext.SchoolYears.Where(a => a.Name == "-").FirstOrDefault().Id;
                }
                List<int> teacherIds = new List<int>();
                string Teacher_1 = _context.usrs.Where(a => a.usr_uuid == item.teacher_uuid).FirstOrDefault().usr_login;
                string Teacher_2 = item.teacher_2_uuid == null ? string.Empty : _context.usrs.Where(a => a.usr_uuid == item.teacher_2_uuid).FirstOrDefault().usr_login;
                string Teacher_3 = item.teacher_3_uuid == null ? string.Empty : _context.usrs.Where(a => a.usr_uuid == item.teacher_3_uuid).FirstOrDefault().usr_login;
                string Teacher_4 = item.teacher_4_uuid == null ? string.Empty : _context.usrs.Where(a => a.usr_uuid == item.teacher_4_uuid).FirstOrDefault().usr_login;
                string Teacher_5 = item.teacher_5_uuid == null ? string.Empty : _context.usrs.Where(a => a.usr_uuid == item.teacher_5_uuid).FirstOrDefault().usr_login;
                if (!string.IsNullOrEmpty(Teacher_1))
                {
                    teacherIds.Add(Sinaicsp_API.Teacher.GetByEmail(Teacher_1).Id);
                }
                if (!string.IsNullOrEmpty(Teacher_2))
                {
                    teacherIds.Add(Sinaicsp_API.Teacher.GetByEmail(Teacher_2).Id);
                }
                if (!string.IsNullOrEmpty(Teacher_3))
                {
                    teacherIds.Add(Sinaicsp_API.Teacher.GetByEmail(Teacher_3).Id);
                }
                if (!string.IsNullOrEmpty(Teacher_4))
                {
                    teacherIds.Add(Sinaicsp_API.Teacher.GetByEmail(Teacher_4).Id);
                }
                if (!string.IsNullOrEmpty(Teacher_5))
                {
                    teacherIds.Add(Sinaicsp_API.Teacher.GetByEmail(Teacher_5).Id);
                }


                Sinaicsp_API.CSP _item = new Sinaicsp_API.CSP();
                _item.SubjectId = newSubjectId;
                _item.StudentId = newStudentId;
                _item.SchoolYearId = newschoolYearId;
                _item.Materials = item.iep_materials;
                _item.Comments = item.iep_comments;
                _item.FebruaryNotes = item.iep_feb_notes;
                _item.JuneNotes = item.iep_june_notes;
                foreach (int teachItem in teacherIds)
                {
                    Sinaicsp_API.TeacherCSP _juTable = new Sinaicsp_API.TeacherCSP();
                    _juTable.TeacherId = teachItem;
                    _juTable.CreationDate = DateTime.Now;
                    _juTable.CreatedByUserId = -1;
                    _item.TeacherCSPs.Add(_juTable);
                }
                _item.Comments = string.Empty;
                _item.FebruaryNotes = string.Empty;
                _item.JuneNotes = string.Empty;
                _item.CreationDate = DateTime.Now;
                _item.CreatedByUserId = -1;

                // CSP Goals
                List<iep_data> alliepData = _context.iep_data.Where(a => a.iep_uuid == item.iep_uuid).ToList();
                foreach (iep_data dataitem in alliepData)
                {
                    Sinaicsp_API.CSPGoalCatalog _CSPGoalitem = new Sinaicsp_API.CSPGoalCatalog();
                    _CSPGoalitem.TextGoal = dataitem.iep_data_text;


                    if (dataitem.iep_data_grade_1_id != null)
                        _CSPGoalitem.Rate1 = _context.grades.Where(a => a.grade_id == dataitem.iep_data_grade_1_id).FirstOrDefault().grade_text;
                    if (dataitem.iep_data_grade_2_id != null)
                        _CSPGoalitem.Rate2 = _context.grades.Where(a => a.grade_id == dataitem.iep_data_grade_2_id).FirstOrDefault().grade_text;
                    if (dataitem.iep_data_grade_3_id != null)
                        _CSPGoalitem.Rate3 = _context.grades.Where(a => a.grade_id == dataitem.iep_data_grade_3_id).FirstOrDefault().grade_text;

                    _CSPGoalitem.DateInitiated = string.IsNullOrEmpty(_CSPGoalitem.DateInitiated) ? " " : _CSPGoalitem.DateInitiated;
                    _CSPGoalitem.Rate1 = string.IsNullOrEmpty(_CSPGoalitem.Rate1) ? " " : _CSPGoalitem.Rate1;
                    _CSPGoalitem.Rate2 = string.IsNullOrEmpty(_CSPGoalitem.Rate2) ? " " : _CSPGoalitem.Rate2;
                    _CSPGoalitem.Rate3 = string.IsNullOrEmpty(_CSPGoalitem.Rate3) ? " " : _CSPGoalitem.Rate3;

                    _CSPGoalitem.CreationDate = DateTime.Now;
                    _item.CreatedByUserId = -1;
                    _item.CSPGoalCatalogs.Add(_CSPGoalitem);
                }
                List<iep_data> Child_allSubjectGoals = alliepData.Where(a => a.iep_data_parent_uuid != null).ToList();
                foreach (iep_data Childitem in Child_allSubjectGoals)
                {
                    if (_context.iep_data.Where(a => a.iep_data_uuid == Childitem.iep_data_parent_uuid).FirstOrDefault() != null)
                    {
                        string goalParent = _context.iep_data.Where(a => a.iep_data_uuid == Childitem.iep_data_parent_uuid).FirstOrDefault().iep_data_text;
                        Sinaicsp_API.CSPGoalCatalog _ChildGoal = _item.CSPGoalCatalogs.Where(a => a.TextGoal == Childitem.iep_data_text).FirstOrDefault();
                        Sinaicsp_API.CSPGoalCatalog _ParentGoal = _item.CSPGoalCatalogs.Where(a => a.TextGoal == goalParent).FirstOrDefault();
                        if (_ParentGoal != null && _ParentGoal.TextGoal != _ChildGoal.TextGoal)
                        {
                            _ChildGoal.ParentCSPGoalCatalog = _ParentGoal;
                        }
                    }
                }

                _Ncontext.CSPs.Add(_item);
                _Ncontext.SaveChanges();
            }


            Console.WriteLine("Items Generated successfully");
            Console.ReadKey();
        }
    }
}
