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
    
    public partial class Inclusion
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Classes { get; set; }
        public string Teacher { get; set; }
        public string SessionStart { get; set; }
        public string SessionEnd { get; set; }
        public System.DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedByUserId { get; set; }
        public int StudentId { get; set; }
    
        public virtual Student Student { get; set; }
    }
}