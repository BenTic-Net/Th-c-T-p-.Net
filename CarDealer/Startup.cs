using System;
using CarDealer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

[assembly: OwinStartupAttribute(typeof(CarDealer.Startup))]
namespace CarDealer
{
    public partial class Startup
    {


        

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
            GetAllController();
            AddController();



        }

      
        public void GetAllController()
        {
            using (  ApplicationDbContext context = new ApplicationDbContext())
            {
                Assembly asm =  Assembly.GetAssembly(typeof(CarDealer.MvcApplication));

                var controlleractionlist = asm.GetTypes()
                    .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                    .SelectMany(type =>
                        type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                    .Where(m => !m
                        .GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true)
                        .Any())
                    .Select(x => new
                    {
                        Controller = x.DeclaringType.Name,
                        Action = x.Name,
                        ReturnType = x.ReturnType.Name,
                        Attributes = String.Join(",",
                            x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")))
                    })
                    .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();

                foreach (var item in controlleractionlist)
                {

                    if (item.Controller == "AccountController" || (!item.Action.Contains("Edit") &&
                                                                   !item.Action.Contains("Create") && !item.Action.Contains("Delete") &&
                                                                   !item.Action.Contains("Detail")) || item.Controller == "ManageController" || item.Action == "EditDetail" || item.Action == "DetailsCar" || item.Action == "DeleteConfirmed")
                        continue;
                    AspController itemAdd = context.AspControllers.Find(item.Controller, item.Action);
                    if (itemAdd == null)
                    {
                        itemAdd = new AspController {Controller = item.Controller, Action = item.Action};
                        context.AspControllers.Add(itemAdd);
                        var all =  context.AspControllers.GroupBy(c => c.Controller);
                        context.SaveChanges();
                       
                    }

                }
            }
          
            
        }
        

        public void AddController()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                //  var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                string rid = roleManager.FindByName("Admin").Id;
                var ctx = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)context).ObjectContext;
                ctx.ExecuteStoreCommand("DELETE FROM [AspRoleControllers] WHERE RoleID={0}", rid);
                var all = context.AspControllers.ToList();
                foreach (var item in all)
                {
                    string controller = item.Controller;
                    string action = item.Action;
                    context.AspRoleControllers.Add(new AspRoleController
                    {
                        RoleId = rid,
                        Controller = item.Controller,
                        Action = item.Action
                    });

                    if (context.AspRoleControllers.Find(rid, controller, "Index") == null) 
                    context.AspRoleControllers.Add(new AspRoleController
                    {
                        RoleId = rid,
                        Controller = controller,
                        Action = "Index"
                    });

                    if (controller == "CarsController" && action == "Details")
                    {
                        context.AspRoleControllers.Add(new AspRoleController
                        {
                            RoleId = rid,
                            Controller = controller,
                            Action = "DetailsCar"
                        });
                    }
                    else if (controller == "CarsController" && action == "Edit")
                    {
                        context.AspRoleControllers.Add(new AspRoleController
                        {
                            RoleId = rid,
                            Controller = controller,
                            Action = "EditDetail"
                        });
                    }

                    if (controller == "UsersController" && action == "Delete")
                    {
                        var c = context.AspRoleControllers.Find(rid, controller, "DeleteConfirmed");
                        if (c == null)
                        {
                            context.AspRoleControllers.Add(new AspRoleController
                            {
                                RoleId = rid,
                                Controller = controller,
                                Action = "DeleteConfirmed"
                            });
                        }
                    }

                    context.SaveChanges();
                }
            }

           
        }


        // In this method we will create default User roles and Admin user for login  
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

           


            // In Startup I am creating first Admin Role and creating a default Admin User   
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin role   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                 

                var user = new ApplicationUser();
                user.UserName = "shanu";
                user.Email = "syedshanumcain@gmail.com";

                string userPWD = "A@Z200711";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin  
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            //Get All Controller
           





            // creating Creating Manager role   
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);

            }

            // creating Creating Employee role   
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Employee";
                roleManager.Create(role);

            }
            
            
        }
    }

}
