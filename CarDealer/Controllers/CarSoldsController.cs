using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using CarDealer.Models;
using PagedList;
namespace CarDealer.Controllers
{
    [ClientCmsAttr]
    [AuthAttribute]
    public class CarSoldsController : Controller
    {
       // private DataContext db = new DataContext();
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: CarSolds
        public ActionResult Index(string txtCarName, string txtCustomer,  DateTime? DtDatesold, int? page, string Col, string Ord, string ddlPayMeth,int? ddlPayStat)
        {

            var carSolds = context.CarSolds.Include(c => c.ToCar);
           
            ViewBag.ddlPayStat = new SelectList(Param._TransPayment, "Key", "Value");
            ViewBag.ddlPayMeth = new SelectList(Param._TransPayMeth, "Key", "Value");
            if (!string.IsNullOrEmpty(txtCarName))
                carSolds = carSolds.Where(c => c.ToCar.Name.Contains(txtCarName));
            if (!string.IsNullOrEmpty(txtCustomer))
                carSolds = carSolds.Where(c => c.Name.Contains(txtCustomer));
            if (DtDatesold != null)
            {
                carSolds = carSolds.Where(c => c.Datesold == DtDatesold);
            }
            if (!string.IsNullOrEmpty(ddlPayMeth))
                carSolds = carSolds.Where(c => c.PaymentMethod == ddlPayMeth);
            if (ddlPayStat != null)
                carSolds = carSolds.Where(c => c.PaymentStatus == ddlPayStat);

            if (Col == "CarName")
                if (Ord == "DESC")
                    carSolds = carSolds.OrderByDescending(c => c.ToCar.Name);
                else carSolds = carSolds.OrderBy(c => c.ToCar.Name);

            else if (Col == "Customer")
                if (Ord == "DESC")
                    carSolds = carSolds.OrderByDescending(c => c.Name);
                else carSolds = carSolds.OrderBy(c => c.Name);
            else if (Col == "AgreedPrice")
                if (Ord == "DESC")
                    carSolds = carSolds.OrderByDescending(c => c.AgreedPrice);
                else carSolds = carSolds.OrderBy(c => c.AgreedPrice);
            else if (Col == "Datesold")
                if (Ord == "DESC")
                    carSolds = carSolds.OrderByDescending(c => c.Datesold);
                else carSolds = carSolds.OrderBy(c => c.Datesold);

            else carSolds = carSolds.OrderBy(c => c.CarId);

            return View(carSolds.ToPagedList(page??1, 5));
        }

        // GET: CarSolds/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarSold carSold = context.CarSolds.Find(id);
            if (carSold == null)
            {
                return HttpNotFound();
            }
            return View(carSold);
        }

        // GET: CarSolds/Create
        public ActionResult Create()
        {
            ViewBag.CarId = new SelectList(context.Cars, "CarId", "Name");
            ViewBag.UserId = new SelectList(context.Users, "Id","UserName");
            ViewBag.PaymentStatus = new SelectList(Param._TransPayment, "Key", "Value");
            return View();
        }

        // POST: CarSolds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CarSoldId,CarId,UserId,Name,Email,PhoneNumber,AgreedPrice,Datesold,PaymentStatus,PaymentMethod,PaymentStartDate,PaymentEndDate,ActurePaymentAmount,OtherDetail")] CarSold carSold)
        {

            if (!string.IsNullOrEmpty(carSold.UserId))
            {
                if (string.IsNullOrEmpty(context.Users.Find(carSold.UserId).PhoneNumber))
                {
                    
                    TempData["Msg"] = "Customer Doesnt Have PhoneNumber";
                    ViewBag.CarId = new SelectList(context.Cars, "CarId", "Name", carSold.CarId);
                    ViewBag.UserId = new SelectList(context.Users, "Id", "UserName", carSold.UserId);
                    ViewBag.PaymentStatus = new SelectList(Param._TransPayment, "Key", "Value",carSold.PaymentStatus);
                    return View(carSold);
                }
                carSold.PhoneNumber = context.Users.Find(carSold.UserId).PhoneNumber;
                carSold.Email = context.Users.Find(carSold.UserId).Email;
                carSold.Name = context.Users.Find(carSold.UserId).UserName;
            }
            else if (string.IsNullOrEmpty(carSold.UserId) && (string.IsNullOrEmpty(carSold.Name) ||
                                                              string.IsNullOrEmpty(carSold.Email) ||
                                                              string.IsNullOrEmpty(carSold.PhoneNumber)))
              return  new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            if (context.Cars.Find(carSold.CarId).Quantity == 0 || context.Cars.Find(carSold.CarId).Warranty==0 || context.Cars.Find(carSold.CarId).Warranty == 2)
            {
                TempData["Msg"] = "Out of Order";
                ViewBag.CarId = new SelectList(context.Cars, "CarId", "Name", carSold.CarId);
                ViewBag.UserId = new SelectList(context.Users, "Id", "UserName", carSold.UserId);
                ViewBag.PaymentStatus = new SelectList(Param._TransPayment, "Key", "Value", carSold.PaymentStatus);
                return View(carSold);
            }

            if (ModelState.IsValid)
            {
                
                context.CarSolds.Add(carSold);
                Car c = context.Cars.Find(carSold.CarId);
                c.Quantity--;
                context.Entry(c).State = EntityState.Modified;                              
                context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CarId = new SelectList(context.Cars, "CarId", "Name", carSold.CarId);
            return View(carSold);
        }

        // GET: CarSolds/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarSold carSold = context.CarSolds.Find(id);
            if (carSold == null)
            {
                return HttpNotFound();
            }


            ViewBag.PaymentStatus = new SelectList(Param._TransPayment, "Key", "Value",carSold.PaymentStatus);
            ViewBag.CarId = new SelectList(context.Cars, "CarId", "Name", carSold.CarId);
            ViewBag.UserId = new SelectList(context.Users, "Id", "UserName", carSold.UserId);
            return View(carSold);
        }

        // POST: CarSolds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CarSoldId,CarId,UserId,Email,PhoneNumber,AgreedPrice,Datesold,PaymentStatus,PaymentMethod,PaymentStartDate,PaymentEndDate,ActurePaymentAmount,OtherDetail")] CarSold carSold)
        {


            if (context.Cars.Find(carSold.CarId).Quantity == 0 || context.Cars.Find(carSold.CarId).Warranty == 0 || context.Cars.Find(carSold.CarId).Warranty == 2)
            {
                TempData["Msg"] = "Out of Order";
                ViewBag.UserId = new SelectList(context.Users, "Id", "UserName", carSold.UserId);
                ViewBag.CarId = new SelectList(context.Cars, "CarId", "Name", carSold.CarId);
                ViewBag.PaymentStatus = new SelectList(Param._TransPayment, "Key", "Value", carSold.PaymentStatus);
                return View(carSold);
            }



            if (ModelState.IsValid)
            {
                
                context.Entry(carSold).State = EntityState.Modified;

                if (carSold.PaymentStatus == 3 && context.CarSolds.Single(c => c.CarSoldId == carSold.CarSoldId).PaymentStatus != 3)
                {
                    context.Cars.Single(c => c.CarId == carSold.CarId).Quantity++;

                    //context.Entry(oc).State = EntityState.Modified;
                   // context.SaveChanges();
                }

                context.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CarId = new SelectList(context.Cars, "CarId", "Name", carSold.CarId);
            ViewBag.PaymentStatus = new SelectList(Param._TransPayment, "Key", "Value");
            ViewBag.UserId = new SelectList(context.Users, "Id", "UserName", carSold.UserId);
            return View(carSold);
        }

        // GET: CarSolds/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarSold carSold = context.CarSolds.Find(id);
            if (carSold == null)
            {
                return HttpNotFound();
            }
            return View(carSold);
        }

        // POST: CarSolds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id)
        {
            CarSold carSold = context.CarSolds.Find(id);
            context.CarSolds.Remove(carSold);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
