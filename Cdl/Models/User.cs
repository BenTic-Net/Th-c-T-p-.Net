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

    public class User
    {
        //Id nvarchar(128)   Unchecked
        //    Email   nvarchar(256)   Checked
        //    EmailConfirmed  bit Unchecked
        //PasswordHash nvarchar(MAX)   Checked
        //    SecurityStamp   nvarchar(MAX)   Checked
        //    PhoneNumber nvarchar(MAX)   Checked
        //    PhoneNumberConfirmed    bit Unchecked
        //TwoFactorEnabled bit Unchecked
        //    LockoutEndDateUtc   datetime Checked
        //LockoutEnabled bit Unchecked
        //    AccessFailedCount   int Unchecked
        //UserName nvarchar(256)   Unchecked


        //[Key]
        public string Id { get; set; }

        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int? AccessFailedCount { get; set; }
        public string UserName { get; set; }
    }
}
