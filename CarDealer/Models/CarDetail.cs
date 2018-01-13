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

    public class CarDetail
    {
        [Key]
        public long CarId { get; set; }
        public Nullable<System.DateTime> FirstRegistrationDate { get; set; }
        public string MoreImage { get; set; }
        public string Feature { get; set; }
        public Nullable<byte> NumberOfSeat { get; set; }
        public Nullable<byte> NumberOfDoor { get; set; }
        public Nullable<byte> CupicCapacity { get; set; }
        public string Horsepower { get; set; }
        public Nullable<byte> Cylider { get; set; }
        public string FuelType { get; set; }
        
        public Nullable<byte> FuelConsumption { get; set; }
        //public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        //public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string TranmisionType { get; set; }
        public string EmissionClass { get; set; }
        public string InteriorColor { get; set; }
        public string ExteriorColor { get; set; }
        public string WheelType { get; set; }
        [DataType(DataType.MultilineText)]
        public string SellerNote { get; set; }

        // public virtual Car ToCar { get; set; }
    }
}