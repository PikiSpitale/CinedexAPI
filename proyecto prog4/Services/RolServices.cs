using Microsoft.AspNetCore.DataProtection.Repositories;
using proyecto_prog4.Models.Rol;
using proyecto_prog4.Repositories;
using proyecto_prog4.Utils;
using System.Net;
using System.Linq;

namespace proyecto_prog4.Services
{
    public class RolServices
    {
        private readonly IRolRepository _repo;

        public RolServices(IRolRepository repo)
        {
            _repo = repo;
        }

        async public Task<List<Rol>> GetAll()
        {
            var roles = await _repo.GetAll();
            return roles.ToList();
        }

        async public Task<Rol> GetOneByName(string name)
        {
            var rol = await _repo.GetOne(x => x.Nombre == name) ?? null!;
            return rol;
        }

        async public Task<List<Rol>> GetManyByIds(List<int> Ids)
        {
            if (Ids.Count == 0 || Ids == null)
            {
                throw new HttpResponseError(
                    HttpStatusCode.BadRequest,
                    "La lista de Ids esta vacia"
                );
            }

            var roles = await _repo.GetAll(x => Ids.Contains(x.Id));
            if (roles.ToList().Count > 0)
            {
                return roles.ToList();
            }

            throw new HttpResponseError(
                    HttpStatusCode.BadRequest,
                    "Nigun Id coincide"
            );
        }
    }
}
