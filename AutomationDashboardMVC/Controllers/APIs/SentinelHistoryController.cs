using AutomationDashboardMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AutomationDashboardMVC.Controllers
{
    public class SentinelHistoryController : ApiController
    {
        public class Job
        {
            public int Id { get; set; }
            public string AutomationRunType { get; set; }
            public string Status { get; set; }
            public string TimeForCompletion { get; set; }
            public Job(int Id, string AutomationRunType, bool Status, string TimeForCompletion)
            {
                this.Id = Id;
                this.AutomationRunType = AutomationRunType;
                this.Status = Status ? "Finished" : "InProgress";
                this.TimeForCompletion = TimeForCompletion; 
            }
        }
        public List<Job> Get()
        {
            using (var db = new AutomationDBEntities())
            {
                var ManagerJobs = db.ManagerJobs.OrderByDescending(col => col.Id).ToList();
                List<Job> Jobs = new List<Job>();
                foreach (var job in ManagerJobs)
                {
                    Job j = new Job(job.Id, job.AutomationRunType, job.Status == true, job.TimeForCompletion);
                    Jobs.Add(j);
                }
                return Jobs;
            }
        }
    }
}
