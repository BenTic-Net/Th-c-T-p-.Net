using CarDealer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;

namespace CarDealer.Controllers
{
    // [Authorize(Roles ="Admin")]
    [Authorize]
    [AuthAttribute]
    public class UsersController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        //Check ForAdmin
        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                
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

        // GET: Users
        
        public ActionResult Index()
        {
            var alluser = context.Users.ToList();

            //Get All User with role
            var usersWithRoles = (from user in context.Users
                                  from userRole in user.Roles
                                  join role in context.Roles on userRole.RoleId equals
                                  role.Id
                                  select new UserViewModel()
                                  {
                                      Id = user.Id,
                                      UserName = user.UserName,
                                      PhoneNumber = user.PhoneNumber,
                                      Email = user.Email,
                                      Role = role.Name
                                  }).ToList();
            //
          //Get user without role
            var userwithoutrole = context.Users.Where(x => x.Roles.Count == 0);
                foreach(var u in userwithoutrole)
            {
                UserViewModel newu = new UserViewModel();
                newu.Id = u.Id;
                newu.UserName = u.UserName;
                newu.PhoneNumber = u.PhoneNumber;
                newu.Email = u.Email;
                newu.Role = "";
                usersWithRoles.Add(newu);
            }


            if (User.Identity.IsAuthenticated)
            { 
                var user = User.Identity;
                ViewBag.Name = user.Name;

                ViewBag.displayMenu = "No";

                if (isAdminUser())
                {
                    ViewBag.displayMenu = "Yes";
                }
                return View(usersWithRoles);
            }
            else
            {
                ViewBag.Name = "Not Logged IN";
            }
            return View();
            
        }

        public ActionResult Create()
        {
            ViewBag.Name = new SelectList(context.Roles.Where(u => !u.Name.Contains("Admin")).ToList(), "Name", "Name");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterViewModel model)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            /*if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    
                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);*/
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = UserManager.Create(user, model.Password);
                if (result.Succeeded)
                {
                  ///  await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771%20 Jump ; 
                    // Send an email with this link  
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);  
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);  
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");  
                    //Assign Role to user Here     
                    UserManager.AddToRole(user.Id, model.UserRoles);
                    //Ends Here   
                    return RedirectToAction("Index", "Users");
                }
                ViewBag.Name = new SelectList(context.Roles.Where(u => !u.Name.Contains("Admin"))
                                          .ToList(), "Name", "Name");
              //  AddErrors(result);
            }

            // If we got this far, something failed, redisplay form  
            return View(model);
        }


        //GET
        public ActionResult Edit(string id)
        {
           

            var thisuser = (from user in context.Users
                            from userRole in user.Roles
                            join role in context.Roles on userRole.RoleId equals
                            role.Id
                            select new UserViewModel()
                            {
                                Id = user.Id,
                                UserName = user.UserName,
                                Email = user.Email,
                                Role = role.Name,
                                PhoneNumber = user.PhoneNumber,
                                
                                  }).ToList();

            //Get user without role
            var userwithoutrole = context.Users.Where(x => x.Roles.Count == 0);
            foreach (var u in userwithoutrole)
            {
                UserViewModel newu = new UserViewModel();
                newu.Id = u.Id;
                newu.UserName = u.UserName;
                newu.PhoneNumber = u.PhoneNumber;
                newu.Email = u.Email;
                newu.Role = "";
                thisuser.Add(newu);
            }

            if (thisuser.Single(x => x.Id == id) != null)
            {
                ViewBag.ListRole = new SelectList(context.Roles.Where(u => !u.Name.Contains("Admin")).ToList(), "Name", "Name");
                return View(thisuser.Single(x => x.Id == id));
            }
            
            else return HttpNotFound();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = null)] UserViewModel u)
        {
            TempData["EditMsg"] = "Edit fail!";
            if (ModelState.IsValid)
            {
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                //Get user
                var edituser = UserManager.FindById(u.Id);
                //Get Role id
               // if (edituser.Roles.Count == 0)
               // {
                    UserManager.AddToRoles(u.Id, u.Role);
                  //  goto h;
                   
                //}

                //var oldroleid = edituser.Roles.SingleOrDefault().RoleId;
                //    //Get Role Name
                //    var oldrolename = roleManager.Roles.SingleOrDefault(x => x.Id == oldroleid).Name;
                //// Update it with the values from the view model            

                //if (u.Id != oldrolename && oldrolename != "Admin")
                //{
                //    UserManager.RemoveFromRole(u.Id, oldrolename);
                //    UserManager.AddToRoles(u.Id, u.Role);
                //}
                //h:
                edituser.UserName = u.UserName;
                edituser.Email = u.Email;
                edituser.PhoneNumber = u.PhoneNumber;
                edituser.LockoutEnabled = u.Lockout;
                edituser.LockoutEndDateUtc = u.LockoutEndDate;
                edituser.AccessFailedCount = u.AccessFailedCount;


               
                // Apply the changes if any to the db
               if( UserManager.Update(edituser).Succeeded)
                TempData["EditMsg"] = "Edit user success";
               else TempData["EditMsg"] = "Edit fail!";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {

            var thisuser = (from user in context.Users
                            from userRole in user.Roles
                            join role in context.Roles on userRole.RoleId equals
                            role.Id
                            select new UserViewModel()
                            {
                                Id = user.Id,
                                UserName = user.UserName,
                                Email = user.Email,
                                Role = role.Name,
                                PhoneNumber = user.PhoneNumber,

                            }).ToList();

            //Get user without role
            var userwithoutrole = context.Users.Where(x => x.Roles.Count == 0);
            foreach (var u in userwithoutrole)
            {
                UserViewModel newu = new UserViewModel();
                newu.Id = u.Id;
                newu.UserName = u.UserName;
                newu.PhoneNumber = u.PhoneNumber;
                newu.Email = u.Email;
                newu.Role = "";
                thisuser.Add(newu);
            }

            if (thisuser.Single(x => x.Id == id) != null)
            {
                ViewBag.ListRole = new SelectList(context.Roles.ToList(), "Name", "Name");
                return View(thisuser.Single(x => x.Id == id));
            }

            else return HttpNotFound();
            
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
           var deluser= UserManager.Users.SingleOrDefault(u => u.Id == id);
            


            UserManager.Delete(deluser);
           
            return RedirectToAction("Index");
        }


    }
}