using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarDealer.Models
{
    public class VerifyCode
    {
        [Key]
        public string Code { get; set; }
    }
}