using proyecto_prog4.Models.Genres.Dto;
using proyecto_prog4.Models.Movie;
using proyecto_prog4.Models.Movie.Dto;
using proyecto_prog4.Models.MovieGenres;
using proyecto_prog4.Repositories;

namespace proyecto_prog4.Services
{
    public class MovieServices
    {
        private readonly IMovieRepository _movieRepository;

        public MovieServices(IMovieRepository movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public async Task<List<MoviesDTO>> GetAll()
        {
            var movies = await _movieRepository.GetAll();
            return movies.Select(MapMovieToDto).ToList();
        }

        public async Task<Movie?> GetOne(int id)
        {
            return await _movieRepository.GetOne(x => x.Id == id);
        }

        public async Task<MoviesDTO> Create(CreateMovieDTO dto)
        {
            var movie = new Movie
            {
                Title = dto.Title,
                Description = dto.Description ?? string.Empty,
                ReleaseDate = string.IsNullOrEmpty(dto.ReleaseDate)
                    ? DateTime.Now
                    : DateTime.Parse(dto.ReleaseDate),
                PosterUrl = dto.PosterPath ?? string.Empty,
                Rating = dto.Rating
            };

            if (dto.GenreIds != null && dto.GenreIds.Any())
            {
                movie.Genres = dto.GenreIds
                    .Distinct()
                    .Select(id => new MovieGenres
                    {
                        GenreId = id,
                        Movie = movie
                    })
                    .ToList();
            }

            await _movieRepository.CreateOne(movie);

            var createdMovie = await _movieRepository.GetOne(x => x.Id == movie.Id);
            if (createdMovie == null)
            {
                return MapMovieToDto(movie);
            }

            return MapMovieToDto(createdMovie);
        }

        private static MoviesDTO MapMovieToDto(Movie movie)
        {
            return new MoviesDTO
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                PosterPath = movie.PosterUrl,
                ReleaseDate = movie.ReleaseDate.ToString("yyyy-MM-dd"),
                Rating = movie.Rating,
                Genres = movie.Genres?.Select(g => new GenreDTO
                {
                    Id = g.GenreId,
                    Name = g.Genre?.Name ?? string.Empty
                }).ToList() ?? new List<GenreDTO>()
            };
        }

        public async Task<bool> Update(int id, UpdateMovieDTO dto)
        {
            var movie = await _movieRepository.GetOne(x => x.Id == id);
            if (movie == null) return false;

            if (!string.IsNullOrEmpty(dto.Title))
                movie.Title = dto.Title;
            if (!string.IsNullOrEmpty(dto.Description))
                movie.Description = dto.Description;
            if (!string.IsNullOrEmpty(dto.ReleaseDate))
                movie.ReleaseDate = DateTime.Parse(dto.ReleaseDate);
            if (!string.IsNullOrEmpty(dto.PosterPath))
                movie.PosterUrl = dto.PosterPath;
            if (dto.Rating.HasValue)
                movie.Rating = dto.Rating.Value;

            if (dto.GenreIds != null)
            {
                var targetGenres = dto.GenreIds.Distinct().ToList();
                movie.Genres ??= new List<MovieGenres>();

                var toRemove = movie.Genres
                    .Where(mg => !targetGenres.Contains(mg.GenreId))
                    .ToList();
                foreach (var remove in toRemove)
                {
                    movie.Genres.Remove(remove);
                }

                var existingIds = movie.Genres
                    .Select(mg => mg.GenreId)
                    .ToHashSet();

                foreach (var genreId in targetGenres)
                {
                    if (!existingIds.Contains(genreId))
                    {
                        movie.Genres.Add(new MovieGenres
                        {
                            GenreId = genreId,
                            MovieId = movie.Id
                        });
                    }
                }
            }

            await _movieRepository.UpdateOne(movie);
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var movie = await _movieRepository.GetOne(x => x.Id == id);
            if (movie == null) return false;

            await _movieRepository.DeleteOne(movie);
            return true;
        }
    }
}

