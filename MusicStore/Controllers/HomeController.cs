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
    public class HomeController : Controller
    {
        MovieStoreEntities storeDB = new MovieStoreEntities();
        //
        // GET: /Home/


        public ActionResult Index()
        {
            // Get most popular movies
            var movies = GetTopSellingMovies(5);
            return View(movies);
        }
        public List<Movie> GetTopSellingMovies(int count)
        {
            // Group the order details by album and return
            // the albums with the highest count
            return storeDB.Movies
                .OrderByDescending(m => m.MovieOrderDetails.Count())
                .Take(count)
                .ToList();
        }
	}
}