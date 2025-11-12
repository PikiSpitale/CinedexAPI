using proyecto_prog4.Models.Genres.Dto;

namespace proyecto_prog4.Models.Movie.Dto
{
    public class MoviesDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? PosterPath { get; set; }
        public string? ReleaseDate { get; set; }
        public double Rating { get; set; }
        public List<GenreDTO>? Genres { get; set; }
    }
}

