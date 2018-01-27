using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarDealer.Models;
using CarDealer.Areas.Client.Models;
using System.Net;

namespace CarDealer.Areas.Client.Controllers
{
    public class CarDetailController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: Client/CarDetail
        public ActionResult Index( long? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var car = from c in context.Cars join cdt in context.CarDetails on c.CarId equals cdt.CarId where c.CarId == Id select new DetailCarViewModel() { Name = c.Name, AskingPrice = c.AskingPrice, DownPrice = c.AskingPrice - c.AskingPrice * c.Discount / 100, MoreImage = cdt.MoreImage, SellerNote = cdt.SellerNote, CupicCapacity=cdt.CupicCapacity, CurentMile=c.CurentMile, Cylider=cdt.Cylider, FuelConsumption=cdt.FuelConsumption, FuelType=cdt.FuelType, Horsepower=cdt.Horsepower, ModelName=c.ToCarModel.Name, NumberOfSeat=cdt.NumberOfSeat, TranmisionType= cdt.TranmisionType };

            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car.ToList()[0]);
        }
    }
}