using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CaseStudy.Models;
using CaseStudy.Utils;

namespace CaseStudy.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString(SessionVariables.LoginStatus) == null)
            {
                HttpContext.Session.SetString(SessionVariables.LoginStatus, "not logged in");
            }
            if (HttpContext.Session.GetString(SessionVariables.LoginStatus) == "not logged in")
            {
                if (HttpContext.Session.GetString(SessionVariables.Message) == null)
                {
                    HttpContext.Session.SetString(SessionVariables.Message, "please login!");
                }
            }
            ViewBag.Status = HttpContext.Session.GetString(SessionVariables.LoginStatus);
            ViewBag.Message = HttpContext.Session.GetString(SessionVariables.Message);
            return View();
        }


    }
}
