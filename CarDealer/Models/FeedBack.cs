//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CarDealer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class FeedBack
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [DataType(DataType.PhoneNumber, ErrorMessage = "Provided phone number not valid")]
        [RegularExpression(@"^(\d)$", ErrorMessage = "Wrong mobile")]
        [StringLength(13, MinimumLength = 10)]
        public string Phone { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.MultilineText)]
        [Required]
        public string Content { get; set; }
        public string Address { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> Status { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
