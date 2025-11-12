using System.ComponentModel.DataAnnotations;

namespace proyecto_prog4.Models.Genres.Dto
{
    public class GenreUpdateDTO
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}

