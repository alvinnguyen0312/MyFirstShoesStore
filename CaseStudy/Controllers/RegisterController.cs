using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using CaseStudy.Models;

namespace CaseStudy.Controllers
{
    public class RegisterController : Controller
    {
        UserManager<ApplicationUser> _usrMgr;
        SignInManager<ApplicationUser> _signInMgr;
        public RegisterController(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
        {
            _usrMgr = userManager;
            _signInMgr = signInManager;
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        // POST:/Register/Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var appUser = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Firstname = model.Firstname,
                    Address1 = model.Address1,
                    City = model.City,
                    Lastname = model.Lastname,
                    Age = model.Age,
                    Mailcode = model.Mailcode,
                    Country = model.Country,
                    CreditcardType = model.CreditcardType,
                    Region = model.Region
                };
                var addUserResult = await _usrMgr.CreateAsync(appUser, model.Password);
                if (addUserResult.Succeeded)
                {
                    await _signInMgr.SignInAsync(appUser, isPersistent: false);
                    HttpContext.Session.SetString("loginstatus", model.Firstname + " is logged in");
                    HttpContext.Session.SetString("message", "Registered, logged on as " + model.Email);
                }
                else
                {
                    ViewBag.message = "registration failed - " + addUserResult.Errors.First().Description;
                    return View("Index");
                }
            }
            return Redirect("/Home");
        }
    }
}
  