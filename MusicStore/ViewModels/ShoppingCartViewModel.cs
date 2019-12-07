using MusicStore.Models;
using MusicStore.Models.Database.Movie;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MusicStore.ViewModels
{
    public class ShoppingCartViewModel
    {
        public List<MovieCart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}