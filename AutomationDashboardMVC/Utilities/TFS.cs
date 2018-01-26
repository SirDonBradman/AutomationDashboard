using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.TestManagement.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System.Net;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.Client;
using AutomationDashboardMVC.Models;
using System.Data.Entity.Infrastructure;

namespace AutomationDashboardMVC.Utilities
{
    public class TFS : IDisposable
    {
        private static AutomationDashboardEntities db = null;
        private static TfsTeamProjectCollection _tfs;
        private static HashSet<int> getLatestInProgress;
        static TFS()
        {
            db = new AutomationDashboardEntities();
            getLatestInProgress = new HashSet<int>();
        }
        private static void Authenticate(TFSCredentials credentials)
        {
            NetworkCredential netCred = new NetworkCredential(credentials.username, credentials.password);
            Microsoft.VisualStudio.Services.Common.WindowsCredential winCred = new Microsoft.VisualStudio.Services.Common.WindowsCredential(netCred);
            VssCredentials vssCred = new VssClientCredentials(winCred);
            _tfs = new TfsTeamProjectCollection(new Uri("https://tfs.aurigo.com/tfs/DefaultCollection"), vssCred);
            _tfs.Authenticate();
        }
        private static void GetLatestSuites(int TeamId)
        {
            ITestManagementService test_service = (ITestManagementService)_tfs.GetService(typeof(ITestManagementService));
            string TeamName = db.ProductTeams.FirstOrDefault(team => team.TeamId == TeamId)?.TeamProject;
            var projectasd = test_service.GetTeamProject(TeamName);
            GetTestPlans(projectasd, TeamId);
        }

        private static void GetTestPlans(ITestManagementTeamProject testproject, int TeamId)
        {

            ITestPlanCollection plans = testproject.TestPlans.Query("Select * From TestPlan");

            List<TestPlan> testPlans = new List<TestPlan>();
            foreach (ITestPlan plan in plans)
            {
                TestPlan testPlan = new TestPlan();
                testPlan.TeamId = TeamId;
                testPlan.TestPlanName = plan.Name;
                testPlan.PlanId = plan.Id;
                testPlan.TestCaseCount = 0;

                if (plan.RootSuite != null && plan.RootSuite.Entries.Count > 0)
                {
                    var Suites = GetPlanSuites(plan.RootSuite.Entries);
                    foreach (var suite in Suites)
                    {
                        suite.TestPlan = testPlan;
                        testPlan.TestSuites.Add(suite);
                    }
                }
                testPlans.Add(testPlan);
            }

            foreach(var item in testPlans)
            {
                item.TestCaseCount = item.TestSuites.Where(s => s.ParentSuite==-1).Sum(col => col.TotalTests);
            }

            DeleteTestSuitesFromDB(TeamId);

            foreach (var testPlan in testPlans)
            {
                db.TestPlans.Add(testPlan);
                db.Entry(testPlan).State = System.Data.Entity.EntityState.Added;
            }
            db.SaveChanges();
        }

        private static List<TestSuite> GetPlanSuites(ITestSuiteEntryCollection suites, int parentSuiteId = -1)
        {
            List<TestSuite> Suites = new List<TestSuite>();
            foreach (ITestSuiteEntry suite_entry in suites)
            {
                var _newSuite = new TestSuite();
                _newSuite.SuiteName = suite_entry.Title;
                _newSuite.SuiteId = suite_entry.Id;
                _newSuite.ParentSuite = parentSuiteId;

                IStaticTestSuite suite = suite_entry.TestSuite as IStaticTestSuite;
                if (suite != null)
                {
                    _newSuite.TotalTests = suite.AllTestCases.Count;
                    int AutomatedTests = 0;
                    foreach(var test in suite.AllTestCases)
                    { 
                        var Fields = test.CustomFields;
                        if(Fields["Automation status"].Value.ToString().Equals("Automated", StringComparison.CurrentCultureIgnoreCase))
                        {
                            AutomatedTests++;
                        }
                    }
                    _newSuite.TotalAutomatedTests = AutomatedTests;
                    
                    Suites.Add(_newSuite);

                    if (suite.Entries.Count > 0)
                    {
                        var subSuites = GetPlanSuites(suite.Entries, _newSuite.SuiteId);
                        foreach (var subSuiteEntry in subSuites)
                        {
                            Suites.Add(subSuiteEntry);
                        }
                    }
                }
            }
            return Suites;
        }

        public static void GetLatestTestSuites(int TeamId, TFSCredentials credentials)
        {
            if (getLatestInProgress.Contains(TeamId))
                return;
            try
            {
                getLatestInProgress.Add(TeamId);
                Authenticate(credentials);
                GetLatestSuites(TeamId);
                UpdateTotalTestNumbers(TeamId);
            }
            finally
            {
                getLatestInProgress.Remove(TeamId);
            }
        }
        private static void DeleteTestSuitesFromDB(int TeamId)
        {
            foreach (var item in db.TestSuites)
                db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            foreach (var item in db.TestPlans)
                db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            db.Dispose();
            db = new AutomationDashboardEntities();
        }

        private static void UpdateTotalTestNumbers(int TeamId)
        {
            ITestManagementService test_service1 = (ITestManagementService)_tfs.GetService(typeof(ITestManagementService));
            var productTeam = db.ProductTeams.First(team => team.TeamId == TeamId);
            var projectasd1 = test_service1.GetTeamProject(productTeam.TeamProject);
            int TotalTFSTests = 0, TotalAutomatedTests = 0;
            foreach (var query in projectasd1.Queries)
            {
                if (query.Name == "TotalTFSTestCases")
                {
                    var TestCases = query.Execute();
                    TotalTFSTests = TestCases.Count();
                }
                if (query.Name == "TotalAutomatedTests")
                {
                    var TestCases = query.Execute();
                    TotalAutomatedTests = TestCases.Count();
                }
            }
            if (TotalTFSTests > 0)
                productTeam.TotalTFSTests = TotalTFSTests;

            if (TotalAutomatedTests > 0)
                productTeam.TotalAutomatedTests = TotalAutomatedTests;

            db.Entry(productTeam).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

        public void Dispose()
        {
            if (db != null)
                db.Dispose();
        }
    }
}