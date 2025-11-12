using AutoMapper;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using proyecto_prog4.Config;
using proyecto_prog4.Enums;
using proyecto_prog4.Models.Rol;
using proyecto_prog4.Models.User.Dto;
using proyecto_prog4.Models.Usuario;
using proyecto_prog4.Repositories;
using proyecto_prog4.Utils;
using System.Net;

namespace proyecto_prog4.Services
{

    public class UserServices
    {
        private readonly AppDbContext _context;
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly RolServices _rolServices;
        public UserServices(IUserRepository repo, AppDbContext context, IMapper mapper, RolServices rolServices)
        {
            _context = context;
            _repo = repo;
            _mapper = mapper;
            _rolServices = rolServices;
        }

        async private Task<Usuario> GetOneByIdOrException(int id)
        {
            var u = await _context.Usuario
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (u == null)
            {
                throw new HttpResponseError(
                    HttpStatusCode.NotFound,
                    $"No se encontro el Usuario con ID = {id}"
                );
            }
            return u;
        }

        async public Task<Usuario> GetOneById(int id) => await GetOneByIdOrException(id);

        public async Task<Usuario> GetUserByEmailOrUsernameAsync(string? email, string? userName)
        {
            Usuario? usuario;

            if (!string.IsNullOrEmpty(userName))
            {
                usuario = await _repo.GetByUserName(userName);
            }
            else if (!string.IsNullOrEmpty(email))
            {
                usuario = await _repo.GetByEmail(email);
            }
            else
            {
                throw new HttpResponseError(
                    HttpStatusCode.BadRequest,
                    "UserName and email are empty"
                );
            }
            if (usuario is null)
            {
                throw new HttpResponseError(HttpStatusCode.NotFound, "Usuario no encontrado");
            }
            return usuario;
        }


        async public Task<Usuario> CreateOne(RegisterDTO registerDTO)
        {
            var user = _mapper.Map<Usuario>(registerDTO);

            var rolDefault = await _rolServices.GetOneByName(ROL.USER);

            user.Roles = new List<Rol>() { rolDefault };

            await _repo.CreateOne(user);

            return user;
        }

        async public Task<Usuario> UpdateOne(Usuario usuario)
        {
            await _repo.UpdateOne(usuario);
            return usuario;
        }

        public async Task<List<Usuario>> GetAllUsersAsync()
        {
            return await _context.Usuario
                .Include(u => u.Roles)
                .ToListAsync();
        }
    }
}
