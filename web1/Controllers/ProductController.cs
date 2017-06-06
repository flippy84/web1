using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web1.Models;

namespace web1.Controllers
{
    public class ProductController : Controller
    {
        public ActionResult Index()
        {
            BookshopDatabase d = new BookshopDatabase();
            var products = from p in d.Products select p;

            return View(products.ToList());
        }
    }

    public static class Extensions
    {
        public static IQueryable<T> Page<T>(this IQueryable<T> query, int page)
        {
            return query.Skip((page - 1) * 10).Take(10);
        }
    }
}