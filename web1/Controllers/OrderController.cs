using System;
using System.Collections.Generic;
using System.Transactions;
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

        public ActionResult Lookup()
        {
            return View();
        }
        
        public ActionResult Details(int ?id)
        {
            if (id == null)
                return RedirectToAction("Lookup");

            return View(new OrderModel().GetDetails(id.Value));
        }

        [HttpPost]
        public ActionResult Checkout(OrderDetailModel orderDetails)
        {
            if (ModelState.IsValid)
            {
                int? orderId = new OrderModel().Add(this, orderDetails);
                if (orderId != null)
                    return RedirectToAction("Done", new { id = orderId });
            }

            return View(orderDetails);
        }
        
        public ActionResult Done(int? id)
        {
            return View(id);
        }
    }
}