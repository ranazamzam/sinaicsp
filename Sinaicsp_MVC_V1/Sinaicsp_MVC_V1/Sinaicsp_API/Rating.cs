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
    
    public partial class Rating
    {
        public int Id { get; set; }
        public int CreatedByUserId { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string Description { get; set; }
        public string RateValue { get; set; }
        public int SchoolId { get; set; }
    
        public virtual School School { get; set; }
    }
}
