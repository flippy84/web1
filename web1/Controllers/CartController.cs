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
            var cartId = CreateOrGetCartID();
            BookshopDatabase db = new BookshopDatabase();
            var rows = from r in db.Carts join product in db.Products on r.ProductId equals product.ProductId where r.CartId == cartId select Tuple.Create(product, r);
            
            return View(rows.ToList() ?? new List<Tuple<Product, Cart>>());
        }

        public ActionResult Add(int? id)
        {
            if (id != null)
            {
                BookshopDatabase db = new BookshopDatabase();
                var cartId = CreateOrGetCartID();
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

        public ActionResult Suggestions()
        {
            var guid = CreateOrGetCartID();
            var db = new BookshopDatabase();

            var products = db.Carts
                .Where(x => x.CartId == guid)
                .Select(x => x.ProductId);

            var orders = db.Carts
                .Join(db.OrderRows, x => x.ProductId, y => y.ProductId, (x, y) => y.OrderId)
                .Distinct();

            var related = db.OrderRows
                .Join(db.Products, i => i.ProductId, j => j.ProductId, (i, j) => new { i, j })
                .Where(x => orders.Contains(x.i.OrderId))
                .Where(x => !products.Contains(x.i.ProductId))
                .Select(x => x.j);


            /*var related = from cart in db.Carts
                          where cart.CartId == guid
                          join r1 in db.OrderRows on cart.ProductId equals r1.ProductId
                          select r1 into rows
                          from r2 in db.OrderRows
                          where r2.OrderId == rows.OrderId && r2.ProductId != rows.ProductId
                          select r2 into products
                          from p in db.Products
                          where p.ProductId == products.ProductId
                          select p;*/

            return View(related.Count() > 0 ? related.ToList() : new List<Product>());
        }

        public ActionResult Remove(int? id)
        {
            if (id != null)
            {
                BookshopDatabase db = new BookshopDatabase();
                var cartId = CreateOrGetCartID();
                var row = from r in db.Carts where r.CartId == cartId && r.ProductId == id select r;

                if (row.Any())
                {
                    var product = row.First();
                    product.Amount -= 1;
                    if (product.Amount == 0)
                    {
                        db.Carts.DeleteOnSubmit(product);
                    }
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

        public ActionResult Inline()
        {
            return Index();
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