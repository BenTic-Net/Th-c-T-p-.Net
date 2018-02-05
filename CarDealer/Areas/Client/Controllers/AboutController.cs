using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarDealer.Areas.Client.Controllers
{
    public class AboutController : Controller
    {
        // GET: Client/About
        public ActionResult Index()
        {
            CarDealer.Models.ApplicationDbContext context = new CarDealer.Models.ApplicationDbContext();


            return View(context.Abouts.FirstOrDefault());
        }
    }
}