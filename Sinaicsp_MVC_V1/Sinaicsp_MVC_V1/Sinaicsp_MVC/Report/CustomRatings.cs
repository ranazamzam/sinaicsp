using Sinaicsp_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sinaicsp_MVC.Report
{
    public class CustomRatings
    {
        public static List<CustomRatings>Convert(List<Rating> ratings)
        {
            List<CustomRatings> retVal = new List<Report.CustomRatings>();
            List<Rating> firstSection = ratings.GetRange(0, ratings.Count / 2).ToList();
            ratings.RemoveRange(0, ratings.Count / 2);
            foreach (Rating item in firstSection)
            {
                retVal.Add(new CustomRatings() { Col1 = item.Description });
            }
            for (int i = 0; i < ratings.Count; i++)
            {
                retVal[i].Col2 = ratings[i].Description;
            }
            return retVal;
        }
        public string Col1 { get; set; }
        public string Col2 { get; set; }
    }
}