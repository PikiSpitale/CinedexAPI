using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proyecto_prog4.Enums;
using proyecto_prog4.Models.Genres.Dto;
using proyecto_prog4.Models.Movie;
using proyecto_prog4.Models.Movie.Dto;
using proyecto_prog4.Services;
using proyecto_prog4.Utils;

namespace proyecto_prog4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly MovieServices _movieService;

        public MovieController(MovieServices movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<Movie>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] string? search)
        {
            var movies = await _movieService.GetAll();

            if (!string.IsNullOrEmpty(search))
            {
                movies = movies.Where(m =>
                    (m.Title?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (m.Description?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false)
                ).ToList();
            }

            return Ok(movies);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MoviesDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _movieService.GetOne(id);
            if (movie == null)
                return NotFound(new { message = "Movie not found" });

            var dto = new MoviesDTO
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                PosterPath = movie.PosterUrl,
                ReleaseDate = movie.ReleaseDate.ToString("yyyy-MM-dd"),
                Rating = movie.Rating,
                Genres = movie.Genres?
                    .Select(g => new GenreDTO
                    {
                        Id = g.GenreId,
                        Name = g.Genre?.Name ?? string.Empty
                    })
                    .ToList()
            };

            return Ok(dto);
        }

        [HttpPost]
        [Authorize(Roles = $"{ROL.ADMIN},{ROL.MOD}")]
        [ProducesResponseType(typeof(MoviesDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Create([FromBody] CreateMovieDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var movie = await _movieService.Create(dto);
            return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = $"{ROL.ADMIN},{ROL.MOD}")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMovieDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _movieService.Update(id, dto);
            if (!result)
                return NotFound(new { message = "Movie not found" });

            return Ok(new { message = "Movie updated successfully" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = $"{ROL.ADMIN},{ROL.MOD}")]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _movieService.Delete(id);
            if (!result)
                return NotFound(new { message = "Movie not found" });

            return NoContent();
        }
    }
}