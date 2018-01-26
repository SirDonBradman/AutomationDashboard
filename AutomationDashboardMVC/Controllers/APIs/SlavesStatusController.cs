using AutomationDashboardMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AutomationDashboardMVC.Controllers
{
    public class SlavesStatusController : ApiController
    {
        public class SlavesStatus
        {
            public int Busy { get; set; }
            public int Idle { get; set; }
            public SlavesStatus(int Busy, int Idle)
            {
                this.Busy = Busy;
                this.Idle = Idle;
            }
        }
        public SlavesStatus Get()
        {
            using (var db = new AutomationDBEntities())
            {
                int busy = db.SlaveMachines.Count(machines => machines.Status == true);
                int idle = db.SlaveMachines.Count(machines => machines.Status == false);

                SlavesStatus status = new SlavesStatus(busy,idle);
                return status;
            }
        }
    }
}
