using System;

namespace proyecto_prog4.Models.UserFavorite.Dto
{
    public class FavoriteMovieDTO
    {
        public int MovieId { get; set; }
        public string Title { get; set; } = null!;
        public string PosterUrl { get; set; } = null!;
        public double Rating { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
