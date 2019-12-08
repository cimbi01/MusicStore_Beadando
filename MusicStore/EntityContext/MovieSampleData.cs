using MusicStore.Models;
using MusicStore.Models.Database.Movie;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MusicStore.EntityContext
{
    public class MovieSampleData : DropCreateDatabaseIfModelChanges<MovieStoreEntities>
    {
        protected override void Seed(MovieStoreEntities context)
        {
            var movie_genres = new List<MovieGenre>()
            {
                new MovieGenre() {MovieGenreName = "Dráma"},
                new MovieGenre() {MovieGenreName = "Kaland"},
                new MovieGenre() {MovieGenreName = "Életrajzi"},
                new MovieGenre() {MovieGenreName = "Krimi"},
                new MovieGenre() {MovieGenreName = "Vígjáték"},
            };

            var movie_directors = new List<MovieDirector>()
            {
                new MovieDirector() { MovieDirectorName = "James Cameron"},
                new MovieDirector() { MovieDirectorName = "Peter Jackson"},
                new MovieDirector() { MovieDirectorName = "Steven Spielberg"},
                new MovieDirector() { MovieDirectorName = "Martin Scorsese"},
                new MovieDirector() { MovieDirectorName = "Robert Zemeckis"},
            };

            new List<Movie>()
            {
                new Movie {
                    MovieTitle = "Titanik",
                    MovieGenre = movie_genres.Single(g => g.MovieGenreName == "Dráma"),
                    MoviePrice = 8.99M,
                    MovieDirector = movie_directors.Single(d => d.MovieDirectorName == "James Cameron"),
                    MoviePosterUrl = "/Content/Images/placeholder.gif" },
                new Movie {
                    MovieTitle = "A Gyűrűk Ura: A király visszatér",
                    MovieGenre = movie_genres.Single(g => g.MovieGenreName == "Kaland"),
                    MoviePrice = 8.99M,
                    MovieDirector = movie_directors.Single(d => d.MovieDirectorName == "Peter Jackson"),
                    MoviePosterUrl = "/Content/Images/placeholder.gif" },
                new Movie {
                    MovieTitle = "Schindler listája",
                    MovieGenre = movie_genres.Single(g => g.MovieGenreName == "Életrajzi"),
                    MoviePrice = 8.99M,
                    MovieDirector = movie_directors.Single(d => d.MovieDirectorName == "Steven Spielberg"),
                    MoviePosterUrl = "/Content/Images/placeholder.gif" },
                new Movie {
                    MovieTitle = "A tégla",
                    MovieGenre = movie_genres.Single(g => g.MovieGenreName == "Krimi"),
                    MoviePrice = 8.99M,
                    MovieDirector = movie_directors.Single(d => d.MovieDirectorName == "Martin Scorsese"),
                    MoviePosterUrl = "/Content/Images/placeholder.gif" },
                new Movie {
                    MovieTitle = "Vissza a jövőbe",
                    MovieGenre = movie_genres.Single(g => g.MovieGenreName == "Vígjáték"),
                    MoviePrice = 8.99M,
                    MovieDirector = movie_directors.Single(d => d.MovieDirectorName == "Robert Zemeckis"),
                    MoviePosterUrl = "/Content/Images/placeholder.gif" },
            }.ForEach(m => context.Movies.Add(m));

            // Admin hozzáadása
            context.Accounts.Add(new Account()
            {
                UserName = "Admin",
                Email = "admin@musictore.hu",
                IsAdmin = true,
                Password = "Admin123!"
            });
            //felhasznalo, hogy ne kelljen mindig regisztralni a teszthez
            context.Accounts.Add(new Account()
            {
                UserName = "asd",
                Email = "asd@asd.asd",
                IsAdmin = false,
                Password = "asd123"
            });
        }
    }
}