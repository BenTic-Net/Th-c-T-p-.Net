using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarDealer.Models;
using CarDealer.Areas.Client.Models;
using System.Net;
using Microsoft.AspNet.Identity;

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
            var car = from c in context.Cars join cdt in context.CarDetails on c.CarId equals cdt.CarId where c.CarId == Id select new DetailCarViewModel() { Name = c.Name, AskingPrice = c.AskingPrice, DownPrice = c.AskingPrice - c.AskingPrice * c.Discount / 100, MoreImage = cdt.MoreImage, SellerNote = cdt.SellerNote, CupicCapacity=cdt.CupicCapacity, CurentMile=c.CurentMile, Cylider=cdt.Cylider, FuelConsumption=cdt.FuelConsumption, FuelType=cdt.FuelType, Horsepower=cdt.Horsepower, ModelName=c.ToCarModel.Name, NumberOfSeat=cdt.NumberOfSeat, TranmisionType= cdt.TranmisionType ,Id=c.CarId, ThumpImage=c.ThumpImage};

            if (car == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.IsAuthenticated)
            {
                string uid = User.Identity.GetUserId();
                if (context.UFavorite.Where(c => c.CarId == car.FirstOrDefault().Id && c.UseriD == uid).Count() != 0)
                    ViewBag.check = "checked";
            }
            return View(car.ToList()[0]);
        }

        public ActionResult DealerPartial()
        {
            string dlr = context.Roles.Where(c => c.Name == "Employee").FirstOrDefault().Id;
            var Dl = context.Users.Where(u => u.Roles.FirstOrDefault().RoleId == dlr);
            var q = (from d in Dl select new ProfileViewModel() { Phonenumber = d.PhoneNumber, FullName = d.FullName, Email = d.Email, UImage = d.Image }).ToList();
            var rand = new Random();
            int r = rand.Next(0, q.Count());

            return View(q[r]);
        }

        public ActionResult FeedBackForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FeedBackForm([Bind(Include = "Content,Name,Email,Address,CreatedDate,ModifiedDate,Status,Type,Phone")]FeedBack feed)
        {
            if (User.Identity.IsAuthenticated)
            {
                feed.Name = User.Identity.Name;
                feed.Email = context.Users.Find(User.Identity.GetUserId()).Email;

            }

            if (!string.IsNullOrEmpty(feed.Name) || !string.IsNullOrEmpty(feed.Content) || !string.IsNullOrEmpty(feed.Email))
            {
                feed.CreatedDate = DateTime.Now;
                context.FeedBacks.Add(feed);
                context.SaveChanges();
                return Content("Success");
            }

            return Content("Fail");


        }

        public ActionResult RecentListedPartial ()
        {
            var q = (from car in context.Cars
                     where car.Warranty == 1
                     orderby car.CreatedTime descending
                     select new ShortCarViewModel()
                     {
                         AskingPrice = car.AskingPrice,
                         CurentMile = car.CurentMile,
                         Name = car.Name,
                         ThumpImage = car.ThumpImage,
                         ModelName=car.ToCarModel.Name,
                          ShortNote=car.ShortNote,
                          
                         id = car.CarId
                     }).ToList().Take(4);

            return PartialView(q);
        }




    }
}