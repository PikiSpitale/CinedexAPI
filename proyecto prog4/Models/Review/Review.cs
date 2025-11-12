using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using proyecto_prog4.Models.Usuario;
using proyecto_prog4.Models.Movie;

namespace proyecto_prog4.Models.Review
{
    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Range(1, 10)]
        public int Rating { get; set; }

        [Required]
        public string Content { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Relaciones
        [ForeignKey(nameof(Usuario))]
        public int UserId { get; set; }
        public Usuario.Usuario Usuario { get; set; } = null!;

        [ForeignKey(nameof(Movie))]
        public int MovieId { get; set; }
        public Movie.Movie Movie { get; set; } = null!;
    }
}
