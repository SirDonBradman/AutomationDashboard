using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutomationDashboardMVC.Utilities;
using AutomationDashboardMVC.Models;

namespace AutomationDashboardMVC.Controllers.APIs
{
    public class TFSController : ApiController
    {
        // POST: api/TFS/5
        public HttpResponseMessage Post(int id, [FromBody]TFSCredentials credentials)
        {
            try
            {
                //Get the latest suites for a team with the given credentials
                TFS.GetLatestTestSuites(id, credentials);
                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            catch(Exception)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, false);
            }
        }
    }
}
