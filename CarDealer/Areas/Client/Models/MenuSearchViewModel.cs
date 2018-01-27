using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarDealer.Models;
namespace CarDealer.Areas.Client.Models
{
    public class MenuSearchViewModel
    {
        public List<Manufacture> Mnfct { get; set; }
        public List<CarModel> model { get; set; }
        public List<CarType> Bodystyle { get; set; }

    }
}