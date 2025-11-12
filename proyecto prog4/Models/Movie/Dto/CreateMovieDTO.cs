using System.ComponentModel.DataAnnotations;

namespace proyecto_prog4.Models.Movie.Dto
{
    public class CreateMovieDTO
    {
        [Required]
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? PosterPath { get; set; }
        public string? ReleaseDate { get; set; }
        public double Rating { get; set; }
        [Required]
        public List<int>? GenreIds { get; set; }
    }
}

