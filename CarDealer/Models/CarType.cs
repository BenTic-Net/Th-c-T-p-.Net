//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealer.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class CarType
    {
        [Key]
        public short CarTypeId { get; set; }
        
        [Display(Name ="Body Style")]
        [Required]
        public string Name { get; set; }
        public string MetaDescription { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Metatitle { get; set; }
        public string MetaKeywords { get; set; }
        public int Waranty { get; set; }  
        //public Nullable<bool> ShowOnHome { get; set; }
        public string SeoTitle { get; set; }    
        
        public short? ParentId { get; set; }

        public virtual ICollection<CarType> ToChild { get; set; }

        [ForeignKey("ParentId")]
        public virtual CarType ToParent { get; set; }

        public virtual ICollection<Car> ToCars { get; set; }
    }
}
