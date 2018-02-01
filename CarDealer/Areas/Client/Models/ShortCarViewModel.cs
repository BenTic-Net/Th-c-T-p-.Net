using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarDealer.Areas.Client.Models
{
    public class ShortCarViewModel
    {
        [DataType(DataType.Currency)]
        public decimal? AskingPrice { get; set; }
        public long? id { get; set; }
        public Nullable<int> CurentMile { get; set; }
        public string Name { get; set; }
        public string ModelName { get; set; }
        public string ThumpImage { get; set; }
        [DataType(DataType.Currency)]
        public decimal? DownPrice { get; set; }
        public string ShortNote { get; set; }
        public string Manufacture { get; set; }
        public string Bodystyle { get; set; }
    }
}