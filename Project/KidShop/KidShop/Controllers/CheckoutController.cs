using KidShop.Areas.Admin.Models.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using KidShop.Areas.Admin.Models.DataModel;
using KidShop.Areas.Admin.Models.ViewModel;

namespace KidShop.Controllers
{
    public class CheckoutController : Controller
    {
        KidShopDbContext storeDB = new KidShopDbContext();
        const string PromoCode = "FREE";

        //
        // GET: /Checkout/AddressAndPayment

        public ActionResult AddressAndPayment()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            ViewBag.CountCart = cart.GetCount();
            ViewBag.Cart = viewModel;
            return View();
        }

        //
        // POST: /Checkout/AddressAndPayment
        [HttpPost]
        public ActionResult AddressAndPayment(Order order)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            if (ModelState.IsValid)
            {
                order.Username = Session["Email"].ToString();
                order.OrderDate = DateTime.Now;

                //Save Order
                storeDB.Orders.Add(order);
                storeDB.SaveChanges();

                //Process the order
                
                cart.CreateOrder(order);

                return RedirectToAction("Complete", new { id = order.OrderId });
            }
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            ViewBag.CountCart = cart.GetCount();
            ViewBag.Cart = viewModel;
            return View(order);
        }

        //
        // GET: /Checkout/Complete

        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = storeDB.Orders.Any(
                o => o.OrderId == id);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }
    }
}