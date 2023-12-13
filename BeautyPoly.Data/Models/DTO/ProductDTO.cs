using BeautyPoly.Models;

namespace BeautyPoly.Data.Models.DTO
{
    public class ProductDTO
    {
        public Product Product { get; set; }
        public List<int> ListOptionID { get; set; }
        public List<SkuDTO> ListSku { get; set; }
        public List<string> Images { get; set; }
    }

    public class SkuDTO
    {
        public string OptionValueID { get; set; }
        public int Quantity { get; set; }
        public double CapitalPrice { get; set; }
        public double Price { get; set; }
    }
}
