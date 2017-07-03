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

        public ActionResult Lookup()
        {
            return View();
        }
        
        public ActionResult Details(int ?id)
        {
            if (id == null)
                return RedirectToAction("Lookup");

            BookshopDatabase db = new BookshopDatabase();
            
            var items = from r in db.OrderRows join p in db.Products on r.ProductId equals p.ProductId where r.OrderId == id.Value select p;

            return View(Tuple.Create(id.Value, items.ToList()));
        }

        [HttpPost]
        public ActionResult Checkout(OrderDetails orderDetails)
        {
            if (ModelState.IsValid)
            {
                BookshopDatabase db = new BookshopDatabase();
                Order order = new Order(orderDetails);
                var guid = CreateOrGetCartID();

                var cartItems = (from i in db.Carts where i.CartId == guid select i).ToList();
                var stockItems = (from i in db.Carts join p in db.Products on i.ProductId equals p.ProductId select p).ToList();

                for (int i = 0; i < cartItems.Count(); i++)
                {
                    if (cartItems[i].Amount <= stockItems[i].Stock)
                    {
                        stockItems[i].Stock -= cartItems[i].Amount;
                    }
                    else
                    {
                        return View(orderDetails);
                    }
                }

                db.Orders.InsertOnSubmit(order);
                db.SubmitChanges();

                for (int i = 0; i < cartItems.Count(); i++)
                {
                    db.OrderRows.InsertOnSubmit(new OrderRow(order.OrderId, cartItems[i].ProductId, cartItems[i].Amount));
                }

                db.Carts.DeleteAllOnSubmit(cartItems);
                db.SubmitChanges();

                return RedirectToAction("Done", new { id = order.OrderId });
            }

            return View(orderDetails);
        }
        
        public ActionResult Done(int? id)
        {
            return View(id);
        }

        private Guid CreateOrGetCartID()
        {
            string cartIDKey = "CartID";
            Guid guid;

            if (Request.Cookies[cartIDKey] != null && Guid.TryParse(Request.Cookies[cartIDKey].Value, out guid))
            {
                return guid;
            }
            else
            {
                guid = Guid.NewGuid();
                Response.SetCookie(new HttpCookie(cartIDKey, guid.ToString()));
                return guid;
            }
        }
    }
}