using BeautyPoly.Data.Repositories;
using BeautyPoly.Models;
using Microsoft.AspNetCore.Mvc;

namespace BeautyPoly.View.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        CategoryRepo categoryRepo;

        public CategoryController(CategoryRepo categoryRepo)
        {
            this.categoryRepo = categoryRepo;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("admin/category/getall")]
        public async Task<IActionResult> GetAll()
        {
            return Json(await categoryRepo.GetAllAsync(),new System.Text.Json.JsonSerializerOptions() { MaxDepth= 32});
        }
    }
}
