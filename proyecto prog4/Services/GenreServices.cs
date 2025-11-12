using Microsoft.EntityFrameworkCore;
using proyecto_prog4.Config;
using proyecto_prog4.Models.Genres;

namespace proyecto_prog4.Services
{
    public class GenresServices
    {
        private readonly AppDbContext _context;

        public GenresServices(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Genre>> GetAll()
        {
            return await _context.Genres
                .Include(g => g.Movies)
                    .ThenInclude(mg => mg.Movie)
                .OrderBy(g => g.Name)
                .ToListAsync();
        }

        public async Task<Genre?> GetById(int id)
        {
            return await _context.Genres
                .Include(g => g.Movies)
                    .ThenInclude(mg => mg.Movie)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<Genre?> Create(string name)
        {
            var trimmed = name.Trim();
            var exists = await _context.Genres.AnyAsync(g => g.Name == trimmed);
            if (exists)
                return null;

            var genre = new Genre { Name = trimmed };
            _context.Genres.Add(genre);
            await _context.SaveChangesAsync();
            return genre;
        }

        public async Task<bool> Update(int id, string name)
        {
            var genre = await _context.Genres.FirstOrDefaultAsync(g => g.Id == id);
            if (genre is null)
                return false;

            if (!string.IsNullOrWhiteSpace(name))
            {
                var trimmed = name.Trim();
                var exists = await _context.Genres
                    .AnyAsync(g => g.Id != id && g.Name == trimmed);

                if (exists)
                    throw new InvalidOperationException("Genre name already in use.");

                genre.Name = trimmed;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var genre = await _context.Genres
                .Include(g => g.Movies)
                .FirstOrDefaultAsync(g => g.Id == id);

            if (genre is null)
                return false;

            if (genre.Movies.Any())
                _context.RemoveRange(genre.Movies);

            _context.Genres.Remove(genre);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
