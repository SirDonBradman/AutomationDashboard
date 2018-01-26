using AutomationDashboardMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AutomationDashboardMVC.Controllers
{
    public class PrimeHistoryController : ApiController
    {
        public partial class Task
        {
            public int Id { get; set; }
            public int? JobId { get; set; }
            public string IsComplete { get; set; }
            public string Suite { get; set; }
            public string TimeForCompletion { get; set; }
            public string SlaveMachine { get; set; }
            public Task(int? JobId, bool IsComplete, string SlaveMachine, string SuiteName, string TimeForCompletion, int Id)
            {
                this.Id = Id;
                this.JobId = JobId;
                this.IsComplete = IsComplete ? "Completed" : "In Progress";
                this.SlaveMachine = SlaveMachine;
                this.Suite = SuiteName;
                this.TimeForCompletion = TimeForCompletion;
            }
        }
        public List<Task> Get()
        {
            using (var db = new AutomationDBEntities())
            {
                var AutomationTasks = db.AutomationTasks.OrderByDescending(col => col.JobId).OrderBy(col => col.IsComplete).ToList();
                List<Task> Tasks = new List<Task>();
                foreach(var task in AutomationTasks)
                {
                    Task t = new Task(task.JobId, task.IsComplete == true, task.SlaveId, task.SuiteName, task.TimeForCompletion, task.Id);
                    Tasks.Add(t);
                }
                return Tasks;
            }
        }
        public List<Task> Get(int id)
        {
            using (var db = new AutomationDBEntities())
            {
                var AutomationTasks = db.AutomationTasks.Where(rows => rows.JobId==id).OrderByDescending(col => col.JobId).OrderBy(col => col.IsComplete).ToList();
                List<Task> Tasks = new List<Task>();
                foreach (var task in AutomationTasks)
                {
                    Task t = new Task(task.JobId, task.IsComplete == true, task.SlaveId, task.SuiteName, task.TimeForCompletion, task.Id);
                    Tasks.Add(t);
                }
                return Tasks;
            }
        }
    }
}
