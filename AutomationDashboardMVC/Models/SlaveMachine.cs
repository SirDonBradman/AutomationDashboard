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
    
    public partial class SlaveMachine
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SlaveMachine()
        {
            this.AutomationTasks = new HashSet<AutomationTask>();
        }
    
        public string SlaveId { get; set; }
        public bool Status { get; set; }
        public Nullable<bool> UpdateOptimus { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AutomationTask> AutomationTasks { get; set; }
    }
}
