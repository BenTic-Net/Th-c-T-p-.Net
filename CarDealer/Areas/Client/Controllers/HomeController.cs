using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarDealer.Areas.Client.Models;
using CarDealer.Models;
namespace CarDealer.Areas.Client.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: Client/Home
        public ActionResult Index()
        {                       
            return View();
        }

        public ActionResult SearchFormPartial()
        {
            MenuSearchViewModel m = new MenuSearchViewModel();
            m.Mnfct = context.Manufactures.ToList();
            m.model = context.CarModels.ToList();
            m.Bodystyle = context.CarTypes.ToList();

            return PartialView(m);
        }
        public ActionResult NewCarPartial()
        {
            var newcar = (from car in context.Cars join cm in context.CarModels on car.ModelID equals cm.ID where car.CurentMile >= 0 && car.CurentMile <= 5 && car.Warranty==1 orderby car.CreatedTime select new ShortCarViewModel() { AskingPrice = car.AskingPrice, CurentMile = car.CurentMile, Name = car.Name, ModelName = cm.Name, ShortNote = car.ShortNote, ThumpImage = car.ThumpImage, id=car.CarId}).ToList().Take(6);
            return PartialView(newcar);
        }
        public ActionResult UsedCarPartial()
        {
            var Usedcar = (from car in context.Cars join cm in context.CarModels on car.ModelID equals cm.ID where car.CurentMile >=10 && car.Warranty == 1 orderby car.CurentMile ascending select new ShortCarViewModel() { AskingPrice = car.AskingPrice, CurentMile = car.CurentMile, Name = car.Name, ModelName = cm.Name, ShortNote = car.ShortNote, ThumpImage = car.ThumpImage, id = car.CarId }).ToList().Take(6);
            return PartialView(Usedcar);
        }

        public ActionResult SpecialOffer()
        {
            var Discountcar = (from car in context.Cars join cm in context.CarModels on car.ModelID equals cm.ID where car.Discount>0 && car.Warranty == 1 orderby car.Discount descending select new ShortCarViewModel() { AskingPrice = car.AskingPrice, DownPrice= car.AskingPrice-car.AskingPrice*car.Discount/100, CurentMile = car.CurentMile, Name = car.Name, ModelName = cm.Name, ShortNote = car.ShortNote, ThumpImage = car.ThumpImage, id = car.CarId }).ToList().Take(3);
            return PartialView(Discountcar);
        }

        public ActionResult TrendingSlidePartial()
        {
            return PartialView();
        }
        public ActionResult NewsPartial()
        {
            var q = context.News.OrderByDescending(c => c.CreatedOn).Take(3);
            return PartialView(q);
        }
    }
}