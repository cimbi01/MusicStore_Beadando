using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStore.Models.Database.Movie
{
    public class MovieOrderDetail
    {
        public int MovieOrderDetailId { get; set; }
        public int MovieOrderId { get; set; }
        public int MovieId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public virtual Movie Movie { get; set; }
        public virtual MovieOrder MovieOrder { get; set; }
    }
}