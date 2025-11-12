namespace proyecto_prog4.Models.Movie.Dto
{
    public class UpdateMovieDTO
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? PosterPath { get; set; }
        public string? ReleaseDate { get; set; }
        public double? Rating { get; set; }
        public List<int>? GenreIds { get; set; }
    }
}

