﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AutomationDashboardMVC.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class AutomationDashboardEntities : DbContext
    {
        public AutomationDashboardEntities()
            : base("name=AutomationDashboardEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AutomateData> AutomateDatas { get; set; }
        public virtual DbSet<CoreModSmoke> CoreModSmokes { get; set; }
        public virtual DbSet<DailySmoke> DailySmokes { get; set; }
        public virtual DbSet<ExpectedTestsForSuite> ExpectedTestsForSuites { get; set; }
        public virtual DbSet<FullPass> FullPasses { get; set; }
        public virtual DbSet<ProductTeam> ProductTeams { get; set; }
        public virtual DbSet<TestPlan> TestPlans { get; set; }
        public virtual DbSet<TestSuite> TestSuites { get; set; }
        public virtual DbSet<WeeklyTriaging> WeeklyTriagings { get; set; }
    
        public virtual int spDeleteTestPlansForTeam(Nullable<int> teamId)
        {
            var teamIdParameter = teamId.HasValue ?
                new ObjectParameter("TeamId", teamId) :
                new ObjectParameter("TeamId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spDeleteTestPlansForTeam", teamIdParameter);
        }
    
        public virtual int spDeleteTestSuitesForTeam(Nullable<int> teamId)
        {
            var teamIdParameter = teamId.HasValue ?
                new ObjectParameter("TeamId", teamId) :
                new ObjectParameter("TeamId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("spDeleteTestSuitesForTeam", teamIdParameter);
        }
    
        public virtual ObjectResult<spGetAggregateWeeklyStatus_Result> spGetAggregateWeeklyStatus(Nullable<int> teamId)
        {
            var teamIdParameter = teamId.HasValue ?
                new ObjectParameter("TeamId", teamId) :
                new ObjectParameter("TeamId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetAggregateWeeklyStatus_Result>("spGetAggregateWeeklyStatus", teamIdParameter);
        }
    
        public virtual ObjectResult<spGetCurrentStatusOfAutomationForATeam_Result> spGetCurrentStatusOfAutomationForATeam(Nullable<int> teamId)
        {
            var teamIdParameter = teamId.HasValue ?
                new ObjectParameter("TeamId", teamId) :
                new ObjectParameter("TeamId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetCurrentStatusOfAutomationForATeam_Result>("spGetCurrentStatusOfAutomationForATeam", teamIdParameter);
        }
    
        public virtual ObjectResult<spGetPreviousAndCurrentWeekAutomationAggregateData_Result> spGetPreviousAndCurrentWeekAutomationAggregateData(Nullable<int> teamId)
        {
            var teamIdParameter = teamId.HasValue ?
                new ObjectParameter("TeamId", teamId) :
                new ObjectParameter("TeamId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetPreviousAndCurrentWeekAutomationAggregateData_Result>("spGetPreviousAndCurrentWeekAutomationAggregateData", teamIdParameter);
        }
    
        public virtual ObjectResult<spGetStatusOfAutomationForATeamWithSpecifiedDate_Result> spGetStatusOfAutomationForATeamWithSpecifiedDate(Nullable<int> teamId, Nullable<System.DateTime> date)
        {
            var teamIdParameter = teamId.HasValue ?
                new ObjectParameter("TeamId", teamId) :
                new ObjectParameter("TeamId", typeof(int));
    
            var dateParameter = date.HasValue ?
                new ObjectParameter("date", date) :
                new ObjectParameter("date", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetStatusOfAutomationForATeamWithSpecifiedDate_Result>("spGetStatusOfAutomationForATeamWithSpecifiedDate", teamIdParameter, dateParameter);
        }
    
        public virtual ObjectResult<spGetSuiteDetails_Result> spGetSuiteDetails(Nullable<int> suiteId)
        {
            var suiteIdParameter = suiteId.HasValue ?
                new ObjectParameter("suiteId", suiteId) :
                new ObjectParameter("suiteId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetSuiteDetails_Result>("spGetSuiteDetails", suiteIdParameter);
        }
    
        public virtual ObjectResult<spGetTestSuitesForTeam_Result> spGetTestSuitesForTeam(Nullable<int> teamId)
        {
            var teamIdParameter = teamId.HasValue ?
                new ObjectParameter("TeamId", teamId) :
                new ObjectParameter("TeamId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetTestSuitesForTeam_Result>("spGetTestSuitesForTeam", teamIdParameter);
        }
    
        public virtual ObjectResult<spGetWeeklyProgressForProductArea_Result> spGetWeeklyProgressForProductArea(string productArea, Nullable<int> teamId, Nullable<System.DateTime> from, Nullable<System.DateTime> to)
        {
            var productAreaParameter = productArea != null ?
                new ObjectParameter("productArea", productArea) :
                new ObjectParameter("productArea", typeof(string));
    
            var teamIdParameter = teamId.HasValue ?
                new ObjectParameter("teamId", teamId) :
                new ObjectParameter("teamId", typeof(int));
    
            var fromParameter = from.HasValue ?
                new ObjectParameter("from", from) :
                new ObjectParameter("from", typeof(System.DateTime));
    
            var toParameter = to.HasValue ?
                new ObjectParameter("to", to) :
                new ObjectParameter("to", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetWeeklyProgressForProductArea_Result>("spGetWeeklyProgressForProductArea", productAreaParameter, teamIdParameter, fromParameter, toParameter);
        }
    
        public virtual ObjectResult<spGetWeeklyProgressForProductAreaBetweenDates_Result> spGetWeeklyProgressForProductAreaBetweenDates(string productArea, Nullable<int> teamId, Nullable<System.DateTime> date1, Nullable<System.DateTime> date2)
        {
            var productAreaParameter = productArea != null ?
                new ObjectParameter("productArea", productArea) :
                new ObjectParameter("productArea", typeof(string));
    
            var teamIdParameter = teamId.HasValue ?
                new ObjectParameter("teamId", teamId) :
                new ObjectParameter("teamId", typeof(int));
    
            var date1Parameter = date1.HasValue ?
                new ObjectParameter("Date1", date1) :
                new ObjectParameter("Date1", typeof(System.DateTime));
    
            var date2Parameter = date2.HasValue ?
                new ObjectParameter("Date2", date2) :
                new ObjectParameter("Date2", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetWeeklyProgressForProductAreaBetweenDates_Result>("spGetWeeklyProgressForProductAreaBetweenDates", productAreaParameter, teamIdParameter, date1Parameter, date2Parameter);
        }
    
        public virtual ObjectResult<spGetWeeklyTriagingForWeekForGivenTeam_Result> spGetWeeklyTriagingForWeekForGivenTeam(Nullable<System.DateTime> currentFriday, Nullable<System.DateTime> previousFriday, Nullable<int> teamId)
        {
            var currentFridayParameter = currentFriday.HasValue ?
                new ObjectParameter("CurrentFriday", currentFriday) :
                new ObjectParameter("CurrentFriday", typeof(System.DateTime));
    
            var previousFridayParameter = previousFriday.HasValue ?
                new ObjectParameter("PreviousFriday", previousFriday) :
                new ObjectParameter("PreviousFriday", typeof(System.DateTime));
    
            var teamIdParameter = teamId.HasValue ?
                new ObjectParameter("TeamId", teamId) :
                new ObjectParameter("TeamId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<spGetWeeklyTriagingForWeekForGivenTeam_Result>("spGetWeeklyTriagingForWeekForGivenTeam", currentFridayParameter, previousFridayParameter, teamIdParameter);
        }
    }
}
