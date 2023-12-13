using BeautyPoly.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautyPoly.Data.Models.DTO
{
    public class OrderDTO
    {
        public int OrderID { get; set; }
        public int? PotentialCustomerID { get; set; }
        public int? TransactStatusID { get; set; }
        public int? CouponID { get; set; }
        public int? AccountID { get; set; }
        public string? AccountName { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public string? OrderCode { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string? Note { get; set; }
        public double? TotalMoney { get; set; }
        public string? Address { get; set; }
        public string? MedthodPayment { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsDelete { get; set; }
        public List<productPick> prods { get; set; }

    }
    public class productPick
    {
        public int ProductID { get; set; }
        public int Quantity { get; set;}
        public int Price { get; set;}
    }
    
}
