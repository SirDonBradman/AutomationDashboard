using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutomationDashboardMVC.Models;

namespace AutomationDashboardMVC.Controllers
{
    public class TestPlansController : ApiController
    {
        // GET: api/TestPlans
        public IEnumerable<TestSuiteNode> Get(int id)
        {
            using (var db = new AutomationDashboardEntities())
            {
                var Suites = db.spGetTestSuitesForTeam(id).ToList();
                var Plans = db.TestPlans.Where(p => p.TeamId == id).ToList();

                Dictionary<int, TestSuiteNode> Nested = new Dictionary<int, TestSuiteNode>();
                Dictionary<int, TestSuiteNode> Flat = new Dictionary<int, TestSuiteNode>();

                foreach(var item in Plans)
                {
                    TestSuiteNode node = new TestSuiteNode()
                    {
                        text = item.TestPlanName + "  - TestPlan",
                    };
                    node.tags.Add(item.TestCaseCount.ToString());
                    Flat.Add(item.PlanId, node);
                    Nested.Add(item.PlanId, node);
                }

                while (Flat.Count < Suites.Count)
                {
                    foreach (var suiteEntry in Suites)
                    {
                        if (Flat.ContainsKey(suiteEntry.SuiteId) || (suiteEntry.ParentSuite!=-1 && !Flat.ContainsKey(suiteEntry.ParentSuite)))
                            continue;

                        TestSuiteNode node = new TestSuiteNode()
                        {
                            text = suiteEntry.SuiteName,
                        };
                        node.tags.Add(suiteEntry.TotalTests.ToString());
                        Flat.Add(suiteEntry.SuiteId, node);
                        if (suiteEntry.ParentSuite == -1)
                        {
                            Flat[suiteEntry.TestPlanId].nodes.Add(node);
                        }
                        else
                        {
                            Flat[suiteEntry.ParentSuite].nodes.Add(node);
                        }
                    }
                }

                foreach (var item in Flat.Values)
                {
                    if (item.nodes.Count == 0)
                    {
                        item.icon = "glyphicon glyphicon-leaf";
                    }
                }

                return Nested.Values.ToList();
            }
        }
    }
}
