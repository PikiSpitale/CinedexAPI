using proyecto_prog4.Config;
using proyecto_prog4.Models.Rol;

namespace proyecto_prog4.Repositories
{
    public interface IRolRepository : IRepository<Rol> { }
    public class RolRepository : Repository<Rol>, IRolRepository
    {
        private readonly AppDbContext _db;

        public RolRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
