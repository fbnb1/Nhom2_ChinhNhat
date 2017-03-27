using KidShop.Areas.Admin.Models.BusinessModel;
using KidShop.Areas.Admin.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KidShop.Controllers
{
    public class ShoppingCartController : Controller
    {

        KidShopDbContext db = new KidShopDbContext();
        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            // Return the view
            return View(viewModel);
        }
        //
        // GET: /Store/AddToCart/5
      //  [HttpPost]
        public ActionResult AddToCart(int id, int qty)
        {
            // Retrieve the item from the database
            var addedItem = db.ProductDetail.Single(item => item.ProductDetailId == id);

            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            

            // Display the confirmation message
            ShoppingCartRemoveViewModel results;
            if (addedItem == null)
            {
                results = new ShoppingCartRemoveViewModel
                {
                    Message = "Xảy ra lỗi khi thêm sản phẩm vào giỏ hàng",
                    CartTotal = cart.GetTotal(),
                    CartCount = cart.GetCount(),
                };
            }
            else
            {
                int count = cart.AddToCart(addedItem, qty);
                results = new ShoppingCartRemoveViewModel
                {
                    Message = qty + " sản phẩm " + addedItem.Product.ProductName + " mầu " + addedItem.Color + " size " + addedItem.Size + " đã được thêm vào giỏ hàng.",
                    CartTotal = cart.GetTotal(),
                    CartCount = cart.GetCount(),
                    ItemCount = count,
                    DeleteId = id
                };
            }
            return Json(results);

            // Go back to the main store page for more shopping
            // return RedirectToAction("Index");
        }
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            // Get the name of the item to display confirmation

            // Get the name of the album to display confirmation
            string itemName = db.ProductDetail
                .Single(item => item.ProductDetailId == id).Product.ProductName;

            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = "One (1) " + Server.HtmlEncode(itemName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }
        //
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}