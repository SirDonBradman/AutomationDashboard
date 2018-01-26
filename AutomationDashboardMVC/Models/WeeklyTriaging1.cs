using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomationDashboardMVC.Models
{
    public partial class WeeklyTriaging
    {
        public WeeklyTriaging()
        {

        }
        public WeeklyTriaging(int TeamId, string ProductArea, DateTime Date, int? TestCasesDebugged)
        {
            this.TeamId = TeamId;
            this.ProductArea = ProductArea;
            this.Date = Date;
            this.NoOfTestCasesDebugged = TestCasesDebugged;
        }
    }
}