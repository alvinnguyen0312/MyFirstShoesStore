using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CaseStudy.Models;
using System.IO;

namespace CaseStudy.Controllers
{
    public class DataController : Controller
    {
        AppDbContext _ctx;
        public DataController(AppDbContext context)
        {
            _ctx = context;
        }
        //public async Task<IActionResult> Index()
        //{
        //    UtilityModel util = new UtilityModel(_ctx);
        //    string msg = "";
        //    var json = await getMenuItemJsonFromWebAsync();
        //    try
        //    {
        //        msg = (util.loadShoesTables(json)) ? "tables loaded" : "problem loading tables";
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = ex.Message;
        //    }
        //    ViewBag.LoadedMsg = msg;
        //    return View();
        //}
        public async Task<IActionResult> Index()
        {
            UtilityModel util = new UtilityModel(_ctx);
            var json = await getMenuItemJsonFromWebAsync();
            try
            {
                //ViewBag.LoadedMsg = (util.loadShoesTables(json)) ? "Menu and Item tables loaded" : "problem loading tables";
            }
            catch (Exception ex)
            {
                ViewBag.LoadedMsg = ex.Message;
            }
            return View("Index");
        }
        private async Task<String> getMenuItemJsonFromWebAsync()
        {
            string url = "https://raw.githubusercontent.com/alvinnguyen0312/project-3/master/shoes.json";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

    }
}