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