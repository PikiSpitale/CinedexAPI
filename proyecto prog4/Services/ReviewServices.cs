using proyecto_prog4.Models.Review;
using proyecto_prog4.Models.Review.Dto;
using proyecto_prog4.Repositories;

namespace proyecto_prog4.Services
{
    public class ReviewServices
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewServices(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<List<ReviewsDTO>> GetAll()
        {
            var reviews = await _reviewRepository.GetAll();
            return reviews.Select(r => new ReviewsDTO
            {
                Id = r.Id,
                Content = r.Content,
                Rating = r.Rating,
                MovieId = r.MovieId,
                UserId = r.UserId,
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        public async Task<ReviewsDTO?> GetById(int id)
        {
            var review = await _reviewRepository.GetOne(x => x.Id == id);
            if (review == null) return null;

            return new ReviewsDTO
            {
                Id = review.Id,
                Content = review.Content,
                Rating = review.Rating,
                MovieId = review.MovieId,
                UserId = review.UserId,
                CreatedAt = review.CreatedAt
            };
        }

        public async Task<List<ReviewsDTO>> GetByMovieId(int movieId)
        {
            var reviews = await _reviewRepository.GetByMovieId(movieId);
            return reviews.Select(r => new ReviewsDTO
            {
                Id = r.Id,
                Content = r.Content,
                Rating = r.Rating,
                MovieId = r.MovieId,
                UserId = r.UserId,
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        public async Task<List<ReviewsDTO>> GetByUserId(int userId)
        {
            var reviews = await _reviewRepository.GetByUserId(userId);
            return reviews.Select(r => new ReviewsDTO
            {
                Id = r.Id,
                Content = r.Content,
                Rating = r.Rating,
                MovieId = r.MovieId,
                UserId = r.UserId,
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        public async Task<Review> Create(CreateReviewDTO dto)
        {
            var review = new Review
            {
                Content = dto.Content,
                Rating = dto.Rating,
                MovieId = dto.MovieId,
                UserId = dto.UserId,
                CreatedAt = DateTime.Now
            };

            await _reviewRepository.CreateOne(review);
            return review;
        }

        public async Task<bool> Update(int id, UpdateReviewDTO dto)
        {
            var review = await _reviewRepository.GetOne(x => x.Id == id);
            if (review == null) return false;

            if (!string.IsNullOrEmpty(dto.Content))
                review.Content = dto.Content;

            if (dto.Rating.HasValue && dto.Rating > 0)
                review.Rating = dto.Rating.Value;

            await _reviewRepository.UpdateOne(review);
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var review = await _reviewRepository.GetOne(x => x.Id == id);
            if (review == null) return false;

            await _reviewRepository.DeleteOne(review);
            return true;
        }
    }
}