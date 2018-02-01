using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using CarDealer.Areas.Client.Models;
using CarDealer.Models;
using System.Threading.Tasks;

namespace CarDealer.Areas.Client.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: Client/Home
        public ActionResult Index()
        {
            ViewBag.ddlManufacture = new SelectList(context.Manufactures, "FullName", "FullName");
            ViewBag.ddlModel = new SelectList(context.CarModels, "Name", "Name");
            //var q = from m in context.CarModels join mf in context.Manufactures on m.ID equals mf.ManufactureId select new Dictionary<string,string> { { m.Name, mf.FullName } };
            //TempData["Link"] = new SelectList(q, "Key", "Value");
            ViewBag.ddlBodystyle = new SelectList(context.CarTypes, "Name", "Name");
            return View();
        }

        public ActionResult UpdateModel(string BrandName)
        {

            ViewBag.ddlModel = new SelectList(context.CarModels, "Name", "Name");
            if (!string.IsNullOrEmpty(BrandName))
            {
                ViewBag.ddlModel = new SelectList(context.CarModels.Where(c => c.ToManufacture.FullName == BrandName), "Name", "Name");
            }
            return PartialView();
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
        public ActionResult BlogPartial()
        {
            var q = (from n in context.News where n.Waranty == 1 select new BlogViewModel() { Content = n.Content, ViewCount = n.ViewCount, CreatedOn = n.CreatedOn, Topic = n.Topic, Title = n.Title, Image = n.Image, NewId = n.NewId, CreatedBy = n.CreatedBy }).ToList().Take(3);
                
              foreach(var n in q)
            {
                n.Commentcount = context.FeedBacks.Where(t => t.Type.Contains("New-" + n.NewId)).Count();
            }
                
            return PartialView(q);
        }
    }
}