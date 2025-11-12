using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proyecto_prog4.Enums;
using proyecto_prog4.Models.Genres;
using proyecto_prog4.Models.Genres.Dto;
using MovieGenreLink = proyecto_prog4.Models.MovieGenres.MovieGenres;
using proyecto_prog4.Services;

namespace proyecto_prog4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenreController : ControllerBase
    {
        private readonly GenresServices _genreService;

        public GenreController(GenresServices genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var genres = await _genreService.GetAll();
            var dto = genres.Select(MapGenre);
            return Ok(dto);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var genre = await _genreService.GetById(id);
            if (genre == null)
                return NotFound(new { message = "Genre not found" });

            return Ok(MapGenre(genre));
        }

        [HttpPost]
        [Authorize(Roles = ROL.ADMIN)]
        public async Task<IActionResult> Create([FromBody] GenreCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var genre = await _genreService.Create(dto.Name);
            if (genre is null)
                return Conflict(new { message = "Genre already exists" });

            return CreatedAtAction(nameof(GetById), new { id = genre.Id }, MapGenre(genre));
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = ROL.ADMIN)]
        public async Task<IActionResult> Update(int id, [FromBody] GenreUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var updated = await _genreService.Update(id, dto.Name);
                if (!updated)
                    return NotFound(new { message = "Genre not found" });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }

            return Ok(new { message = "Genre updated successfully" });
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = ROL.ADMIN)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _genreService.Delete(id);
            if (!deleted)
                return NotFound(new { message = "Genre not found" });

            return NoContent();
        }

        private static GenreWithMoviesDTO MapGenre(Genre genre)
        {
            var movies = genre.Movies ?? new List<MovieGenreLink>();

            return new GenreWithMoviesDTO
            {
                Id = genre.Id,
                Name = genre.Name,
                Movies = movies
                    .Select(mg => new MovieSummaryDTO
                    {
                        Id = mg.MovieId,
                        Title = mg.Movie?.Title,
                        PosterUrl = mg.Movie?.PosterUrl,
                        ReleaseDate = mg.Movie?.ReleaseDate ?? default
                    })
                    .ToList()
            };
        }
    }
}
