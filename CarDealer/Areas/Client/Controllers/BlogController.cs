using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarDealer.Models;
using CarDealer.Areas.Client.Models;
using System.Net;
using System.Text.RegularExpressions;

namespace CarDealer.Areas.Client.Controllers
{
    public class BlogController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: Client/Blog
        //public ActionResult Index()
        //{
        //    var q = (from n in context.News where n.Waranty == 1 select new BlogViewModel() { Content = n.Content, ViewCount = n.ViewCount, CreatedOn = n.CreatedOn, Topic = n.Topic, Title = n.Title, Image = n.Image, NewId = n.NewId, CreatedBy = n.CreatedBy }).ToList();
        //    List<string> Tag = new List<string>();
            
        //    foreach(var a in q)
        //    {
        //        foreach (var b in a.Topics)
        //            if (!Tag.Contains(b))
        //                Tag.Add(b);
        //    }

        //    ViewBag.TagList = Tag;
        //    return View(q);
        //}

        public ActionResult Index(string tag, string title)
        {

            

            var q = (from n in context.News where n.Waranty == 1 select new BlogViewModel() { Content = n.Content, ViewCount = n.ViewCount, CreatedOn = n.CreatedOn, Topic = n.Topic, Title = n.Title, Image = n.Image, NewId = n.NewId, CreatedBy = n.CreatedBy }).ToList().Take(5);
           
            foreach(var n in q)
            {
                List<string> t= Regex.Replace(n.Content, "<.*?>", String.Empty).Split(' ').Take(70).ToList();
                n.Content = string.Join(" ",t);
            }
            if(!string.IsNullOrEmpty(title))
            {
                q = q.Where(t => t.Title.Contains(title)).ToList() ;
            }
            if (!string.IsNullOrEmpty(tag))
            {
                q = q.Where(t => t.Topics.Contains(tag)).ToList();
            }
            foreach (var n in q)
            {
                n.Commentcount = context.FeedBacks.Where(t => t.Type.Contains("New-" + n.NewId)).Count();
            }
            return View(q);
        }

        public ActionResult GetData(int pageIndex, int pageSize)
        {

           

            var q = (from n in context.News where n.Waranty == 1 select new BlogViewModel() { Content = n.Content, ViewCount = n.ViewCount, CreatedOn = n.CreatedOn, Topic = n.Topic, Title = n.Title, Image = n.Image, NewId = n.NewId, CreatedBy = n.CreatedBy }).OrderBy(c=>c.NewId).Skip(pageIndex * pageSize).Take(pageSize).ToList();
            if (q.Count() == 0)
                return Json("Out", JsonRequestBehavior.AllowGet);
            foreach (var n in q)
            {
                List<string> t= Regex.Replace(n.Content, "<.*?>", String.Empty).Split(' ').Take(70).ToList();
                n.Content = string.Join(" ", t);
            }

            foreach (var n in q)
            {
                n.Commentcount = context.FeedBacks.Where(t => t.Type.Contains("New-" + n.NewId)).Count();
            }

            return Json(q.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Detail(long? id)
        {

            if (id==null) 
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (context.News.Find(id) == null)
                return HttpNotFound();
            var c = context.News.Find(id);
            var q = (from n in context.News where n.Waranty == 1 && n.NewId == id select new BlogViewModel() { Content = n.Content, ViewCount = n.ViewCount, CreatedOn = n.CreatedOn, Topic = n.Topic, Title = n.Title, CreatedBy = n.CreatedBy, NewId=n.NewId }).ToList()[0];
          
            return View(q);
        }   
        

        public ActionResult CommentPartial(long? id)
        {
            var q = context.FeedBacks.Where(f => f.Type.Contains("Comment") && f.Type.Contains("New-"+id));
            ViewBag.Cmcount = q.Count();
            return View(q);
        }


        public ActionResult PostComment()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PostComment([Bind(Include = "Content,Name,Email,Address,CreatedDate,ModifiedDate,Status,Type")]FeedBack feed)
        {
            
            if(User.Identity.IsAuthenticated)
            {
                feed.Name = User.Identity.Name;                
            }
         
            if(!string.IsNullOrEmpty(feed.Name) || !string.IsNullOrEmpty(feed.Content) || !string.IsNullOrEmpty(feed.Type))
            {
                feed.CreatedDate = DateTime.Now;                
                context.FeedBacks.Add(feed);
                context.SaveChanges();
                try
                {
                    long NewId = Int32.Parse(feed.Type.Split('-')[1]);
                    return RedirectToAction("CommentPartial", new { id = NewId });
                }
                catch { return Content("Fail"); }
                
            }
            
            return RedirectToAction("CommentPartial");


        }
        public ActionResult TagPartial()
        {

            var q1 = (from n in context.News where n.Waranty == 1 select new BlogViewModel() { Topic = n.Topic }).ToList();
            List<string> Tag = new List<string>();

            foreach (var a in q1)
            {
                foreach (var b in a.Topics)
                    if (!Tag.Contains(b))
                        Tag.Add(b);
            }
            ViewBag.TagList = Tag;
            return View();
        }
        
        public ActionResult RecentPostPartial()
        {
            var q = from n in context.News where n.Waranty == 1 orderby n.CreatedOn descending select new BlogViewModel() { Image = n.Image, Title = n.Title, NewId= n.NewId };
            return View(q.Take(5));
        }
    }
}