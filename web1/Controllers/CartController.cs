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
            return View(new CartModel().GetContents(this));
        }

        public ActionResult Add(int? id)
        {
            new CartModel().Add(this, id);

            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.ToString());
            else
                return RedirectToAction("Index", "Product");
        }

        public ActionResult Suggestions()
        {
            return View(new CartModel().GetSuggestions(this));
        }

        public ActionResult Remove(int? id)
        {
            new CartModel().Remove(this, id);

            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.ToString());
            else
                return RedirectToAction("Index", "Product");
        }

        public ActionResult Inline()
        {
            return Index();
        }
    }
}