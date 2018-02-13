using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarDealer.Models;
namespace CarDealer.Controllers
{
    public class MenusController : Controller
    {

        ApplicationDbContext context = new ApplicationDbContext();
        ushort LastLevel=new ushort();
        string BuildTreeView(int? id, int level,  IQueryable<Menu> Menus)
        {
           
            var SMenus = Menus.Where(c => (c.ParentId == id)).ToList();
            SMenus = SMenus.AsQueryable().OrderBy(c => c.DisplayOrder).ToList().Where(v => v.Status == true).ToList();
            if (SMenus == null || level == 0 || SMenus.Count() == 0) return "";
            LastLevel = (ushort)level;
            string result = "<ul name='level-" + level + "-parent-"+id+"'>";
            foreach (var m in SMenus)
            {
                result += "<li id='"+m.ID+"'><a href=" + m.Link + "><span>" + m.Text + "</span></a>";
                result += BuildTreeView(m.ID, level + 1,  Menus);
                result += "</li>";
            }
            result += "</ul>";
            return result;

        }

        // GET: Menus
        public ActionResult Edit(string type)
        {
            ViewBag.Menu = BuildTreeView(null, 1, context.Menus.Where(m => m.Type == "Admin"));
            ViewBag.LastLevel = LastLevel;
            return View();
        }
       [HttpPost]
        public ActionResult CreateLink(Menu m)
        {
          
            if (!string.IsNullOrEmpty(m.Text) && !string.IsNullOrEmpty(m.Type))
            {
                if (m.ParentId == 0)
                    m.ParentId = null;
                int? i=  context.Menus.Where(m1 => m1.ParentId == m.ParentId).Select(m2 => m2.DisplayOrder).OrderByDescending(c => c.Value).FirstOrDefault();
                if(i!=null)
                m.DisplayOrder = ++i;
                
                context.Menus.Add(m);
                context.SaveChanges();
                return Content("{Success}");
            }
            return Content("{Error}");
        }
        
        [HttpPost]
        public ActionResult EditLink(Menu m)
        {
            if (!string.IsNullOrEmpty(m.Text))
            {
                context.Menus.Single(mn => mn.ID == m.ID).Link=m.Link;
                context.Menus.Single(mn => mn.ID == m.ID).Text = m.Text;
                context.SaveChanges();
                return Content("Success");
            }
            return Content("Error");
    }
}