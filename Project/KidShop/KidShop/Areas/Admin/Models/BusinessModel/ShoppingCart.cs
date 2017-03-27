using KidShop.Areas.Admin.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KidShop.Areas.Admin.Models.BusinessModel
{
    public class ShoppingCart
    {
        KidShopDbContext db = new KidShopDbContext();
        string ShoppingCartId { get; set; }
        public const string CartSessionKey = "CartId";

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }

        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public int AddToCart(ProductDetail product, int qty)
        {
            // Get the matching cart and item instances
            var cartItem = db.Carts.SingleOrDefault(c => c.CartId == ShoppingCartId && c.ProductDetailId == product.ProductDetailId);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new Cart
                {
                    ProductDetailId = product.ProductDetailId,
                    CartId = ShoppingCartId,
                    Count = qty,
                    ThanhTien = product.Price,
                    DateCreated = DateTime.Now
                };
                db.Carts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity
                cartItem.Count+=qty;
                cartItem.ThanhTien += product.Price;
            }
            // Save changes
            db.SaveChanges();

            return cartItem.Count;
        }

        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = db.Carts.Single(cart => cart.CartId == ShoppingCartId && cart.ProductDetailId == id);
            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = cartItem.Count;
                }
                else
                {
                    db.Carts.Remove(cartItem);
                }
                // Save changes
                db.SaveChanges();
            }
            return itemCount;
        }

        public void EmptyCart()
        {
            var cartItems = db.Carts.Where(cart => cart.CartId == ShoppingCartId);

            foreach (var cartItem in cartItems)
            {
                db.Carts.Remove(cartItem);
            }
            // Save changes
            db.SaveChanges();
        }

        public List<Cart> GetCartItems()
        {
            return db.Carts.Where(
                cart => cart.CartId == ShoppingCartId).ToList();
        }

        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in db.Carts
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }

        public decimal GetTotal()
        {
            // Multiply item price by count of that item to get 
            // the current price for each of those items in the cart
            // sum all item price totals to get the cart total
            decimal? total = (from cartItems in db.Carts
                              where cartItems.CartId == ShoppingCartId
                              select (int?)cartItems.Count *
                              cartItems.ProductDetail.Price).Sum();

            return total ?? decimal.Zero;
        }

        public Order CreateOrder(Order order)
        {
            decimal orderTotal = 0;
            order.OrderDetails = new List<OrderDetail>();

            var cartItems = GetCartItems();
            // Iterate over the items in the cart, 
            // adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetail
                {
                    ProductDetailId = item.ProductDetailId,
                    OrderId = order.OrderId,
                    UnitPrice = item.ProductDetail.Price,
                    Quantity = item.Count
                };
                // Set the order total of the shopping cart
                orderTotal += (item.Count * item.ProductDetail.Price);
                order.OrderDetails.Add(orderDetail);
                db.OrderDetails.Add(orderDetail);
            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;

            // Save the order
            db.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order;
        }

        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            if (context.Session[CartSessionKey] == null)
            {
                if (context.Session["Email"] != null)
                {
                    context.Session[CartSessionKey] = context.Session["Email"];
                }
                else
                {
                    // Generate a new random GUID using System.Guid class
                    Guid tempCartId = Guid.NewGuid();
                    // Send tempCartId back to client as a cookie
                    context.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return context.Session[CartSessionKey].ToString();
        }

        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = db.Carts.Where(c => c.CartId == ShoppingCartId);

            foreach (Cart item in shoppingCart)
            {
                item.CartId = userName;
            }

            db.SaveChanges();
        }
    }
}