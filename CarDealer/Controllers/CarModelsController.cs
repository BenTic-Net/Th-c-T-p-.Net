using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarDealer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;
namespace CarDealer.Controllers
{
    [Authorize]
    public class CarModelsController : Controller
    {
        //private DataContext db = new DataContext();
        ApplicationDbContext context= new ApplicationDbContext();
        // GET: CarModels


        public ActionResult Index(string txtModel , int? Waranty, string Col, string Ord, int? page)
        {
            var carmodel = context.CarModels.Include(c=>c.ToManufacture);
            ViewBag.Waranty = new SelectList(Param._TransWrt, "Key", "Value");
            if (!string.IsNullOrEmpty(txtModel))
                carmodel = carmodel.Where(c => c.Name.Contains(txtModel));
            if (Waranty != null)
                carmodel = carmodel.Where(c => c.Waranty == Waranty);
            if (Col == "Name")
                if (Ord == "DESC")
                    carmodel = carmodel.OrderByDescending(c => c.Name);
                else carmodel = carmodel.OrderBy(c => c.Name);
            else if (Col == "ManufactureName")
                if (Ord == "DESC")
                    carmodel = carmodel.OrderByDescending(c => c.ToManufacture.FullName);
                else carmodel = carmodel.OrderBy(c => c.ToManufacture.FullName);

            else if (Col == "Waranty")
                if (Ord == "DESC")
                    carmodel = carmodel.OrderByDescending(c => c.Waranty);
                else carmodel = carmodel.OrderBy(c => c.Waranty);

           else carmodel = carmodel.OrderBy(c => c.ID);

            return View(carmodel.ToPagedList(page ?? 1,5));
        }



        // GET: CarModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarModel carModel = context.CarModels.Find(id);
            if (carModel == null)
            {
                return HttpNotFound();
            }
            return View(carModel);
        }

        // GET: CarModels/Create
        public ActionResult Create()
        {

            ViewBag.ManufactureId = new SelectList(context.Manufactures, "ManufactureId", "FullName");
            return View();
        }

        // POST: CarModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,MetaTitle,DisplayOrder,SeoTitle,CreatedDate,CreatedBy,ModifeeDate,ModifieBy,MetaKeywords,MetaDescriptions,Waranty,ManufactureId")] CarModel carModel)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            carModel.Waranty = 0;
            carModel.CreatedBy = User.Identity.Name;
            carModel.CreatedDate = DateTime.Now;


            if (ModelState.IsValid)
            {
                context.CarModels.Add(carModel);
                var tu = UserManager.FindById(User.Identity.GetUserId());
                ApplicationUser u = new ApplicationUser();
                if (tu != null)
                {                                        
                        u.Id = tu.Id;
                        u.PhoneNumber = tu.PhoneNumber;
                        u.Email = tu.Email;
                        u.UserName = tu.UserName;
                        u.AccessFailedCount = tu.AccessFailedCount;
                        u.EmailConfirmed = tu.EmailConfirmed;
                        u.LockoutEnabled = tu.LockoutEnabled;
                        u.LockoutEndDateUtc = tu.LockoutEndDateUtc;
                        u.PasswordHash = tu.PasswordHash;
                        u.PhoneNumberConfirmed = tu.PhoneNumberConfirmed;
                        u.SecurityStamp = tu.SecurityStamp;
                        u.TwoFactorEnabled = tu.TwoFactorEnabled;                    
                }

                context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(carModel);
        }

        // GET: CarModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarModel carModel = context.CarModels.Find(id);
            if (carModel == null)
            {
                return HttpNotFound();
            }

            ViewBag.Name = new SelectList(Param._TransWrt, "Key", "Value");
            ViewBag.ManufactureId = new SelectList(context.Manufactures, "ManufactureId", "FullName",carModel.ToManufacture.FullName);
            var Ur = User.Identity.GetUserId().GetUserRole();
            if (context.AspRoleControllers.Where(r => r.RoleId == Ur).Select(a => a.Action).Contains("EditStat"))
                ViewBag.showwrt = "Y";
            else ViewBag.showwrt = "N";
            return View(carModel);
        }

        // POST: CarModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,MetaTitle,DisplayOrder,SeoTitle,CreatedDate,CreatedBy,ModifeeDate,ModifieBy,MetaKeywords,MetaDescriptions,Waranty,ManufactureId")] CarModel carModel)
        {

            
            carModel.ModifeeDate=DateTime.Now;
            carModel.ModifieBy = User.Identity.Name;
            var Ur = User.Identity.GetUserId().GetUserRole();
            if (!context.AspRoleControllers.Where(r => r.RoleId == Ur).Select(a => a.Action).Contains("EditStat"))
                carModel.Waranty = 0;
            if (ModelState.IsValid)
            {
                context.Entry(carModel).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(carModel);
        }

        // GET: CarModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarModel carModel = context.CarModels.Find(id);
            if (carModel == null)
            {
                return HttpNotFound();
            }
            return View(carModel);
        }

        // POST: CarModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            CarModel carModel = context.CarModels.Find(id);
            context.CarModels.Remove(carModel);
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
