using AutoMapper;
using MongoDB.Driver;
using Project3Vitour.Dtos.ReviewDtos;
using Project3Vitour.Dtos.TourDtos;
using Project3Vitour.Entities;
using Project3Vitour.Settings;

namespace Project3Vitour.Services.TourServices
{
    public class TourReviewService : ITourReviewService
    {
        private readonly IMongoCollection<TourReview> _reviewCollection;
        private readonly IMapper _mapper;

        public TourReviewService(IMapper mapper, IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _reviewCollection = database.GetCollection<TourReview>(settings.ReviewCollectionName);
            _mapper = mapper;
        }

        public async Task<List<TourReviewDto>> GetReviewsByTourIdAsync(string tourId)
        {
            var reviews = await _reviewCollection
                .Find(x => x.TourID == tourId && x.IsApproved == true)  // ← sadece onaylılar
                .SortByDescending(x => x.CreatedAt)
                .ToListAsync();
            return _mapper.Map<List<TourReviewDto>>(reviews);
        }

        public async Task CreateReviewAsync(CreateReviewDto dto)  // ← ReviewDtos'tan gelecek
        {
            var review = _mapper.Map<TourReview>(dto);
            await _reviewCollection.InsertOneAsync(review);
        }

        public async Task<double> GetAverageRatingAsync(string tourId)
        {
            var reviews = await _reviewCollection
                .Find(x => x.TourID == tourId && x.IsApproved == true)
                .ToListAsync();
            if (!reviews.Any()) return 0;
            return reviews.Average(r =>
                (r.ValueRating + r.DestinationRating + r.AccommodationRating + r.TransportRating) / 4.0);
        }

        public async Task ApproveReviewAsync(string id)
        {
            var update = Builders<TourReview>.Update.Set(x => x.IsApproved, true);
            await _reviewCollection.UpdateOneAsync(x => x.ReviewID == id, update);
        }

        public async Task DeleteReviewAsync(string id)
        {
            await _reviewCollection.DeleteOneAsync(x => x.ReviewID == id);
        }

        public async Task<List<TourReview>> GetAllReviewsAsync()
        {
            return await _reviewCollection.Find(_ => true)
                .SortByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<TourReviewDto>> GetReviewsByTourIdPagedAsync(string tourId, int skip, int take)
        {
            var reviews = await _reviewCollection
                .Find(x => x.TourID == tourId && x.IsApproved == true)
                .SortByDescending(x => x.CreatedAt)
                .Skip(skip)
                .Limit(take)
                .ToListAsync();
            return _mapper.Map<List<TourReviewDto>>(reviews);
        }
    }
}