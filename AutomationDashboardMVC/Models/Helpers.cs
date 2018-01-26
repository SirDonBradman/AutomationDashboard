using Excel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

namespace AutomationDashboardMVC.Models
{
    public class Helpers
    {
        public static Dictionary<string, AutomateData> automateData = new Dictionary<string, AutomateData>();
        public static DataTable ReadDataFromExcel(string FileName)
        {
            FileStream stream = File.Open(FileName, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            excelReader.IsFirstRowAsColumnNames = true;
            DataSet result = excelReader.AsDataSet();
            DataTableCollection table = result.Tables;
            DataTable resultTable = table["Sheet1"];

            return resultTable;
        }
        public static DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }
        public static DateTime GetPreviousFriday(DateTime date)
        {
            date = date.Date;
            switch (date.DayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return date.AddDays(-2);
                case DayOfWeek.Monday:
                    return date.AddDays(-3);
                case DayOfWeek.Tuesday:
                    return date.AddDays(-4);
                case DayOfWeek.Wednesday:
                    return date.AddDays(-5);
                case DayOfWeek.Thursday:
                    return date.AddDays(-6);
                case DayOfWeek.Friday:
                    return date.AddDays(-7);
                case DayOfWeek.Saturday:
                    return date.AddDays(-1);
                default:
                    return date.AddDays(-1);
            }
        }
        static void ClearExcelFile(string ExcelFilePath, int RowsToBedeleted)
        {
            try
            {
                new FileInfo(ExcelFilePath) { IsReadOnly = false };
                ExcelPackage Browser = new ExcelPackage(new FileInfo(ExcelFilePath));
                var BrowserWorkBook = Browser.Workbook;
                var BrowserWorkSheet = BrowserWorkBook.Worksheets[1];

                BrowserWorkSheet.DeleteRow(2, RowsToBedeleted + 5);
                Browser.Save();

                BrowserWorkSheet.Dispose();
            }
            catch (Exception)
            {
                Debug.WriteLine("Error occured in ClearExcelFile");
            }
        }

        static void UpdateTriaging(int TeamId, List<spGetCurrentStatusOfAutomationForATeam_Result> automationStatus)
        {
            var CurrentFriday = GetPreviousFriday(DateTime.Now).AddDays(7);
            using (var db = new AutomationDashboardEntities())
            {
                foreach (var productArea in automationStatus)
                {
                    var entry = db.WeeklyTriagings.FirstOrDefault(row => row.TeamId == TeamId && row.ProductArea == productArea.ProductArea && row.Date == CurrentFriday);
                    if (entry==null)
                    {
                        productArea.NoOfTestCasesDebugged = 0;
                    }
                    else
                    {
                        productArea.NoOfTestCasesDebugged = entry.NoOfTestCasesDebugged;
                    }
                }
            }
        }
        public static void WriteAutomationStatusToExcel(string ExcelFilePath, int TeamId)
        {
            var db = new AutomationDashboardEntities();
            var automationStatus = db.spGetCurrentStatusOfAutomationForATeam(TeamId).ToList();
            UpdateTriaging(TeamId, automationStatus);

            //clear the excel file
            ClearExcelFile(ExcelFilePath, automationStatus.Count);

            try
            {
                new FileInfo(ExcelFilePath) { IsReadOnly = false };
                ExcelPackage Browser = new ExcelPackage(new FileInfo(ExcelFilePath));
                var BrowserWorkBook = Browser.Workbook;
                var BrowserWorkSheet = BrowserWorkBook.Worksheets[1];

                for (int i = 2; i <= automationStatus.Count + 1; i++)
                {
                    BrowserWorkSheet.Cells[i, 1].Value = automationStatus[i - 2].ProductArea;
                    BrowserWorkSheet.Cells[i, 2].Value = automationStatus[i - 2].NoOfTFSTestcase;
                    BrowserWorkSheet.Cells[i, 3].Value = automationStatus[i - 2].NoOfAutomatedTestcases;
                    BrowserWorkSheet.Cells[i, 4].Value = automationStatus[i - 2].NoOfTestCasesDebugged;
                    if (TeamId == 1)
                    {
                        BrowserWorkSheet.Cells[i, 5].Value = automationStatus[i - 2].ExpectedTestCases;
                    }
                    Browser.Save();
                }

                BrowserWorkSheet.Dispose();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error occured in ClearExcelFile \n"+e.ToString());
            }

        }
        public static void PopulateInCollectionFromExcel(string FileName, int TeamId)
        {
            //get the path of the file passed as parameter
            string path = Path.GetFullPath(FileName);

            automateData.Clear();

            //Start reading the data and populating the in memory list
            DataTable table = ReadDataFromExcel(FileName);

            for (int row = 0; row < table.Rows.Count; row++)
            {
                try
                {
                    var record = new AutomateData();

                    record.ProductArea = table.Rows[row][0].ToString();
                    if (record.ProductArea == "")
                        continue;
                    record.NoOfTFSTestcase = int.Parse(table.Rows[row][1].ToString());
                    record.NoOfAutomatedTestcases = int.Parse(table.Rows[row][2].ToString());
                    record.NoOfTestCasesDebugged = int.Parse(table.Rows[row][3].ToString());
                    record.TeamId = TeamId;
                    record.date = GetPreviousFriday(DateTime.Now).AddDays(7);
                    if (TeamId == 1)
                        record.ExpectedTestCases = int.Parse(table.Rows[row][4].ToString());
                    else
                        record.ExpectedTestCases = 0;

                    automateData.Add(record.ProductArea, record);
                }
                catch
                {
                    Debug.WriteLine("DataRow Not in Correct Format");
                }
            }
        }
    }
}