namespace proyecto_prog4.Models.User.Dto
{
    public class UsuarioWithRolesDTO
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public List<string>? Roles { get; set; }
    }
}