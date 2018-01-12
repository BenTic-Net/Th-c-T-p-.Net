
using System;
using System.Collections.Generic;
using System.Linq;
using CarDealer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace CarDealer
{
    public class AuthAttribute : ActionFilterAttribute, IAuthenticationFilter
    {
        private bool _authenticate;
        


        public void OnAuthentication(AuthenticationContext filtercontext)

        {
            bool skipAuthorization = filtercontext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
                                     || filtercontext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);

            if (skipAuthorization)
            {
                return;
            }

            _authenticate = (filtercontext.ActionDescriptor.GetCustomAttributes(typeof(OverrideAuthenticationAttribute), true).Length == 0);
        }
        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
                                     || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);

            if (skipAuthorization)
            {
                return;
            }
            var user = filterContext.HttpContext.User;
            if(user==null || !user.Identity.IsAuthenticated(filterContext))
            {                                 
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        

       //

    }
    public static class FunctionExtend
    {
        public static bool IsAuthenticated(this IIdentity identity, AuthenticationChallengeContext filtercontext)
        {
            bool rs = false;
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                string userId = identity.GetUserId();
                ApplicationUser user = context.Users.Find(userId);
                if (user != null)
                {
                    foreach (var item in user.Roles)
                    {
                        
                        rs = IsSelected(item.RoleId, filtercontext.ActionDescriptor.ControllerDescriptor.ControllerName, filtercontext.ActionDescriptor.ActionName);
                        if (rs)
                        {
                            return rs;
                        }

                    }
                }
                else return rs;
            }
            return rs;
        }

        public static bool IsSelected(string role  , string controller, string action )
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var rolesController = context.AspRoleControllers.Find(role, controller + "Controller", action);
                return rolesController == null ? false : true;
            }
        }
        public static bool IsSelected(this AspController ctrl, string roleId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var rolesController = context.AspRoleControllers.Find(roleId,ctrl.Controller, ctrl.Action );
                return rolesController == null ? false : true;
            }
        }

        public static List<string> CheckViewCtrl(this string userid)
        {
            List<string> ctrl=new List<string>();
            using (ApplicationDbContext context =new ApplicationDbContext())
            {
                ApplicationUser user = context.Users.Find(userid);
                foreach (var item in user.Roles)
                {
                    ctrl= context.AspRoleControllers.Where(r => r.RoleId == item.RoleId).Select(c => c.Controller).Distinct().ToList();
                }
                if(ctrl.Count!=0)
                return ctrl;
                return null;
            }
        }

        public static List<string> CheckViewAct(this string controller, string userid)
        {
            List<string> Act = new List<string>();
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                ApplicationUser user = context.Users.Find(userid);
                foreach (var item in user.Roles)
                {
                    Act=  context.AspRoleControllers.Where(r => r.RoleId == item.RoleId && r.Controller.Contains(controller)).Select(c => c.Action).Distinct().ToList();
                }
                if(Act.Count!=0)
                return Act;
                return null;
                /// if (ctrl.Count != 0)
                //       return ctrl;
                //  return null;
            }
        }
    }
}