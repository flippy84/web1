using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System;
using System.Collections.Generic;

namespace web1.Models
{
   public class ProductModel
    {
        public List<Product> GetProducts()
        {
            BookshopDatabase d = new BookshopDatabase();
            var products = from p in d.Products select p;

            return products.Count() > 0 ? products.ToList() : new List<Product>();
        }

        public Product GetDetails(int id)
        {
            BookshopDatabase db = new BookshopDatabase();

            var product = (from p in db.Products where p.ProductId == id select p).SingleOrDefault();

            return product;
        }
    }
}