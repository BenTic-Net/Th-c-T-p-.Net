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

namespace CarDealer.Controllers
{
    public class AboutsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Abouts
        public ActionResult Index()
        {
            return View(db.Abouts.ToList());
        }

        // GET: Abouts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            About about = db.Abouts.Find(id);
            if (about == null)
            {
                return HttpNotFound();
            }
            return View(about);
        }

        // GET: Abouts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Abouts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,MetaTitle,Description,Detail,CreatedDate,CreatedBy,ModifeeDate,ModifieBy,MetaKeywords,MetaDescriptions,Status")] About about)
        {
            if (ModelState.IsValid)
            {
                about.CreatedBy = User.Identity.Name;
                about.CreatedDate = DateTime.Now;
                about.Status = false;

                db.Abouts.Add(about);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(about);
        }

        // GET: Abouts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            About about = db.Abouts.Find(id);
            if (about == null)
            {
                return HttpNotFound();
            }

            var Ur = User.Identity.GetUserId().GetUserRole();
            if (db.AspRoleControllers.Where(r => r.RoleId == Ur).Select(a => a.Action).Contains("EditStat"))
                ViewBag.showwrt = "Y";
            else ViewBag.showwrt = "N";

            return View(about);
        }

        // POST: Abouts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,MetaTitle,Description,Detail,CreatedDate,CreatedBy,ModifeeDate,ModifieBy,MetaKeywords,MetaDescriptions,Status")] About about)
        {

            var Ur = User.Identity.GetUserId().GetUserRole();
            if (!db.AspRoleControllers.Where(r => r.RoleId == Ur).Select(a => a.Action).Contains("EditStat"))
                about.Status = false;


            if (ModelState.IsValid)
            {
                about.ModifeeDate = DateTime.Now;
                about.ModifieBy = User.Identity.Name;

                db.Entry(about).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(about);
        }

        // GET: Abouts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            About about = db.Abouts.Find(id);
            if (about == null)
            {
                return HttpNotFound();
            }
            return View(about);
        }

        // POST: Abouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            About about = db.Abouts.Find(id);
            db.Abouts.Remove(about);
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
