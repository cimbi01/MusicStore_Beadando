using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MusicStore.Models.Database.Movie
{
    [Bind(Exclude = "MovieId")]
    public class Movie
    {
        [ScaffoldColumn(false)]
        public int MovieId { get; set; }
        [DisplayName("MovieGenre")]
        public int MovieGenreId { get; set; }
        [DisplayName("MovieDirector")]
        public int MovieDirectorId { get; set; }
        [Required(ErrorMessage = "A Movie Title is required")]
        [StringLength(160)]
        public string MovieTitle { get; set; }
        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 100.00, ErrorMessage = "Price must be between 0.01 and 100.00")]
        public decimal MoviePrice { get; set; }
        [DisplayName("Movie Poster URL")]
        [StringLength(1024)]
        public string MoviePosterUrl { get; set; }
        public virtual MovieGenre MovieGenre { get; set; }
        public virtual MovieDirector MovieDirector { get; set; }
        public virtual List<MovieOrderDetail> MovieOrderDetails { get; set; }
    }
}