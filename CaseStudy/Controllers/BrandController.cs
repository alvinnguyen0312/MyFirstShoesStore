using CaseStudy.Models;
using CaseStudy.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace CaseStudy.Controllers
{
    public class BrandController : Controller
    {
        AppDbContext _db;
        public BrandController(AppDbContext context)
        {
            _db = context;
        }

        public IActionResult Index(BrandViewModel vm)
        {
            // only build the catalogue once
            if (HttpContext.Session.Get<List<Brand>>(SessionVariables.Brand) == null)
            {
                // no session information so let's go to the database
                try
                {
                    BrandModel brModel = new BrandModel(_db);
                    // now load the brands
                    List<Brand> brands = brModel.GetAll();
                    HttpContext.Session.Set(SessionVariables.Brand, brands);
                    vm.SetBrands(brands);
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Catalogue Problem - " + ex.Message;
                }
            }
            else
            {
                // no need to go back to the database as information is already in the session
                vm.SetBrands(HttpContext.Session.Get<List<Brand>>(SessionVariables.Brand));
               // ProductModel prdModel = new ProductModel(_db);
                //vm.Products = prdModel.GetAllByBrand(vm.Id);
            }
            return View(vm);
        }

        public IActionResult SelectBrand(BrandViewModel vm)
        {
            BrandModel brModel = new BrandModel(_db);
            ProductModel prdModel = new ProductModel(_db);
            List<Product> items = prdModel.GetAllByBrand(vm.BrandId);
            List<ProductViewModel> vms = new List<ProductViewModel>();
            if (items.Count > 0)
            {
                foreach (Product item in items)
                {
                    ProductViewModel mvm = new ProductViewModel();
                    mvm.Qty = 0;
                    mvm.BrandID = item.BrandId;
                    mvm.BrandName = brModel.GetName(item.BrandId);
                    mvm.Description = item.Description;
                    mvm.Id = item.Id;//productID code in string
                    mvm.MSRP = item.MSRP;
                    mvm.PRDName = item.ProductName;
                    mvm.GRPName = item.GraphicName;
                    mvm.QTYOnBackOrder = item.QtyOnBackOrder;
                    mvm.QTYOnHand = item.QtyOnHand;
                    vms.Add(mvm);
                }
                ProductViewModel[] myProducts = vms.ToArray();
                HttpContext.Session.Set<ProductViewModel[]>(SessionVariables.Product, myProducts);
            }
            vm.SetBrands(HttpContext.Session.Get<List<Brand>>(SessionVariables.Brand));
            return View("Index", vm); // need the original Index View here
        }

        public ActionResult SelectProduct(BrandViewModel vm)
        {
            Dictionary<string, object> order;
            if (HttpContext.Session.Get<Dictionary<string, Object>>(SessionVariables.Order) == null)
            {
                order = new Dictionary<string, object>();
            }
            else
            {
                order = HttpContext.Session.Get<Dictionary<string, object>>(SessionVariables.Order);
            }
            ProductViewModel[] prdt = HttpContext.Session.Get<ProductViewModel[]>(SessionVariables.Product);
            String retMsg = "";
            foreach (ProductViewModel item in prdt)
            {
                if (item.Id == vm.selectedId)
                {
                    if (vm.Qty > 0) // update only selected item
                    {
                        item.Qty = vm.Qty;
                        retMsg = vm.Qty + " - item(s) Added!";
                        order[item.Id] = item;
                    }
                    else
                    {
                        item.Qty = 0;
                        order.Remove(item.Id);
                        retMsg = "item(s) Removed!";
                    }
                    vm.BrandId = item.BrandID;
                    break;
                }
            }
            ViewBag.AddMessage = retMsg;
            HttpContext.Session.Set<Dictionary<string, Object>>(SessionVariables.Order, order);
            vm.SetBrands(HttpContext.Session.Get<List<Brand>>(SessionVariables.Brand));
            return View("Index", vm);
        }
    }
}