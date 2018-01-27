using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarDealer.Areas.Client.Models
{
    public class DetailCarViewModel
    {
        public long? Id { get; set; }
        [DataType(DataType.Currency)]
        public decimal? AskingPrice { get; set; }
        public Nullable<int> CurentMile { get; set; }
        public string Name { get; set; }
        public string ModelName { get; set; }
        public string ThumpImage { get; set; }
        [DataType(DataType.Currency)]
        public decimal? DownPrice { get; set; }        
        public string Manufacture { get; set; }

        [DataType(DataType.DateTime)]
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
        public Nullable<System.DateTime> ModifiedOn { get; set; }        
        public string ModifiedBy { get; set; }
        public string TranmisionType { get; set; }
        public string EmissionClass { get; set; }
        public string InteriorColor { get; set; }
        public string ExteriorColor { get; set; }
        public string WheelType { get; set; }
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string SellerNote { get; set; }
    }
}