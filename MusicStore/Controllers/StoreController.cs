using MusicStore.EntityContext;
using MusicStore.Models;
using MusicStore.Models.Database.Movie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MusicStore.Controllers
{
    public class StoreController : Controller
    {
        private readonly MovieStoreEntities storeDB = new MovieStoreEntities();
        //
        // GET: /Store/

            /// <summary>
        /// Kilistázza a műfajokat
        /// </summary>
        /// <returns></returns>

        public ActionResult Index()
        {
            var genres = this.storeDB.MovieGenres.ToList();
            return View(genres);
        }
        //
        // GET: /Store/Browse

        /// <summary>
        ///A az albumokat amiknek a genre-jük genre továbbadja a view-nak
        /// </summary>
        /// <param name="genre"></param>
        /// <returns></returns>
        public ActionResult Browse(string genre)
        {
            // Retrieve Genre and its Associated Albums from database
            //Include("Albums")指定返回结果要包含关联Album
            MovieGenre example = this.storeDB.MovieGenres.Include("Movies").Single(p => p.MovieGenreName == genre);
            return View(example);
        }
        //
        // GET: /Store/Details

        /// <summary>
        // Az AlbumId alapján lekéri az albumot és azt az albumot adja tovább a View-nak
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = this.storeDB.Movies.Find(id);
            return View(movie);
        }
        //
        // GET: /Store/GenreMenu

        /// <summary>
        /// Oldal sáv a genreknek
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]

        public ActionResult GenreMenu()
        {
            var genres = this.storeDB.MovieGenres.ToList();
            return PartialView(genres);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.storeDB.Dispose();
            }
            base.Dispose(disposing);
        }
	}
}