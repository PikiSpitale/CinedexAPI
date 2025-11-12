namespace proyecto_prog4.Models.Review.Dto
{
    public class ReviewsDTO
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public int Rating { get; set; }
        public int MovieId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
    