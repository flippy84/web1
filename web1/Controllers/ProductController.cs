using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public ActionResult Details(int? id)
        {
            if (id != null)
            {
                BookshopDatabase db = new BookshopDatabase();
                var product = (from p in db.Products where p.ProductId == id select p).SingleOrDefault();

                return View(product);
            }
            return RedirectToAction("Index");
        }
    }
}