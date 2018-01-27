using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarDealer.Areas.Client.Models
{
    public class UFavoriteViewModal
    {
        public long? CarId { get; set; }
        public string CarName { get; set; }
        public string thumbImage { get; set; }
        public bool? Active { get; set; }    
    }
}