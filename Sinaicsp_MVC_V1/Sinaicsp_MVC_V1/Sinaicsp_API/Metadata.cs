using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Sinaicsp_API
{
    public class SchoolMetadata
    {
        [Required(ErrorMessage ="*")]
        public string Name { get; set; }
    }
}