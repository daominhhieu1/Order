﻿using BeautyPoly.Common;
using BeautyPoly.Data.Repositories;
using BeautyPoly.Data.ViewModels;
using BeautyPoly.Models;
using BeautyPoly.View.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BeautyPoly.View.Controllers
{
    public class HomeController : Controller
    {

        ProductRepo productRepo;
        public HomeController(ProductRepo productRepo)
        {
            this.productRepo = productRepo;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Product = await productRepo.GetAllAsync();
            return View();
        }

        public IActionResult GetProduct()
        {
            var list = SQLHelper<ProductViewModel>.ProcedureToList("spGetProductToView", new string[] { }, new object[] { });
            return Json(list, new System.Text.Json.JsonSerializerOptions());
        }
        public IActionResult GetOptionValueDetail(int productID, int optionID)
        {
            var list = SQLHelper<OptionValueViewModel>.ProcedureToList("spGetOptionValueByOptionID", new string[] { "ProductID", "@OptionID" }, new object[] { productID, optionID });
            return Json(list, new System.Text.Json.JsonSerializerOptions());
        }
        public IActionResult GetOptionDetail(int id)
        {
            var list = SQLHelper<OptionDetailViewModel>.ProcedureToList("spGetOptionDetail", new string[] { "ProductID" }, new object[] { id });
            return Json(list, new System.Text.Json.JsonSerializerOptions());
        }
        public IActionResult GetProductSkuByValue(string listOptionValueID, int productID)
        {
            var model = SQLHelper<ProductSkusViewModel>.ProcedureToModel("spGetProductSkuByOptionValueID", new string[] { "@ListOptionValueID", "@ProductID" }, new object[] { listOptionValueID, productID });
            return Json(model, new System.Text.Json.JsonSerializerOptions());
        }
    }
}