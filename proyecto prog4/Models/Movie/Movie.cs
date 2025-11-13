using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace proyecto_prog4.Models.Movie
{
    public class Movie
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime ReleaseDate { get; set; }
        public string PosterUrl { get; set; } = null!;
        public double Rating { get; set; }
        public ICollection<MovieGenres.MovieGenres> Genres { get; set;} = new List<MovieGenres.MovieGenres>();
        public ICollection<UserFavorite.UserFavorite> Favoritos { get; set; } = new List<UserFavorite.UserFavorite>();
    }
}
