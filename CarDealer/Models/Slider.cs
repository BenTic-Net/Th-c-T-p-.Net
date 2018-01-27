using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarDealer.Models
{
    public class Slider
    {
        [Key]
        public int SliderId { get; set; }
        public int Waranty { get; set; }
        public string Content { get; set; }
        public List<string> Image { get; set; }
        public string Type { get; set; }

    }
}