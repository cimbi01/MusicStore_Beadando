using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStore.Models.Database.Movie
{
    public class MovieGenre
    {
        public int MovieGenreId { get; set; }
        public string MovieGenreName { get; set; }
        public string MovieGenreDescription { get; set; }
        public List<Movie> Movies { get; set; }
    }
}