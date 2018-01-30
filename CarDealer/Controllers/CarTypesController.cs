using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarDealer.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace CarDealer.Controllers
{
    [Authorize]
    public class CarTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        List<CarType> dsCate = new List<CarType>();


        


        void BuildTreeMenu(int? CateParent, int level)
        {
            var categories = db.CarTypes.Where(c => c.ParentId == CateParent).ToList();
            if (categories == null || categories.Count == 0)
                return;
            foreach (var cate in categories)
            {
                string space = "";
                for (int i = 0; i < level; i++)
                {
                    space += "|--";
                }
                space += "-";
                cate.Name = space + cate.Name;
                dsCate.Add(cate);
                BuildTreeMenu(cate.CarTypeId, level + 1);
                //categories.AddRange(BuildTreeMenu(cate.CateId, level++));
            }

            //return categories;
        }
        // GET: CarTypes
        public ActionResult Index()
        {
            var carTypes = db.CarTypes.Include(c => c.ToParent);
            return View(carTypes.ToList());
        }

        // GET: CarTypes/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarType carType = db.CarTypes.Find(id);
            if (carType == null)
            {
                return HttpNotFound();
            }
            return View(carType);
        }

        // GET: CarTypes/Create
        public ActionResult Create()
        {
            BuildTreeMenu(null, 0);
            //ViewBag.CateParentId = new SelectList(db.Categories, "CateId", "CateName");
            ViewBag.ParentId = new SelectList(dsCate, "CarTypeId", "Name");
            //ViewBag.Name = new SelectList(Param._TransWrt, "Key", "Value");
           // ViewBag.ParentId = new SelectList(db.CarTypes, "CarTypeId", "Name");
            return View();
        }

        // POST: CarTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CarTypeId,Name,MetaDescription,CreatedOn,ModifiedOn,CreatedBy,ModifiedBy,Metatitle,MetaKeywords,Status,ShowOnHome,SeoTitle,ParentId")] CarType carType)
        {

            carType.Waranty = 0;
            carType.CreatedBy = User.Identity.Name;
            carType.CreatedOn = DateTime.Now;
            

            if (ModelState.IsValid)
            {
                db.CarTypes.Add(carType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ParentId = new SelectList(db.CarTypes, "CarTypeId", "Name", carType.ParentId);
            return View(carType);
        }

        // GET: CarTypes/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BuildTreeMenu(null, 0);
            db=new ApplicationDbContext();            
            CarType carType = db.CarTypes.Find(id);
            if (carType == null)
            {
                return HttpNotFound();
            }

            ViewBag.Name = new SelectList(Param._TransWrt, "Key", "Value");
            
            
            //ViewBag.CateParentId = new SelectList(db.Categories, "CateId", "CateName");
            ViewBag.ParentId = new SelectList(dsCate, "CarTypeId", "Name", carType.ParentId);
            //ViewBag.ParentId = new SelectList(db.CarTypes, "CarTypeId", "Name", carType.ParentId);

            var Ur = User.Identity.GetUserId().GetUserRole();
            if (db.AspRoleControllers.Where(r => r.RoleId == Ur).Select(a => a.Action).Contains("EditStat"))
                ViewBag.showwrt = "Y";
            else ViewBag.showwrt = "N";


            return View(carType);
        }

        // POST: CarTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CarTypeId,Name,MetaDescription,CreatedOn,ModifiedOn,CreatedBy,ModifiedBy,Metatitle,MetaKeywords,Status,ShowOnHome,SeoTitle,ParentId")] CarType carType)
        {

            var Ur = User.Identity.GetUserId().GetUserRole();
            if (!db.AspRoleControllers.Where(r => r.RoleId == Ur).Select(a => a.Action).Contains("EditStat"))
                carType.Waranty = 0;
            if (ModelState.IsValid)
            {
                db.Entry(carType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ParentId = new SelectList(db.CarTypes, "CarTypeId", "Name", carType.ParentId);
            return View(carType);
        }

        // GET: CarTypes/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarType carType = db.CarTypes.Find(id);
            if (carType == null)
            {
                return HttpNotFound();
            }
            return View(carType);
        }

        // POST: CarTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(short id)
        {

            


            CarType carType = db.CarTypes.Find(id);

            var categories = db.CarTypes.Where(c => c.ParentId == carType.CarTypeId).ToList();
            if (categories == null || categories.Count != 0 || db.CarTypes.Include(c=>c.ToCars).Single(v=>v.CarTypeId==id) !=null)
            { 
            TempData["Msg"] = "DeleteFail Exist Child";
                return RedirectToAction("Index");
            }
            db.CarTypes.Remove(carType);
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
