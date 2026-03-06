using AutoMapper;
using MongoDB.Driver;
using Project3Vitour.Dtos.BookingDtos;
using Project3Vitour.Entities;
using Project3Vitour.Settings;

namespace Project3Vitour.Services.BookingServices
{
    public class BookingService : IBookingService
    {
        private readonly IMongoCollection<Booking> _collection;
        private readonly IMapper _mapper;

        public BookingService(IMapper mapper, IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<Booking>(settings.BookingCollectionName);
            _mapper = mapper;
        }

        public async Task<string> CreateBookingAsync(CreateBookingDto dto)
        {
            var booking = _mapper.Map<Booking>(dto);
            var count = await _collection.CountDocumentsAsync(_ => true);
            booking.BookingNumber = $"RES-{DateTime.Now.Year}-{(count + 1):D5}";
            await _collection.InsertOneAsync(booking);
            return booking.BookingNumber;
        }

        public async Task<List<ResultBookingDto>> GetAllBookingsAsync()
        {
            var bookings = await _collection.Find(_ => true)
                .SortByDescending(x => x.CreatedAt)
                .ToListAsync();
            return _mapper.Map<List<ResultBookingDto>>(bookings);
        }

        public async Task<ResultBookingDto> GetBookingByIdAsync(string id)
        {
            var booking = await _collection.Find(x => x.BookingID == id).FirstOrDefaultAsync();
            return _mapper.Map<ResultBookingDto>(booking);
        }

        public async Task UpdateStatusAsync(string id, string status)
        {
            var update = Builders<Booking>.Update.Set(x => x.Status, status);
            await _collection.UpdateOneAsync(x => x.BookingID == id, update);
        }

        public async Task DeleteBookingAsync(string id)
        {
            await _collection.DeleteOneAsync(x => x.BookingID == id);
        }

        public async Task<int> GetTotalParticipantsAsync(string tourId)
        {
            var bookings = await _collection
                .Find(x => x.TourID == tourId && x.Status != "Cancelled")
                .ToListAsync();
            return bookings.Sum(x => x.ParticipantCount);
        }

        public async Task<List<ResultBookingDto>> GetBookingsByTourIdAsync(string tourId)
        {
            var bookings = await _collection
                .Find(x => x.TourID == tourId && x.Status != "Cancelled")
                .SortByDescending(x => x.CreatedAt)
                .ToListAsync();
            return _mapper.Map<List<ResultBookingDto>>(bookings);
        }
    }
}