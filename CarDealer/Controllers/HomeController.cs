using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarDealer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.IO;

namespace CarDealer.Controllers
{
    [ClientCmsAttr]
    public class HomeController : Controller
    {
        ApplicationDbContext context =new ApplicationDbContext();

       
                                 string BuildTreeView(int? id, int level,string type,IQueryable<Menu> Menus)
                                 {
                                     
                                     var SMenus = Menus.Where(c => (c.ParentId == id )).ToList();
                                          SMenus = SMenus.AsQueryable().OrderBy(c => c.DisplayOrder).ToList().Where(v=>v.Status==true).ToList();
                                     if (SMenus == null || level == 0 ||SMenus.Count()==0) return "";
                                     string result = "<ul name='level"+level+"'>";
                                     foreach (var m in SMenus)
                                     {
                                            result += "<li><a href="+m.Link+">" + m.Text+ "</a>";                   
                                        result+= BuildTreeView(m.ID, level + 1,type,Menus);
                                        result += "</li>";
                                     }
                                     result += "</ul>";
                                     return result;

                                 }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase img)
        {
            if(img!=null)
            {
                if (Directory.Exists(Server.MapPath("~/Content/Upload/User/" + User.Identity.Name)))
                {
                    Directory.Delete(Server.MapPath("~/Content/Upload/User/" + User.Identity.Name),true);
                    Directory.CreateDirectory(Server.MapPath("~/Content/Upload/User/" + User.Identity.Name));
                }
                img.SaveAs(Server.MapPath("~/Content/Upload/User/") + User.Identity.Name + "/" + img.FileName);
                context.Users.Find(User.Identity.GetUserId()).Image = "/Content/Upload/User/" + User.Identity.Name + "/" + img.FileName;context.SaveChanges();
                return Redirect(Request.UrlReferrer.ToString());
            }

            return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult MenuRender()
        {

            var Menus = context.Menus.Where(m=>m.Type=="Admin");
            string Usercontroller = "";
            string ur = User.Identity.GetUserId().GetUserRole();
            foreach (var c in context.AspRoleControllers.Where(r=>r.RoleId==ur).Select(r=>r.Controller))
            {
                
                Usercontroller += c.Replace("Controller", "") + '-';
            }
            foreach(var m in Menus)
            {
                if (!string.IsNullOrEmpty(m.Link))
                {

                    if (!Usercontroller.Contains(m.Link.Replace("/", string.Empty)))
                    {
                        m.Status = false;
                    }
                }
            }
            
            ViewBag.Menu = BuildTreeView(null, 1,"Admin",Menus);

            return PartialView("_MenuAdminPartial",
                context.AspRoleControllers.ToList());            
        }

        public ActionResult Index()
        {

            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}