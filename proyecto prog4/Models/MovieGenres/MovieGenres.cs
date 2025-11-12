using proyecto_prog4.Models.Genres;

namespace proyecto_prog4.Models.MovieGenres
{
    public class MovieGenres
    {
        public int GenreId { get; set; }
        public Genre Genre { get; set; } = null!;
        public int MovieId { get; set; }
        public Movie.Movie Movie { get; set; } = null!;
    }
}


