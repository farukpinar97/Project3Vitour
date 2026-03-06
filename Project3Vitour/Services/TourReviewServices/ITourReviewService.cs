using Project3Vitour.Dtos.ReviewDtos;  // ← TourDtos değil
using Project3Vitour.Dtos.TourDtos;    // ← TourReviewDto için
using Project3Vitour.Entities;

namespace Project3Vitour.Services.TourServices
{
    public interface ITourReviewService
    {
        Task<List<TourReviewDto>> GetReviewsByTourIdAsync(string tourId);
        Task CreateReviewAsync(CreateReviewDto dto);           // ← ReviewDtos
        Task<double> GetAverageRatingAsync(string tourId);
        Task ApproveReviewAsync(string id);
        Task DeleteReviewAsync(string id);
        Task<List<TourReview>> GetAllReviewsAsync();
        Task<List<TourReviewDto>> GetReviewsByTourIdPagedAsync(string tourId, int skip, int take);
    }
}