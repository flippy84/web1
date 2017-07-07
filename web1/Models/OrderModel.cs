using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;

namespace web1.Models
{
    public class OrderModel
    {
        public int? Add(Controller controller, OrderDetailModel orderDetails)
        {
            BookshopDatabase db = new BookshopDatabase();
            Order order;

            using (var transaction = new TransactionScope())
            {
                order = new Order(orderDetails);
                var cartId = new CartModel().GetCartId(controller);

                var cartItems = (from i in db.Carts where i.CartId == cartId select i).ToList();
                var stockItems = (from i in db.Carts join p in db.Products on i.ProductId equals p.ProductId select p).ToList();

                for (int i = 0; i < cartItems.Count(); i++)
                {
                    if (cartItems[i].Amount <= stockItems[i].Stock)
                    {
                        stockItems[i].Stock -= cartItems[i].Amount;
                    }
                    else
                    {
                        return null;
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
                transaction.Complete();
            }

            return order.OrderId;
        }

        public Tuple<int, List<Product>> GetDetails(int id)
        {
            BookshopDatabase db = new BookshopDatabase();

            var items = from r in db.OrderRows join p in db.Products on r.ProductId equals p.ProductId where r.OrderId == id select p;

            return Tuple.Create(id, items.ToList());
        }
    }
}