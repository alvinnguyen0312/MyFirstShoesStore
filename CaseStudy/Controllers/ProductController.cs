using CaseStudy.Models;
using Microsoft.AspNetCore.Mvc;

namespace CaseStudy.Controllers
{
    public class ProductController : Controller
    {
        AppDbContext _db;
        public ProductController(AppDbContext context)
        {
            _db = context;
        }
        public IActionResult Index(BrandViewModel brand )
        {
            ProductModel model = new ProductModel(_db);
            ProductViewModel viewModel = new ProductViewModel();
            viewModel.Products = model.GetAllByBrand(brand.Id);
            viewModel.BrandName = model.GetBrand(brand.Id).Name;
            return View(viewModel);
        }
    }
}