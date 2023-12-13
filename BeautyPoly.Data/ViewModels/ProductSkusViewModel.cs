﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeautyPoly.Data.ViewModels
{
    public class ProductSkusViewModel
    {
        public int ProductSkusID { get; set; }
        public int? ProductID { get; set; }
        public string? Sku { get; set; }
        public double? CapitalPrice { get; set; }
        public double? Price { get; set; }
        public int? Quantity { get; set; }
        public bool? IsDelete { get; set; }
        public int CountOption { get; set; }
        public string? ProductSkuName { get; set; }
        public int QuantityCart {  get; set; }
        public string? ProductName {  get; set; }
    }
}
