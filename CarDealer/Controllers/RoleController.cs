using CarDealer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace CarDealer.Controllers
{
    [AuthAttribute]
    public class RoleController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult GetController()
        {
            Assembly asm = Assembly.GetAssembly(typeof(CarDealer.MvcApplication));

            var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                    .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                    .Select(x => new { Controller = x.DeclaringType.Name, Action = x.Name, ReturnType = x.ReturnType.Name, Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))) })
                    .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();

            foreach(var item in controlleractionlist)
            {

                if (item.Controller == "AccountController" ||( !item.Action.Contains("Edit") &&
                    !item.Action.Contains("Create") && !item.Action.Contains("Delete") &&
                    !item.Action.Contains("Detail") )|| item.Controller == "ManageController" || item.Action== "EditDetail" || item.Action == "DetailsCar")
                    continue;
                try
                {
                    AspController itemAdd = context.AspControllers.Find(item.Controller, item.Action);
                    if(itemAdd==null)
                    {
                        itemAdd = new AspController { Controller = item.Controller, Action = item.Action };
                        context.AspControllers.Add(itemAdd);
                        context.SaveChanges();
                    }

                    
                }

                catch (Exception)
                {
                    throw new Exception();
                }
            }

            

            return View(context.AspControllers.ToList());

        }

        public ActionResult EditRolesAtuthentication(string id)
        {
            var role = context.Roles.SingleOrDefault(x => x.Id == id);
            if(role!=null)
            {

                var controllerSelected = context.AspRoleControllers.Where(c => c.RoleId==id).ToList();
                var controller = context.AspControllers.ToList();
                ControllerRoleModel model = new ControllerRoleModel { Role = role, ControllerSelecteds = controllerSelected, Controllers = controller };
                TempData["RoleName"] = role.Name;
                return View(model);
            }

            
            return HttpNotFound();
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRolesAtuthentication(string userId, params string[] selectedController)
        {
            var ctx = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext;
            ctx.ExecuteStoreCommand("DELETE FROM [AspRoleControllers] WHERE RoleID={0}", userId);
            if(selectedController!=null)
            foreach (var item in selectedController)
            {


                string controller = item.Split('-')[0];
                string action = item.Split('-')[1];

                


                context.AspRoleControllers.Add(new AspRoleController { RoleId = userId, Controller = controller, Action = action });
                if (context.AspRoleControllers.Find(userId, controller, "Index")==null) 
                context.AspRoleControllers.Add(new AspRoleController
                {
                    RoleId = userId,
                    Controller = controller,
                    Action = "Index"
                });

                if (controller == "CarsController" && action == "Details")
                {
                    context.AspRoleControllers.Add(new AspRoleController
                    {
                        RoleId = userId,
                        Controller = controller,
                        Action = "DetailsCar"
                    });
                }
                else if (controller == "CarsController" && action == "Edit")
                {
                    context.AspRoleControllers.Add(new AspRoleController
                    {
                        RoleId = userId,
                        Controller = controller,
                        Action = "EditDetail"
                    });
                }

                if (controller == "UsersController" && action == "Delete")
                {
                    context.AspRoleControllers.Add(new AspRoleController
                    {
                        RoleId = userId,
                        Controller = controller,
                        Action = "DeleteConfirmed"
                    });
                }
                context.SaveChanges();
            }

            //var q= from rc in context.AspRoleControllers group by rc.
           

            return RedirectToAction("Index");
        }

        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
              //  ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s.Contains("Admin"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        // GET: Role
        public ActionResult Index()
        {
           

            var Roles = context.Roles.ToList();
            return View(Roles);
            
        }

        //GET
        [HttpPost]
        [ValidateAntiForgeryToken]
       public ActionResult Create(string newroll)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (!roleManager.RoleExists(newroll))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = newroll;
                roleManager.Create(role);
                TempData["Msg"] = "Create Success";
                return RedirectToAction("Index");
            }
            TempData["Msg"] = "Create fail";
            return RedirectToAction("Index");
        }
    }
}