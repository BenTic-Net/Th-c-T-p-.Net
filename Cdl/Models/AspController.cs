using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CarDealer.Models
{
    public class AspController
    {
        [Key, Column(Order =1)]
        public string Controller { get; set; }
        [Key, Column(Order = 2)]
        public string Action { get; set; }
        public string Aria { get; set; }
        public string Description { get; set; }
        public byte IsDelete { get; set; }
       
    }
    public class ControllerModel
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Attrubutes  { get; set; }

    }
    public class ControllerRoleModel
    {
        public List<AspRoleController> ControllerSelecteds { get; set; }
        public List<AspController> Controllers { get; set; }
        public IdentityRole Role { get; set; }
    }
}