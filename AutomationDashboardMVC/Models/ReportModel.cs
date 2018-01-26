using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomationDashboardMVC.Models
{
    public class ReportModel
    {
        
        public string HTMLReport { get; set; }
        public ReportModel(string HTML)
        {
            HTMLReport = HTML;
        }
    }
}