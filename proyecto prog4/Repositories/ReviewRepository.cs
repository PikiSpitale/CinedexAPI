using Microsoft.EntityFrameworkCore;
using proyecto_prog4.Models.Review;
using proyecto_prog4.Config;
using System.Linq.Expressions;

namespace proyecto_prog4.Repositories
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IEnumerable<Review>> GetByMovieId(int movieId);
        Task<IEnumerable<Review>> GetByUserId(int userId);
    }

    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        private readonly AppDbContext _db;

        public ReviewRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Review>> GetByMovieId(int movieId)
        {
            return await dbSet
                .Where(r => r.MovieId == movieId)
                .Include(r => r.Usuario)
                .Include(r => r.Movie)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetByUserId(int userId)
        {
            return await dbSet
                .Where(r => r.UserId == userId)
                .Include(r => r.Usuario)
                .Include(r => r.Movie)
                .ToListAsync();
        }
    }
}
