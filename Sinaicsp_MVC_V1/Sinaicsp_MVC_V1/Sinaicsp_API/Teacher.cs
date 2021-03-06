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
    
    public partial class Teacher
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Teacher()
        {
            this.TeacherCSPs = new HashSet<TeacherCSP>();
        }
    
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public System.DateTime CreationDate { get; set; }
        public Nullable<System.DateTime> LastLogin { get; set; }
        public bool IsDeleted { get; set; }
        public int SchoolId { get; set; }
        public string Title { get; set; }
        public int CreatedByUserId { get; set; }
    
        public virtual School School { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TeacherCSP> TeacherCSPs { get; set; }
    }
}
