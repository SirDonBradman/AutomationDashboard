//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AutomationDashboardMVC.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ActiveAutomationBuild
    {
        public string Build { get; set; }
        public int JobId { get; set; }
    
        public virtual ManagerJob ManagerJob { get; set; }
    }
}
