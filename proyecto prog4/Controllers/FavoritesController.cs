using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using proyecto_prog4.Models.UserFavorite.Dto;
using proyecto_prog4.Services;
using proyecto_prog4.Utils;
using System.Net;

namespace proyecto_prog4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoritesController : ControllerBase
    {
        private readonly FavoritesServices _favoritesService;

        public FavoritesController(FavoritesServices favoritesService)
        {
            _favoritesService = favoritesService;
        }

        [HttpPost]
        public async Task<IActionResult> AddFavorite([FromBody] AddFavoriteDTO dto)
        {
            try
            {
                int usuarioId = GetAuthenticatedUserId();
                await _favoritesService.AddFavoriteAsync(usuarioId, dto.MovieId);
                return NoContent();
            }
            catch (HttpResponseError ex)
            {
                return StatusCode((int)ex.StatusCode, new HttpMessage(ex.Message));
            }
        }

        [HttpDelete("{movieId:int}")]
        public async Task<IActionResult> RemoveFavorite(int movieId)
        {
            try
            {
                int usuarioId = GetAuthenticatedUserId();
                await _favoritesService.RemoveFavoriteAsync(usuarioId, movieId);
                return NoContent();
            }
            catch (HttpResponseError ex)
            {
                return StatusCode((int)ex.StatusCode, new HttpMessage(ex.Message));
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<FavoriteMovieDTO>>> GetFavorites()
        {
            int usuarioId = GetAuthenticatedUserId();
            var favorites = await _favoritesService.GetFavoritesAsync(usuarioId);
            return Ok(favorites);
        }

        private int GetAuthenticatedUserId()
        {
            var claim = User.FindFirst("id");
            if (claim == null)
            {
                throw new HttpResponseError(
                    HttpStatusCode.Unauthorized,
                    "El usuario no está autenticado"
                );
            }

            return int.Parse(claim.Value);
        }
    }
}
