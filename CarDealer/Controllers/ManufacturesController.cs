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

namespace CarDealer.Controllers
{
    [Authorize]
    public class ManufacturesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        ApplicationDbContext context=new ApplicationDbContext();
        // GET: Manufactures
        public ActionResult Index()
        {
            return View(db.Manufactures.ToList());
        }

        // GET: Manufactures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manufacture manufacture = db.Manufactures.Find(id);
            if (manufacture == null)
            {
                return HttpNotFound();
            }
            return View(manufacture);
        }

        // GET: Manufactures/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manufactures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ManufactureId,FullName,ShortName,Detail")] Manufacture manufacture)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            if (ModelState.IsValid)
            {
                db.Manufactures.Add(manufacture);
               // User UserManager.FindById(User.Identity.GetUserId();

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(manufacture);
        }

        // GET: Manufactures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manufacture manufacture = db.Manufactures.Find(id);
            if (manufacture == null)
            {
                return HttpNotFound();
            }
            return View(manufacture);
        }

        // POST: Manufactures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ManufactureId,FullName,ShortName,Detail")] Manufacture manufacture)
        {
            if (ModelState.IsValid)
            {
                db.Entry(manufacture).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(manufacture);
        }

        // GET: Manufactures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Manufacture manufacture = db.Manufactures.Find(id);
            if (manufacture == null)
            {
                return HttpNotFound();
            }
            
            return View(manufacture);
        }

        // POST: Manufactures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Manufacture manufacture = db.Manufactures.Find(id);
            if (db.Manufactures.Include(m => m.ToModels).Single(n=>n.ManufactureId==id).ToModels!= null)
            {
                ViewBag.Msg = "Must delete all model of this Brand first";
                return View();
            }
            db.Manufactures.Remove(manufacture);
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
