namespace proyecto_prog4.Models.Genres.Dto
{
    public class GenreWithMoviesDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<MovieSummaryDTO> Movies { get; set; } = new();
    }

    public class MovieSummaryDTO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? PosterUrl { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}

