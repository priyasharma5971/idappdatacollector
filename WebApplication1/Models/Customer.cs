using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustName { get; set; }
        public bool IsValid { get; set; }
        
        public string ValidFrom { get; set; }
       
        public string ValidTill { get; set; }
        public string EnteredOnDate { get; set; }

        public string CustAddress { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string CustCode { get; set; }
       
       
        public string password { get; set; }
        public int dealerid { get; set; }
        public Nullable<decimal> dealerprice { get; set; }
        public Nullable<decimal> customerprice { get; set; }
        public int size { get; set; }

    }
}