using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutomationDashboardMVC.Models;
using System.Text;
using Newtonsoft.Json.Linq;

namespace AutomationDashboardMVC.Controllers
{
    public class AutomationController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var AllowedTypes = "Accepted values for automation are - fullpass dailysmokes coremodsmokes apidailysmokes withoutplanningsmokes {Individual Module ID's}";
            return Request.CreateResponse(HttpStatusCode.OK, AllowedTypes);
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody]JObject JOB)
        {
            try
            {
                int id;
                using (var db = new AutomationDBEntities())
                {
                    if (!db.ManagerJobs.Any())
                        id = 1;
                    else
                        id = db.ManagerJobs.Max(col => col.Id) + 1;

                    string automationRuntype = JOB["AutomationRunType"].ToString();
                    JOB.Property("AutomationRunType").Remove();
                    //var uploadModules = JOB.Property("UploadModules");
                    //if (uploadModules != null)
                    //    uploadModules.Value = false;
                    var importLibraryData = JOB.Property("ImportLibraryData");
                    if (importLibraryData != null)
                        importLibraryData.Value = false;

                    ManagerJob job = new ManagerJob()
                    {
                        Id = id,
                        Status = null,
                        AutomationRunType = automationRuntype,
                        Config = Encoding.UTF8.GetBytes(JOB.ToString())
                    };

                    string BuildURL = JOB["BuildURL"].ToString();

                    if (db.ActiveAutomationBuilds.Any(row => row.Build.Equals(BuildURL, StringComparison.CurrentCultureIgnoreCase)))
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, "Request cannot be accecpted at this time because another run is in progress on the specified build. For more details, please check the active automation runs on the dashboard or contact the Optimus Team.");
                    }
                    var activeAutomation = new ActiveAutomationBuild(job.Id, BuildURL);

                    db.ManagerJobs.Add(job);
                    db.Entry(job).State = System.Data.Entity.EntityState.Added;

                    db.ActiveAutomationBuilds.Add(activeAutomation);
                    db.Entry(activeAutomation).State = System.Data.Entity.EntityState.Added;

                    db.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Accepted, "JobId:"+job.Id);
                }
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.ToString());
            }
        }
    }
}
