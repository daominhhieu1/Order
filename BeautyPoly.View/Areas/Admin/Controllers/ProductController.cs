using BeautyPoly.Common;
using BeautyPoly.Data.Models.DTO;
using BeautyPoly.Data.Repositories;
using BeautyPoly.Helper;
using BeautyPoly.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeautyPoly.View.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        OptionRepo optionRepo;
        CategoryRepo categoryRepo;
        OptionValueRepo optionValueRepo;
        ProductRepo productRepo;
        ProductSkuRepo productSkuRepo;
        ProductDetailRepo productDetailRepo;
        OptionDetailRepo optionDetailRepo;
        ProductImagesRepo productImagesRepo;

        public ProductController(OptionRepo optionRepo, CategoryRepo categoryRepo, OptionValueRepo optionValueRepo, ProductRepo productRepo, ProductSkuRepo productSkuRepo, ProductDetailRepo productDetailRepo, OptionDetailRepo optionDetailRepo, ProductImagesRepo productImagesRepo)
        {
            this.optionRepo = optionRepo;
            this.categoryRepo = categoryRepo;
            this.optionValueRepo = optionValueRepo;
            this.productRepo = productRepo;
            this.productSkuRepo = productSkuRepo;
            this.productDetailRepo = productDetailRepo;
            this.optionDetailRepo = optionDetailRepo;
            this.productImagesRepo = productImagesRepo;
        }

        [Route("admin/product")]
        public async Task<IActionResult> IndexAsync()
        {
            var listCate = await categoryRepo.GetAllAsync();
            List<SelectListItem> list = new List<SelectListItem>();
            list = listCate.Select(cate => new SelectListItem
            {
                Text = cate.CateName,
                Value = cate.CateId.ToString()
            }).ToList();
            list.Insert(0, new SelectListItem("Chọn danh mục", "0"));
            ViewBag.Category = list;
            return View();
        }




        [HttpPost("admin/product/create")]
        public async Task<IActionResult> AddProduct([FromBody] ProductDTO productDTO)
        {
            try
            {
                Product product = new Product()
                {
                    ProductName = productDTO.Product.ProductName,
                    ProductCode = productDTO.Product.ProductCode,
                    CateID = productDTO.Product.CateID,
                };
                await productRepo.InsertAsync(product);
                int index = 1;
                foreach (var item in productDTO.Images)
                {
                    string fileName = product.ProductCode + "_" + index.ToString() + ".png";
                    Utilities.ConvertAndSaveImage(item, fileName);
                    ProductImages productImages = new ProductImages();
                    productImages.Image = fileName;
                    productImages.ProductID = product.ProductID;
                    await productImagesRepo.InsertAsync(productImages);
                    index++;
                }


                foreach (int id in productDTO.ListOptionID)
                {
                    OptionDetails optionDetails = new OptionDetails();
                    optionDetails.OptionID = id;
                    optionDetails.ProductID = product.ProductID;
                    await optionDetailRepo.InsertAsync(optionDetails);
                }
                foreach (var s in productDTO.ListSku)
                {

                    string[] stringID = s.OptionValueID.Split('-');
                    string sku = product.ProductCode;
                    for (int i = 0; i < stringID.Length; i++)
                    {
                        var resultValue = await optionValueRepo.GetByIdAsync(TextUtils.ToInt(stringID[i].Trim()));
                        if (resultValue != null)
                        {
                            var resultOption = await optionRepo.GetByIdAsync((int)resultValue.OptionID);
                            sku += TextUtils.ToString(resultOption.OptionName[0]).ToUpper() + TextUtils.ToString(resultValue.OptionValueName[0]).ToUpper();
                        }
                    }
                    ProductSkus productSkus = new ProductSkus();
                    productSkus.ProductID = product.ProductID;
                    productSkus.Sku = sku;
                    productSkus.CapitalPrice = s.CapitalPrice;
                    productSkus.Price = s.Price;
                    productSkus.Quantity = s.Quantity;
                    await productSkuRepo.InsertAsync(productSkus);
                    for (int i = 0; i < stringID.Length; i++)
                    {
                        var resultValue = await optionValueRepo.GetByIdAsync(TextUtils.ToInt(stringID[i].Trim()));
                        if (resultValue != null)
                        {
                            ProductDetails productDetails = new ProductDetails();
                            productDetails.OptionValueID = resultValue.OptionValueID;
                            var resultOptionDetail = optionDetailRepo.FindAsync(p => p.ProductID == product.ProductID && p.OptionID == resultValue.OptionID).Result.FirstOrDefault();
                            productDetails.OptionDetailsID = resultOptionDetail.OptionDetailsID;
                            productDetails.ProductSkusID = productSkus.ProductSkusID;
                            await productDetailRepo.InsertAsync(productDetails);
                        }
                    }
                }
                return Json(1);
            }
            catch (Exception ex)
            {
                return Json(ex);
            }
        }
        [HttpGet("admin/product/get-option")]
        public async Task<IActionResult> GetOption()
        {
            var result = await optionRepo.GetAllAsync();
            return Json(result.Where(p => p.IsDelete == false || p.IsDelete == null).ToList(), new System.Text.Json.JsonSerializerOptions());
        }
        [HttpGet("admin/product/get-option-value")]
        public async Task<IActionResult> GetAllOptionValues()
        {
            var result = await optionValueRepo.GetAllAsync();
            return Json(result.Where(p => p.IsDelete == false || p.IsDelete == null).ToList(), new System.Text.Json.JsonSerializerOptions());
        }
        [HttpGet("admin/product/get-product")]
        public async Task<IActionResult> GetAllProduct()
        {
            var result = await productRepo.GetAllAsync();
            return Json(result.Where(p => p.IsDelete == false || p.IsDelete == null).ToList(), new System.Text.Json.JsonSerializerOptions());
        }



    }
}
