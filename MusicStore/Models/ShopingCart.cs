using MusicStore.EntityContext;
using MusicStore.Models.Database.Movie;
using Mvc3ToolsUpdateWeb_Default.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStore.Models
{
    public class ShoppingCart
    {
        readonly MovieStoreEntities storeDB = new MovieStoreEntities();
        string ShoppingCartId { get; set; }
        //存在Session中的键值 保存ShoppingCartId
        public const string CartSessionKey = "CartId";

        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();
            //visszaadja a felhasználó nevét, vagy egy üres stringet ("")
            cart.ShoppingCartId = cart.GetCartId(context);
            return cart;
        }
        // Helper method to simplify shopping cart calls
        public static ShoppingCart GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }
        public void AddToCart(Movie movie)
        {
            // Get the matching cart and album instances
            var cartItem = this.storeDB.MovieCarts.SingleOrDefault(
                c => c.MovieCartId == ShoppingCartId
                && c.MovieId == movie.MovieId);
            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new MovieCart()
                {
                    MovieId=movie.MovieId,
                    MovieCartId=ShoppingCartId,
                    Count=1,
                    DateCreated=DateTime.Now
                };
                this.storeDB.MovieCarts.Add(cartItem);
            }
            else
            {
                // If the item does exist in the cart, then add one to the quantity
                cartItem.Count++;
            }
            this.storeDB.SaveChanges();
        }
        public int RemoveFromCart(int id)
        {
            // Get the cart
            var cartItem = this.storeDB.MovieCarts.Single(
                cart => cart.MovieCartId == ShoppingCartId
                && cart.MovieRecordId == id);
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
                    this.storeDB.MovieCarts.Remove(cartItem);
                }
                this.storeDB.SaveChanges();
            }
            return itemCount;
        }
        public void EmptyCart()
        {
            var cartItems = this.storeDB.MovieCarts.Where(cart => cart.MovieCartId == ShoppingCartId);
            foreach (var cartItem in cartItems)
            {
                this.storeDB.MovieCarts.Remove(cartItem);
            }
            this.storeDB.SaveChanges();
        }
        public List<MovieCart> GetCartItems()
        {
            return this.storeDB.MovieCarts.Where(cart => cart.MovieCartId == ShoppingCartId).ToList();
        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in this.storeDB.MovieCarts
                          where cartItems.MovieCartId == ShoppingCartId
                          select (int?)cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }
        public decimal GetTotal()
        {
            // Multiply album price by count of that album to get
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = (from cartItem in this.storeDB.MovieCarts
                              where cartItem.MovieCartId == ShoppingCartId
                              select (int?)cartItem.Count * cartItem.Movie.MoviePrice)
                                .Sum();
            return total ?? 0;
        }
        public int CreateOrder(MovieOrder order)
        {
            //order have create and is going to update information
            decimal orderTotal = 0;
            var cartItem = GetCartItems();
            // Iterate over the items in the cart, adding the order details for each
            foreach(var item in cartItem)
            {
                var orderDetail=new MovieOrderDetail()
                {
                    MovieId=item.MovieId,
                    MovieOrderId=order.MovieOrderId,
                    UnitPrice=item.Movie.MoviePrice,
                    Quantity=item.Count
                };
                // Set the order total of the shopping cart
                orderTotal += (item.Count * item.Movie.MoviePrice);
                this.storeDB.MovieOrderDetails.Add(orderDetail);
            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;
            //Save Order
            this.storeDB.MovieOrders.Add(order);
            this.storeDB.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.MovieOrderId;

        }
        // We're using HttpContextBase to allow access to cookies.
        public string GetCartId(HttpContextBase context)
        {
            // ha van bejelentkezett user
            if (((Account)context.Session[AccountController.LOGGEDINUSER_SESSION_KEY]) != null)
            {
                context.Session[CartSessionKey] = ((Account)context.Session [AccountController.LOGGEDINUSER_SESSION_KEY]).UserName;
            }
            // ha nincs bejelentkezett felhasznalo
            else
            {
                context.Session[CartSessionKey] = "";
            }
            return context.Session[CartSessionKey].ToString();
        }
        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = this.storeDB.MovieCarts.Where(c => c.MovieCartId == ShoppingCartId);
            foreach (MovieCart item in shoppingCart)
            {
                // ha már van ilyen elem, akkor a count-ot növelje majd törölje
                MovieCart cart = this.storeDB.MovieCarts.FirstOrDefault(c => c.MovieId == item.MovieId && c.MovieCartId == userName);
                if(cart != null)
                {
                    cart.Count++;
                    this.storeDB.MovieCarts.Remove(item);
                }
                else
                {
                    item.MovieCartId = userName;
                }
            }
            this.storeDB.SaveChanges();
        }
    }
}