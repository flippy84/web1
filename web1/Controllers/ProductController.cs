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
            return View(new ProductModel().GetProducts());
        }

        public ActionResult Details(int? id)
        {
            if (id != null)
            {
                return View(new ProductModel().GetDetails(id.Value));
            }
            return RedirectToAction("Index");
        }
    }
}