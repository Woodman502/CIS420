using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace asp.netmvc5.Models
{
    public class GrantManagerModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [Display(Name= "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [Display(Name = "Type")]
        public string Type { get; set; }
    }
}