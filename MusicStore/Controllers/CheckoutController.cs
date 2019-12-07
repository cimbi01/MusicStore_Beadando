using MusicStore.EntityContext;
using MusicStore.Models;
using MusicStore.Models.Database.Movie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStore.Controllers
{
    public class CheckoutController : Controller
    {
        readonly MovieStoreEntities storeDB = new MovieStoreEntities();
        //
        // GET: /Checkout/
        public ActionResult AddressAndPayment()
        {
            return View();
        }
        //
        // POST: /Checkout/AddressAndPayment
        /// <summary>
        /// Submit order-re kattintva lefut
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            var order = new MovieOrder();
            TryUpdateModel(order);
            try
            {
                order.Username = User.Identity.Name;
                order.OrderDate = DateTime.Now;
                //Process the order
                var cart = ShoppingCart.GetCart(HttpContext);
                cart.CreateOrder(order);
                return RedirectToAction("Complete", new { id = order.MovieOrderId });
            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }
        //
        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = this.storeDB.MovieOrders.Any(
            o => o.MovieOrderId == id &&
            o.Username == User.Identity.Name);
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