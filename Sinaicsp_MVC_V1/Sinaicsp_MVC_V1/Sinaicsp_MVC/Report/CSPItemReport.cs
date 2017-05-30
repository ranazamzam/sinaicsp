namespace Sinaicsp_MVC.Report
{
    using Sinaicsp_API;
    using System;
    using System.Linq;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Collections.Generic;

    /// <summary>
    /// Summary description for CSPItemReport.
    /// </summary>
    public partial class CSPItemReport : Telerik.Reporting.Report
    {
        public CSPItemReport(int cspId)
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            string rate1 = string.Empty;
            string rate2 = string.Empty;
            string rate3 = string.Empty;
            int year = int.Parse(DateTime.Now.Year.ToString().Replace("20", ""));
            int orgYear = year;
            int month = 7;
            for (int i = 0; i < 12; i++)
            {
                if (month == 11 && year == orgYear)
                {
                    rate1 = month + "/" + year;
                }
                if (month == 2)
                {
                    rate2 = month + "/" + year;
                }
                if (month == 6)
                {
                    rate3 = month + "/" + year;
                }
                month++;
                if (month > 12)
                {
                    month = 1;
                    year++;
                }
            }


            CSP _Item = CSP.GetById(cspId);
            txt_studentName.Value = _Item.StudentName;
            txt_instructor.Value = _Item.TeacherNames;
            txt_material.Value = _Item.Materials;
            txt_subject.Value = _Item.SubjectName;

            txt_tblStudentName.Value = _Item.Student.FirstName + " Will:";
            txt_rate1.Value = rate1;
            txt_rate2.Value = rate2;
            txt_rate3.Value = rate3;
            tb_cspNotes.DataSource = CustomCSPGoalCatalog.Convert(_Item.CSPGoalCatalogs.ToList());
            txt_febNote.Value = _Item.FebruaryNotes;

            List<CustomRatings> ratings = CustomRatings.Convert(_Item.Student.School.Ratings.Where(a => a.IsDeleted == false).ToList());
            string tableStart = "<table>" +
                "<tr><td style='text-align:center' colspan='2'><p style='text-decoration:underline;font-weight:bold'> CSP RATING SCALE</p ></td ></tr >";
            string tableContent = string.Empty;

            foreach (CustomRatings item in ratings)
            {
                tableContent+= "<tr><td width='120' > "+item.Col1+ " </td ><td > " + item.Col2 + " </td ></tr>";
            }
            string tableEnd = "</table>";
            tb_ratings.DataSource = ratings;
        }
    }
}