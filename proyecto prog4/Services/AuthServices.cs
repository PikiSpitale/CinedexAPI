using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using proyecto_prog4.Config;
using proyecto_prog4.Models.Rol;
using proyecto_prog4.Models.User.Dto;
using proyecto_prog4.Models.Usuario;
using proyecto_prog4.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace proyecto_prog4.Services
{
    public class AuthServices
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserServices _userServices;
        private readonly RolServices _rolServices;
        private readonly IEncoderServices _encoderServices;
        private readonly IMapper _mapper;
        private string _secret;

        public AuthServices(AppDbContext context, IConfiguration configuration, UserServices userServices, IEncoderServices encoderServices, IMapper mapper, RolServices rolServices)
        {
            _context = context;
            _configuration = configuration;
            _userServices = userServices;
            _encoderServices = encoderServices;
            _mapper = mapper;
            _rolServices = rolServices;
            _secret = _configuration.GetSection("Secrets")?.GetSection("JWT")?.Value?.ToString() ?? null!;
        }

        public async Task<LoginResponseDTO> RegisterAsync(RegisterDTO register)
        {
            var existingUser = await GetUserByEmailOrUsernameAsync(register.Email);
            if (existingUser != null) 
            { 
                throw new Exception("Email already registered");
            }
            if (register.Password != register.ConfirmPassword)
            {
                throw new HttpResponseError(HttpStatusCode.BadRequest, "Password doesn't match");
            }

            register.Password = _encoderServices.Encode(register.Password);

            var userCreated = await _userServices.CreateOne(register);

            // Generar token para el usuario recién creado
            var token = GenerateToken(userCreated);
            return new LoginResponseDTO { Token = token, User = _mapper.Map<UsuarioWithRolesDTO>(userCreated) };
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginDTO dto, HttpContext context)
        {
            bool IsEmail = dto.EmailOrUsername.Contains("@");
            Usuario user;
            if (IsEmail)
            {
                user = await _userServices.GetUserByEmailOrUsernameAsync(dto.EmailOrUsername, null);
            }
            else
            {
                user = await _userServices.GetUserByEmailOrUsernameAsync(null, dto.EmailOrUsername);
            }

            if (user == null)
            {
                throw new HttpResponseError(HttpStatusCode.BadRequest, "Invalid credentials");
            }

            bool IsPassMatch = _encoderServices.Verify(dto.Password, user.Password);

            if (!IsPassMatch)
            {
                throw new HttpResponseError(HttpStatusCode.BadRequest, "Invalid credentials");
            }
            await SetCookie(user, context);
            var token = GenerateToken(user);

            return new LoginResponseDTO { Token = token, User = _mapper.Map<UsuarioWithRolesDTO>(user) };
        }

        async public Task Logout(HttpContext context)
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        async public Task SetCookie(Usuario usuario, HttpContext context)
        {
            var claims = new List<Claim>()
            {
                new Claim("id", usuario.Id.ToString())
            };

            if (usuario.Roles != null)
            {
                foreach (var rol in usuario.Roles)
                {
                    var claim = new Claim(ClaimTypes.Role, rol.Nombre);
                    claims.Add(claim);
                }
            }

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await context.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(1),
                }
            );
        }

        public string GenerateToken(Usuario usuario)
        {
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim("id", usuario.Id.ToString()));

            if (usuario.Roles != null)
            {
                foreach (var role in usuario.Roles)
                {
                    var claim = new Claim(ClaimTypes.Role, role.Nombre);
                    claims.AddClaim(claim);
                }
            }
            var key = Encoding.UTF8.GetBytes(_secret);
            var symmetricKey = new SymmetricSecurityKey(key);

            var credentials = new SigningCredentials(
                symmetricKey,
                SecurityAlgorithms.HmacSha256Signature
            );

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credentials,
                Issuer = "movieapp",
                Audience = "movieapp-users"
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(tokenConfig);

            return token;
        }

        private async Task<Usuario?> GetUserByEmailOrUsernameAsync(string emailOrUsername)
        {
            return await _context.Usuario
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u =>
                    u.Email == emailOrUsername || u.UserName == emailOrUsername);
        }

        public async Task<List<Usuario>> GetAllUsersAsync()
        {
            return await Task.FromResult(_context.Usuario.ToList());
        }

        async public Task<UsuarioWithRolesDTO> AssignRoles(int id, List<int> rolesIds)
        {
            var usuario = await _userServices.GetOneById(id);
            List<Rol> roles = await _rolServices.GetManyByIds(rolesIds);
            usuario.Roles.Clear();
            foreach (var role in roles)
            {
                if (!usuario.Roles.Any(r => r.Id == role.Id))
                    usuario.Roles.Add(role);
            }
            var updated = await _userServices.UpdateOne(usuario);
            return _mapper.Map<UsuarioWithRolesDTO>(updated);
        }
    }
}
