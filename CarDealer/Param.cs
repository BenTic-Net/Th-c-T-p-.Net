using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarDealer.Models;

namespace CarDealer
{
    public static class Param
    {
        /*private static*/ public static Dictionary<int,string> _TransWrt = new Dictionary<int, string> { {0,"waitting" }, { 1,"Show On Home"},{2,"Dispose" }};
       public static List<string> VrfCodes = new List<string>();
        public static Dictionary<int, string> _TransPayment = new Dictionary<int, string> { { 0, "Not Paid" }, { 1, "Paid" }, { 2, "Paying" }, { 3, "Dispose" } };
        public static Dictionary<string, string> _TransPayMeth = new Dictionary<string, string> { { "Part", "Part" }, { "On Hand", "On Hand" }};

        public static Dictionary<string, string> _p = new Dictionary<string, string> { { "MsgToDl", "Message To Dealer" }, { "RqMrInf", "Request More Info" }, { "MkOfr", "Make An Offer" } };
        public static Dictionary<int, string> TransWrt { get; set; }

        public static string Traslate(this int n)
        {
            Dictionary<int, string> _TransWrt = new Dictionary<int, string> { { 0, "waitting" }, { 1, "Show On Home" }, { 2, "Dispose" } };
            
            
           
                return _TransWrt[n];
        }

        public static string TransP(this int n)
        {
         Dictionary<int, string> _TransPayment = new Dictionary<int, string> { { 0, "Not Paid" }, { 1, "Paid" }, { 2, "Paying" }, { 3, "Dispose" } };



            return _TransPayment[n];
        }

        public static string TransCtrl(this string c)
        {
            int i = c.Length - 10;
           return c.Remove(i,10);

        }

        public static string GetUserRole(this string uid)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                if(uid!=null)
               return context.Users.Find(uid).Roles.ToList()[0].RoleId;
            }

            return "";
        }
        public static string TransFeedType(this string type)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Dictionary<string, string> p = new Dictionary<string, string> { { "MsgToDl", "Message To Dealer" }, { "RqMrInf", "Request More Info" }, { "MkOfr", "Make An Offer" } };
                return p[type];
            }
        }
    }
}