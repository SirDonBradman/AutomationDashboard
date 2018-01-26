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
    public class WeeklyProgressController : ApiController
    {
        [OutputCache(Duration = 600, VaryByParam = "id")]
        public JObject Get(int id)
        {
            var db = new AutomationDashboardEntities();

            var JSONFile = HttpContext.Current.Server.MapPath("~/jsons/WeeklyProgress.json");

            string json = System.IO.File.ReadAllText(JSONFile);
            JObject rss = JObject.Parse(json);


            List<spGetWeeklyProgressForProductArea_Result> WeeklyProgress = new List<spGetWeeklyProgressForProductArea_Result>();
            var ProductAreas = db.AutomateDatas.Where(r => r.TeamId == id).Select(col => col.ProductArea).Distinct().ToList();
            DateTime Date1, Date2;

            //if friday then show one week data
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                Date1 = DateTime.Now;
                Date2 = Helpers.GetPreviousFriday(Date1);
            }
            //else show data from previous week till date
            else
            {
                Date1 = Helpers.GetPreviousFriday(DateTime.Now).AddDays(7);
                Date2 = Helpers.GetPreviousFriday(Date1);
                Date2 = Helpers.GetPreviousFriday(Date2);
            }
            
            var FirstData = db.spGetStatusOfAutomationForATeamWithSpecifiedDate(id, Date1).ToDictionary(s => s.ProductArea);
            var SecondData = db.spGetStatusOfAutomationForATeamWithSpecifiedDate(id, Date2).ToDictionary(s => s.ProductArea);
            
            foreach (string productArea in ProductAreas)
            {
                bool flag = true;
                var progressData = new spGetWeeklyProgressForProductArea_Result();
                progressData.ProductArea = productArea;
                progressData.TeamId = id;
                if (FirstData.ContainsKey(productArea) && SecondData.ContainsKey(productArea))
                {
                    progressData.TFSTestCasesThisWeek = FirstData[productArea].NoOfTFSTestcase - SecondData[productArea].NoOfTFSTestcase;
                    progressData.AutomatedThisWeek = FirstData[productArea].NoOfAutomatedTestcases - SecondData[productArea].NoOfAutomatedTestcases;

                    if (progressData.TFSTestCasesThisWeek == 0 && progressData.AutomatedThisWeek == 0)
                        flag = false;
                }
                else if (!FirstData.ContainsKey(productArea) && !SecondData.ContainsKey(productArea))
                {
                    progressData.TFSTestCasesThisWeek = 0;
                    progressData.AutomatedThisWeek = 0;
                    flag = false;
                }
                else if (FirstData.ContainsKey(productArea))
                {
                    progressData.TFSTestCasesThisWeek = FirstData[productArea].NoOfTFSTestcase;
                    progressData.AutomatedThisWeek = FirstData[productArea].NoOfAutomatedTestcases;
                }
                if(flag)
                    WeeklyProgress.Add(progressData);
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
