using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MusicStore.Models;
using MusicStore.EntityContext;
using MusicStore.AccountManagement.Filters;
using MusicStore.Models.Database.Movie;
using MusicStore.ViewModels;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace MusicStore.Controllers
{
    /// <summary>
    /// Csak az admin
    /// Szerkeszthet, adhat hozzá, vehet el elemeket
    /// </summary>
    //暂时无法ASP WEB管理器
    [AdminFilter]
    public class StoreManagerController : AsyncController
    {
        private static readonly string[] FILE_EXTENSIONS = {"csv"};
        private readonly MovieStoreEntities db = new MovieStoreEntities();

        // GET: /StoreManager/
        public ActionResult Index()
        {
            return View(this.db.Movies.ToList());
        }

        // GET: /StoreManager/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = this.db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // GET: /StoreManager/Create
        public ActionResult Create()
        {
            //动态表达式
            //1.传递数据到UI
            //SelectList重要
            ViewBag.MovieDirectorId = new SelectList(this.db.MovieDirectors, "MovieDirectorId", "MovieDirectorName");
            ViewBag.MovieGenreId = new SelectList(this.db.MovieGenres, "MovieGenreId", "MovieGenreName");
            return View();
        }

        // POST: /StoreManager/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include= "MovieId,MovieGenreId,MovieDirectorId,MovieTitle,MoviePrice,MoviePosterUrl")] Movie movie)
        {
            //如果输入正确，符合规则则添加专辑
            //规则来源于实体注解
            //重定向到index
            if (ModelState.IsValid)
            {
                this.db.Movies.Add(movie);
                this.db.SaveChanges();
                return RedirectToAction("Index");
            }
            //否则返回Create View并且显示错误信息
            ViewBag.MovieDirectorId = new SelectList(this.db.MovieDirectors, "MovieDirectorId", "MovieDirectorName", movie.MovieDirectorId);
            ViewBag.MovieGenreId = new SelectList(this.db.MovieGenres, "MovieGenreId", "MovieGenreName", movie.MovieGenreId);
            return View(movie);
        }

        // GET: /StoreManager/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = this.db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            ViewBag.MovieDirectorId = new SelectList(this.db.MovieDirectors, "MovieDirectorId", "MovieDirectorName", movie.MovieDirectorId);
            ViewBag.MovieGenreId = new SelectList(this.db.MovieGenres, "MovieGenreId", "MovieGenreName", movie.MovieGenreId);
            return View(movie);
        }

        // POST: /StoreManager/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关
        // 详细信息，请参阅 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include= "MovieId,MovieGenreId,MovieDirectorId,MovieTitle,MoviePrice,MoviePosterUrl")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                Movie dbmovie = this.db.Movies.Single(m => m.MovieTitle == movie.MovieTitle);
                dbmovie.MovieDirectorId = movie.MovieDirectorId;
                dbmovie.MovieGenreId = movie.MovieGenreId;
                dbmovie.MoviePosterUrl = movie.MoviePosterUrl;
                dbmovie.MoviePrice = movie.MoviePrice;
                dbmovie.MovieTitle = movie.MovieTitle;
                this.db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MovieDirectorId = new SelectList(this.db.MovieDirectors, "MovieDirectorId", "MovieDirectorName", movie.MovieDirectorId);
            ViewBag.MovieGenreId = new SelectList(this.db.MovieGenres, "MovieGenreId", "MovieGenreName", movie.MovieGenreId);
            return View(movie);
        }

        // GET: /StoreManager/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie album = this.db.Movies.Find(id);
            if (album == null)
            {
                //404方法
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: /StoreManager/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = this.db.Movies.Find(id);
            this.db.Movies.Remove(movie);
            this.db.SaveChanges();
            //采用重定向
            return RedirectToAction("Index");
        }

        //Szebb lenne int> minden int-hez 0-tól hibáktípusok számáig
        public ActionResult UploadFile(string error)
        {
            if (error != null)
            {
                ModelState.AddModelError("", error);
            }
            return View();
        }
        /// <summary>
        /// File feltöltés
        /// Csak csv-t fogad el
        /// </summary>
        ///<returns></returns>
        public async Task<ActionResult> UploadFileProcess(HttpPostedFileBase file)
        {
            // ha nem lett file kiválasztva
            if (file == null)
            {
                return RedirectToAction("UploadFile",
                    new { error = "No file found." });
            }
            //Ha nem megfelelő a file végződés
            if (!FILE_EXTENSIONS.Contains(file.FileName.Split('.').Last()))
            {
                return RedirectToAction("UploadFile",
                    new { error = "The file extension is incorrect.\nCorrect file extensions: csv" });
            }
            List<string> csvData = new List<string>();
            using (StreamReader reader = new StreamReader(file.InputStream, Encoding.Default,  true))
            {
                while (!reader.EndOfStream)
                {
                    csvData.Add(reader.ReadLine());
                }
            }
            for (int i = 0; i < csvData.Count; i++)
            {
                string line = csvData[i];
                string[] datas = line.Split(';');
                if(datas.Length != 5)
                {
                    return RedirectToAction("UploadFile",
                        new { error = "Incorrect file format. Line: " + (line + 1) + "\nCorrect file format: MovieTitle;MovieGenre;MoviePrice;DirectorName;PosterUrl\nEncoding:ANSI" });
                }
                string title = datas[0];
                string genre_in = datas[1];
                MovieGenre genre = this.db.MovieGenres.FirstOrDefault(g => g.MovieGenreName == genre_in);
                decimal price = Convert.ToDecimal(datas[2]);
                string director_in = datas[3];
                MovieDirector director = this.db.MovieDirectors.FirstOrDefault(d => d.MovieDirectorName == director_in);
                string posterUrl = datas[4];
                // a megadott director nincs benne az adatbázisban
                if(director == null)
                {
                    return RedirectToAction("UploadFile",
                        new { error = "The given director: " + director_in + " cannot be found in the database" });
                }
                if(genre == null)
                {
                    return RedirectToAction("UploadFile",
                        new { error = "The given genre: " + genre_in + " cannot be found in the database" });
                }
                // ha az adott film már szerepel a listában, akkor csak átírjuk az adatait
                Movie movie = this.db.Movies.FirstOrDefault(m => m.MovieTitle == title);
                if (movie != null)
                {
                    movie.MovieTitle = title;
                    movie.MovieGenre = genre;
                    movie.MoviePrice = price;
                    movie.MovieDirector = director;
                    movie.MoviePosterUrl = posterUrl;
                }
                // ha nem volt benne az adatbázisban
                else
                {
                    this.db.Movies.Add(new Movie()
                    {
                        MovieTitle = title,
                        MovieGenre = genre,
                        MoviePrice = price,
                        MovieDirector = director,
                        MoviePosterUrl = posterUrl
                    });
                }
                this.db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Elküldi az adatbázis tartalmát
        /// </summary>
        /// <returns></returns>
        public ActionResult DownloadFiles()
        {
            string lines = "";
            foreach (var item in this.db.Movies)
            {
                string title = item.MovieTitle;
                string genre = item.MovieGenre.MovieGenreName;
                string price = item.MoviePrice.ToString();
                string director = item.MovieDirector.MovieDirectorName;
                string poster = item.MoviePosterUrl;
                lines += title + ";" + genre + ";" + price + ";" + director + ";" + poster + '\n';
             }
            var byteArray = Encoding.Default.GetBytes(lines);
            return File(byteArray, "text/plain");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
