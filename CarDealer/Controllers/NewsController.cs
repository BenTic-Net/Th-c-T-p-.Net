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
using PagedList;
namespace CarDealer.Controllers
{
    [AuthAttribute]
    public class NewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        

        // GET: News
        public ActionResult Index(string txtTitle,string txtTopic,int? Waranty, int? page, string Ord, string Col)
        {
            var news = db.News.ToList();
            ViewBag.Waranty = new SelectList(Param._TransWrt, "Key", "Value");
            if (!string.IsNullOrEmpty(txtTitle))
                news = news.Where(n => n.Title.Contains(txtTitle)).ToList();
            if (!string.IsNullOrEmpty(txtTopic))
                news = news.Where(n => n.Topic.Contains(txtTopic)).ToList();
            if (Waranty != null)
                news = news.Where(n => n.Waranty == Waranty).ToList();
            if (Col == "Title")
                if (Ord == "DESC")
                    news = news.OrderByDescending(n => n.Title).ToList();
                else news = news.OrderBy(n => n.Title).ToList().ToList();
            else if (Col == "Topic")
                if (Ord == "DESC")
                    news = news.OrderByDescending(n => n.Topic).ToList();
                else news = news.OrderBy(n => n.Topic).ToList();
            else news = news.OrderBy(n => n.NewId).ToList();          
            return View(news.ToPagedList(page??1, 5));
        }

        // GET: News/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            New @new = db.News.Find(id);
            if (@new == null)
            {
                return HttpNotFound();
            }
            return View(@new);
        }

        // GET: News/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NewId,Title,Image,TopHot,ViewCount,CreatedOn,ModifiedOn,CreatedBy,ModifiedBy,Topic,Content,Waranty")] New @new)
        {

           
            if (ModelState.IsValid)
            {
                @new.Waranty = 0;
                @new.Image = "/Content/Upload" + Session["path"]+"/";
               
                @new.CreatedOn = DateTime.Now;
                @new.CreatedBy = User.Identity.GetUserName();
                db.News.Add(@new);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@new);
        }

        // GET: News/Edit/5
        public ActionResult Edit(long? id)
        {
         //   Param l=new Param();
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            New @new = db.News.Find(id);
            if (@new == null)
            {
                
                return HttpNotFound();
            }
            ViewBag.Name = new SelectList(Param._TransWrt, "Key", "Value");
            return View(@new);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NewId,Title,Image,TopHot,ViewCount,CreatedOn,ModifiedOn,CreatedBy,ModifiedBy,Topic,Waranty,Content")] New @new)
        {

            ViewBag.Name = new SelectList(Param._TransWrt, "Key", "Value");
            if (ModelState.IsValid)
            {
                @new.ModifiedOn = DateTime.Now;
                @new.ModifiedBy = User.Identity.GetUserName();
                db.Entry(@new).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@new);
        }

        // GET: News/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            New @new = db.News.Find(id);
            if (@new == null)
            {
                return HttpNotFound();
            }
            return View(@new);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id)
        {
            New @new = db.News.Find(id);
            db.News.Remove(@new);
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
