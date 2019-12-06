using MusicStore.Models;
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
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public MovieStoreEntities()
        {}
    }
}