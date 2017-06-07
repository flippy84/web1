using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web1.Models;

namespace web1.Controllers
{
    public class CartController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add(int? id)
        {
            if (id != null)
            {
                BookshopDatabase db = new BookshopDatabase();
                var cartId = GetCartId();
                var row = from r in db.Carts where r.CartId == cartId && r.ProductId == id select r;

                if (row.Any())
                {
                    var product = row.First();
                    product.Amount += 1;
                }
                else
                {
                    db.Carts.InsertOnSubmit(new Cart { CartId = cartId, ProductId = id.Value, Amount = 1 });
                }
                db.SubmitChanges();
            }

            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.ToString());
            else
                return RedirectToAction("Index", "Product");
        }

        private Guid GetCartId()
        {
            Guid guid;
            if (Request.Cookies["CartId"] != null && Guid.TryParse(Request.Cookies["CartId"].Value, out guid))
            {
                return guid;
            }
            else
            {
                guid = Guid.NewGuid();
                Response.SetCookie(new HttpCookie("CartId", guid.ToString()));
                return guid;
            }
        }
    }
}