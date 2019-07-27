using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CaseStudy.Utils;
using CaseStudy.Models;
using System.Collections.Generic;
using System;
namespace CaseStudy.Controllers
{
    public class OrderController : Controller
    {
        AppDbContext _db;
        public OrderController(AppDbContext context)
        {
            _db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult ClearOrder() // clear out current order
        {
            HttpContext.Session.Remove(SessionVariables.Order);
            HttpContext.Session.SetString(SessionVariables.Message, "Order Cleared");
            return Redirect("/Home");
        }
        // Add the order, pass the session variable info to the db
        public ActionResult AddOrder()
        {
            OrderModel model = new OrderModel(_db);
            int retVal = -1;
            string retMessage = ""; try
            {
                Dictionary<string, object> orderItems = HttpContext.Session.Get<Dictionary<string,object>>(SessionVariables.Order);
                retVal = model.AddOrder(orderItems,HttpContext.Session.Get<ApplicationUser>(SessionVariables.User));
                if (retVal > 0) // Order Added
                {
                    retMessage = "Order " + retVal + " Created! ";
                    if (model.msgBackOrder != "")
                    {
                        retMessage += model.msgBackOrder;
                    }
                }
                else // problem
                {
                    retMessage = "Order not added, try again later";
                }
                retMessage = "";


            }
            catch (Exception ex) // big problem
            {
                retMessage = "Order was not created, try again later! - " + ex.Message;
            }
            HttpContext.Session.Remove(SessionVariables.Order); // clear out current order once persisted
            HttpContext.Session.SetString(SessionVariables.Message, retMessage);
            return Redirect("/Home");
        }
    }
}