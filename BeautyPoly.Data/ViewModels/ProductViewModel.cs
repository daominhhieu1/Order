﻿namespace BeautyPoly.Data.ViewModels
{
    public class ProductViewModel
    {
        public int ProductID { get; set; }
        public int? CateID { get; set; }
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? ModifiledDate { get; set; }
        public int? Status { get; set; }
        public bool? IsSale { get; set; }
        public bool? IsDelete { get; set; }
        public string? PriceText { get; set; }
        public string? CateName { get; set; }
        public string? Image { get; set; }
    }
}
