using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarDealer.Models
{
    public class SoldViewModel
    {
        public long CarSoldId { get; set; }
        public long CarId { get; set; }
        public string UserId { get; set; }
        public Nullable<decimal> AgreedPrice { get; set; }
        public Nullable<System.DateTime> Datesold { get; set; }
        public Nullable<int> PaymentStatus { get; set; }
        public string PaymentMethod { get; set; }
        public Nullable<System.DateTime> PaymentStartDate { get; set; }
        public Nullable<System.DateTime> PaymentEndDate { get; set; }
        public Nullable<decimal> ActurePaymentAmount { get; set; }
        public string OtherDetail { get; set; }

        //[ForeignKey("UserId")]
        public virtual ApplicationUser ToUser { get; set; }

        
        public virtual Car ToCar { get; set; }

    }
}