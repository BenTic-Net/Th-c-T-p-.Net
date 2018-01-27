using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarDealer.Areas.Client.Models
{
    public class ProfileViewModel
    {


        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }  
        [DataType(DataType.MultilineText)]
        [Required]
        public string Address { get; set; }        
        public string FullName { get; set; }
        [DataType(DataType.PhoneNumber, ErrorMessage = "Provided phone number not valid")]
        [RegularExpression(@"^(\d)$", ErrorMessage = "Wrong mobile")]
        [StringLength(13, MinimumLength = 10)]
        public string Phonenumber { get; set; }
        public string UImage { get; set; }
    }
}