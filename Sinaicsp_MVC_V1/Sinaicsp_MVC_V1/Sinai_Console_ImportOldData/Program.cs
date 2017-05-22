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

            Console.WriteLine("Items Generated successfully");
            Console.ReadKey();
        }
    }
}
