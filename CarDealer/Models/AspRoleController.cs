using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarDealer.Models
{
    public class AspRoleController
    {
        [Key, Column(Order = 1)]
        public string RoleId { get; set; }
        [Key, Column(Order = 2)]
        public string Controller { get; set; }
        [Key, Column(Order = 3)]
        public string Action { get; set; }
        public string Description { get; set; }
        

        
    }
  
}