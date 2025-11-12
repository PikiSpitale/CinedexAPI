using proyecto_prog4.Models.Usuario;

namespace proyecto_prog4.Models.UserFavorite
{
    public class UserFavorite
    {
        public int UsuarioId { get; set; }
        public Usuario.Usuario Usuario { get; set; } = null!;
        public int MovieId { get; set; }
        public Movie.Movie Movie { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
    }
}
