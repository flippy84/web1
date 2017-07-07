using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web1.Models
{
    public class CartModel
    {
        public void Add(Controller controller, int? id)
        {
            if (id != null)
            {
                BookshopDatabase db = new BookshopDatabase();
                var cartId = GetCartId(controller);
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
        }

        public void Remove(Controller controller, int? id)
        {
            if (id != null)
            {
                BookshopDatabase db = new BookshopDatabase();
                var cartId = GetCartId(controller);
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
        }

        public List<Product> GetSuggestions(Controller controller)
        {
            var cartId = GetCartId(controller);
            var db = new BookshopDatabase();

            var products = db.Carts
                .Where(x => x.CartId == cartId)
                .Select(x => x.ProductId);

            var orders = db.Carts
                .Join(db.OrderRows, x => x.ProductId, y => y.ProductId, (x, y) => y.OrderId)
                .Distinct();

            var related = db.OrderRows
                .Join(db.Products, i => i.ProductId, j => j.ProductId, (i, j) => new { i, j })
                .Where(x => orders.Contains(x.i.OrderId))
                .Where(x => !products.Contains(x.i.ProductId))
                .Select(x => x.j);

            return related.Count() > 0 ? related.ToList() : new List<Product>();
        }

        public List<Tuple<Product, Cart>> GetContents(Controller controller)
        {
            var cartId = GetCartId(controller);
            BookshopDatabase db = new BookshopDatabase();

            var rows = from r in db.Carts join product in db.Products on r.ProductId equals product.ProductId where r.CartId == cartId select Tuple.Create(product, r);

            return rows.ToList() ?? new List<Tuple<Product, Cart>>();

        }

        public Guid GetCartId(Controller controller)
        {
            string cartIdKey = "CartID";
            Guid cartId;

            if (controller.Request.Cookies[cartIdKey] != null && Guid.TryParse(controller.Request.Cookies[cartIdKey].Value, out cartId))
            {
                return cartId;
            }
            else
            {
                cartId = Guid.NewGuid();
                controller.Response.SetCookie(new HttpCookie(cartIdKey, cartId.ToString()));
                return cartId;
            }
        }
    }
}