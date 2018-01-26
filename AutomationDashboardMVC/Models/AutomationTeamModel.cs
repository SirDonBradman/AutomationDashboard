using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomationDashboardMVC.Models
{
    public class AutomationTeamModel
    {
        public AutomateData automateData;
        public AutomationTeamModel(int TeamId)
        {
            var db = new AutomationDashboardEntities();

            automateData = db.AutomateDatas.Where(e => e.TeamId == TeamId).First();
        }
    }
}