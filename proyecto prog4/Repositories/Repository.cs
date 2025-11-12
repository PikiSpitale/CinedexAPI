using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using proyecto_prog4.Config; // 👉 Asegurate de tener tu DbContext en esta carpeta o cambiar el using según tu estructura.

namespace proyecto_prog4.Repositories
{
    // 🔹 Interfaz genérica para CRUD
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null);
        Task<T> GetOne(Expression<Func<T, bool>>? filter = null);
        Task CreateOne(T entity);
        Task UpdateOne(T entity);
        Task DeleteOne(T entity);
        Task Save();
    }

    // 🔹 Implementación del repositorio genérico
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDbContext _db;   // 🔸 Cambia el nombre del contexto si el tuyo es diferente
        internal DbSet<T> dbSet;

        public Repository(AppDbContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            return await query.ToListAsync();
        }

        public async Task<T> GetOne(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
                query = query.Where(filter);

            return await query.FirstOrDefaultAsync();
        }

        public async Task CreateOne(T entity)
        {
            await dbSet.AddAsync(entity);
            await Save();
        }

        public async Task UpdateOne(T entity)
        {
            dbSet.Update(entity);
            await Save();
        }

        public async Task DeleteOne(T entity)
        {
            dbSet.Remove(entity);
            await Save();
        }

        public async Task Save() => await _db.SaveChangesAsync();
    }
}
