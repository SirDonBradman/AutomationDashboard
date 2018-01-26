using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutomationDashboardMVC.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;
using AutomationDashboardMVC.Utilities;

namespace AutomationDashboardMVC.Models
{
    public class UIAutomationAggregate
    {
        public int TotalTFSTests { get; private set; }
        public int TotalAutomatedTests { get; private set; }
        public List<spGetTestSuitesForTeam_Result> testSuites { get; set; }
        public List<DailySmoke> dailySmokes;
        public List<CoreModSmoke> CoreModSmokes;
        public List<FullPass> FullPass;
        public List<ProductTeam> productTeams;
        public ProductTeam CurrentTeam;
        public ApplicationUser CurrentUser;
        public UIAutomationAggregate(int TeamId)
        {
            var dbU = new ApplicationDbContext();
            CurrentUser = dbU.Users.FirstOrDefault(u => u.Email == HttpContext.Current.User.Identity.Name);

            var db = new AutomationDashboardEntities();
            var productTeam = db.ProductTeams.First(team => team.TeamId == TeamId);

            TotalTFSTests = productTeam.TotalTFSTests ?? 0;
            TotalAutomatedTests = productTeam.TotalAutomatedTests ?? 0;

            dailySmokes = db.DailySmokes.Where(e => e.TeamId == TeamId).OrderByDescending(column => column.Date).ToList();
            foreach (var smokeItem in dailySmokes)
            {
                smokeItem.detail_results = null;
            }
            FullPass = db.FullPasses.Where(e => e.TeamId == TeamId).OrderByDescending(column => column.Date).ToList();
            foreach (var fullPassReport in FullPass)
            {
                fullPassReport.detail_results = null;
            }
            testSuites = db.spGetTestSuitesForTeam(TeamId).OrderBy(col => col.SuiteName).ToList();

            CoreModSmokes = db.CoreModSmokes.Where(e => e.TeamId == TeamId).OrderByDescending(column => column.Date).ToList();
            foreach (var coreModSmokesReport in CoreModSmokes)
            {
                coreModSmokesReport.detail_results = null;
            }
            productTeams = db.ProductTeams.Where(t => t.TeamId > 0).ToList();
            CurrentTeam = db.ProductTeams.First(t => t.TeamId == TeamId);
        }
    }
}