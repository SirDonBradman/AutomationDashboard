using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutomationDashboardMVC.Models
{
    public class TestSuiteNode
    {
        public bool showTags { get; set; } = true;
        public string text { get; set; }
        public string icon { get; set; } = "glyphicon glyphicon-folder-open";
        public List<string> tags { get; set; } = new List<string>();
        public List<TestSuiteNode> nodes = new List<TestSuiteNode>();
    }
}