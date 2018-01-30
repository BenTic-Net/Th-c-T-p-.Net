using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarDealer.Models;
using CarDealer.Areas.Client.Models;
using PagedList;
using System.Net;

namespace CarDealer.Areas.Client.Controllers
{
    public class ListCarController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: Client/ListCar
        public ActionResult Index(int? page, string ddlModel, string ddlManufacture, string price, string NewOld, string Name)
        {
            var q = (from car in context.Cars
                     join model in context.CarModels on car.ModelID equals model.ID
                     join manu in context.Manufactures on model.ManufactureId equals manu.ManufactureId
                     where car.Warranty==1
                     select new ShortCarViewModel()
                     {
                         AskingPrice = car.AskingPrice,
                         CurentMile = car.CurentMile,
                         Manufacture = manu.FullName,
                         ModelName = model.Name,
                         Name = car.Name,
                         ShortNote = car.ShortNote,
                         ThumpImage = car.ThumpImage,
                         
                         id = car.CarId
                     }).ToList();
            if (!string.IsNullOrEmpty(ddlModel))
                q = q.Where(c => c.ModelName==ddlModel).ToList();
            if (!string.IsNullOrEmpty(ddlManufacture))
                q = q.Where(c => c.Manufacture==ddlManufacture).ToList();

            if (!string.IsNullOrEmpty(Name))
                q = q.Where(c => c.Name.Contains(Name)).ToList();
            if (!string.IsNullOrEmpty(price))
            {
                long min;
                long max;
                try
                {
                     min = Int64.Parse(price.Split(',')[0]);
                     max = Int64.Parse(price.Split(',')[1]);
                }
                catch { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
                q = q.Where(c => c.AskingPrice >= min && c.AskingPrice <= max).ToList();
            }
            if(!string.IsNullOrEmpty(NewOld))
            {
                if (NewOld == "New")
                    q = q.Where(c => c.CurentMile <= 5).ToList();
                else if (NewOld == "Old")
                    q = q.Where(c => c.CurentMile >= 5).ToList();
                else return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.Number = q.Count();
            q = q.OrderBy(c => c.id).ToList();
            return View(q.ToPagedList(page??1,2));
        }
        public ActionResult CarListPartial(int? page, string ddlModel, string ddlManufacture, string price, string NewOld)
        {
            var q = (from car in context.Cars
                     join model in context.CarModels on car.ModelID equals model.ID
                     join manu in context.Manufactures on model.ManufactureId equals manu.ManufactureId
                     where car.Warranty == 1
                     select new ShortCarViewModel()
                     {
                         AskingPrice = car.AskingPrice,
                         CurentMile = car.CurentMile,
                         Manufacture = manu.FullName,
                         ModelName = model.Name,
                         Name = car.Name,
                         ShortNote = car.ShortNote,
                         ThumpImage = car.ThumpImage,

                         id = car.CarId
                     }).ToList();
            if (!string.IsNullOrEmpty(ddlModel))
                q = q.Where(c => c.ModelName == ddlModel).ToList();
            if (!string.IsNullOrEmpty(ddlManufacture))
                q = q.Where(c => c.Manufacture == ddlManufacture).ToList();
            if (!string.IsNullOrEmpty(price))
            {
                long min;
                long max;
                try
                {
                    min = Int64.Parse(price.Split(',')[0]);
                    max = Int64.Parse(price.Split(',')[1]);
                }
                catch { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
                q = q.Where(c => c.AskingPrice >= min && c.AskingPrice <= max).ToList();
            }
            if (!string.IsNullOrEmpty(NewOld))
            {
                if (NewOld == "New")
                    q = q.Where(c => c.CurentMile <= 5).ToList();
                else if (NewOld == "Old")
                    q = q.Where(c => c.CurentMile >= 5).ToList();
                else return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ViewBag.Number = q.Count();

            return View(q.ToPagedList(page ?? 1, 2));
        }
        public ActionResult MenuSearchPartial()
        {
            ViewBag.ddlManufacture = new SelectList(context.Manufactures, "FullName", "FullName");
            ViewBag.ddlModel = new SelectList(context.CarModels, "Name", "Name");            
            return PartialView();
        }
        public ActionResult NewCarPartial()
        {

            var q = (from car in context.Cars
                    where car.Warranty==1 orderby car.CreatedTime descending
                     select new ShortCarViewModel()
                     {
                         AskingPrice = car.AskingPrice,
                         CurentMile = car.CurentMile,                        
                         Name = car.Name,                         
                         ThumpImage = car.ThumpImage,
                         
                         id = car.CarId
                     }).ToList().Take(4);
            return PartialView(q);
        }
    }
}