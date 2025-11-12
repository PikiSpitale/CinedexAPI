using Microsoft.EntityFrameworkCore;
using proyecto_prog4.Models.Usuario;
using proyecto_prog4.Config;
using System.Linq.Expressions;

namespace proyecto_prog4.Repositories
{
    public interface IUserRepository : IRepository<Usuario>
    {
        Task<Usuario?> GetByEmail(string email);
        Task<Usuario?> GetByUserName(string userName);
    }

    public class UserRepository : Repository<Usuario>, IUserRepository
    {
        private readonly AppDbContext _db;

        public UserRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Usuario?> GetByEmail(string email)
        {
            return await dbSet
                .Where(u => u.Email == email)
                .Include(u => u.Roles)
                .FirstOrDefaultAsync();
        }

        public async Task<Usuario?> GetByUserName(string userName)
        {
            return await dbSet
                .Where(u => u.UserName == userName)
                .Include(u => u.Roles)
                .FirstOrDefaultAsync();
        }
    }
}