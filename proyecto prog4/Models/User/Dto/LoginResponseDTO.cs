using proyecto_prog4.Models.Usuario;

namespace proyecto_prog4.Models.User.Dto
{
    public class LoginResponseDTO
    {
        public string Token { get; set; } = null!;
        public UsuarioWithRolesDTO User { get; set; } = null!;
    }
}