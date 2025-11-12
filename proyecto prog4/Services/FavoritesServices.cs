using Microsoft.EntityFrameworkCore;
using proyecto_prog4.Config;
using proyecto_prog4.Models.UserFavorite;
using proyecto_prog4.Models.UserFavorite.Dto;
using proyecto_prog4.Utils;
using System.Net;

namespace proyecto_prog4.Services
{
    public class FavoritesServices
    {
        private readonly AppDbContext _context;

        public FavoritesServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddFavoriteAsync(int usuarioId, int movieId)
        {
            var exists = await _context.UserFavorites
                .AnyAsync(x => x.UsuarioId == usuarioId && x.MovieId == movieId);
            if (exists) throw new HttpResponseError(HttpStatusCode.BadRequest, "Ya es favorito");

            var favorite = new UserFavorite
            {
                UsuarioId = usuarioId,
                MovieId = movieId,
                FechaCreacion = DateTime.UtcNow
            };

            _context.UserFavorites.Add(favorite);
            await _context.SaveChangesAsync();
        }

        public async Task<List<FavoriteMovieDTO>> GetFavoritesAsync(int usuarioId)
        {
            return await _context.UserFavorites
                .Where(x => x.UsuarioId == usuarioId)
                .Select(x => new FavoriteMovieDTO
                {
                    MovieId = x.MovieId,
                    Title = x.Movie.Title,
                    PosterUrl = x.Movie.PosterUrl,
                    Rating = x.Movie.Rating,
                    AddedAt = x.FechaCreacion
                })
                .ToListAsync();
        }

        public async Task RemoveFavoriteAsync(int usuarioId, int movieId)
        {
            var favorite = await _context.UserFavorites
                .FirstOrDefaultAsync(x => x.UsuarioId == usuarioId && x.MovieId == movieId);

            if (favorite == null)
                throw new HttpResponseError(HttpStatusCode.NotFound, "No existe el favorito");

            _context.UserFavorites.Remove(favorite);
            await _context.SaveChangesAsync();
        }
    }
}
