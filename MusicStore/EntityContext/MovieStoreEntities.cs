using MusicStore.Models;
using MusicStore.Models.Database.Movie;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MusicStore.EntityContext
{
    public class MovieStoreEntities : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<MovieDirector> MovieDirectors { get; set; }
        public DbSet<MovieCart> MovieCarts { get; set; }
        public DbSet<MovieOrder> MovieOrders { get; set; }
        public DbSet<MovieOrderDetail> MovieOrderDetails { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public MovieStoreEntities()
        {
            Database.SetInitializer(new MovieSampleData());
        }
    }
}