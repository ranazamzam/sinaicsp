using Sinaicsp_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sinaicsp_MVC.Report
{
    public class CustomCSPGoalCatalog
    {
        public static List<CustomCSPGoalCatalog> Convert(List<CSPGoalCatalog> cspItems)
        {
            List<CustomCSPGoalCatalog> retVal = new List<Report.CustomCSPGoalCatalog>();
            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToLower().ToCharArray();
            int startIndex = 0;
            List<CSPGoalCatalog> parentsOnly = cspItems.Where(a => a.ParentCSPGoalCatalogId == null && a.IsDeleted == false).OrderBy(a => a.TextOrder).ToList();
            foreach (CSPGoalCatalog item in parentsOnly)
            {
                startIndex++;
                retVal.Add(new CustomCSPGoalCatalog()
                {
                    DateInitiated = item.DateInitiated,
                    Rate1 = item.Rate1,
                    Rate2 = item.Rate2,
                    Rate3 = item.Rate3,
                    GoalText = (startIndex).ToString() + ". " + item.TextGoal
                });

                List<CSPGoalCatalog> childs = item.CSPGoalCatalogs.ToList().Where(a => a.IsDeleted == false).OrderBy(a => a.SubTextOrder).ToList();
                for (int i = 0; i < childs.Count; i++)
                {
                    retVal.Add(new CustomCSPGoalCatalog()
                    {
                        DateInitiated = childs[i].DateInitiated,
                        Rate1 = childs[i].Rate1,
                        Rate2 = childs[i].Rate2,
                        Rate3 = childs[i].Rate3,
                        GoalText = "    "+alpha[i].ToString() + ". " + childs[i].TextGoal
                    });
                }
            }

            return retVal;
        }
        public string GoalText { get; set; }
        public string DateInitiated { get; set; }
        public string Rate1 { get; set; }
        public string Rate2 { get; set; }
        public string Rate3 { get; set; }
    }
}