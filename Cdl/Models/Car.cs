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

    public class Car
    {
        [Key]
        public long CarId { get; set; }
        public Nullable<int> CurentMile { get; set; }
        public Nullable<int> ModelID { get; set; }
        public Nullable<short> CarTypeId { get; set; }
        public Nullable<System.DateTime> CreatedTime { get; set; }
        public Nullable<System.DateTime> ModifiedTime { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Name { get; set; }
        //public byte[] MetaTitle { get; set; }
        public string ThumpImage { get; set; }
        public Nullable<decimal> AskingPrice { get; set; }
        public Nullable<bool> IncludedVAT { get; set; }
        public Nullable<int> Quantity { get; set; }
        //public Nullable<bool> Status { get; set; }
        public Nullable<System.DateTime> TopHot { get; set; }
        public Nullable<int> ViewCount { get; set; }
        public int Warranty { get; set; }
        public Nullable<decimal> Discount { get; set; }
                                                                                
       
        
        [ForeignKey("ModelID")]
        public virtual CarModel ToCarModel { get; set; }

        [ForeignKey("CarTypeId")]
        public virtual CarType ToCarType { get; set; }

        
        public virtual CarDetail ToCarDetail { get; set; }
        
    }
}
