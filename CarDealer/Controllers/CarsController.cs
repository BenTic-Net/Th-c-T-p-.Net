using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarDealer.Models;
using PagedList;
using Microsoft.AspNet.Identity;
using System.IO;

namespace CarDealer.Controllers
{
    [AuthAttribute]
    public class CarsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        
        // GET: Cars
        public ActionResult Index(string txtManufacture, string txtModelName, string txtBodyStyle,string txtName, int? Waranty, int?page,string Col,string Ord)
        {
            var cars = db.Cars.Include(c => c.ToCarModel).Include(c => c.ToCarType).Include(c=>c.ToCarDetail).Include(c=>c.ToCarModel.ToManufacture);
            ViewBag.Waranty = new SelectList(Param._TransWrt, "Key", "Value");
            if (!string.IsNullOrEmpty(txtManufacture))
                cars = cars.Where(c => c.ToCarModel.ToManufacture.FullName.Contains(txtManufacture));           
            if (!string.IsNullOrEmpty(txtBodyStyle))
                cars = cars.Where(c => c.ToCarType.Name.Contains(txtBodyStyle));
            if (Waranty != null)
                cars = cars.Where(c => c.Warranty == Waranty);
            if (Col == "Manufacture")
                if (Ord == "DESC")
                    cars = cars.OrderByDescending(c => c.ToCarModel.ToManufacture.FullName);
                else
                    cars = cars.OrderBy(c => c.ToCarModel.ToManufacture.FullName);
            else if (Col == "ModelName")
                if (Ord == "DESC")
                    cars = cars.OrderByDescending(c => c.ToCarModel.ToManufacture.FullName);
                else
                    cars = cars.OrderBy(c => c.ToCarModel.ToManufacture.FullName);
            else if (Col == "BodyStyle")
                if (Ord == "DESC")
                    cars = cars.OrderByDescending(c => c.ToCarModel.ToManufacture.FullName);
                else
                    cars = cars.OrderBy(c => c.ToCarModel.ToManufacture.FullName);
            else if (Col == "Name")
                if (Ord == "DESC")
                    cars = cars.OrderByDescending(c => c.ToCarModel.ToManufacture.FullName);
                else
                    cars = cars.OrderBy(c => c.ToCarModel.ToManufacture.FullName);
            else if (Col == "AskingPrice")
                if (Ord == "DESC")
                    cars = cars.OrderByDescending(c => c.ToCarModel.ToManufacture.FullName);
                else
                    cars = cars.OrderBy(c => c.ToCarModel.ToManufacture.FullName);
            else if (Col == "Quantity")
                if (Ord == "DESC")
                    cars = cars.OrderByDescending(c => c.ToCarModel.ToManufacture.FullName);
                else
                    cars = cars.OrderBy(c => c.ToCarModel.ToManufacture.FullName);
            else if (Col == "ViewCount")
                if (Ord == "DESC")
                    cars = cars.OrderByDescending(c => c.ToCarModel.ToManufacture.FullName);
                else
                    cars = cars.OrderBy(c => c.ToCarModel.ToManufacture.FullName);
            else cars = cars.OrderBy(c => c.CarId);
            return View(cars.ToPagedList(page??1, 5));
        }

        // GET: Cars/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // GET: Cars/Create
        public ActionResult Create()
        {
            ViewBag.ModelID = new SelectList(db.CarModels, "ID", "Name");
            ViewBag.CarTypeId = new SelectList(db.CarTypes, "CarTypeId", "Name");

            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CarId,CurentMile,ModelID,CarTypeId,CreatedTime,ModifiedTime,CreatedBy,ModifiedBy,Name,ThumpImage,AskingPrice,IncludedVAT,Quantity,Status,TopHot,ViewCount,Warranty,Discount,SellerNote")] Car car)
        {
            if (ModelState.IsValid)
            {
                car.CreatedTime=DateTime.Now;
                car.CreatedBy = User.Identity.Name;
                car.Warranty = 0;

                CarDetail cdt = new CarDetail();
                db.Cars.Add(car);
                db.CarDetails.Add(cdt);
                db.SaveChanges();
                return RedirectToAction("Edit",new { id=db.Cars.ToList().Last(c=>c.CreatedBy==User.Identity.Name).CarId});
            }

            ViewBag.ModelID = new SelectList(db.CarModels, "ID", "Name", car.ModelID);
            ViewBag.CarTypeId = new SelectList(db.CarTypes, "CarTypeId", "Name", car.CarTypeId);
            return View(car);
        }

        // GET: Cars/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            ViewBag.ModelID = new SelectList(db.CarModels, "ID", "Name", car.ModelID);
            ViewBag.CarTypeId = new SelectList(db.CarTypes, "CarTypeId", "Name", car.CarTypeId);
            ViewBag.Name = new SelectList(Param._TransWrt, "Key", "Value");
            var Ur = User.Identity.GetUserId().GetUserRole();
            if (db.AspRoleControllers.Where(r => r.RoleId == Ur).Select(a => a.Action).Contains("EditStat"))
                ViewBag.showwrt = "Y";
            else ViewBag.showwrt = "N";
            ViewBag.Carid = id;
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CarId,CurentMile,ModelID,CarTypeId,CreatedTime,ModifiedTime,CreatedBy,ModifiedBy,ThumpImage,Name,AskingPrice,IncludedVAT,Quantity,TopHot,ViewCount,Warranty,Discount,ShortNote")] Car car)
        {
            var Ur = User.Identity.GetUserId().GetUserRole();
            if (!db.AspRoleControllers.Where(r => r.RoleId == Ur).Select(a => a.Action).Contains("EditStat"))
                car.Warranty = 0;
          
            if (ModelState.IsValid)
            {
                car.ModifiedBy = User.Identity.Name;
                car.ModifiedTime = DateTime.Now;
                car.CreatedBy = db.Cars.AsNoTracking().Single(c => c.CarId == car.CarId).CreatedBy;
                car.CreatedTime = db.Cars.AsNoTracking().Single(c => c.CarId == car.CarId).CreatedTime;
                car.ViewCount = db.Cars.AsNoTracking().Single(c => c.CarId == car.CarId).ViewCount;
                car.ThumpImage = db.Cars.AsNoTracking().Single(c => c.CarId == car.CarId).ThumpImage;
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ModelID = new SelectList(db.CarModels, "ID", "Name", car.ModelID);
            ViewBag.CarTypeId = new SelectList(db.CarTypes, "CarTypeId", "Name", car.CarTypeId);
            return View(car);
        }


        public ActionResult DetailsCar(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarDetail carDt = db.CarDetails.Find(id);
            if (carDt == null)
            {
                return HttpNotFound();
            }
            return View(carDt);
        }

        public ActionResult EditDetail(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarDetail carDt = db.CarDetails.Find(id);
            if (carDt == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarId = id;
            return View(carDt);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDetail([Bind(Include = "CarId,FirstRegistrationDate,MoreImage,Feature,NumberOfSeat,NumberOfDoor,CupicCapacity,Horsepower,Cylider,FuelType,FuelConsumption,ModifiedOn,ModifiedBy,TranmisionType,EmissionClass,InteriorColor,ExteriorColor,WheelType,SellerNote")] CarDetail carDt)
        {

           
            if (ModelState.IsValid)
            {
                carDt.ModifiedBy = User.Identity.Name;
                carDt.ModifiedOn=DateTime.Now;

                //Car c = new Car { ThumpImage = "/Content/Upload/" + Session["path"] + "/_thumbs/Images/" };
                carDt.MoreImage = "/Content/Upload/" + Session["path"] + "images/";
                db.Entry(carDt).State = EntityState.Modified;
                //db.Entry(c).State = EntityState.Modified;
                string y = Session["path"].ToString();
                db.Cars.Find(carDt.CarId).ThumpImage = "/Content/Upload/" + Session["path"] + "_thumbs/Images/";

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CarId = carDt.CarId; 
            return View(carDt);
        }

        // GET: Cars/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = db.Cars.Find(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            ViewBag.CarId = id;
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id)
        {
            Car car = db.Cars.Find(id);
            CarDetail cdt = db.CarDetails.Find(id);
            db.Cars.Remove(car);
            db.CarDetails.Remove(cdt);
            string mappedPath = Server.MapPath(@"~/Content/Upload/" + Session["path"]);

            Directory.Delete(mappedPath, true);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
