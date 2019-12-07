using MusicStore.EntityContext;
using MusicStore.Models;
using MusicStore.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStore.Controllers
{
    public class ShoppingCartController : Controller
    {
        readonly MovieStoreEntities storeDB = new MovieStoreEntities();
        //
        // GET: /ShoppingCart/
        /// <summary>
        /// visszaadja username alapján a chart-ot
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            return View(viewModel);
        }
        //
        // GET: /Store/AddToCart/5
        /// <summary>
        ///AlbumId alapján
        ///a username-hez tartozo chart-hoz
        ///hozzáadja az albumot
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult AddToCart(int id)
        {
            // Retrieve the album from the database
            var addedAlbum = storeDB.Movies
            .Single(movie => movie.MovieId == id);
            // Add it to the shopping cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddToCart(addedAlbum);
            // Go back to the main store page for more shopping
            return RedirectToAction("Index");
        }
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5

        /// <summary>
        /// Albumid alapjan eltávolítja az Albumot
        /// A username-hez tartozp Chart-ból
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Get the name of the album to display confirmation
            string albumName = storeDB.MovieCarts
            .Single(item => item.MovieRecordId == id).Movie.MovieTitle;
            // Remove from cart
            int itemCount = cart.RemoveFromCart(id);
            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel()
            {
                Message = Server.HtmlEncode(albumName) +
                "has been removed from your shopping cart.",
                CartTotal=cart.GetTotal(),
                CartCount=cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }
        //
        // GET: /ShoppingCart/CartSummary
        //返回的一个子视图
        /// <summary>
        /// Visszaadja, hogy hány elem van a cart-ban
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            ViewData["CartCount"] = cart.GetCount();
            return PartialView(cart);
        }
	}
}