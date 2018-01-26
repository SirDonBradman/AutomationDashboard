using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomationDashboardMVC.Models
{
    public partial class ActiveAutomationBuild
    {
        public ActiveAutomationBuild(int JobId, string BuildId)
        {
            this.JobId = JobId;
            this.Build = BuildId;
        }
        public ActiveAutomationBuild()
        {
        }
    }
}