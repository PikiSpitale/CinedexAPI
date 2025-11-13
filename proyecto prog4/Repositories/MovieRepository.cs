using Microsoft.EntityFrameworkCore;
using proyecto_prog4.Models.Movie;
using proyecto_prog4.Config;
using System.Linq;
using System.Linq.Expressions;

namespace proyecto_prog4.Repositories
{
    public interface IMovieRepository : IRepository<Movie>
    {
        new Task<IEnumerable<Movie>> GetAll(Expression<Func<Movie, bool>>? filter = null);
        new Task<Movie?> GetOne(Expression<Func<Movie, bool>>? filter = null);
    }

    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        private readonly AppDbContext _db;

        public MovieRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async new Task<IEnumerable<Movie>> GetAll(Expression<Func<Movie, bool>>? filter = null)
        {
            IQueryable<Movie> query = dbSet;
            if (filter != null)
                query = query.Where(filter);

            return await query
                .Include(x => x.Genres)
                    .ThenInclude(mg => mg.Genre)
                .ToListAsync();
        }

        public async new Task<Movie?> GetOne(Expression<Func<Movie, bool>>? filter = null)
        {
            IQueryable<Movie> query = dbSet;
            if (filter != null)
                query = query.Where(filter);

            return await query
                .Include(x => x.Genres)
                    .ThenInclude(mg => mg.Genre)
                .FirstOrDefaultAsync();
        }
    }
}
