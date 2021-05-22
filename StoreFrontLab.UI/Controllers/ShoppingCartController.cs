using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StoreFrontLab.UI.Models;

namespace StoreFrontLab.UI.Controllers
{
    public class ShoppingCartController : Controller
    {
        // GET: ShoppingCart
        public ActionResult Index()
        {
            var shoppingCart = (Dictionary<int, CartItemViewModel>)Session["cart"];
            if (shoppingCart == null || shoppingCart.Count == 0)
            {
                shoppingCart = new Dictionary<int, CartItemViewModel>();
                ViewBag.Message = "There are no cart items in your cart";
            }
            else
            {
                ViewBag.Message = null;
            }
            return View(shoppingCart);
        }

        public ActionResult RemoveFromCart(int id)
        {
            Dictionary<int, CartItemViewModel> shoppingCart = (Dictionary<int, CartItemViewModel>)Session["cart"];
            shoppingCart.Remove(id);
            Session["cart"] = shoppingCart;
            return RedirectToAction("Index");

        }

        public ActionResult UpdateCart(int productID, int qty)
        {
            Dictionary<int, CartItemViewModel> shoppingCart = (Dictionary<int, CartItemViewModel>)Session["cart"];
            shoppingCart[productID].Qty = qty;
            Session["cart"] = shoppingCart;
            return RedirectToAction("index");
        }
    }
}