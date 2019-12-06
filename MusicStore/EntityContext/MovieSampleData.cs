using MusicStore.Models;
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
            // Admin hozzáadása
            context.Accounts.Add(new Account() {
                UserName = "Admin",
                Email = "admin@musictore.hu",
                IsAdmin = true,
                Password = "Admin123!" });

            //Filmek hozzáadása
            context.Movies.Add(new Movie()
            {
                Title = "Az első ember",
                Director = "Damien Chazelle",
                ReleaseYear = 2018,
                Runtime = 141
            });
            context.Movies.Add(new Movie()
            {
                Title = "Schindler listája",
                Director = "Steven Spielberg",
                ReleaseYear = 1993,
                Runtime = 195
            });
            context.Movies.Add(new Movie()
            {
                Title = "Titanic",
                Director = "James Cameron",
                ReleaseYear = 1997,
                Runtime = 194
            });
            context.Movies.Add(new Movie()
            {
                Title = "Felhő atlasz",
                Director = "Lana Wachowski",
                ReleaseYear = 2012,
                Runtime = 172
            });
        }
    }
}