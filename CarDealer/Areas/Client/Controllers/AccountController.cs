﻿using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CarDealer.Models;
using System.Net;
using CarDealer.Areas.Client.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net.Mail;
using System.Collections.Generic;

namespace CarDealer.Areas.Client.Controllers
{
   


    [Authorize]
    //[ClientCmsAttr] 
    public class AccountController : Controller
    {
        
        
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        ApplicationDbContext context;

        [AllowAnonymous]
        public ActionResult UnAuthFav()
        {
            return View();
        }
       
        [AllowAnonymous]        
        private async Task SendmailAsync(string Sendto,string code)
        {
            var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress(Sendto));  // replace with valid value 
            message.From = new MailAddress("long231998@gmail.com");  // replace with valid value
            message.Subject = "Your email subject";
            message.Body =code;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                await smtp.SendMailAsync(message);
               
            }


            //using (var smtp = new SmtpClient())
            //{
            //    var credential = new NetworkCredential
            //    {
            //        UserName = "long231998@gmail.com",  // replace with valid value
            //        Password = "long1974"  // replace with valid value
            //    };
            //    smtp.Credentials = credential;
            //    smtp.Host = "smtp.gmail.com";
            //    smtp.Port = 465;
            //    smtp.EnableSsl = true;
            //    await smtp.SendMailAsync(message);

            //MailMessage mailMessage = new MailMessage();
            //MailAddress fromAddress = new MailAddress("long231998@gmail.com");
            //mailMessage.From = fromAddress;
            //mailMessage.To.Add(Sendto);
            //mailMessage.Body = "This is Testing Email Without Configured SMTP Server";
            //mailMessage.IsBodyHtml = true;
            //mailMessage.Subject = " Testing Email";
            //SmtpClient smtpClient = new SmtpClient();
            //smtpClient.Host = "localhost";
            //smtpClient.Send(mailMessage);        
        }
        [AllowAnonymous]
        public ActionResult SendMail()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> SendMail(string mailto)
        {
            if (context.Users.Where(u => u.Email == mailto).Count() > 0)
            {
                Random random = new Random();
                CodeGenerater code = new CodeGenerater();

                await SendmailAsync(mailto, code.GetCode());
                ViewBag.Msg = "Check Your Email For Verifitation Code";
                return RedirectToAction("VerifiEmail",new { email=mailto});
            }

            else { ViewBag.Msg = "Email not Registered"; return View(); }
        }
        [AllowAnonymous]
        public ActionResult VerifiEmail(string email)
        {
            ViewBag.Email = email;
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerifiEmail(string code,string email)
        {
            if(context.VerifyCodes.Find(code) !=null )

            {
                var User = UserManager.FindByEmail(email);
                if (User == null) return View();
                SignInManager.SignIn(User, isPersistent: false, rememberBrowser: false);
                return View("NewPassWord");
            }
            ViewBag.Msg = "Wrong Code";
            return JavaScript("location.reload()");
        }
        
        public ActionResult NewPassWord()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> NewPassWordAsync(CarDealer.Areas.Client.Models.SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("NewPassWord",model);
            }
            else
            {

                string uid = User.Identity.GetUserId();
                var user1 = context.Users.Single(u => u.Id == uid);
                string oldpass = user1.PasswordHash;
                var removepassResult = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());

                if (removepassResult.Succeeded)
                {
                    var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                        if (user != null)
                        {
                            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        }
                        return JavaScript("location.reload()");
                    }
                    AddErrors(result);

                }
                else { context.Users.Single(u => u.Id == User.Identity.GetUserId()).PasswordHash = oldpass;context.SaveChanges();ModelState.AddModelError("", "Some Error occur, Try again"); return View(model); }

            }
            return View(model);
        }
        public ActionResult UFavoriteCar()
       {
            if (!User.Identity.IsAuthenticated)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            string uid = User.Identity.GetUserId();
            var uf = (from c in context.Cars join f in context.UFavorite on c.CarId equals f.CarId where f.UseriD == uid select new UFavoriteViewModal() { Active = f.Active, CarName = c.Name, thumbImage = c.ThumpImage + "Car.jpg" , CarId=c.CarId}).ToList();
            ViewBag.count = uf.Count();
            return View(uf);
       }
        
        public ActionResult ManageFavorite(string ation)
        {
            if(!User.Identity.IsAuthenticated)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string act = ation.Split('-')[0];
            long CarId;
            try
            {
                string n = ation.Split('-')[1];
               CarId = Int64.Parse(n); }
            catch { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            string uid = User.Identity.GetUserId();

            if (act == "active" && context.UFavorite.Where(f => f.CarId == CarId && f.UseriD == uid) != null)
            {
                
                context.UFavorite.Where(f => f.CarId == CarId && f.UseriD ==uid ).First().Active = false;
                context.SaveChanges();
                return Content("Success");
            }
           if(act == "Deactive" && context.UFavorite.Where(f => f.CarId == CarId && f.UseriD == uid) != null)
            {
                
                context.UFavorite.Where(f => f.CarId == CarId && f.UseriD == uid).First().Active = true;
                context.SaveChanges();
                return Content("Success");
            }
            if(act=="add")
            {
                context.UFavorite.Add(new CarDealer.Models.Favorite() { Active = true, CarId = CarId, UseriD = uid });
                context.SaveChanges();
                return Content("Success");
            }
            if(act=="del")
            {
                var d = context.UFavorite.Where(c => c.CarId == CarId && c.UseriD == uid).First();
                context.UFavorite.Remove(d);
                context.SaveChanges();
                return Content("Success");
            }
            return Content("Fail");
;        }
        public ActionResult UserProfile()
        {
            if (!User.Identity.IsAuthenticated)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
           
            return View();
        }

        public ActionResult GeneralPrfPartial()
        {
            var user = context.Users.Find(User.Identity.GetUserId());
            ProfileViewModel profile = new ProfileViewModel() { Address = user.Address, Email = user.Email, FullName = user.FullName, Phonenumber = user.PhoneNumber };
            return View(profile);
        }
        [HttpPost]
        public ActionResult SaveProfile([Bind(Include = "Email,Address,FullName,Phonenumber")] ProfileViewModel profile)
        {
            if(ModelState.IsValid)
            {
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var u = UserManager.FindById(User.Identity.GetUserId());
                u.FullName = profile.FullName;
                u.Email = profile.Email;
                u.Address = profile.Address;
                u.PhoneNumber = profile.Phonenumber;

                if (UserManager.Update(u).Succeeded)
                    return Content("Update Success");
                return Content( UserManager.Update(u).Errors.ToString());
            }
            return View(profile);
        }
        public AccountController()
        {
            context = new ApplicationDbContext();
            
        }


        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
             
            ViewBag.ReturnUrl = returnUrl;
            return PartialView();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(CarDealer.Models.LoginViewModel model, string returnUrl)
        {
            
            /* if (!ModelState.IsValid)
             {
                 return View(model);
             }

             // This doesn't count login failures towards account lockout
             // To enable password failures to trigger account lockout, change to shouldLockout: true
             var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
             switch (result)
             {
                 case SignInStatus.Success:
                     return RedirectToLocal(returnUrl);
                 case SignInStatus.LockedOut:
                     return View("Lockout");
                 case SignInStatus.RequiresVerification:
                     return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                 case SignInStatus.Failure:
                 default:
                     ModelState.AddModelError("", "Invalid login attempt.");
                     return View(model);
             }*/
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            // This doesn't count login failures towards account lockout  
            // To enable password failures to trigger account lockout, change to shouldLockout: true  
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    {

                        return JavaScript("location.reload()");
                    }
                case SignInStatus.LockedOut:
                    {
                        ModelState.AddModelError("", "Account Locked.");return PartialView(model);
                    }
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    

                    return PartialView(model);
            }
        }

        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new CarDealer.Models.VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(CarDealer.Models.VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(CarDealer.Areas.Client.Models.RegisterViewModel model)
        {
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
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771%20 Jump ; 
                    // Send an email with this link  
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);  
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);  
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");  
                    //Assign Role to user Here     
                    //  await this.UserManager.AddToRoleAsync(user.Id, "Guest");
                    //Ends Here   
                    return JavaScript("location.reload()");
                }
               
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form  
            return PartialView(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(CarDealer.Models.ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(CarDealer.Models.ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new CarDealer.Models.SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(CarDealer.Models.SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new CarDealer.Models.ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(CarDealer.Models.ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}