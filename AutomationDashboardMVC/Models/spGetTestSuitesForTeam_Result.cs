//------------------------------------------------------------------------------
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
    
    public partial class spGetTestSuitesForTeam_Result
    {
        public int SuiteId { get; set; }
        public string SuiteName { get; set; }
        public int TestPlanId { get; set; }
        public int ParentSuite { get; set; }
        public int TotalTests { get; set; }
        public int TotalAutomatedTests { get; set; }
        public int ExpectedTests { get; set; }
    }
}
