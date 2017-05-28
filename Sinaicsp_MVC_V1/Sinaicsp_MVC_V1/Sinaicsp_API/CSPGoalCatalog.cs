//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sinaicsp_API
{
    using System;
    using System.Collections.Generic;
    
    public partial class CSPGoalCatalog
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CSPGoalCatalog()
        {
            this.CSPGoalCatalogs = new HashSet<CSPGoalCatalog>();
        }
    
        public int Id { get; set; }
        public int CreatedByUserId { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string TextGoal { get; set; }
        public int CSPId { get; set; }
        public Nullable<int> ParentCSPGoalCatalogId { get; set; }
        public string DateInitiated { get; set; }
        public string Rate1 { get; set; }
        public string Rate2 { get; set; }
        public string Rate3 { get; set; }
        public int TextOrder { get; set; }
        public int SubTextOrder { get; set; }
    
        public virtual CSP CSP { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CSPGoalCatalog> CSPGoalCatalogs { get; set; }
        public virtual CSPGoalCatalog ParentCSPGoalCatalog { get; set; }
    }
}
