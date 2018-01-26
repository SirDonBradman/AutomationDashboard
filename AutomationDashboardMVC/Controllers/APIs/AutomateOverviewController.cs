using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using AutomationDashboardMVC.Models;
using System.Web;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace AutomationDashboardMVC.Controllers
{
    public class AutomateOverviewController : ApiController
    {
        public JObject Get(string id)
        {
            var db = new AutomationDashboardEntities();
            if (id.Contains("All"))
            {
                string ID = id.Substring(4);
                int teamId = int.Parse(ID); 
                var automateDatas = db.ProductTeams.First(team => team.TeamId == teamId);

                var JSONFile = HttpContext.Current.Server.MapPath("~/jsons/automateOverview.json");

                string json = File.ReadAllText(JSONFile);
                JObject rss = JObject.Parse(json);
                JArray labels = (JArray)rss["data"]["labels"];
                JArray labelsData = rss["data"]["datasets"][0]["data"] as JArray;

                if (teamId == 1)
                    labels.Add("Expected Test Cases");
                labels.Add("Testcases in TFS");
                labels.Add("Automated Test Cases");

                if (teamId == 1)
                    labelsData.Add(automateDatas.TotalExpectedTests);
                labelsData.Add(automateDatas.TotalTFSTests);
                labelsData.Add(automateDatas.TotalAutomatedTests);

                if (teamId != 1)
                {
                    JArray BackgroundColor = (JArray)rss["data"]["datasets"][0]["backgroundColor"];
                    BackgroundColor.RemoveAt(0);
                }
                return rss;
            }
            else
            {
                int ID = int.Parse(id);
                var Suite = db.TestSuites.Where(row => row.SuiteId == ID).First();
                int? teamId = Suite.TestPlan.TeamId;
                var suiteDetails = db.spGetSuiteDetails(Suite.SuiteId).First();

                var JSONFile = HttpContext.Current.Server.MapPath("~/jsons/automateOverview.json");

                string json = File.ReadAllText(JSONFile);
                JObject rss = JObject.Parse(json);
                JArray labels = (JArray)rss["data"]["labels"];
                JArray labelsData = rss["data"]["datasets"][0]["data"] as JArray;

                

                if (teamId == 1)
                    labels.Add("Expected Test Cases");
                labels.Add("Testcases in TFS");
                labels.Add("Automated Test Cases");

                if (teamId == 1)
                    labelsData.Add(suiteDetails?.ExpectedTests);
                labelsData.Add(suiteDetails?.TotalTests);
                labelsData.Add(suiteDetails?.TotalAutomatedTests);

                if (teamId != 1)
                {
                    JArray BackgroundColor = (JArray)rss["data"]["datasets"][0]["backgroundColor"];
                    BackgroundColor.RemoveAt(0);
                }

                return rss;
            }
        }

        public JObject Post(string ProjectName)
        {
            var db = new AutomationDashboardEntities();
            int TeamId = db.ProductTeams.First(team => team.TeamName == ProjectName).TeamId;

            var JSONFile = HttpContext.Current.Server.MapPath("~/jsons/WeeklyProgress.json");

            string json = File.ReadAllText(JSONFile);
            JObject rss = JObject.Parse(json);


            List<spGetWeeklyProgressForProductArea_Result> WeeklyProgress = new List<spGetWeeklyProgressForProductArea_Result>();
            var ProductAreas = db.AutomateDatas.Where(r => r.TeamId == TeamId).Select(col => col.ProductArea).Distinct();

            DateTime Date1 = Helpers.GetPreviousFriday(DateTime.Now);
            DateTime Date2 = Helpers.GetPreviousFriday(Date1);

            foreach (var productArea in ProductAreas)
            {
                spGetWeeklyProgressForProductArea_Result data = db.spGetWeeklyProgressForProductArea(productArea, TeamId, Date2, Date1).FirstOrDefault();
                if (data != null && (data.TFSTestCasesThisWeek > 0 || data.AutomatedThisWeek > 0))
                {
                    WeeklyProgress.Add(data);
                }
            }
            JArray DS1 = rss["data"]["datasets"][0]["data"] as JArray;
            JArray DS2 = rss["data"]["datasets"][1]["data"] as JArray;

            for (int i = 0; i < WeeklyProgress.Count; i++)
            {
                string PA = WeeklyProgress[i].ProductArea;

                JArray labels = (JArray)rss["data"]["labels"];
                labels.Add(PA);


                DS1.Add(WeeklyProgress[i].TFSTestCasesThisWeek);

                DS2.Add(WeeklyProgress[i].AutomatedThisWeek);
            }
            return rss;
        }
    }
}
