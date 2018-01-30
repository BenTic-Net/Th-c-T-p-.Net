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
namespace CarDealer.Controllers
{
    [ClientCmsAttr]
    [AuthAttribute]
    public class FeedBacksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: FeedBacks
        public ActionResult Index(string type, bool? Readed, int? page)
        {
            var f = db.FeedBacks.Where(t => !t.Type.Contains("New")).ToList();
            ViewBag.type = new SelectList(Param._p, "Key", "Value");
            ViewBag.Readed = new SelectList(new Dictionary<bool, string> { { true, "Yes" }, { false, "No" } }, "Key", "Value");
            if (!string.IsNullOrEmpty(type))
                f = f.Where(t => t.Type == type).ToList();
            if (Readed != null)
                f = f.Where(t => t.Status == Readed).ToList();
            
            return View(f.OrderByDescending(d=>d.CreatedDate).ToPagedList(page??1,10));
        }

        // GET: FeedBacks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedBack feedBack = db.FeedBacks.Find(id);
            if (feedBack == null)
            {
                return HttpNotFound();
            }
            db.FeedBacks.Find(id).Status = true; db.SaveChanges();
            return View(feedBack);
        }

        //GET: FeedBacks/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //POST: FeedBacks/Create
        //To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,Name,Phone,Email,Content,Address,CreatedDate,ModifiedDate,Status,Type")] FeedBack feedBack)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.FeedBacks.Add(feedBack);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(feedBack);
        //}

        //GET: FeedBacks/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    FeedBack feedBack = db.FeedBacks.Find(id);
        //    if (feedBack == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(feedBack);
        //}

        //POST: FeedBacks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,Name,Phone,Email,Content,Address,CreatedDate,ModifiedDate,Status,Type")] FeedBack feedBack)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(feedBack).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(feedBack);
        //}

        // GET: FeedBacks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FeedBack feedBack = db.FeedBacks.Find(id);
            if (feedBack == null)
            {
                return HttpNotFound();
            }
            return View(feedBack);
        }

        // POST: FeedBacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FeedBack feedBack = db.FeedBacks.Find(id);
            db.FeedBacks.Remove(feedBack);
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
