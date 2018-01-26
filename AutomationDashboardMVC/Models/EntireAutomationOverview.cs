using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomationDashboardMVC.Models
{
    public class TeamAutomationWeeklyProgress
    {
        public spGetWeeklyProgressForProductAreaBetweenDates_Result Week1 = new spGetWeeklyProgressForProductAreaBetweenDates_Result();
        public spGetWeeklyProgressForProductAreaBetweenDates_Result Week2 = new spGetWeeklyProgressForProductAreaBetweenDates_Result();
        public spGetWeeklyProgressForProductAreaBetweenDates_Result Week3 = new spGetWeeklyProgressForProductAreaBetweenDates_Result();
        public int? TFSTotal { get; set; }
        public int? AutomatedTotal { get; set; }
        public List<spGetPreviousAndCurrentWeekAutomationAggregateData_Result> WeeklyStatus;
        public string TeamName;

        public int TeamId;
        public DateTime Date1, Date2, Date3, Date4;

        static List<spGetWeeklyProgressForProductAreaBetweenDates_Result> GetProgress(int TeamId, DateTime From, DateTime To, List<string> ProductAreas)
        {
            var db = new AutomationDashboardEntities();
            var FirstData = db.spGetStatusOfAutomationForATeamWithSpecifiedDate(TeamId, To).ToDictionary(s=>s.ProductArea);
            var SecondData = db.spGetStatusOfAutomationForATeamWithSpecifiedDate(TeamId, From).ToDictionary(s => s.ProductArea);
            List<spGetWeeklyProgressForProductAreaBetweenDates_Result> list = new List<spGetWeeklyProgressForProductAreaBetweenDates_Result>();

            foreach (string productArea in ProductAreas)
            {
                var progressData = new spGetWeeklyProgressForProductAreaBetweenDates_Result();
                progressData.ProductArea = productArea;
                progressData.TeamId = TeamId;
                if(FirstData.ContainsKey(productArea) && SecondData.ContainsKey(productArea))
                {
                    progressData.TFSTestCasesThisWeek = FirstData[productArea].NoOfTFSTestcase - SecondData[productArea].NoOfTFSTestcase;
                    progressData.AutomatedThisWeek = FirstData[productArea].NoOfAutomatedTestcases - SecondData[productArea].NoOfAutomatedTestcases;

                }
                else if(!FirstData.ContainsKey(productArea) && !SecondData.ContainsKey(productArea))
                {
                    progressData.TFSTestCasesThisWeek = 0;
                    progressData.AutomatedThisWeek = 0;
                }
                else if(FirstData.ContainsKey(productArea))
                {
                    progressData.TFSTestCasesThisWeek = FirstData[productArea].NoOfTFSTestcase;
                    progressData.AutomatedThisWeek = FirstData[productArea].NoOfAutomatedTestcases;
                }

                var temp = db.WeeklyTriagings.Where(row => row.TeamId == TeamId && row.Date <= To && row.Date > From && row.ProductArea == productArea).FirstOrDefault();
                progressData.NoOfTestCasesDebugged = temp == null ? 0 : temp.NoOfTestCasesDebugged;

                list.Add(progressData);
            }

            return list;
        }
        public TeamAutomationWeeklyProgress(int TeamId)
        {
            this.TeamId = TeamId;
            var db = new AutomationDashboardEntities();
            WeeklyStatus = db.spGetPreviousAndCurrentWeekAutomationAggregateData(TeamId).ToList();

            var ProductAreas = db.AutomateDatas.Where(e => e.TeamId == TeamId).Select(col => col.ProductArea).Distinct().ToList();

            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
                Date1 = DateTime.Now.Date;
            else
                Date1 = Helpers.GetPreviousFriday(DateTime.Now);

            Date2 = Helpers.GetPreviousFriday(Date1);
            Date3 = Helpers.GetPreviousFriday(Date2);
            Date4 = Helpers.GetPreviousFriday(Date3);
            
            var data3 = GetProgress(TeamId, Date4, Date3, ProductAreas);
            var data2 = GetProgress(TeamId, Date3, Date2, ProductAreas);
            var data1 = GetProgress(TeamId, Date2, Date1, ProductAreas);

            Week1.TFSTestCasesThisWeek = data1.Select(r=>r.TFSTestCasesThisWeek).Sum();
            Week1.AutomatedThisWeek = data1.Select(r => r.AutomatedThisWeek).Sum();
            Week1.NoOfTestCasesDebugged = data1.Select(r => r.NoOfTestCasesDebugged).Sum();

            Week2.TFSTestCasesThisWeek = data2.Select(r => r.TFSTestCasesThisWeek).Sum();
            Week2.AutomatedThisWeek = data2.Select(r => r.AutomatedThisWeek).Sum();
            Week2.NoOfTestCasesDebugged = data2.Select(r => r.NoOfTestCasesDebugged).Sum();

            Week3.TFSTestCasesThisWeek = data3.Select(r => r.TFSTestCasesThisWeek).Sum();
            Week3.AutomatedThisWeek = data3.Select(r => r.AutomatedThisWeek).Sum();
            Week3.NoOfTestCasesDebugged = data3.Select(r => r.NoOfTestCasesDebugged).Sum();

            try
            {
                TFSTotal = WeeklyStatus[1].TotalTFS;
                AutomatedTotal = WeeklyStatus[1].TotalAutomated;
            }
            catch
            {
                TFSTotal = 0;
                AutomatedTotal = 0;
            }
            TeamName = db.ProductTeams.Where(r => r.TeamId == TeamId).First().TeamName;
        }
    }
}