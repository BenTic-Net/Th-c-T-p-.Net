using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
namespace CarDealer.Models
{
    public class UserViewModel
    {
        /*Id
Email
EmailConfirmed
PasswordHash
SecurityStamp
PhoneNumber
PhoneNumberConfirmed
TwoFactorEnabled
LockoutEndDateUtc
LockoutEnabled
AccessFailedCount
UserName
LockoutEndDateUtc	datetime	Checked
 */
       
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public int AccessFailedCount { get; set; }
        public string Role { get; set; }
        public string ImageProfile { get; set; }
        public string FullName { get; set; }
        
        public bool Lockout { get; set; }

        public DateTime? LockoutEndDate { get; set; }
    }
}