using Project3Vitour.Dtos.BookingDtos;

namespace Project3Vitour.Services.BookingServices
{
    public interface IBookingService
    {
        Task<string> CreateBookingAsync(CreateBookingDto dto);
        Task<List<ResultBookingDto>> GetAllBookingsAsync();
        Task<ResultBookingDto> GetBookingByIdAsync(string id);
        Task UpdateStatusAsync(string id, string status);
        Task DeleteBookingAsync(string id);

        Task<int> GetTotalParticipantsAsync(string tourId);

        Task<List<ResultBookingDto>> GetBookingsByTourIdAsync(string tourId);
    }
}