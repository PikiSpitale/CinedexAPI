using proyecto_prog4.Models.MovieGenres;

namespace proyecto_prog4.Models.Genres
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<MovieGenres.MovieGenres> Movies { get; set; } = new List<MovieGenres.MovieGenres>();
    }
}
