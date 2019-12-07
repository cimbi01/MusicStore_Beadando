using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicStore.Models.Database.Movie
{
    public class MovieCart
    {
        [Key]
        public int MovieRecordId { get; set; }
        public string MovieCartId { get; set; }
        public int MovieId { get; set; }
        public int Count { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual Movie Movie { get; set; }
    }
}