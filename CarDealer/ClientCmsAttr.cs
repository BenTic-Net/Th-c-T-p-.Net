using CarDealer.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace CarDealer
{
    public class ClientCmsAttr: ActionFilterAttribute, IAuthenticationFilter
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
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                if (user == null  || user.Identity.IsAuthenticated==false)
                {

                    filterContext.Result = new HttpUnauthorizedResult();
                    return;
                    
                }
                if (context.Users.Find(user.Identity.GetUserId()).Roles.Count == 0)
                    filterContext.Result = new HttpUnauthorizedResult();
                return;
            }
        }
    }
}