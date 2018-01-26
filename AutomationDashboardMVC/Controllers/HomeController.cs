extern alias AutomationReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutomationDashboardMVC.Models;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Data;
using System.Data.Entity;
using System.Xml;
using System.Text;
using AutomationReport.AventStack.ExtentReports.Reporter;
using AutomationReport.AventStack.ExtentReports;
using Newtonsoft.Json;

namespace AutomationDashboardMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult PageUnderConstruction()
        {
            return View("~/Views/Report/PageUnderConstruction.cshtml");
        }
        public ActionResult GetConfigForSuite(int Id)
        {
            using (var db = new AutomationDBEntities())
            {
                var module = db.AutomationTasks.FirstOrDefault(row => row.Id == Id);
                string jsonConfig = "content not available";
                if (module != null)
                    jsonConfig = Encoding.UTF8.GetString(module.ConfigurationsUsedForExecution);
                var jobject = JObject.Parse(jsonConfig);
                return Content(jobject.ToString(), "application/json");
            }
        }
        public ActionResult GetConfigForJob(int Id)
        {
            using (var db = new AutomationDBEntities())
            {
                var job = db.ManagerJobs.FirstOrDefault(row => row.Id == Id);
                string config = "content not available";
                if (job != null)
                    config = Encoding.UTF8.GetString(job.Config);

                return Content(config, "application/json");
            }
        }
        public ActionResult GetXMLReportForSuite(int id)
        {
            using (var db = new AutomationDBEntities())
            {
                var record = db.AutomationTasks.Where(row => row.Id == id).FirstOrDefault();
                string report = Encoding.UTF8.GetString(record.AutomationReport);
                return Content(report, "text/xml");
            }
        }
        private XmlDocument GetConsolidatedReport(int id)
        {
            using (var db = new AutomationDBEntities())
            {
                var Tasks = db.AutomationTasks.Where(rows => rows.JobId == id).ToList();
                XmlDocument reportFile = new XmlDocument();
                string firstReport = Encoding.UTF8.GetString(Tasks[0].AutomationReport);
                reportFile.LoadXml(firstReport);
                Tasks.RemoveAt(0);

                XmlNode TestsTag = reportFile.GetElementsByTagName("tests")[0];

                foreach (var task in Tasks)
                {
                    if (task.AutomationReport == null)
                        continue;

                    string taskReport = Encoding.UTF8.GetString(task.AutomationReport);
                    XmlDocument document = new XmlDocument();
                    document.LoadXml(taskReport);
                    var taskTests = document.GetElementsByTagName("test");
                    foreach (XmlNode test in taskTests)
                    {
                        var importedNode = reportFile.ImportNode(test, true);
                        TestsTag.AppendChild(importedNode);
                    }
                }
                return reportFile;
            }
        }
        public ActionResult ViewFullPassXMLReport(int id)
        {
            var reportFile = GetConsolidatedReport(id);
            return Content(reportFile.OuterXml, "text/xml");
        }
        private string GenerateExtentReport(XmlDocument ReportFile)
        {
            DateTime dateObject = DateTime.Now;
            string Date = dateObject.ToString("dd_MM_yyyy");
            string Time = dateObject.ToString("HH_mm_ss");
            string CurrentDateTime = Date + "_" + Time;

            const string SUCCESS = "success";
            const string FAIL = "fail";
            const string INFO = "info";
            const string ERROR = "error";
            const string COMPLETE = "complete";
            const string SKIP = "skip";
            const string Attribute_Category = "category";
            const string Attribute_Name = "name";
            const string TEST = "test";

            string FileName = Server.MapPath("~/Excel/") + "ConsolidatedReport" + CurrentDateTime + ".html";

            ExtentHtmlReporter htmlReporter = new ExtentHtmlReporter(FileName);
            htmlReporter.Configuration().DocumentTitle = "Masterworks Automation";
            htmlReporter.Configuration().ReportName = "Automation Reports";
            ExtentReports Report = new ExtentReports();
            Report.AttachReporter(htmlReporter);

            var Tests = ReportFile.GetElementsByTagName(TEST);
            foreach (XmlNode test in Tests)
            {
                ExtentTest extentTest = Report.CreateTest(test.Attributes[Attribute_Name].Value);
                extentTest.AssignCategory(test.Attributes[Attribute_Category].Value);

                foreach (XmlNode log in test.ChildNodes)
                {
                    switch (log.LocalName)
                    {
                        case SUCCESS:
                            extentTest.Pass(log.InnerXml);
                            break;
                        case INFO:
                            extentTest.Info(log.InnerXml);
                            break;
                        case ERROR:
                            extentTest.Error(log.InnerXml);
                            break;
                        case FAIL:
                            extentTest.Fail(log.InnerXml);
                            break;
                        case SKIP:
                            extentTest.Skip(log.InnerXml);
                            break;
                    }
                }
            }
            Report.Flush();
            return FileName;
        }
        public ActionResult ViewSuiteHTMLReport(int id)
        {
            using (var db = new AutomationDBEntities())
            {
                var record = db.AutomationTasks.Where(row => row.Id == id).FirstOrDefault();
                string report = Encoding.UTF8.GetString(record.AutomationReport);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(report);
                string FileName = GenerateExtentReport(doc);
                var rm = new ReportModel(System.IO.File.ReadAllText(FileName));
                System.IO.File.Delete(FileName);
                return View("~/Views/Report/ViewResult.cshtml", rm);
            }
        }
        public ActionResult ViewFullPassHTMLReport(int id)
        {
            var ReportFile = GetConsolidatedReport(id);
            string FileName = GenerateExtentReport(ReportFile);
            var rm = new ReportModel(System.IO.File.ReadAllText(FileName));
            System.IO.File.Delete(FileName);
            return View("~/Views/Report/ViewResult.cshtml", rm);
        }
        public ActionResult Sentinel()
        {
            return View("~/Views/Report/Sentinel.cshtml");
        }
        [NonAction]
        private void ClearApplicationCache()
        {
            List<string> URLs = new List<string>(){
                Url.Action("Landing", "Home"),
                Url.Action("GetDataForTeam","Home"),
                Url.Action("Index","Home"),
            };
            foreach (var url in URLs)
            {
                try
                {
                    HttpResponse.RemoveOutputCacheItem(url);
                }
                catch
                { }
            }
        }
        [HttpGet]
        public void GetLastAutomationStatus(int TeamId)
        {
            string FileName;
            if (TeamId == 1)
                FileName = "Team" + TeamId.ToString() + ".xlsx";
            else
                FileName = "Team2.xlsx";

            string Path = Server.MapPath("~/Excel/" + FileName);

            //update the excel file from the database
            Helpers.WriteAutomationStatusToExcel(Path, TeamId);

            //Create a response stream to create and write the Excel file
            Response.Clear();
            Response.AddHeader("content-disposition", "attachment;filename=AutomationStatus.xlsx");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";

            Response.TransmitFile(Path);
            Response.End();
        }
        [NonAction]
        public static void UpdateWeeklyAutomationStatus(string FileName, int TeamId)
        {
            //Retrieve data from the excel file and store it in a dictionary
            Helpers.PopulateInCollectionFromExcel(FileName, TeamId);

            var db = new AutomationDashboardEntities();
            Dictionary<string, spGetCurrentStatusOfAutomationForATeam_Result> CurrentStatus = db.spGetCurrentStatusOfAutomationForATeam(TeamId).ToDictionary(e => e.ProductArea);
            List<AutomateData> RowsToBeInserted = new List<AutomateData>();
            List<WeeklyTriaging> WeeklyTriagingToBeInserted = new List<WeeklyTriaging>();
            var CurrentFriday = Helpers.GetPreviousFriday(DateTime.Now).AddDays(7);

            //Filter out the rows to be inserted
            foreach (var record in Helpers.automateData)
            {
                if (!CurrentStatus.ContainsKey(record.Key))
                {
                    //Insert this data in the database as this is a new entry which does not exist in the database
                    RowsToBeInserted.Add(record.Value);
                }
                else
                {
                    //check if there is progress in the field and insert only those fields in the database

                    spGetCurrentStatusOfAutomationForATeam_Result StatusFromDatabase = CurrentStatus[record.Key];
                    AutomateData StatusFromUser = record.Value;

                    if (StatusFromUser.NoOfAutomatedTestcases > StatusFromDatabase.NoOfAutomatedTestcases ||
                        StatusFromUser.NoOfTFSTestcase > StatusFromDatabase.NoOfTFSTestcase)
                    {
                        //only then select the record for insert
                        RowsToBeInserted.Add(StatusFromUser);
                    }
                }

                //check if any modifications is done to the triaging data in the excel
                var entry = db.WeeklyTriagings.FirstOrDefault(row => row.TeamId == TeamId && row.ProductArea == record.Key && row.Date == CurrentFriday.Date);

                if ((record.Value.NoOfTestCasesDebugged > 0 && entry == null) || (entry != null && entry.NoOfTestCasesDebugged < record.Value.NoOfTestCasesDebugged))
                {
                    WeeklyTriaging triaging = new WeeklyTriaging(TeamId, record.Key, CurrentFriday, record.Value.NoOfTestCasesDebugged);
                    WeeklyTriagingToBeInserted.Add(triaging);
                }
            }

            foreach (var record in RowsToBeInserted)
            {
                //if the record exists in the database then only update the existing one
                var entry = db.AutomateDatas.FirstOrDefault(row => row.TeamId == TeamId && row.ProductArea == record.ProductArea && row.date == record.date.Date);
                if (entry == null)
                    db.AutomateDatas.Add(record);
                else
                {
                    entry.NoOfAutomatedTestcases = record.NoOfAutomatedTestcases;
                    entry.NoOfTFSTestcase = record.NoOfTFSTestcase;
                    db.Entry(entry).State = EntityState.Modified;
                }

                db.SaveChanges();
            }

            foreach (var record in WeeklyTriagingToBeInserted)
            {
                var entry = db.WeeklyTriagings.FirstOrDefault(row => row.TeamId == TeamId && row.ProductArea == record.ProductArea && row.Date == CurrentFriday.Date);
                if (entry == null)
                    db.WeeklyTriagings.Add(record);
                else
                {
                    entry.NoOfTestCasesDebugged = record.NoOfTestCasesDebugged;
                    db.Entry(entry).State = EntityState.Modified;
                }
                db.SaveChanges();
            }
        }
        [HttpPost]
        public ActionResult UpdateTeamAutomationStatus(HttpPostedFileBase AutomationStatusExcelFile, int TeamId = 1)
        {
            if (AutomationStatusExcelFile != null)
            {
                string ExcelPath;
                if (TeamId == 1)
                    ExcelPath = Server.MapPath("~/Excel/Team" + TeamId + ".xlsx");
                else
                    ExcelPath = Server.MapPath("~/Excel/Team2.xlsx");

                if (System.IO.File.Exists(ExcelPath))
                {
                    try
                    {
                        System.IO.File.Delete(ExcelPath);
                    }
                    catch
                    {
                        Debug.WriteLine("ExcelFile is in use... Cannot Delete it");
                    }
                }

                AutomationStatusExcelFile.SaveAs(ExcelPath);

                UpdateWeeklyAutomationStatus(ExcelPath, TeamId);
            }
            Session["TeamId"] = TeamId;
            UIAutomationAggregate model = new UIAutomationAggregate(TeamId);

            //clear the cache
            ClearApplicationCache();

            return View("~/Views/Report/Index.cshtml", model);
        }

        //[OutputCache(Duration = 600)]
        public ActionResult Landing()
        {
            var db = new AutomationDashboardEntities();
            List<TeamAutomationWeeklyProgress> model = new List<TeamAutomationWeeklyProgress>(20);
            foreach (int TeamId in db.ProductTeams.Select(col => col.TeamId))
            {
                TeamAutomationWeeklyProgress temp = new TeamAutomationWeeklyProgress(TeamId);
                model.Add(temp);
            }
            return View("~/Views/Report/Landing.cshtml", model);
        }
        //[OutputCache(Duration = 600)]
        public ActionResult Index()
        {
            int TeamId = 1;
            Session["TeamId"] = TeamId;
            UIAutomationAggregate model = new UIAutomationAggregate(TeamId);
            return View("~/Views/Report/Index.cshtml", model);
        }

        [HttpGet]
        //[OutputCache(Duration = 600, VaryByParam = "TeamId")]
        public ActionResult GetDataForTeam(int id = 1)
        {
            UIAutomationAggregate model = null;
            try
            {
                model = new UIAutomationAggregate(id);
            }
            catch (Exception e)
            {
                var path = Server.MapPath("~/Excel/ErrorLog.txt");
                System.IO.File.AppendAllText(path, e.ToString() + Environment.NewLine + Environment.NewLine);
                return RedirectToAction("Error");
            }
            return View("~/Views/Report/Index.cshtml", model);

        }
        [HttpGet]
        //[OutputCache(Duration = 600)]
        public ActionResult GetOptimusArchitecture()
        {
            string DocumentationPath = Server.MapPath("~/Excel/OptimusArchitecture.pdf");
            var fileStream = new FileStream(DocumentationPath, FileMode.Open, FileAccess.Read);
            var fsResult = new FileStreamResult(fileStream, "application/pdf");
            return fsResult;
        }
        [HttpPost]
        public ActionResult ViewResult(long ID, string Source)
        {
            byte[] binaryData = null;
            var db = new AutomationDashboardEntities();
            switch (Source)
            {
                case "DailySmokes":
                    binaryData = db.DailySmokes.Where(row => row.id == ID).First().detail_results;
                    break;
                case "CoreModSmokes":
                    binaryData = db.CoreModSmokes.Where(row => row.id == ID).First().detail_results;
                    break;
                case "FullPass":
                    binaryData = db.FullPasses.Where(row => row.id == ID).First().detail_results;
                    break;
            }

            var rm = new ReportModel(System.Text.Encoding.UTF8.GetString(binaryData));
            return View("~/Views/Report/ViewResult.cshtml", rm);
        }
        [OutputCache(Duration = 600, VaryByParam = "CurrentTeamId")]
        public ActionResult About(int CurrentTeamId)
        {
            ViewBag.Message = "Your application description page.";
            AutomationTeamModel model = new AutomationTeamModel(CurrentTeamId);
            return View("~/Views/Report/About.cshtml", model);
        }

        [AllowAnonymous]
        public ActionResult GetLatestData()
        {

            return View();
        }
     
        public ActionResult GetOptimusDocumentation()
        {
            string DocumentationPath = Server.MapPath("~/Excel/Getting Started- Optimus.pdf");
            var fileStream = new FileStream(DocumentationPath, FileMode.Open, FileAccess.Read);
            var fsResult = new FileStreamResult(fileStream, "application/pdf");
            return fsResult;
        }
        public ActionResult GetFlowChart()
        {
            string DocumentationPath = Server.MapPath("~/Excel/FlowChart.pdf");
            var fileStream = new FileStream(DocumentationPath, FileMode.Open, FileAccess.Read);
            var fsResult = new FileStreamResult(fileStream, "application/pdf");
            return fsResult;
        }

        public ActionResult GetParallelExecDocs()
        {
            string DocumentationPath = Server.MapPath("~/Excel/Parallel.pdf");
            var fileStream = new FileStream(DocumentationPath, FileMode.Open, FileAccess.Read);
            var fsResult = new FileStreamResult(fileStream, "application/pdf");
            return fsResult;
        }

        public ActionResult InitiateAutomation()
        {
            return View("~/Views/Report/InitiateAutomation.cshtml");
        }
    }
}