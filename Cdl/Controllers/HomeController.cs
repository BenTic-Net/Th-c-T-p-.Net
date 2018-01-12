using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarDealer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CarDealer.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        ApplicationDbContext context =new ApplicationDbContext();



        public ActionResult MenuRender()
        {
            
                ;
           
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