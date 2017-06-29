using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web1.Models;

namespace web1.Controllers
{
    public class OrderController : Controller
    {
        public ActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Checkout(OrderDetails orderDetails)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Done");
            }
            return View(orderDetails);
        }
        
        public ActionResult Done(int? id)
        {
            return View();
        }
    }
}