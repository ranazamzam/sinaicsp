﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinaicsp_API
{
    public partial class Subject
    {
        public static List<Subject> GetAll()
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.Subjects.ToList();
        }
        public static Subject GetById(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            return _context.Subjects.FirstOrDefault(a => a.Id == id);
        }
        public static bool AddNew(string name, int LoggeduserId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            if (_context.Schools.ToList().Where(a => a.Name.ToLower() == name.ToLower()).Count() == 0)
            {
                Subject _item = new Sinaicsp_API.Subject();
                _item.Name = name;
                _item.CreationDate = DateTime.Now;
                _item.CreatedByUserId = LoggeduserId;
                _context.Subjects.Add(_item);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public static bool AddNew(int parentSubjectId, string name, int LoggeduserId)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            if (_context.Schools.ToList().Where(a => a.Name.ToLower() == name.ToLower()).Count() == 0)
            {
                Subject _item = new Sinaicsp_API.Subject();
                _item.Name = name;
                _item.ParentId = parentSubjectId;
                _item.CreationDate = DateTime.Now;
                _context.Subjects.Add(_item);
                _item.CreatedByUserId = LoggeduserId;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public static bool Update(int id, string name)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            Subject itemById = _context.Subjects.FirstOrDefault(a => a.Id == id);
            Subject itemByName = _context.Subjects.ToList().FirstOrDefault(a => a.Name.ToLower() == name.ToLower());

            if (itemByName == null || itemByName.Id == itemById.Id)
            {
                Subject _item = itemById;
                _item.Name = name;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public static void SoftDelete(int id)
        {
            SinaicspDataModelContainer _context = new Sinaicsp_API.SinaicspDataModelContainer();
            Subject _item = _context.Subjects.FirstOrDefault(a => a.Id == id);
            _item.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}